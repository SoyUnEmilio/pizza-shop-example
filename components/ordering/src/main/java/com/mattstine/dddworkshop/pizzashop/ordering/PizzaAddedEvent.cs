namespace com.mattstine.dddworkshop.pizzashop.ordering;

using lombok.AccessLevel;
using lombok.AllArgsConstructor;
using lombok.Value;

/**
 * @author Matt Stine
 */

(access = AccessLevel.PACKAGE)
class PizzaAddedEvent : OnlineOrderEvent {
    private readonly OnlineOrderRef @ref;
    private readonly Pizza pizza;
}
