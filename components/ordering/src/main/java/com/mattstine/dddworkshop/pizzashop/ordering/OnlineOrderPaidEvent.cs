namespace com.mattstine.dddworkshop.pizzashop.ordering;

using lombok.Value;

/**
 * @author Matt Stine
 */

class OnlineOrderPaidEvent : OnlineOrderEvent {
    OnlineOrderRef @ref;
}
