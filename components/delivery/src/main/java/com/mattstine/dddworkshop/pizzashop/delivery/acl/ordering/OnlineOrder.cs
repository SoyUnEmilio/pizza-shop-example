namespace com.mattstine.dddworkshop.pizzashop.delivery.acl.ordering
{
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
    using System.Collections.Generic;

    /**
     * @author Matt Stine
     */
    public class OnlineOrder
    {
        Type type;
        EventLog eventLog;
        OnlineOrderRef @ref;
        List<Pizza> pizzas;
        State state;

        private OnlineOrder(Type type, EventLog eventLog, OnlineOrderRef @ref)
        {
            this.type = type;
            this.eventLog = eventLog;
            this.@ref = @ref;
            this.pizzas = new List<Pizza>();
            this.state = State.NEW;
        }

        public bool IsNew()
        {
            return state == State.NEW;
        }

        enum State
        {
            NEW
        }

        public enum Type
        {
            DELIVERY
        }
    }
}