namespace com.mattstine.dddworkshop.pizzashop.delivery
{
    using com.mattstine.dddworkshop.pizzashop.delivery.acl.kitchen;
    using com.mattstine.dddworkshop.pizzashop.delivery.acl.ordering;
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
    using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports;
    using com.mattstine.dddworkshop.pizzashop.payments;
    using System;
    using System.Collections.Generic;

    /**
     * @author Matt Stine
     */

    public class DeliveryOrder : Aggregate
    {

        public enum State : int
        {
            READY_FOR_DELIVERY
        }

        DeliveryOrderRef @ref;
        KitchenOrderRef kitchenOrderRef;
        OnlineOrderRef onlineOrderRef;
        List<Pizza> pizzas;
        EventLog eventLog;

        DeliveryOrder.State state;

        private DeliveryOrder(DeliveryOrderRef @ref,
                              KitchenOrderRef kitchenOrderRef,
                              OnlineOrderRef onlineOrderRef,
                              List<Pizza> pizzas,
                              EventLog eventLog)
        {
            this.@ref = @ref;
            this.kitchenOrderRef = kitchenOrderRef;
            this.onlineOrderRef = onlineOrderRef;
            this.pizzas = pizzas;
            this.eventLog = eventLog;

            this.state = State.READY_FOR_DELIVERY;
        }

        /**
         * Private no-args ctor to support reflection ONLY.
         */
        private DeliveryOrder()
        {
            this.@ref = null;
            this.kitchenOrderRef = null;
            this.onlineOrderRef = null;
            this.pizzas = null;
            this.eventLog = null;
        }

        public DeliveryOrder identity()
        {
            return DeliveryOrder.builder()
                    .@ref(DeliveryOrderRef.IDENTITY)
                    .kitchenOrderRef(KitchenOrderRef.IDENTITY)
                    .onlineOrderRef(OnlineOrderRef.IDENTITY)
                    .eventLog(EventLog.IDENTITY)
                    .build();
        }

        public Func<DeliveryOrder, DeliveryOrderEvent, DeliveryOrder> accumulatorFunction(EventLog eventLog)
        {
            return new Accumulator(eventLog);
        }

        public OrderState GetState()
        {
            return new OrderState(@ref, kitchenOrderRef, onlineOrderRef, pizzas);
        }

        public bool isReadyForDelivery()
        {
            return this.state == State.READY_FOR_DELIVERY;
        }


        private class Accumulator : Func<DeliveryOrder, DeliveryOrderEvent, DeliveryOrder>
        {

            private readonly EventLog eventLog;

            Accumulator(EventLog eventLog)
            {
                this.eventLog = eventLog;
            }

            public DeliveryOrder apply(DeliveryOrder deliveryOrder, DeliveryOrderEvent deliveryOrderEvent)
            {
                if (deliveryOrderEvent.GetType() == typeof(DeliveryOrderAddedEvent))
                {
                    DeliveryOrderAddedEvent doae = (DeliveryOrderAddedEvent)deliveryOrderEvent;
                    OrderState orderState = doae.State;
                    return DeliveryOrder.builder()
                            .eventLog(eventLog)
                            .@ref(orderState.getRef())
                        .kitchenOrderRef(orderState.getKitchenOrderRef())
                        .onlineOrderRef(orderState.getOnlineOrderRef())
                        .pizzas(orderState.getPizzas())
                        .build();
                }
                throw new IllegalStateException("Unknown DeliveryOrderEvent");
            }
        }

        /*
         * Pizza Value Object for KitchenOrder Details Only
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

        public class OrderState : AggregateState
        {
            DeliveryOrderRef @ref;
            KitchenOrderRef kitchenOrderRef;
            OnlineOrderRef onlineOrderRef;
            List<Pizza> pizzas;

            public OrderState(DeliveryOrderRef @ref, KitchenOrderRef kitchenOrderRef, OnlineOrderRef onlineOrderRef, List<Pizza> pizzas)
            {
                this.@ref = @ref;
                this.kitchenOrderRef = kitchenOrderRef;
                this.onlineOrderRef = onlineOrderRef;
                this.pizzas = pizzas;
            }
        }
    }
}
