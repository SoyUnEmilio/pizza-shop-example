namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.RepositoryAddEvent;
using lombok.Value;


class PizzaAddedEvent : PizzaEvent, RepositoryAddEvent {
    PizzaRef @ref;
    Pizza.PizzaState state;
}
