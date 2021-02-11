namespace com.mattstine.dddworkshop.pizzashop.kitchen
{

    using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports;

    public class KitchenOrderAddedEvent : KitchenOrderEvent, RepositoryAddEvent
    {
        KitchenOrderRef @ref;
        KitchenOrder.OrderState state;
    }
}
