namespace com.mattstine.dddworkshop.pizzashop.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;
using lombok.Builder;
using lombok.NonNull;
using lombok.Value;

/**
 * @author Matt Stine
 */

public class Pizza {

    Size size;

    @SuppressWarnings("unused")
    
    private Pizza(Size size) {
        this.size = size;
    }

    Amount calculatePrice() {
        return size.getPrice();
    }

    public enum Size {
        MEDIUM(Amount.of(6, 0)),
        LARGE(Amount.of(8, 9));

        final Amount price;

        Size(Amount price) {
            this.price = price;
        }

        public Amount getPrice() {
            return price;
        }
    }
}
