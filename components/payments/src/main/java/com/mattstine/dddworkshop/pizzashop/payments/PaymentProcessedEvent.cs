namespace com.mattstine.dddworkshop.pizzashop.payments;



/**
 * @author Matt Stine
 */

class PaymentProcessedEvent : PaymentEvent {
    PaymentRef @ref;
    Status status;

    bool isSuccessful() {
        return status == Status.SUCCESSFUL;
    }

    bool isFailed() {
        return status == Status.FAILED;
    }

    public enum Status {
        SUCCESSFUL, FAILED
    }
}
