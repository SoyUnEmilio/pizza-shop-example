namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using lombok.Value;


class KitchenOrderAssemblyFinishedEvent : KitchenOrderEvent {
    KitchenOrderRef @ref;
}
