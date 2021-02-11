namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using lombok.Value;


class KitchenOrderBakeStartedEvent : KitchenOrderEvent {
    KitchenOrderRef @ref;
}
