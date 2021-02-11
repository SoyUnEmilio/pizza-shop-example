namespace com.mattstine.dddworkshop.pizzashop.delivery
{
    using com.mattstine.dddworkshop.pizzashop.delivery.acl.kitchen;
    using com.mattstine.dddworkshop.pizzashop.delivery.acl.ordering;
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;

/**
 * @author Matt Stine
 */
public class DeliveryServiceTests
    {

        private DeliveryService service;
        private EventLog eventLog;
        private DeliveryOrderRepository deliveryOrderRepository;

        public DeliveryServiceTests()
        {

            eventLog = mock(EventLog.class);
        deliveryOrderRepository = mock(DeliveryOrderRepository.class);
        OrderingService orderingService = mock(OrderingService.class);
        KitchenService kitchenService = mock(KitchenService.class);
        service = new DeliveryService(eventLog, deliveryOrderRepository, orderingService, kitchenService);
    }

    [Fact]
    public void subscribes_to_kitchen_orders_topic()
    {
        verify(eventLog).subscribe(eq(new Topic("kitchen_orders")), isA(EventHandler.class));
    }

[Fact]
    public void should_return_deliveryOrder_by_kitchenOrderRef()
{
    KitchenOrderRef kitchenOrderRef = new KitchenOrderRef();

    DeliveryOrder deliveryOrder = DeliveryOrder.builder()
            .@ref(new DeliveryOrderRef())
                .kitchenOrderRef(new KitchenOrderRef())
                .onlineOrderRef(new OnlineOrderRef())
                .pizza(DeliveryOrder.Pizza.builder().size(DeliveryOrder.Pizza.Size.MEDIUM).build())
                .eventLog(eventLog)
                .build();

when(deliveryOrderRepository.findByKitchenOrderRef(eq(kitchenOrderRef))).thenReturn(deliveryOrder);

assertThat(service.findDeliveryOrderByKitchenOrderRef(kitchenOrderRef)).isEqualTo(deliveryOrder);
    }
}
}