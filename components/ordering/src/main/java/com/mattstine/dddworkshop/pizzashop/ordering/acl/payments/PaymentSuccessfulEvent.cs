namespace com.mattstine.dddworkshop.pizzashop.ordering.acl.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;

using lombok.experimental.NonFinal;

/**
 * @author Matt Stine
 */

public class PaymentSuccessfulEvent : Event {
    
    PaymentRef @ref;
}
