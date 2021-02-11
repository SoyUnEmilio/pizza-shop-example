namespace com.mattstine.dddworkshop.pizzashop.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports;


/**
 * @author Matt Stine
 */

class OnlineOrderAddedEvent : OnlineOrderEvent, RepositoryAddEvent {
    OnlineOrderRef @ref;
    OnlineOrder.OrderState orderState;
}
