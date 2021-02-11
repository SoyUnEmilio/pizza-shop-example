namespace com.mattstine.dddworkshop.pizzashop.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.ordering.acl.payments.PaymentRef;
using com.mattstine.dddworkshop.pizzashop.ordering.acl.payments.PaymentService;
using com.mattstine.dddworkshop.pizzashop.ordering.acl.payments.PaymentSuccessfulEvent;

/**
 * @author Matt Stine
 */
class DefaultOrderingService : OrderingService {
    private readonly EventLog eventLog;
    private readonly OnlineOrderRepository repository;
    private readonly PaymentService paymentService;

    DefaultOrderingService(EventLog eventLog, OnlineOrderRepository repository, PaymentService paymentService) {
        this.eventLog = eventLog;
        this.repository = repository;
        this.paymentService = paymentService;

        eventLog.subscribe(new Topic("payments"), e -> {
            if (e instanceof PaymentSuccessfulEvent) {
                PaymentSuccessfulEvent pse = (PaymentSuccessfulEvent) e;
                this.markOrderPaid(pse.getRef());
            }
        });
    }

    
    public OnlineOrderRef createOrder(OnlineOrder.Type type) {
        OnlineOrderRef ref = repository.nextIdentity();

        OnlineOrder order = OnlineOrder.builder().type(type)
                .eventLog(eventLog)
                .ref(@ref)
                .build();

        repository.add(order);

        return @ref;
    }

    
    public void addPizza(OnlineOrderRef @ref, Pizza pizza) {
        OnlineOrder onlineOrder = repository.findByRef(@ref);
        onlineOrder.addPizza(pizza);
    }

    
    public void requestPayment(OnlineOrderRef @ref) {
        PaymentRef paymentRef = paymentService.createPaymentOf(Amount.of(10, 0));
        paymentService.requestPaymentFor(paymentRef);
        OnlineOrder onlineOrder = repository.findByRef(@ref);
        onlineOrder.assignPaymentRef(paymentRef);
    }

    
    public OnlineOrder findByRef(OnlineOrderRef @ref) {
        return repository.findByRef(@ref);
    }

    private void markOrderPaid(PaymentRef paymentRef) {
        OnlineOrder onlineOrder = repository.findByPaymentRef(paymentRef);
        onlineOrder.markPaid();
    }
}
