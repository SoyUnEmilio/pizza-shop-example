namespace com.mattstine.dddworkshop.pizzashop.delivery.acl.kitchen
{
    using com.mattstine.dddworkshop.pizzashop.delivery.acl.ordering.OnlineOrderRef;
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
    using System.Collections.Generic;

    /**
     * @author Matt Stine
     */
    public class KitchenOrder
    {
        KitchenOrderRef @ref;
        OnlineOrderRef onlineOrderRef;
        List<Pizza> pizzas;
        EventLog eventLog;
        State state;

        private KitchenOrder(KitchenOrderRef @ref, OnlineOrderRef onlineOrderRef, List<Pizza> pizzas, EventLog eventLog)
        {
            this.@ref = @ref;
            this.onlineOrderRef = onlineOrderRef;
            this.pizzas = pizzas;
            this.eventLog = eventLog;

            this.state = State.NEW;
        }

        enum State
        {
            NEW
        }

        /*
         * Pizza Value Object for OnlineOrder Details Only
         */

        public class Pizza
        {
            Size size;


            private Pizza(Size size)
            {
                this.size = size;
            }

            public enum Size
            {
                SMALL, MEDIUM, LARGE
            }
        }
    }
}