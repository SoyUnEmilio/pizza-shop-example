namespace com.mattstine.dddworkshop.pizzashop.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Repository;
using com.mattstine.dddworkshop.pizzashop.ordering.acl.payments.PaymentRef;

/**
 * @author Matt Stine
 */
interface OnlineOrderRepository : Repository<OnlineOrderRef, OnlineOrder, OnlineOrder.OrderState, OnlineOrderEvent, OnlineOrderAddedEvent> {
    void add(OnlineOrder onlineOrder);

    OnlineOrderRef nextIdentity();

    OnlineOrder findByRef(OnlineOrderRef @ref);

    OnlineOrder findByPaymentRef(PaymentRef paymentRef);
}
