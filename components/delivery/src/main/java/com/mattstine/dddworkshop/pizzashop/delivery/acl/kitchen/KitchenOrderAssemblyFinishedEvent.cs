namespace com.mattstine.dddworkshop.pizzashop.delivery.acl.kitchen
{
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;

    /**
     * @author Matt Stine
     */

    public class KitchenOrderAssemblyFinishedEvent : Event
    {
        KitchenOrderRef @ref;
    }
}