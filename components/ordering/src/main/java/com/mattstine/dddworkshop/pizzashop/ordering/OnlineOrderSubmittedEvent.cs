namespace com.mattstine.dddworkshop.pizzashop.ordering;



/**
 * @author Matt Stine
 */

class OnlineOrderSubmittedEvent : OnlineOrderEvent {
    OnlineOrderRef @ref;
}
