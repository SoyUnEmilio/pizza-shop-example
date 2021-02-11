namespace com.mattstine.dddworkshop.pizzashop.delivery.acl.kitchen
{
    using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Ref;

    /**
     * @author Matt Stine
     */
    public class KitchenOrderRef : Ref
    {
        public static KitchenOrderRef IDENTITY = new KitchenOrderRef("");

        string reference;
    }
}

