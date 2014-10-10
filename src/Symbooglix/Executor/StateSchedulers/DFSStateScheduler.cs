﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Symbooglix
{
    public class DFSStateScheduler : IStateScheduler
    {
        private List<ExecutionState> States;
        private ExecutionState Popped;
        public DFSStateScheduler() 
        { 
            States = new List<ExecutionState>();
            Popped = null;
        }

        public ExecutionState GetNextState() 
        { 
            if (Popped != null)
                return Popped;

            if (States.Count == 0)
                return null;

            if (Popped == null)
            {
                Popped = States[States.Count - 1];
                States.RemoveAt(States.Count - 1);
            }
            return Popped;
        }

        public int GetNumberOfStates() { return States.Count;}

        public void AddState(ExecutionState toAdd)
        {
            Debug.Assert(!States.Contains(toAdd), "ExecutionStates already in the scheduler should not be added again");
            States.Add(toAdd);
        }

        public void RemoveState(ExecutionState toRemove)
        {
            // Fast path: Remove state that was being Executed
            if (toRemove == Popped)
            {
                Popped = null;
                return;
            }

            Debug.Assert(States.Contains(toRemove), "Cannot remove state not stored in the state scheduler");
            States.Remove(toRemove);
        }

        public void RemoveAll(Predicate<ExecutionState> p)
        {
            States.RemoveAll(p);
        }
    }
}
