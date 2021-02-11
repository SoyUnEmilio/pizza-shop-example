namespace com.mattstine.dddworkshop.pizzashop.payments;



/**
 * @author Matt Stine
 */

class PaymentSuccessfulEvent : PaymentEvent {
    PaymentRef @ref;
}
