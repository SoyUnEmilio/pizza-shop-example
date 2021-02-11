namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;



class KitchenOrderPrepStartedEvent : Event, KitchenOrderEvent {
    KitchenOrderRef @ref;
}
