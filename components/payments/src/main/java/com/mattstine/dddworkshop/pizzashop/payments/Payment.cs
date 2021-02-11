namespace com.mattstine.dddworkshop.pizzashop.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports;

/**
 * @author Matt Stine
 */
public class Payment : Aggregate<PaymentEvent>
{
    Amount amount;
    PaymentProcessor paymentProcessor;
    PaymentRef @ref;
    EventLog eventLog;

    State state;

    private Payment(Amount amount,
                    PaymentProcessor paymentProcessor,
                    PaymentRef @ref,
                    EventLog eventLog)
    {
        this.amount = amount;
        this.paymentProcessor = paymentProcessor;
        this.@ref = @ref;
        this.eventLog = eventLog;

        this.state = State.NEW;
    }

    /**
     * Private no-args ctor to support reflection ONLY.
     */
    private Payment()
    {
        this.amount = null;
        this.eventLog = null;
        this.paymentProcessor = null;
        this.@ref = null;
    }

    public bool isNew()
    {
        return state == State.NEW;
    }

    bool isRequested()
    {
        return state == State.REQUESTED;
    }

    bool isSuccessful()
    {
        return state == State.SUCCESSFUL;
    }

    bool isFailed()
    {
        return state == State.FAILED;
    }

    void request()
    {
        if (state != State.NEW)
        {
            throw new IllegalStateException("Payment must be NEW to request payment");
        }

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert paymentProcessor != null;
        paymentProcessor.request(this);

        state = State.REQUESTED;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("payments"), new PaymentRequestedEvent(this.ref));
    }

    void markSuccessful()
    {
        if (state != State.REQUESTED)
        {
            throw new IllegalStateException("Payment must be REQUESTED to mark successful");
        }

        state = State.SUCCESSFUL;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        Assert(eventLog != null);
        eventLog.publish(new Topic("payments"), new PaymentSuccessfulEvent(@ref));
    }

    void markFailed()
    {
        if (state != State.REQUESTED)
        {
            throw new IllegalStateException("Payment must be REQUESTED to mark failed");
        }

        state = State.FAILED;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("payments"), new PaymentFailedEvent(@ref));
    }

    public Payment identity()
    {
        return Payment.builder()
                .amount(Amount.IDENTITY)
                .eventLog(EventLog.IDENTITY)
                .paymentProcessor(PaymentProcessor.IDENTITY)
                .@ref(PaymentRef.IDENTITY)
                .build();
    }

    public BiFunction<Payment, PaymentEvent, Payment> accumulatorFunction(EventLog eventLog)
    {
        return new Accumulator(eventLog);
    }

    public PaymentState state()
    {
        return new PaymentState(state, amount, @ref);
    }

    public enum State
    {
        NEW, REQUESTED, SUCCESSFUL, FAILED
    }

    private class Accumulator : BiFunction<Payment, PaymentEvent, Payment>
    {

        private readonly EventLog instance;

        Accumulator(EventLog instance)
        {
            this.instance = instance;
        }

        public Payment apply(Payment payment, PaymentEvent paymentEvent)
        {
            if (paymentEvent instanceof PaymentAddedEvent) {
                PaymentAddedEvent pae = (PaymentAddedEvent)paymentEvent;
                PaymentState paymentState = pae.getPaymentState();
                return Payment.builder()
                        .amount(paymentState.getAmount())
                        .paymentProcessor(DummyPaymentProcessor.instance())
                        .@ref(paymentState.getRef())
                        .eventLog(instance)
                        .build();
            } else if (paymentEvent instanceof PaymentRequestedEvent) {
                payment.state = State.REQUESTED;
                return payment;
            } else if (paymentEvent instanceof PaymentSuccessfulEvent) {
                payment.state = State.SUCCESSFUL;
                return payment;
            } else if (typeof(paymentEvent) is PaymentFailedEvent)
            {
                payment.state = State.FAILED;
                return payment;
            }
            throw new IllegalStateException("Unknown PaymentEvent");
        }
    }


    class PaymentState : AggregateState
    {
        State state;
        Amount amount;
        PaymentRef @ref;
    }

}
