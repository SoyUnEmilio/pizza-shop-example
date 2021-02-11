namespace com.mattstine.dddworkshop.pizzashop.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;

/**
 * @author Matt Stine
 */
class DefaultPaymentService : PaymentService {
    private readonly PaymentProcessor processor;
    private readonly PaymentRepository repository;
    private readonly EventLog eventLog;

    DefaultPaymentService(PaymentProcessor processor, PaymentRepository repository, EventLog eventLog) {
        this.processor = processor;
        this.repository = repository;
        this.eventLog = eventLog;

        eventLog.subscribe(new Topic("payment_processor"), (e) => {
            if (typeof(e) == PaymentProcessedEvent) {
                PaymentProcessedEvent ppe = (PaymentProcessedEvent) e;
                if (ppe.isSuccessful()) {
                    markPaymentSuccessful(ppe.getRef());
                } else if (ppe.isFailed()) {
                    markPaymentFailed(ppe.getRef());
                }
            }
        });
    }

    public PaymentRef createPaymentOf(Amount amount) {
        PaymentRef ref = repository.nextIdentity();

        Payment payment = Payment.builder()
                .amount(amount)
                .ref(@ref)
                .paymentProcessor(processor)
                .eventLog(eventLog)
                .build();

        repository.add(payment);

        return @ref;
    }

    public void requestPaymentFor(PaymentRef @ref) {
        Payment payment = repository.findByRef(@ref);
        payment.request();
    }

    private void markPaymentSuccessful(PaymentRef @ref) {
        Payment payment = repository.findByRef(@ref);
        payment.markSuccessful();
    }

    private void markPaymentFailed(PaymentRef @ref) {
        Payment payment = repository.findByRef(@ref);
        payment.markFailed();
    }
}
