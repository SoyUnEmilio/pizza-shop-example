namespace com.mattstine.dddworkshop.pizzashop.ordering.acl.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;

/**
 * @author Matt Stine
 */
public interface PaymentService {
    PaymentRef createPaymentOf(Amount of);

    void requestPaymentFor(PaymentRef @ref);
}
