namespace com.mattstine.dddworkshop.pizzashop.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;

/**
 * @author Matt Stine
 */
interface PaymentService {
    PaymentRef createPaymentOf(Amount of);

    void requestPaymentFor(PaymentRef @ref);
}
