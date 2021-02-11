namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Repository;
using com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering.OnlineOrderRef;

interface KitchenOrderRepository : Repository<KitchenOrderRef, KitchenOrder, KitchenOrder.OrderState, KitchenOrderEvent, KitchenOrderAddedEvent> {
    KitchenOrder findByOnlineOrderRef(OnlineOrderRef onlineOrderRef);
}
