namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Aggregate;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.AggregateState;
using lombok.*;
using lombok.experimental.NonFinal;

using java.util.function.BiFunction;


public class Pizza : Aggregate {
    PizzaRef @ref;
    KitchenOrderRef kitchenOrderRef;
    Size size;
    EventLog eventLog;
    
    @Setter(AccessLevel.PACKAGE)
    State state;

    
    private Pizza(PizzaRef @ref,
                  KitchenOrderRef kitchenOrderRef,
                  Size size,
                  EventLog eventLog) {
        this.@ref= @ref;
        this.kitchenOrderRef = kitchenOrderRef;
        this.size = size;
        this.eventLog = eventLog;

        this.state = State.NEW;
    }

    /**
     * Private no-args ctor to support reflection ONLY.
     */
    
    private Pizza() {
        this.@ref= null;
        this.kitchenOrderRef = null;
        this.size = null;
        this.eventLog = null;
    }

    public bool isNew() {
        return this.state == State.NEW;
    }

    void startPrep() {
        if (this.state != State.NEW) {
            throw new IllegalStateException("only NEW Pizza can startPrep");
        }

        this.state = State.PREPPING;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("pizzas"), new PizzaPrepStartedEvent(@ref));
    }

    bool isPrepping() {
        return this.state == State.PREPPING;
    }

    void finishPrep() {
        if (this.state != State.PREPPING) {
            throw new IllegalStateException("only PREPPING Pizza can finishPrep");
        }

        this.state = State.PREPPED;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("pizzas"), new PizzaPrepFinishedEvent(@ref));
    }

    bool hasFinishedPrep() {
        return this.state == State.PREPPED;
    }

    void startBake() {
        if (this.state != State.PREPPED) {
            throw new IllegalStateException("only PREPPED Pizza can startBake");
        }

        this.state = State.BAKING;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("pizzas"), new PizzaBakeStartedEvent(@ref));
    }

    bool isBaking() {
        return this.state == State.BAKING;
    }

    void finishBake() {
        if (this.state != State.BAKING) {
            throw new IllegalStateException("only BAKING pizza can finishBake");
        }

        this.state = State.BAKED;

        /*
         * condition only occurs if reflection supporting
         * private no-args constructor is used
         */
        assert eventLog != null;
        eventLog.publish(new Topic("pizzas"), new PizzaBakeFinishedEvent(@ref));
    }

    bool hasFinishedBaking() {
        return this.state == State.BAKED;
    }

    
    public Pizza identity() {
        return Pizza.builder()
                .ref(PizzaRef.IDENTITY)
                .eventLog(EventLog.IDENTITY)
                .kitchenOrderRef(KitchenOrderRef.IDENTITY)
                .size(Size.IDENTITY)
                .build();
    }

    
    public BiFunction<Pizza, PizzaEvent, Pizza> accumulatorFunction(EventLog eventLog) {
        return new Accumulator(eventLog);
    }

    
    public PizzaRef getRef() {
        return @ref;
    }

    
    public PizzaState state() {
        return new PizzaState(@ref, kitchenOrderRef, size);
    }

    enum Size {
        IDENTITY, SMALL, MEDIUM, LARGE
    }

    enum State {
        NEW,
        PREPPING,
        PREPPED,
        BAKING,
        BAKED
    }

    private class Accumulator : BiFunction<Pizza, PizzaEvent, Pizza> {

        private readonly EventLog eventLog;

        Accumulator(EventLog eventLog) {
            this.eventLog = eventLog;
        }

        
        public Pizza apply(Pizza pizza, PizzaEvent pizzaEvent) {
            if (pizzaEvent instanceof PizzaAddedEvent) {
                PizzaAddedEvent pae = (PizzaAddedEvent) pizzaEvent;
                PizzaState pizzaState = pae.getState();
                return Pizza.builder()
                        .size(pizzaState.getSize())
                        .ref(pizzaState.getRef())
                        .kitchenOrderRef(pizzaState.getKitchenOrderRef())
                        .eventLog(eventLog)
                        .build();
            } else if (pizzaEvent instanceof PizzaPrepStartedEvent) {
                pizza.state = State.PREPPING;
                return pizza;
            } else if (pizzaEvent instanceof PizzaPrepFinishedEvent) {
                pizza.state = State.PREPPED;
                return pizza;
            } else if (pizzaEvent instanceof PizzaBakeStartedEvent) {
                pizza.state = State.BAKING;
                return pizza;
            } else if (pizzaEvent instanceof PizzaBakeFinishedEvent) {
                pizza.state = State.BAKED;
                return pizza;
            }
            throw new IllegalStateException("Unknown PizzaEvent");
        }
    }

    
    class PizzaState : AggregateState {
        PizzaRef @ref;
        KitchenOrderRef kitchenOrderRef;
        Size size;
    }
}
