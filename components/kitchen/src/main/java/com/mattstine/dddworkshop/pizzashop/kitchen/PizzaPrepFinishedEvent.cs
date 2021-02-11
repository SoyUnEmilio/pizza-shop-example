namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using lombok.Value;


class PizzaPrepFinishedEvent : PizzaEvent {
    PizzaRef @ref;
}

