using UnityEngine.Assertions;

namespace GenericStatePattern
{
    public abstract class StateMachine<StateMachineType,EventType> where StateMachineType : StateMachine<StateMachineType,EventType>
    {

        internal State<StateMachineType, EventType> current = null;
        internal State<StateMachineType, EventType> first = null;

        public StateMachine()
        {
            DefineFirst();
            Assert.IsNotNull(first);
            current = first;
        }

        protected abstract void DefineFirst();

        public void handleEvent(EventType evt)
        {
            current.handleEvent(evt);
        }

        internal void goTo(State<StateMachineType, EventType> s)
        {
            current.leave();
            current = s;
            current.enter();
        }
    }
}