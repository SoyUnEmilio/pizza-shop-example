using System;

namespace com.mattstine.dddworkshop.pizzashop.infrastructure.domain.services
{
    /**
     * @author Matt Stine
     */
    public class RefstringGenerator
    {
        /**
         * Generate a random upper cased Guid for use as <code>Ref</code>'s <code>reference</code> property.
         *
         * @return random upper cased Guid string
         */
        public static string generateRefstring()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }
    }
}