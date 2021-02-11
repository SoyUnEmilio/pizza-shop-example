namespace com.mattstine.dddworkshop.pizzashop.kitchen;

using com.mattstine.dddworkshop.pizzashop.infrastructure.domain.services.RefstringGenerator;
using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Ref;
using lombok.Value;


public class PizzaRef : Ref {
    public static PizzaRef IDENTITY = new PizzaRef("");
    string reference;

    public PizzaRef() {
        this.reference = RefstringGenerator.generateRefstring();
    }

    @SuppressWarnings("SameParameterValue")
    PizzaRef(string reference) {
        this.reference = reference;
    }

    @Override
    public string getReference() {
        return reference;
    }
}
