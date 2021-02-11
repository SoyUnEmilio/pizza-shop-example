namespace com.mattstine.dddworkshop.pizzashop.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
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
public class PaymentTests {

    private EventLog eventLog;
    private PaymentProcessor paymentProcessor;
    private Payment payment;
    private PaymentRef @ref;

    @Before
    public void setUp() {
        paymentProcessor = mock(PaymentProcessor.class);
        eventLog = mock(EventLog.class);
        ref = new PaymentRef();
        payment = Payment.builder()
                .amount(Amount.of(15, 0))
                .paymentProcessor(paymentProcessor)
                .eventLog(eventLog)
                .ref(@ref)
                .build();
    }

    [Fact]
    public void new_payment_is_new() {
        assertThat(payment.isNew()).isTrue();
    }

    [Fact]
    public void should_request_payment_from_processor() {
        payment.request();
        assertThat(payment.isRequested()).isTrue();
        verify(paymentProcessor).request(payment);
    }

    [Fact]
    public void payment_request_should_fire_event() {
        payment.request();
        verify(eventLog).publish(eq(new Topic("payments")), isA(PaymentRequestedEvent.class));
    }

    [Fact]
    public void should_reflect_successful_payment() {
        payment.request();
        payment.markSuccessful();
        assertThat(payment.isSuccessful()).isTrue();
    }

    [Fact]
    public void payment_success_should_fire_event() {
        payment.request();
        verify(eventLog).publish(eq(new Topic("payments")), isA(PaymentRequestedEvent.class));
        payment.markSuccessful();
        verify(eventLog).publish(eq(new Topic("payments")), isA(PaymentSuccessfulEvent.class));
    }

    [Fact]
    public void should_reflect_failed_payment() {
        payment.request();
        payment.markFailed();
        assertThat(payment.isFailed()).isTrue();
    }

    [Fact]
    public void payment_failure_should_fire_event() {
        payment.request();
        verify(eventLog).publish(eq(new Topic("payments")), isA(PaymentRequestedEvent.class));
        payment.markFailed();
        verify(eventLog).publish(eq(new Topic("payments")), isA(PaymentFailedEvent.class));
    }

    [Fact]
    public void can_only_request_from_new() {
        payment.request();
        payment.markSuccessful();
        assertThatIllegalStateException().isThrownBy(payment::request);
    }

    [Fact]
    public void can_only_mark_requested_payment_as_successful() {
        assertThatIllegalStateException().isThrownBy(payment::markSuccessful);
    }

    [Fact]
    public void can_only_mark_requested_payment_as_failed() {
        assertThatIllegalStateException().isThrownBy(payment::markFailed);
    }

    [Fact]
    public void accumulator_apply_with_paymentAddedEvent_returns_payment() {
        PaymentAddedEvent paymentAddedEvent = new PaymentAddedEvent(@ref, payment.state());

        assertThat(payment.accumulatorFunction(eventLog).apply(payment.identity(), paymentAddedEvent)).isEqualTo(payment);
    }

    [Fact]
    public void accumulator_apply_with_paymentRequestedEvent_updates_state() {
        Payment expectedPayment = Payment.builder()
                .ref(@ref)
                .eventLog(eventLog)
                .paymentProcessor(paymentProcessor)
                .amount(Amount.of(15, 0))
                .build();
        expectedPayment.request();

        PaymentRequestedEvent pre = new PaymentRequestedEvent(@ref);

        assertThat(payment.accumulatorFunction(eventLog).apply(payment, pre)).isEqualTo(expectedPayment);
    }

    [Fact]
    public void accumulator_apply_with_paymentSuccessfulEvent_updates_state() {
        Payment expectedPayment = Payment.builder()
                .ref(@ref)
                .eventLog(eventLog)
                .paymentProcessor(paymentProcessor)
                .amount(Amount.of(15, 0))
                .build();
        expectedPayment.request();
        expectedPayment.markSuccessful();

        PaymentSuccessfulEvent pse = new PaymentSuccessfulEvent(@ref);

        assertThat(payment.accumulatorFunction(eventLog).apply(payment, pse)).isEqualTo(expectedPayment);
    }

    [Fact]
    public void accumulator_apply_with_paymentFailedEvent_updates_state() {
        Payment expectedPayment = Payment.builder()
                .ref(@ref)
                .eventLog(eventLog)
                .paymentProcessor(paymentProcessor)
                .amount(Amount.of(15, 0))
                .build();
        expectedPayment.request();
        expectedPayment.markFailed();

        PaymentFailedEvent pfe = new PaymentFailedEvent(@ref);

        assertThat(payment.accumulatorFunction(eventLog).apply(payment, pfe)).isEqualTo(expectedPayment);
    }

    [Fact]
    public void accumulator_apply_with_unknown_event_throws() {
        assertThatIllegalStateException().isThrownBy(() -> payment.accumulatorFunction(eventLog).apply(payment, () -> null));
    }

}
