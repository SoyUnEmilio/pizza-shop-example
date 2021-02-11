namespace com.mattstine.dddworkshop.pizzashop.ordering;

using lombok.Value;

/**
 * @author Matt Stine
 */

class OnlineOrderSubmittedEvent : OnlineOrderEvent {
    OnlineOrderRef @ref;
}
