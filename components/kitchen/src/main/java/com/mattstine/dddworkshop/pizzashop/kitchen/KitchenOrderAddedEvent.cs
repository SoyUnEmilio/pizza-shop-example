namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.RepositoryAddEvent;
using lombok.Value;


class KitchenOrderAddedEvent : KitchenOrderEvent, RepositoryAddEvent {
    KitchenOrderRef @ref;
    KitchenOrder.OrderState state;
}
