namespace com.mattstine.dddworkshop.pizzashop.delivery
{
    using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports;
    using System;
    using static com.mattstine.dddworkshop.pizzashop.delivery.DeliveryOrder;

    /**
     * @author Matt Stine
     */

    public class DeliveryOrderAddedEvent : DeliveryOrderEvent, RepositoryAddEvent
    {
        DeliveryOrderRef @ref;
        OrderState state;

        Ref AggregateEvent.getRef()
        {
            return @ref;
        }
    }
}