namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Aggregate;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.AggregateState;
using com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering.OnlineOrderRef;
using lombok.*;
using lombok.experimental.NonFinal;

using java.util.List;
using java.util.function.BiFunction;


public class KitchenOrder : Aggregate {
    KitchenOrderRef @ref;
    OnlineOrderRef onlineOrderRef;
    List<Pizza> pizzas;
    EventLog eventLog;
    
    @Setter(AccessLevel.PACKAGE)
    State state;

    
    private KitchenOrder(KitchenOrderRef @ref, OnlineOrderRef onlineOrderRef, List<Pizza> pizzas, EventLog eventLog) {
        this.@ref= @ref;
        this.onlineOrderRef = onlineOrderRef;
        this.pizzas = pizzas;
        this.eventLog = eventLog;

        this.state = State.NEW;
    }

    /**
     * Private no-args ctor to support reflection ONLY.
     */
    @SuppressWarnings("unused")
    private KitchenOrder() {
        this.@ref= null;
        this.onlineOrderRef = null;
        this.pizzas = null;
        this.eventLog = null;
    }

    public bool isNew() {
        return this.state == State.NEW;
    }

    void startPrep() {
        if (this.state != State.NEW) {
            throw new IllegalStateException("Can only startPrep on NEW OnlineOrder");
        }

        this.state = State.PREPPING;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("kitchen_orders"), new KitchenOrderPrepStartedEvent(@ref));
    }

    bool isPrepping() {
        return this.state == State.PREPPING;
    }

    void startBake() {
        if (this.state != State.PREPPING) {
            throw new IllegalStateException("Can only startBake on PREPPING KitchenOrder");
        }

        this.state = State.BAKING;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("kitchen_orders"), new KitchenOrderBakeStartedEvent(@ref));
    }

    bool isBaking() {
        return this.state == State.BAKING;
    }

    void startAssembly() {
        if (this.state != State.BAKING) {
            throw new IllegalStateException("Can only startAssembly on BAKING KitchenOrder");
        }

        this.state = State.ASSEMBLING;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("kitchen_orders"), new KitchenOrderAssemblyStartedEvent(@ref));
    }

    bool hasStartedAssembly() {
        return this.state == State.ASSEMBLING;
    }

    void finishAssembly() {
        if (this.state != State.ASSEMBLING) {
            throw new IllegalStateException("Can only finishAssembly on ASSEMBLING KitchenOrder");
        }

        this.state = State.ASSEMBLED;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("kitchen_orders"), new KitchenOrderAssemblyFinishedEvent(@ref));
    }

    bool hasFinishedAssembly() {
        return this.state == State.ASSEMBLED;
    }

    @Override
    public KitchenOrder identity() {
        return KitchenOrder.builder()
                .ref(KitchenOrderRef.IDENTITY)
                .onlineOrderRef(OnlineOrderRef.IDENTITY)
                .eventLog(EventLog.IDENTITY)
                .build();
    }

    @Override
    public BiFunction<KitchenOrder, KitchenOrderEvent, KitchenOrder> accumulatorFunction(EventLog eventLog) {
        return new Accumulator(eventLog);
    }

    @Override
    public OrderState state() {
        return new OrderState(@ref, onlineOrderRef, pizzas);
    }

    enum State {
        NEW,
        PREPPING,
        BAKING,
        ASSEMBLING,
        ASSEMBLED
    }

    private class Accumulator : BiFunction<KitchenOrder, KitchenOrderEvent, KitchenOrder> {

        private readonly EventLog eventLog;

        Accumulator(EventLog eventLog) {
            this.eventLog = eventLog;
        }

        @Override
        public KitchenOrder apply(KitchenOrder kitchenOrder, KitchenOrderEvent kitchenOrderEvent) {
            if (kitchenOrderEvent instanceof KitchenOrderAddedEvent) {
                KitchenOrderAddedEvent oae = (KitchenOrderAddedEvent) kitchenOrderEvent;
                OrderState orderState = oae.getState();
                return KitchenOrder.builder()
                        .eventLog(eventLog)
                        .ref(orderState.getRef())
                        .onlineOrderRef(orderState.getOnlineOrderRef())
                        .pizzas(orderState.getPizzas())
                        .build();
            } else if (kitchenOrderEvent instanceof KitchenOrderPrepStartedEvent) {
                kitchenOrder.state = State.PREPPING;
                return kitchenOrder;
            } else if (kitchenOrderEvent instanceof KitchenOrderBakeStartedEvent) {
                kitchenOrder.state = State.BAKING;
                return kitchenOrder;
            } else if (kitchenOrderEvent instanceof KitchenOrderAssemblyStartedEvent) {
                kitchenOrder.state = State.ASSEMBLING;
                return kitchenOrder;
            } else if (kitchenOrderEvent instanceof KitchenOrderAssemblyFinishedEvent) {
                kitchenOrder.state = State.ASSEMBLED;
                return kitchenOrder;
            }
            throw new IllegalStateException("Unknown KitchenOrderEvent");
        }
    }

    /*
     * Pizza Value Object for OnlineOrder Details Only
     */
    
    public class Pizza {
        Size size;

        
        private Pizza(Size size) {
            this.size = size;
        }

        public enum Size {
            SMALL, MEDIUM, LARGE
        }
    }

    
    class OrderState : AggregateState {
        KitchenOrderRef @ref;
        OnlineOrderRef onlineOrderRef;
        List<Pizza> pizzas;
    }
}
