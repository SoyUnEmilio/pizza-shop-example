namespace com.mattstine.dddworkshop.pizzashop.payments;



/**
 * @author Matt Stine
 */

class PaymentFailedEvent : PaymentEvent {
    PaymentRef @ref;
}
