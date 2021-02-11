namespace com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports.Event;
using lombok.Value;
using lombok.experimental.NonFinal;

/**
 * @author Matt Stine
 */

public class OnlineOrderPaidEvent : Event {
    
    OnlineOrderRef @ref;
}
