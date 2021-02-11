namespace com.mattstine.dddworkshop.pizzashop.payments;

using lombok.AccessLevel;
using lombok.NoArgsConstructor;

(access = AccessLevel.PRIVATE)
class DummyPaymentProcessor : PaymentProcessor {
    private static DummyPaymentProcessor singleton;

    public static DummyPaymentProcessor instance() {
        if (singleton == null) {
            singleton = new DummyPaymentProcessor();
        }
        return singleton;
    }

    
    public void request(Payment payment) {
        // Do nothing
    }
}
