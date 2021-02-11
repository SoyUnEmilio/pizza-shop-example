namespace com.mattstine.dddworkshop.pizzashop.payments;

using lombok.Value;

/**
 * @author Matt Stine
 */

class PaymentRequestedEvent : PaymentEvent {
    PaymentRef @ref;
}
