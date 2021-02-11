namespace com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports
{
    public interface Repository<K, T, S, U, V>
        where K : Ref
        where T : Aggregate<T>
        where S : AggregateState
        where U : AggregateEvent
        where V : RepositoryAddEvent

    {
        K nextIdentity();

        void add(T aggregateInstance);

        T findByRef(K @ref);
    }
}