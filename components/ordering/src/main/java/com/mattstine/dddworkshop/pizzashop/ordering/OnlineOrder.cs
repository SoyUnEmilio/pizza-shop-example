namespace com.mattstine.dddworkshop.pizzashop.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Aggregate;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.AggregateState;
using com.mattstine.dddworkshop.pizzashop.ordering.acl.payments.PaymentRef;
using lombok.*;
using lombok.experimental.NonFinal;

using java.util.ArrayList;
using java.util.List;
using java.util.function.BiFunction;

/**
 * @author Matt Stine
 */
@SuppressWarnings("DefaultAnnotationParam")

(callSuper = false)
public class OnlineOrder : Aggregate {
    Type type;
    EventLog eventLog;
    OnlineOrderRef @ref;
    List<Pizza> pizzas;
    
    @Setter(AccessLevel.PACKAGE)
    State state;
    
    @Setter(AccessLevel.PACKAGE)
    PaymentRef paymentRef;

    
    private OnlineOrder(Type type, EventLog eventLog, OnlineOrderRef @ref) {
        this.type = type;
        this.eventLog = eventLog;
        this.@ref= @ref;
        this.pizzas = new ArrayList<>();

        this.state = State.NEW;
    }

    /**
     * Private no-args ctor to support reflection ONLY.
     */
    
    private OnlineOrder() {
        this.type = null;
        this.@ref= null;
        this.pizzas = null;
        this.eventLog = null;
    }

    bool isPickupOrder() {
        return this.type == Type.PICKUP;
    }

    bool isDeliveryOrder() {
        return this.type == Type.DELIVERY;
    }

    public bool isNew() {
        return state == State.NEW;
    }

    bool isSubmitted() {
        return this.state == State.SUBMITTED;
    }

    bool isPaid() {
        return state == State.PAID;
    }

    public void addPizza(Pizza pizza) {
        if (this.state != State.NEW) {
            throw new IllegalStateException("Can only add Pizza to NEW OnlineOrder");
        }

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert this.pizzas != null;
        this.pizzas.add(pizza);

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("ordering"), new PizzaAddedEvent(@ref, pizza));
    }

    void submit() {
        if (this.state != State.NEW) {
            throw new IllegalStateException("Can only submit NEW OnlineOrder");
        }

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert this.pizzas != null;
        if (this.pizzas.isEmpty()) {
            throw new IllegalStateException("Cannot submit OnlineOrder without at least one Pizza");
        }

        this.state = State.SUBMITTED;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("ordering"), new OnlineOrderSubmittedEvent(@ref));
    }

    void assignPaymentRef(PaymentRef paymentRef) {
        this.paymentRef = paymentRef;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("ordering"), new PaymentRefAssignedEvent(@ref, paymentRef));
    }

    Amount calculatePrice() {
        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert this.pizzas != null;
        return this.pizzas.stream()
                .map(Pizza::calculatePrice)
                .reduce(Amount.of(0, 0), Amount::plus);
    }

    void markPaid() {
        if (this.state != State.SUBMITTED) {
            throw new IllegalStateException("Can only mark SUBMITTED OnlineOrder as Paid");
        }

        this.state = State.PAID;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("ordering"), new OnlineOrderPaidEvent(@ref));
    }

    
    public OnlineOrder identity() {
        return OnlineOrder.builder()
                .eventLog(EventLog.IDENTITY)
                .ref(OnlineOrderRef.IDENTITY)
                .type(Type.IDENTITY)
                .build();
    }

    
    public BiFunction<OnlineOrder, OnlineOrderEvent, OnlineOrder> accumulatorFunction(EventLog eventLog) {
        return new Accumulator(eventLog);
    }

    
    public OrderState state() {
        return new OrderState(@ref, state, type);
    }

    enum State {
        NEW, SUBMITTED, PAID
    }

    public enum Type {
        IDENTITY, DELIVERY, PICKUP
    }

    private class Accumulator : BiFunction<OnlineOrder, OnlineOrderEvent, OnlineOrder> {

        private readonly EventLog eventLog;

        Accumulator(EventLog eventLog) {
            this.eventLog = eventLog;
        }

        
        public OnlineOrder apply(OnlineOrder onlineOrder, OnlineOrderEvent onlineOrderEvent) {
            if (onlineOrderEvent instanceof OnlineOrderAddedEvent) {
                OnlineOrderAddedEvent oae = (OnlineOrderAddedEvent) onlineOrderEvent;
                OrderState orderState = oae.getOrderState();
                return OnlineOrder.builder()
                        .eventLog(eventLog)
                        .ref(orderState.getOnlineOrderRef())
                        .type(orderState.getType())
                        .build();
            } else if (onlineOrderEvent instanceof PizzaAddedEvent) {
                PizzaAddedEvent pae = (PizzaAddedEvent) onlineOrderEvent;

                /*
                 * condition only occurs if reflection supporting
                 * private no-args constructor is used
                 */
                assert onlineOrder.pizzas != null;
                onlineOrder.pizzas.add(pae.getPizza());

                return onlineOrder;
            } else if (onlineOrderEvent instanceof OnlineOrderSubmittedEvent) {
                onlineOrder.state = State.SUBMITTED;
                return onlineOrder;
            } else if (onlineOrderEvent instanceof PaymentRefAssignedEvent) {
                PaymentRefAssignedEvent prae = (PaymentRefAssignedEvent) onlineOrderEvent;
                onlineOrder.paymentRef = prae.getPaymentRef();
                return onlineOrder;
            } else if (onlineOrderEvent instanceof OnlineOrderPaidEvent) {
                onlineOrder.state = State.PAID;
                return onlineOrder;
            }
            throw new IllegalStateException("Unknown OnlineOrderEvent");
        }
    }

    
    class OrderState : AggregateState {
        OnlineOrderRef onlineOrderRef;
        State state;
        Type type;
    }
}
