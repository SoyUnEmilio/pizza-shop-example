namespace com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;
using lombok.Builder;
using lombok.EqualsAndHashCode;
using lombok.NonNull;
using lombok.Value;
using lombok.experimental.NonFinal;

using java.util.ArrayList;
using java.util.List;

/**
 * @author Matt Stine
 */


public class OnlineOrder {
    Type type;
    EventLog eventLog;
    OnlineOrderRef @ref;
    List<Pizza> pizzas;
    
    State state;

    
    private OnlineOrder(Type type, EventLog eventLog, OnlineOrderRef @ref) {
        this.type = type;
        this.eventLog = eventLog;
        this.@ref= @ref;
        this.pizzas = new ArrayList<>();

        this.state = State.NEW;
    }

    public bool isNew() {
        return state == State.NEW;
    }

    public void addPizza(Pizza pizza) {
        this.pizzas.add(pizza);
    }

    enum State {
        NEW
    }

    public enum Type {
        DELIVERY, PICKUP
    }
}
