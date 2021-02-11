namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using lombok.Value;


class KitchenOrderAssemblyStartedEvent : KitchenOrderEvent {
    KitchenOrderRef @ref;
}
