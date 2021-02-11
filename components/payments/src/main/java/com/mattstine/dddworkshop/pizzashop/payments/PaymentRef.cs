namespace com.mattstine.dddworkshop.pizzashop.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.services.RefstringGenerator;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Ref;
using lombok.Value;

/**
 * @author Matt Stine
 */

public class PaymentRef : Ref {
    public static PaymentRef IDENTITY = new PaymentRef("");
    string reference;

    public PaymentRef() {
        reference = RefstringGenerator.generateRefstring();
    }

    @SuppressWarnings("SameParameterValue")
    PaymentRef(string reference) {
        this.reference = reference;
    }
}
