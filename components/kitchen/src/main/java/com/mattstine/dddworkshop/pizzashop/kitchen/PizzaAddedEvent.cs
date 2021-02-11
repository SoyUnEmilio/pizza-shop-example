namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports;



class PizzaAddedEvent : PizzaEvent, RepositoryAddEvent {
    PizzaRef @ref;
    Pizza.PizzaState state;
}
