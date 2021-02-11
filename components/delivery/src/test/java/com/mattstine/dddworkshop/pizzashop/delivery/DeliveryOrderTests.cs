namespace com.mattstine.dddworkshop.pizzashop.delivery
{
    using com.mattstine.dddworkshop.pizzashop.delivery.acl.kitchen;
    using com.mattstine.dddworkshop.pizzashop.delivery.acl.ordering;
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;

    /**
     * @author Matt Stine
     */
    public class DeliveryOrderTests
    {

        private DeliveryOrder deliveryOrder;
        private DeliveryOrderRef @ref;
        private EventLog eventLog;

        public DeliveryOrderTests()
        {
            eventLog = mock(EventLog.class);
                @ref = new DeliveryOrderRef();
        deliveryOrder = DeliveryOrder.builder()
                .@ref(@ref)
                .kitchenOrderRef(new KitchenOrderRef())
                .onlineOrderRef(new OnlineOrderRef())
                .pizza(DeliveryOrder.Pizza.builder().size(DeliveryOrder.Pizza.Size.MEDIUM).build())
                    .eventLog(eventLog)
                    .build();

    }

    [Fact]
    public void can_build_new_order()
    {
        assertThat(deliveryOrder).isNotNull();
    }

    [Fact]
    public void new_order_is_ready_for_delivery()
    {
        assertThat(deliveryOrder.isReadyForDelivery()).isTrue();
    }

    [Fact]
    public void accumulator_apply_with_orderAddedEvent_returns_order()
    {
        DeliveryOrderAddedEvent deliveryOrderAddedEvent = new DeliveryOrderAddedEvent(@ref, deliveryOrder.state());
        assertThat(deliveryOrder.accumulatorFunction(eventLog).apply(deliveryOrder.identity(), deliveryOrderAddedEvent)).isEqualTo(deliveryOrder);
    }
}