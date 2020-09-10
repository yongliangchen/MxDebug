using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FiniteStateMachine
{
    public delegate void EnterState(string stateName);
    public delegate void PushState(string stateName, string lastStateName);
    public delegate void PopState();
    protected Dictionary<string, FSState> mStates;
    protected string      mEntryPoint;
    protected Stack<FSState>    mStateStack;
    public FiniteStateMachine()
    {
        mStates = new Dictionary<string, FSState>();
        mStateStack = new Stack<FSState>();
        mEntryPoint = null;
    }
    public void Update() 
    {
        if (CurrentState == null)
        {
            mStateStack.Push(mStates[mEntryPoint]);
            CurrentState.StateObject.OnEnter(null);
        }
        CurrentState.StateObject.OnUpdate();
    }
    public void Register(string stateName, IState stateObject)
    {
        if (mStates.Count == 0)
            mEntryPoint = stateName;
        mStates.Add(stateName, new FSState(stateObject, this, stateName, Enter, Push, Pop));
    }
    public FSState State(string stateName) 
    {
        return mStates[stateName];
    }
    public void EntryPoint(string startName) 
    {
        mEntryPoint = startName;
    }
    public FSState CurrentState
    { 
        get { 
            if (mStateStack.Count == 0)
                return null;
            return mStateStack.Peek(); 
        } 
    }

    public void Enter(string stateName) 
    {
        Push(stateName, Pop(stateName));
    }

    public void Push(string newState) 
    {
        string lastName = null;
        if (mStateStack.Count > 1) 
        {
            lastName = mStateStack.Peek().StateName;
        }
        Push(newState, lastName);
    }

    protected void Push(string stateName, string lastStateName) 
    {
        mStateStack.Push(mStates[stateName]);
        mStateStack.Peek().StateObject.OnEnter(lastStateName);
    }

    public void Pop() 
    {
        Pop(null);
    }

    protected string Pop(string newName)
    {
        FSState lastState = mStateStack.Peek();
        string newState = null;
        if (newName == null && mStateStack.Count > 1) 
        {
            int index = 0;
            foreach (FSState item in mStateStack)
            {
                if (index++ == mStateStack.Count - 2)
                {
                    newState = item.StateName;
                }
            }
        }
        else 
        {
            newState = newName;
        }
        string lastStateName = null;
        if (lastState != null) {
            lastStateName = lastState.StateName;
            lastState.StateObject.OnExit(newState);
        }
        mStateStack.Pop();
        return lastStateName;
    }
    public void Trigger(string eventName)
    {
        CurrentState.Trigger(eventName);
    }
    public void Trigger(string eventName, object param1)
    {
        CurrentState.Trigger(eventName, param1);
    }

    public void Trigger(string eventName, object param1, object param2) 
    {
        CurrentState.Trigger(eventName, param1, param2);
    }

    public void Trigger(string eventName, object param1, object param2, object param3)
    {
        CurrentState.Trigger(eventName, param1, param2, param3);
    }

   
}