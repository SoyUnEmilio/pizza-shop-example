namespace com.mattstine.dddworkshop.pizzashop.kitchen
{

    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
    using com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering;

    public class DefaultKitchenServiceTests
    {

        private KitchenService service;
        private KitchenOrderRepository kitchenOrderRepository;
        private EventLog eventLog;

        public DefaultKitchenServiceTests()
        {
            //    eventLog = mock(EventLog.class);
            //kitchenOrderRepository = mock(KitchenOrderRepository.class);
            //PizzaRepository pizzaRepository = mock(PizzaRepository.class);
            //OrderingService orderingService = mock(OrderingService.class);
            //service = new DefaultKitchenService(eventLog, kitchenOrderRepository, pizzaRepository, orderingService);
        }

        [Fact]
        public void subscribes_to_ordering_topic()
        {
            verify(eventLog).subscribe(eq(new Topic("ordering")), isA(EventHandler.class));
    }

    [Fact]
    public void should_return_kitchenOrder_by_onlineOrderRef()
    {
        OnlineOrderRef onlineOrderRef = new OnlineOrderRef();

        KitchenOrder kitchenOrder = KitchenOrder.builder()
                .eventLog(eventLog)
                .onlineOrderRef(onlineOrderRef)
                .ref (new KitchenOrderRef())
                .build();

        when(kitchenOrderRepository.findByOnlineOrderRef(eq(onlineOrderRef))).thenReturn(kitchenOrder);

        assertThat(service.findKitchenOrderByOnlineOrderRef(onlineOrderRef)).isEqualTo(kitchenOrder);
    }

    [Fact]
    public void subscribes_to_kitchen_orders_topic()
    {
        verify(eventLog).subscribe(eq(new Topic("kitchen_orders")), isA(EventHandler.class));
    }

[Fact]
public void subscribes_to_pizzas_topic()
{
    verify(eventLog).subscribe(eq(new Topic("pizzas")), isA(EventHandler.class));
    }

}
}