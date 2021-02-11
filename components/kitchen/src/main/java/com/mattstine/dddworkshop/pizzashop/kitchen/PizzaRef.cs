namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.services.RefstringGenerator;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Ref;



public class PizzaRef : Ref {
    public static PizzaRef IDENTITY = new PizzaRef("");
    string reference;

    public PizzaRef() {
        this.reference = RefstringGenerator.generateRefstring();
    }

    
    PizzaRef(string reference) {
        this.reference = reference;
    }

    
    public string getReference() {
        return reference;
    }
}
