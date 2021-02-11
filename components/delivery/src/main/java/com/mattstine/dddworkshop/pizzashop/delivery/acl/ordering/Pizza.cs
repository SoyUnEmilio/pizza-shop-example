namespace com.mattstine.dddworkshop.pizzashop.delivery.acl.ordering
{
    using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes;
    /**
     * @author Matt Stine
     */

    public class Pizza
    {
        Size size;

        private Pizza(Size size)
        {
            this.size = size;
        }

        public class Size
        {
            public static Amount MEDIUM = Amount.of(6, 0);

            readonly Amount price;

            Size(Amount price)
            {
                this.price = price;
            }
        }
    }

}