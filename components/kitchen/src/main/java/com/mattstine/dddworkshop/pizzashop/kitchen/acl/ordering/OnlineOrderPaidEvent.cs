namespace com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering
{
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;

    /**
     * @author Matt Stine
     */

    public class OnlineOrderPaidEvent : Event
    {

        OnlineOrderRef @ref;
    }
}

