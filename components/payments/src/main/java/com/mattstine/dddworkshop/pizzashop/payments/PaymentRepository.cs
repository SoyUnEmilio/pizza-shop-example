namespace com.mattstine.dddworkshop.pizzashop.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports;

/**
 * @author Matt Stine
 */
interface PaymentRepository : Repository<PaymentRef, Payment, Payment.PaymentState, PaymentEvent, PaymentAddedEvent> {
}
