namespace com.mattstine.dddworkshop.pizzashop.infrastructure.events.ports
{
    /**
     * @author Matt Stine
     */

    public class Topic
    {
        public string name { get; private set; }

        public Topic(string name)
        {
            this.name = name;
        }
    }
}

