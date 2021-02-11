namespace com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;
using lombok.Builder;
using lombok.NonNull;


/**
 * @author Matt Stine
 */

public class Pizza {

    Size size;

    
    
    private Pizza(Size size) {
        this.size = size;
    }

    public enum Size {
        MEDIUM(Amount.of(6, 0));

        final Amount price;

        Size(Amount price) {
            this.price = price;
        }
    }
}
