namespace com.mattstine.dddworkshop.pizzashop.payments;



/**
 * @author Matt Stine
 */

class PaymentRequestedEvent : PaymentEvent {
    PaymentRef @ref;
}
