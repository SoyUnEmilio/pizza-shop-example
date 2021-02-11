namespace com.mattstine.dddworkshop.pizzashop.payments
{
    using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports;

    /**
     * @author Matt Stine
     */
    public interface PaymentEvent : AggregateEvent
    {
        PaymentRef getRef();
    }
}