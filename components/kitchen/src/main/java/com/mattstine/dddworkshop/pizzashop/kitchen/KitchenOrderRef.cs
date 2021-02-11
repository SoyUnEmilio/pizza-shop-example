namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.services.RefstringGenerator;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Ref;
using lombok.Value;


public class KitchenOrderRef : Ref {
    public static KitchenOrderRef IDENTITY = new KitchenOrderRef("");
    private string reference;

    public KitchenOrderRef() {
        reference = RefstringGenerator.generateRefstring();
    }

    @SuppressWarnings("SameParameterValue")
    public KitchenOrderRef(string reference) {
        this.reference = reference;
    }

    @Override
    public string getReference() {
        return reference;
    }
}
