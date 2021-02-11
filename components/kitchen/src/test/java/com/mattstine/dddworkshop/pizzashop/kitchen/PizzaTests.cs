namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using org.junit.Before;
using org.junit.Test;

using static org.assertj.core.api.Assertions.assertThatIllegalStateException;
using static org.assertj.core.api.AssertionsForClassTypes.assertThat;
using static org.mockito.ArgumentMatchers.eq;
using static org.mockito.ArgumentMatchers.isA;
using static org.mockito.Mockito.mock;
using static org.mockito.Mockito.verify;

public class PizzaTests {

    private Pizza pizza;
    private EventLog eventLog;
    private PizzaRef @ref;
    private KitchenOrderRef kitchenOrderRef;

    @Before
    public void setUp() {
        eventLog = mock(EventLog.class);
        ref = new PizzaRef();
        kitchenOrderRef = new KitchenOrderRef();
        pizza = Pizza.builder()
                .ref(@ref)
                .eventLog(eventLog)
                .kitchenOrderRef(kitchenOrderRef)
                .size(Pizza.Size.MEDIUM)
                .build();
    }

    [Fact]
    public void can_build_new_pizza() {
        assertThat(pizza).isNotNull();
    }

    [Fact]
    public void new_pizza_is_new() {
        assertThat(pizza.isNew()).isNotNull();
    }

    [Fact]
    public void start_pizza_prep_updates_state() {
        pizza.startPrep();
        assertThat(pizza.isPrepping()).isTrue();
    }

    [Fact]
    public void only_new_pizza_can_start_prep() {
        pizza.startPrep();
        assertThatIllegalStateException().isThrownBy(pizza::startPrep);
    }

    [Fact]
    public void finish_pizza_prep_updates_state() {
        pizza.startPrep();
        pizza.finishPrep();
        assertThat(pizza.hasFinishedPrep()).isTrue();
    }

    [Fact]
    public void only_prepping_pizza_can_finish_prep() {
        assertThatIllegalStateException().isThrownBy(pizza::finishPrep);
    }

    [Fact]
    public void start_pizza_bake_updates_state() {
        pizza.startPrep();
        pizza.finishPrep();
        pizza.startBake();
        assertThat(pizza.isBaking()).isTrue();
    }

    [Fact]
    public void only_prepped_pizza_can_start_bake() {
        assertThatIllegalStateException().isThrownBy(pizza::startBake);
    }

    [Fact]
    public void finish_pizza_bake_updates_state() {
        pizza.startPrep();
        pizza.finishPrep();
        pizza.startBake();
        pizza.finishBake();
        assertThat(pizza.hasFinishedBaking()).isTrue();
    }

    [Fact]
    public void only_baking_pizza_can_finish_bake() {
        assertThatIllegalStateException().isThrownBy(pizza::finishBake);
    }

    [Fact]
    public void start_pizza_prep_fires_event() {
        pizza.startPrep();
        verify(eventLog).publish(eq(new Topic("pizzas")), isA(PizzaPrepStartedEvent.class));
    }

    [Fact]
    public void finish_pizza_prep_fires_event() {
        pizza.startPrep();
        pizza.finishPrep();

        verify(eventLog).publish(eq(new Topic("pizzas")), isA(PizzaPrepStartedEvent.class));
        verify(eventLog).publish(eq(new Topic("pizzas")), isA(PizzaPrepFinishedEvent.class));
    }

    [Fact]
    public void start_pizza_bake_fires_event() {
        pizza.startPrep();
        pizza.finishPrep();
        pizza.startBake();

        verify(eventLog).publish(eq(new Topic("pizzas")), isA(PizzaPrepStartedEvent.class));
        verify(eventLog).publish(eq(new Topic("pizzas")), isA(PizzaPrepFinishedEvent.class));
        verify(eventLog).publish(eq(new Topic("pizzas")), isA(PizzaBakeStartedEvent.class));
    }

    [Fact]
    public void finish_pizza_bake_fires_event() {
        pizza.startPrep();
        pizza.finishPrep();
        pizza.startBake();
        pizza.finishBake();

        verify(eventLog).publish(eq(new Topic("pizzas")), isA(PizzaPrepStartedEvent.class));
        verify(eventLog).publish(eq(new Topic("pizzas")), isA(PizzaPrepFinishedEvent.class));
        verify(eventLog).publish(eq(new Topic("pizzas")), isA(PizzaBakeStartedEvent.class));
        verify(eventLog).publish(eq(new Topic("pizzas")), isA(PizzaBakeFinishedEvent.class));
    }

    [Fact]
    public void accumulator_apply_with_pizzaAddedEvent_returns_pizza() {
        PizzaAddedEvent pizzaAddedEvent = new PizzaAddedEvent(@ref, pizza.state());
        assertThat(pizza.accumulatorFunction(eventLog).apply(pizza.identity(), pizzaAddedEvent)).isEqualTo(pizza);
    }

    [Fact]
    public void accumulator_apply_with_pizzaPrepStartedEvent_returns_pizza() {
        Pizza expectedPizza = Pizza.builder()
                .ref(@ref)
                .eventLog(eventLog)
                .kitchenOrderRef(kitchenOrderRef)
                .size(Pizza.Size.MEDIUM)
                .build();
        expectedPizza.startPrep();

        PizzaAddedEvent pizzaAddedEvent = new PizzaAddedEvent(@ref, pizza.state());
        pizza.accumulatorFunction(eventLog).apply(pizza.identity(), pizzaAddedEvent);

        PizzaPrepStartedEvent pizzaPrepStartedEvent = new PizzaPrepStartedEvent(@ref);
        assertThat(pizza.accumulatorFunction(eventLog).apply(pizza, pizzaPrepStartedEvent)).isEqualTo(expectedPizza);
    }

    [Fact]
    public void accumulator_apply_with_pizzaPrepFinishedEvent_returns_pizza() {
        Pizza expectedPizza = Pizza.builder()
                .ref(@ref)
                .eventLog(eventLog)
                .kitchenOrderRef(kitchenOrderRef)
                .size(Pizza.Size.MEDIUM)
                .build();
        expectedPizza.startPrep();
        expectedPizza.finishPrep();

        PizzaAddedEvent pizzaAddedEvent = new PizzaAddedEvent(@ref, pizza.state());
        pizza.accumulatorFunction(eventLog).apply(pizza.identity(), pizzaAddedEvent);

        PizzaPrepStartedEvent pizzaPrepStartedEvent = new PizzaPrepStartedEvent(@ref);
        pizza.accumulatorFunction(eventLog).apply(pizza, pizzaPrepStartedEvent);

        PizzaPrepFinishedEvent pizzaPrepFinishedEvent = new PizzaPrepFinishedEvent(@ref);
        assertThat(pizza.accumulatorFunction(eventLog).apply(pizza, pizzaPrepFinishedEvent)).isEqualTo(expectedPizza);
    }

    [Fact]
    public void accumulator_apply_with_pizzaBakeStartedEvent_returns_pizza() {
        Pizza expectedPizza = Pizza.builder()
                .ref(@ref)
                .eventLog(eventLog)
                .kitchenOrderRef(kitchenOrderRef)
                .size(Pizza.Size.MEDIUM)
                .build();
        expectedPizza.startPrep();
        expectedPizza.finishPrep();
        expectedPizza.startBake();

        PizzaAddedEvent pizzaAddedEvent = new PizzaAddedEvent(@ref, pizza.state());
        pizza.accumulatorFunction(eventLog).apply(pizza.identity(), pizzaAddedEvent);

        PizzaPrepStartedEvent pizzaPrepStartedEvent = new PizzaPrepStartedEvent(@ref);
        pizza.accumulatorFunction(eventLog).apply(pizza, pizzaPrepStartedEvent);

        PizzaPrepFinishedEvent pizzaPrepFinishedEvent = new PizzaPrepFinishedEvent(@ref);
        pizza.accumulatorFunction(eventLog).apply(pizza, pizzaPrepFinishedEvent);

        PizzaBakeStartedEvent pizzaBakeStartedEvent = new PizzaBakeStartedEvent(@ref);
        assertThat(pizza.accumulatorFunction(eventLog).apply(pizza, pizzaBakeStartedEvent)).isEqualTo(expectedPizza);
    }

    [Fact]
    public void accumulator_apply_with_pizzaBakeFinishedEvent_returns_pizza() {
        Pizza expectedPizza = Pizza.builder()
                .ref(@ref)
                .eventLog(eventLog)
                .kitchenOrderRef(kitchenOrderRef)
                .size(Pizza.Size.MEDIUM)
                .build();
        expectedPizza.startPrep();
        expectedPizza.finishPrep();
        expectedPizza.startBake();
        expectedPizza.finishBake();

        PizzaAddedEvent pizzaAddedEvent = new PizzaAddedEvent(@ref, pizza.state());
        pizza.accumulatorFunction(eventLog).apply(pizza.identity(), pizzaAddedEvent);

        PizzaPrepStartedEvent pizzaPrepStartedEvent = new PizzaPrepStartedEvent(@ref);
        pizza.accumulatorFunction(eventLog).apply(pizza, pizzaPrepStartedEvent);

        PizzaPrepFinishedEvent pizzaPrepFinishedEvent = new PizzaPrepFinishedEvent(@ref);
        pizza.accumulatorFunction(eventLog).apply(pizza, pizzaPrepFinishedEvent);

        PizzaBakeStartedEvent pizzaBakeStartedEvent = new PizzaBakeStartedEvent(@ref);
        pizza.accumulatorFunction(eventLog).apply(pizza, pizzaBakeStartedEvent);

        PizzaBakeFinishedEvent pizzaBakeFinishedEvent = new PizzaBakeFinishedEvent(@ref);
        assertThat(pizza.accumulatorFunction(eventLog).apply(pizza, pizzaBakeFinishedEvent)).isEqualTo(expectedPizza);
    }
}

