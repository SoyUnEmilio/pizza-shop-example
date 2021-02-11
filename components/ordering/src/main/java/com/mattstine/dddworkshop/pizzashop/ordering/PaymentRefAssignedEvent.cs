namespace com.mattstine.dddworkshop.pizzashop.ordering;

using com.mattstine.dddworkshop.pizzashop.ordering.acl.payments.PaymentRef;


/**
 * @author Matt Stine
 */

class PaymentRefAssignedEvent : OnlineOrderEvent {
    OnlineOrderRef @ref;
    PaymentRef paymentRef;
}
