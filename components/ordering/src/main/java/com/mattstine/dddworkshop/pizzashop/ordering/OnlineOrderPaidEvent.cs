namespace com.mattstine.dddworkshop.pizzashop.ordering;



/**
 * @author Matt Stine
 */

class OnlineOrderPaidEvent : OnlineOrderEvent {
    OnlineOrderRef @ref;
}
