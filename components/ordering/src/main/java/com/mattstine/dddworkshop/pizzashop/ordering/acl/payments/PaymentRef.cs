namespace com.mattstine.dddworkshop.pizzashop.ordering.acl.payments;

using com.mattstine.dddworkshop.pizzashop.infrastructure.repository.ports.Ref;
using lombok.AllArgsConstructor;
using lombok.NoArgsConstructor;

using lombok.experimental.NonFinal;

/**
 * @author Matt Stine
 */



public class PaymentRef : Ref {
    
    string reference;
}
