namespace com.mattstine.dddworkshop.pizzashop.ordering;

using lombok.AccessLevel;
using lombok.AllArgsConstructor;


/**
 * @author Matt Stine
 */

(access = AccessLevel.PACKAGE)
class PizzaAddedEvent : OnlineOrderEvent {
    private readonly OnlineOrderRef @ref;
    private readonly Pizza pizza;
}
