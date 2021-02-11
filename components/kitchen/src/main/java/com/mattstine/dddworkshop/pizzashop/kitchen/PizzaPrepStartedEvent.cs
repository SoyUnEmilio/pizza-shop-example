namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using lombok.Value;


class PizzaPrepStartedEvent : PizzaEvent {
    PizzaRef @ref;
}
