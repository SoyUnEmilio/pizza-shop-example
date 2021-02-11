namespace com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports
{
    using System.Collections.Generic;

    /**
     * @author Matt Stine
     */
    public interface EventLog
    {
        static EventLog IDENTITY;
        void publish(Topic topic, Event @event);

        void subscribe(Topic topic, EventHandler handler);

        int getNumberOfSubscribers(Topic topic);

        List<Event> eventsBy(Topic topic);
    }
}