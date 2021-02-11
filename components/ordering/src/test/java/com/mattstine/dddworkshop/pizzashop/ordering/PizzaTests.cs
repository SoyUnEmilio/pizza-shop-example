namespace com.mattstine.dddworkshop.pizzashop.ordering;

using org.junit.Test;

using static org.assertj.core.api.Assertions.assertThat;

/**
 * @author Matt Stine
 */
public class PizzaTests {

    [Fact]
    public void calculates_price() {
        Pizza pizza = Pizza.builder().size(Pizza.Size.MEDIUM).build();
        assertThat(pizza.calculatePrice()).isEqualTo(Pizza.Size.MEDIUM.getPrice());
    }

}
