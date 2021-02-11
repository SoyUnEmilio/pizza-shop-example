namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.services.RefstringGenerator;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Ref;



public class KitchenOrderRef : Ref {
    public static KitchenOrderRef IDENTITY = new KitchenOrderRef("");
    private string reference;

    public KitchenOrderRef() {
        reference = RefstringGenerator.generateRefstring();
    }

    
    public KitchenOrderRef(string reference) {
        this.reference = reference;
    }

    
    public string getReference() {
        return reference;
    }
}
