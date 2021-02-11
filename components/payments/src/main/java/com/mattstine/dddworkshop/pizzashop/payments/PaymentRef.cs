namespace com.mattstine.dddworkshop.pizzashop.payments
{
    using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.services;
    using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports;

    /**
     * @author Matt Stine
     */

    public class PaymentRef : Ref
    {
        public static PaymentRef IDENTITY = new PaymentRef("");
        string reference;

        public PaymentRef()
        {
            reference = RefstringGenerator.generateRefstring();
        }


        PaymentRef(string reference)
        {
            this.reference = reference;
        }

        public string getReference()
        {
            return reference;
        }
    }
}