namespace com.mattstine.dddworkshop.pizzashop.payments;

using lombok.Value;

/**
 * @author Matt Stine
 */

class PaymentSuccessfulEvent : PaymentEvent {
    PaymentRef @ref;
}
