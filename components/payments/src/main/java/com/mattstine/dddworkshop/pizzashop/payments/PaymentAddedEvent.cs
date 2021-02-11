namespace com.mattstine.dddworkshop.pizzashop.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.RepositoryAddEvent;
using lombok.Value;

/**
 * @author Matt Stine
 */

class PaymentAddedEvent : PaymentEvent, RepositoryAddEvent {
    PaymentRef @ref;
    Payment.PaymentState paymentState;
}
