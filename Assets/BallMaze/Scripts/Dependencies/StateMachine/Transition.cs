using System;
using System.Collections.Generic;

namespace GenericStatePattern
{

    internal class Transition<StateMachineType,EventType> where StateMachineType : StateMachine<StateMachineType,EventType>
    {
        protected State<StateMachineType, EventType> myState;

        internal Transition(State<StateMachineType, EventType> myState, Type eventType)
        {
            this.myState = myState;
            if (typeof(EventType).IsAssignableFrom(eventType))
            {
                Transition<StateMachineType,EventType> t = this;
                //Vector<Transition> ts = transitionsPerType.get(eventType);
                List<Transition<StateMachineType, EventType>> ts;
                myState.transitionsPerType.TryGetValue(eventType, out ts);
                if (ts == null)
                {
                    ts = new List<Transition<StateMachineType, EventType>>();
                    ts.Add(t);
                    //transitionsPerType.put(eventType, ts);
                    myState.transitionsPerType.Add(eventType, ts);
                }
                else
                {
                    ts.Add(t);
                }
            }
            else
            {
                throw new Exception("The type of the transition (" + eventType + ") doesn't derive from the type of the state machine : " + typeof(EventType));
            }
        }
        public virtual bool guard(EventType evt) { return true; }
        public virtual void action(EventType evt) { }
        public virtual State<StateMachineType, EventType> goTo() { return myState.stateMachine.current; }
    }

}

