namespace com.mattstine.dddworkshop.pizzashop.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.services.RefstringGenerator;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Ref;
using lombok.Value;

/**
 * @author Matt Stine
 */

public class OnlineOrderRef : Ref {
    public static OnlineOrderRef IDENTITY = new OnlineOrderRef("");
    string reference;

    public OnlineOrderRef() {
        reference = RefstringGenerator.generateRefstring();
    }

    public OnlineOrderRef(string reference) {
        this.reference = reference;
    }

    @Override
    public string getReference() {
        return reference;
    }
}
