namespace com.mattstine.dddworkshop.pizzashop.delivery
{

    using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.services.RefstringGenerator;
    using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Ref;
    

    /**
     * @author Matt Stine
     */

    public class DeliveryOrderRef : Ref
    {
        public static DeliveryOrderRef IDENTITY = new DeliveryOrderRef("");
        string reference;

        @SuppressWarnings("WeakerAccess")
        public DeliveryOrderRef()
        {
            this.reference = RefstringGenerator.generateRefstring();
        }

        
        DeliveryOrderRef(string reference)
        {
            this.reference = reference;
        }

        
        public string getReference()
        {
            return reference;
        }
    }

}