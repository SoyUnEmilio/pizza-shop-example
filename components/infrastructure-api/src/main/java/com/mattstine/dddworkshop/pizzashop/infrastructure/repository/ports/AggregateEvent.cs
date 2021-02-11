namespace com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports
{
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;

    /**
     * @author Matt Stine
     */
    public interface AggregateEvent : Event
    {
        Ref getRef();
    }
}
