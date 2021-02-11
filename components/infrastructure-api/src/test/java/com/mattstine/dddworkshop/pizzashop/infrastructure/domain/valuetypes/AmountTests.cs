namespace com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes
{

    /**
     * @author Matt Stine
     */
    public class AmountTests
    {

        [Fact]
        public void adds_amounts_without_overflow()
        {
            assertThat(Amount.of(6, 20)
                    .plus(Amount.of(6, 20)))
                    .isEqualTo(Amount.of(12, 40));
        }

        [Fact]
        public void adds_amounts_with_perfect_overflow()
        {
            assertThat(Amount.of(6, 50)
                    .plus(Amount.of(6, 50)))
                    .isEqualTo(Amount.of(13, 0));
        }

        [Fact]
        public void adds_amounts_with_imperfect_overflow()
        {
            assertThat(Amount.of(6, 50)
                    .plus(Amount.of(6, 60)))
                    .isEqualTo(Amount.of(13, 10));
        }

        [Fact]
        public void dollars_cannot_be_negative()
        {
            assertThatIllegalArgumentException().isThrownBy(()->Amount.of(-1, 0));
        }

        [Fact]
        public void cents_cannot_be_negative()
        {
            assertThatIllegalArgumentException().isThrownBy(()->Amount.of(0, -1));
        }

        [Fact]
        public void cents_must_be_less_than_100()
        {
            assertThatIllegalArgumentException().isThrownBy(()->Amount.of(0, 100));
        }
    }
}

