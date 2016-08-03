using System;
using System.Collections.Generic;

namespace GenericStatePattern
{
    internal class State<StateMachineType,EventType> where StateMachineType : StateMachine<StateMachineType,EventType>
    {
        internal StateMachineType stateMachine;
        //Map<EventType, Vector<Transition> > transitionsPerType = new HashMap<EventType, Vector<Transition>>(); // no static type checking
        internal Dictionary<Type, List<Transition<StateMachineType, EventType>>> transitionsPerType = new Dictionary<Type, List<Transition<StateMachineType, EventType>>>(); // with static type checking

        internal State(StateMachineType stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void enter() { }
        public virtual void leave() { }

        internal void handleEvent(EventType evt)
        {
            List<Transition<StateMachineType, EventType>> ts;
            transitionsPerType.TryGetValue(evt.GetType(), out ts);
            if (ts == null) { return; }
            foreach (Transition<StateMachineType, EventType> t in ts)
            {
                if (t.guard(evt))
                {
                    t.action(evt);
                    stateMachine.goTo(t.goTo());
                    break;
                }
            }
        }
    }
}
