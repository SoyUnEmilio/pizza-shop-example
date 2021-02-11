namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports.Event;
using lombok.Value;


class KitchenOrderPrepStartedEvent : Event, KitchenOrderEvent {
    KitchenOrderRef @ref;
}
