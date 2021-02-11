namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using lombok.Value;


class PizzaBakeFinishedEvent : PizzaEvent {
    PizzaRef @ref;
}
