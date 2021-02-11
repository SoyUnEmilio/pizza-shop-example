namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering.OnlineOrder;
using com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering.OnlineOrderPaidEvent;
using com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering.OnlineOrderRef;
using com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering.OrderingService;


using java.util.Set;


class DefaultKitchenService : KitchenService {
    EventLog eventLog;
    KitchenOrderRepository kitchenOrderRepository;
    PizzaRepository pizzaRepository;
    OrderingService orderingService;

    DefaultKitchenService(EventLog eventLog, KitchenOrderRepository kitchenOrderRepository, PizzaRepository pizzaRepository, OrderingService orderingService) {
        this.kitchenOrderRepository = kitchenOrderRepository;
        this.eventLog = eventLog;
        this.pizzaRepository = pizzaRepository;
        this.orderingService = orderingService;

        this.eventLog.subscribe(new Topic("ordering"), (e) -> {
            if (e instanceof OnlineOrderPaidEvent) {
                addKitchenOrderToRepository((OnlineOrderPaidEvent) e);
            }
        });

        this.eventLog.subscribe(new Topic("kitchen_orders"), (e) -> {
            if (e instanceof KitchenOrderPrepStartedEvent) {
                createAndStartPrepOfKitchenOrderPizzas((KitchenOrderPrepStartedEvent) e);
            }
        });

        this.eventLog.subscribe(new Topic("pizzas"), e -> {
            if (e instanceof PizzaPrepFinishedEvent) {
                PizzaPrepFinishedEvent ppfe = (PizzaPrepFinishedEvent) e;
                Pizza pizza = pizzaRepository.findByRef(ppfe.getRef());
                pizza.startBake();
            } else if (e instanceof PizzaBakeStartedEvent) {
                PizzaBakeStartedEvent pbse = (PizzaBakeStartedEvent) e;
                Pizza pizza = pizzaRepository.findByRef(pbse.getRef());
                KitchenOrder kitchenOrder = kitchenOrderRepository.findByRef(pizza.getKitchenOrderRef());
                if (kitchenOrder.isPrepping()) {
                    kitchenOrder.startBake();
                }
            } else if (e instanceof PizzaBakeFinishedEvent) {
                PizzaBakeFinishedEvent pbfe = (PizzaBakeFinishedEvent) e;
                Pizza pizza = pizzaRepository.findByRef(pbfe.getRef());
                KitchenOrderRef kitchenOrderRef = pizza.getKitchenOrderRef();
                KitchenOrder kitchenOrder = kitchenOrderRepository.findByRef(kitchenOrderRef);

                if (kitchenOrder.isBaking()) {
                    kitchenOrder.startAssembly();
                }

                if (pizzaRepository.findPizzasByKitchenOrderRef(kitchenOrderRef).stream()
                        .allMatch(Pizza::hasFinishedBaking)) {
                    kitchenOrder.finishAssembly();
                }
            }
        });
    }

    
    public void startOrderPrep(KitchenOrderRef kitchenOrderRef) {
        KitchenOrder kitchenOrder = kitchenOrderRepository.findByRef(kitchenOrderRef);
        kitchenOrder.startPrep();
    }

    
    public void finishPizzaPrep(PizzaRef @ref) {
        Pizza pizza = pizzaRepository.findByRef(@ref);
        pizza.finishPrep();
    }

    
    public void removePizzaFromOven(PizzaRef @ref) {
        Pizza pizza = pizzaRepository.findByRef(@ref);
        pizza.finishBake();
    }

    
    public KitchenOrder findKitchenOrderByRef(KitchenOrderRef kitchenOrderRef) {
        return kitchenOrderRepository.findByRef(kitchenOrderRef);
    }

    
    public KitchenOrder findKitchenOrderByOnlineOrderRef(OnlineOrderRef onlineOrderRef) {
        return kitchenOrderRepository.findByOnlineOrderRef(onlineOrderRef);
    }

    
    public Pizza findPizzaByRef(PizzaRef @ref) {
        return pizzaRepository.findByRef(@ref);
    }

    
    public Set<Pizza> findPizzasByKitchenOrderRef(KitchenOrderRef kitchenOrderRef) {
        return pizzaRepository.findPizzasByKitchenOrderRef(kitchenOrderRef);
    }

    private void createAndStartPrepOfKitchenOrderPizzas(KitchenOrderPrepStartedEvent e) {
        KitchenOrder kitchenOrder = kitchenOrderRepository.findByRef(e.getRef());

        kitchenOrder.getPizzas().forEach(pizzaVO -> {
            Pizza pizza = Pizza.builder()
                    .ref(this.pizzaRepository.nextIdentity())
                    .kitchenOrderRef(kitchenOrder.getRef())
                    .eventLog(eventLog)
                    .size(pizzaValueObjectSizeToPizzaAggregateSize(pizzaVO.getSize()))
                    .build();

            pizzaRepository.add(pizza);
            pizza.startPrep();
        });
    }

    private void addKitchenOrderToRepository(OnlineOrderPaidEvent e) {
        OnlineOrderRef onlineOrderRef = e.getRef();
        OnlineOrder onlineOrder = orderingService.findByRef(onlineOrderRef);
        KitchenOrder kitchenOrder = onlineOrderToKitchenOrder(onlineOrder);
        kitchenOrderRepository.add(kitchenOrder);
    }

    private KitchenOrder onlineOrderToKitchenOrder(OnlineOrder onlineOrder) {
        KitchenOrder.KitchenOrderBuilder kitchenOrderBuilder = KitchenOrder.builder()
                .ref(kitchenOrderRepository.nextIdentity())
                .onlineOrderRef(onlineOrder.getRef())
                .eventLog(eventLog);

        onlineOrder.getPizzas().forEach(
                pizza -> kitchenOrderBuilder.pizza(KitchenOrder.Pizza.builder()
                        .size(onlineOrderPizzaSizeToKitchenPizzaSize(pizza.getSize()))
                        .build()));

        return kitchenOrderBuilder.build();
    }

    private Pizza.Size pizzaValueObjectSizeToPizzaAggregateSize(KitchenOrder.Pizza.Size voSize) {
        switch (voSize) {
            case SMALL:
                return Pizza.Size.SMALL;
            case MEDIUM:
                return Pizza.Size.MEDIUM;
            case LARGE:
                return Pizza.Size.LARGE;
            default:
                throw new IllegalStateException("voSize must be member of KitchenOrder.Pizza.Size enum");
        }
    }

    @SuppressWarnings({"SameReturnValue", "SwitchStatementWithTooFewBranches"})
    private KitchenOrder.Pizza.Size onlineOrderPizzaSizeToKitchenPizzaSize(com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering.Pizza.Size orderingSize) {
        switch (orderingSize) {
            case MEDIUM:
                return KitchenOrder.Pizza.Size.MEDIUM;
            default:
                throw new IllegalStateException("orderingSize must be member of ordering.Pizza.Size enum");
        }
    }
}
