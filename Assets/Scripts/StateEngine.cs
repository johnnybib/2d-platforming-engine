using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEngine
{
    public enum states {idle, walking, jumping};
    private states curState;
    private List<StateNode> stateList;
    public StateEngine()
    {
        stateList = new List<StateNode>();
        curState = states.idle;

        stateList.Add(new StateNode(states.idle));
        stateList[0].SetTo(states.walking, states.jumping);

        stateList.Add(new StateNode(states.walking));
        stateList[1].SetTo(states.idle, states.jumping);

        stateList.Add(new StateNode(states.jumping));
        stateList[2].SetTo(states.idle, states.walking);

    }

    public bool Transition(states newState)
    {
        if(stateList[(int)curState].GetTo().Contains(newState))
        {
            curState = newState;
            return true;
        }
        return false;
    }

    private class StateNode
    {
        private states state;
        private List<states> to;
        public StateNode(states state)
        {
            this.state = state;
        }

        public void SetTo(params states[] toStates)        
        {
            to = new List<states>(toStates);
        }

        public List<states> GetTo()
        {
            return to;
        }
    }

}
