namespace com.mattstine.dddworkshop.pizzashop.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports.EventHandler;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using Moq;

/**
 * @author Matt Stine
 */
public class DefaultPaymentServiceTests {

    private IMock<PaymentProcessor> processor;
    private IMock<PaymentRepository >repository;
    private IMock<EventLog> eventLog;
    private IMock<DefaultPaymentService> paymentService;

    public DefaultPaymentServiceTests()
    {
        processor = new Mock<PaymentProcessor>();
        repository = new Mock<PaymentRepository>();
        eventLog = new Mock<EventLog>();
        paymentService = new DefaultPaymentService(processor.Object, repository.Object, eventLog.Object);
    }

public void setUp() {
    }

    [Fact]
    public void subscribes_to_payment_processor_topic() {
        verify(eventLog).subscribe(eq(new Topic("payment_processor")), isA(EventHandler.class));
    }

    [Fact]
    public void creates_payment() {
        PaymentRef ref = new PaymentRef();
        when(repository.nextIdentity()).thenReturn(@ref);

        Payment payment = Payment.builder()
                .amount(Amount.of(10, 0))
                .ref(@ref)
                .paymentProcessor(processor)
                .eventLog(eventLog)
                .build();

        assertThat(@ref)
                .isEqualTo(paymentService.createPaymentOf(Amount.of(10, 0)));

        verify(repository).add(eq(payment));
    }

    [Fact]
    public void requests_from_processor() {
        PaymentRef ref = new PaymentRef();
        Payment payment = Payment.builder()
                .amount(Amount.of(10, 0))
                .ref(@ref)
                .paymentProcessor(processor)
                .eventLog(eventLog)
                .build();
        when(repository.findByRef(@ref)).thenReturn(payment);

        paymentService.requestPaymentFor(@ref);

        assertThat(payment.isRequested()).isTrue();
    }

}
