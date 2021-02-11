namespace com.mattstine.dddworkshop.pizzashop.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.ordering.acl.payments.PaymentRef;
using org.junit.Before;
using org.junit.Test;

using static org.assertj.core.api.Assertions.assertThat;
using static org.assertj.core.api.Assertions.assertThatIllegalStateException;
using static org.mockito.ArgumentMatchers.eq;
using static org.mockito.ArgumentMatchers.isA;
using static org.mockito.Mockito.mock;
using static org.mockito.Mockito.verify;

/**
 * @author Matt Stine
 */
public class OnlineOrderTests {

    private EventLog eventLog;
    private OnlineOrder onlineOrder;
    private Pizza pizza;
    private OnlineOrderRef @ref;

    @Before
    public void setUp() {
        ref = new OnlineOrderRef();
        eventLog = mock(EventLog.class);
        onlineOrder = OnlineOrder.builder()
                .type(OnlineOrder.Type.PICKUP)
                .eventLog(eventLog)
                .ref(@ref)
                .build();
        pizza = Pizza.builder().size(Pizza.Size.MEDIUM).build();
    }

    [Fact]
    public void new_order_is_new() {
        assertThat(onlineOrder.isNew()).isTrue();
    }

    [Fact]
    public void should_create_pickup_order() {
        assertThat(onlineOrder.isPickupOrder()).isTrue();
    }

    [Fact]
    public void should_create_delivery_order() {
        onlineOrder = OnlineOrder.builder()
                .type(OnlineOrder.Type.DELIVERY)
                .eventLog(eventLog)
                .ref(new OnlineOrderRef())
                .build();
        assertThat(onlineOrder.isDeliveryOrder()).isTrue();
    }

    [Fact]
    public void should_add_pizza() {
        onlineOrder.addPizza(pizza);
        assertThat(onlineOrder.getPizzas()).contains(pizza);
    }

    [Fact]
    public void adding_pizza_fires_event() {
        onlineOrder.addPizza(pizza);
        verify(eventLog)
                .publish(eq(new Topic("ordering")),
                        eq(new PizzaAddedEvent(@ref, pizza)));
    }

    [Fact]
    public void can_only_add_pizza_to_new_order() {
        onlineOrder.addPizza(pizza);
        onlineOrder.submit();
        assertThatIllegalStateException().isThrownBy(() -> onlineOrder.addPizza(pizza));
    }

    [Fact]
    public void submit_order_updates_state() {
        onlineOrder.addPizza(pizza);
        onlineOrder.submit();
        assertThat(onlineOrder.isSubmitted()).isTrue();
    }

    [Fact]
    public void submit_order_fires_event() {
        onlineOrder.addPizza(Pizza.builder().size(Pizza.Size.MEDIUM).build());
        onlineOrder.submit();
        verify(eventLog)
                .publish(eq(new Topic("ordering")),
                        isA(OnlineOrderSubmittedEvent.class));
    }

    [Fact]
    public void submit_requires_at_least_one_pizza() {
        assertThatIllegalStateException()
                .isThrownBy(() -> onlineOrder.submit());
    }

    [Fact]
    public void can_only_submit_new_order() {
        onlineOrder.addPizza(pizza);
        onlineOrder.submit();
        assertThatIllegalStateException().isThrownBy(onlineOrder::submit);
    }

    [Fact]
    public void calculates_price() {
        onlineOrder.addPizza(pizza);
        assertThat(onlineOrder.calculatePrice()).isEqualTo(Pizza.Size.MEDIUM.getPrice());
    }

    [Fact]
    public void mark_paid_updates_state() {
        onlineOrder.addPizza(pizza);
        onlineOrder.submit();
        onlineOrder.markPaid();
        assertThat(onlineOrder.isPaid()).isTrue();
    }

    [Fact]
    public void mark_paid_fires_event() {
        onlineOrder.addPizza(pizza);
        verify(eventLog).publish(eq(new Topic("ordering")), isA(PizzaAddedEvent.class));
        onlineOrder.submit();
        verify(eventLog).publish(eq(new Topic("ordering")), isA(OnlineOrderSubmittedEvent.class));
        onlineOrder.markPaid();
        verify(eventLog).publish(eq(new Topic("ordering")), isA(OnlineOrderPaidEvent.class));
    }

    [Fact]
    public void can_only_mark_submitted_order_paid() {
        assertThatIllegalStateException().isThrownBy(onlineOrder::markPaid);
    }

    [Fact]
    public void setting_payment_ref_fires_event() {
        PaymentRef paymentRef = new PaymentRef();
        onlineOrder.assignPaymentRef(paymentRef);

        verify(eventLog).publish(eq(new Topic("ordering")), isA(PaymentRefAssignedEvent.class));
    }

    [Fact]
    public void accumulator_apply_with_orderAddedEvent_returns_order() {
        OnlineOrderAddedEvent orderAddedEvent = new OnlineOrderAddedEvent(@ref, onlineOrder.state());
        assertThat(onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder.identity(), orderAddedEvent)).isEqualTo(onlineOrder);
    }

    [Fact]
    public void accumulator_apply_with_pizzaAddedEvent_updates_state() {
        OnlineOrder expectedOnlineOrder = OnlineOrder.builder()
                .ref(@ref)
                .type(OnlineOrder.Type.PICKUP)
                .eventLog(eventLog)
                .build();
        expectedOnlineOrder.addPizza(pizza);

        PizzaAddedEvent pae = new PizzaAddedEvent(@ref, pizza);

        assertThat(onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder, pae)).isEqualTo(expectedOnlineOrder);
    }

    [Fact]
    public void accumulator_apply_with_orderSubmittedEvent_updates_state() {
        OnlineOrder expectedOnlineOrder = OnlineOrder.builder()
                .ref(@ref)
                .type(OnlineOrder.Type.PICKUP)
                .eventLog(eventLog)
                .build();
        expectedOnlineOrder.addPizza(pizza);
        expectedOnlineOrder.submit();

        PizzaAddedEvent pae = new PizzaAddedEvent(@ref, pizza);
        onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder, pae);

        OnlineOrderSubmittedEvent ose = new OnlineOrderSubmittedEvent(@ref);

        assertThat(onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder, ose)).isEqualTo(expectedOnlineOrder);
    }

    [Fact]
    public void accumulator_apply_with_paymentRefAssignedEvent_updates_state() {
        OnlineOrder expectedOnlineOrder = OnlineOrder.builder()
                .ref(@ref)
                .type(OnlineOrder.Type.PICKUP)
                .eventLog(eventLog)
                .build();
        expectedOnlineOrder.addPizza(pizza);
        expectedOnlineOrder.submit();

        PaymentRef paymentRef = new PaymentRef();
        expectedOnlineOrder.assignPaymentRef(paymentRef);

        PizzaAddedEvent pae = new PizzaAddedEvent(@ref, pizza);
        onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder, pae);

        OnlineOrderSubmittedEvent ose = new OnlineOrderSubmittedEvent(@ref);
        onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder, ose);

        PaymentRefAssignedEvent prae = new PaymentRefAssignedEvent(@ref, paymentRef);
        assertThat(onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder, prae)).isEqualTo(expectedOnlineOrder);
    }

    [Fact]
    public void accumulator_apply_with_orderPaidEvent_updates_state() {
        OnlineOrder expectedOnlineOrder = OnlineOrder.builder()
                .ref(@ref)
                .type(OnlineOrder.Type.PICKUP)
                .eventLog(eventLog)
                .build();
        expectedOnlineOrder.addPizza(pizza);
        expectedOnlineOrder.submit();

        PaymentRef paymentRef = new PaymentRef();
        expectedOnlineOrder.assignPaymentRef(paymentRef);

        expectedOnlineOrder.markPaid();

        PizzaAddedEvent pae = new PizzaAddedEvent(@ref, pizza);
        onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder, pae);

        OnlineOrderSubmittedEvent ose = new OnlineOrderSubmittedEvent(@ref);
        onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder, ose);

        PaymentRefAssignedEvent prae = new PaymentRefAssignedEvent(@ref, paymentRef);
        onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder, prae);

        OnlineOrderPaidEvent ope = new OnlineOrderPaidEvent(@ref);

        assertThat(onlineOrder.accumulatorFunction(eventLog).apply(onlineOrder, ope)).isEqualTo(expectedOnlineOrder);
    }

}
