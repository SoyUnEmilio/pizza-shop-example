namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Repository;

using java.util.Set;

interface PizzaRepository : Repository<PizzaRef, Pizza, Pizza.PizzaState, PizzaEvent, PizzaAddedEvent> {
    Set<Pizza> findPizzasByKitchenOrderRef(KitchenOrderRef kitchenOrderRef);
}
