package com.mattstine.dddworkshop.pizzashop.kitchen;

import com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports.EventLog;
import com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports.Topic;
import com.mattstine.dddworkshop.pizzashop.infrastructure.repository.adapters.InProcessEventSourcedRepository;

class InProcessEventSourcedPizzaRepository extends InProcessEventSourcedRepository<PizzaRef, Pizza, Pizza.PizzaState, PizzaEvent, PizzaAddedEvent> implements PizzaRepository {
    InProcessEventSourcedPizzaRepository(EventLog eventLog,
                                         Topic pizzas) {
        super(eventLog, PizzaRef.class, Pizza.class, Pizza.PizzaState.class, PizzaAddedEvent.class, pizzas);
    }
}
