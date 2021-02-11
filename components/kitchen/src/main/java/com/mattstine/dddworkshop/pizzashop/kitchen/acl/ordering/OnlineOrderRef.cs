namespace com.mattstine.dddworkshop.pizzashop.kitchen.acl.ordering;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Ref;
using lombok.AllArgsConstructor;
using lombok.NoArgsConstructor;

using lombok.experimental.NonFinal;

/**
 * @author Matt Stine
 */



public class OnlineOrderRef : Ref {
    public static OnlineOrderRef IDENTITY = new OnlineOrderRef("");
    
    string reference;
}
