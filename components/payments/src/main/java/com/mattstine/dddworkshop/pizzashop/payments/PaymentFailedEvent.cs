namespace com.mattstine.dddworkshop.pizzashop.payments;

using lombok.Value;

/**
 * @author Matt Stine
 */

class PaymentFailedEvent : PaymentEvent {
    PaymentRef @ref;
}
