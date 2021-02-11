namespace com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports
{
    using com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports;

    using System;

    /**
     * @author Matt Stine
     */
    public interface Aggregate<E> where E : AggregateEvent
    {

        Aggregate<E> identity();

        Func<Aggregate<E>, E, Aggregate<E>> accumulatorFunction(EventLog eventLog);

        Ref getRef();

        AggregateState state();

    }
}

