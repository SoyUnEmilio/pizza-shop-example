namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using lombok.Value;


class PizzaBakeStartedEvent : PizzaEvent {
    PizzaRef @ref;
}
