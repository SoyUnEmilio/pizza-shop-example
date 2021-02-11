namespace  com.mattstine.dddworkshop.pizzashop.delivery
{
    using com.mattstine.dddworkshop.pizzashop.delivery.acl.kitchen.KitchenOrderRef;
    using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Repository;

    /**
     * @author Matt Stine
     */
    public interface DeliveryOrderRepository : Repository<DeliveryOrderRef, DeliveryOrder, DeliveryOrder.OrderState, DeliveryOrderEvent, DeliveryOrderAddedEvent> {
        DeliveryOrder findByKitchenOrderRef(KitchenOrderRef kitchenOrderRef);
    }
}
