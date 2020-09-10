using System;
using System.Collections;
using System.Collections.Generic;

public class FSState {
    protected FiniteStateMachine.EnterState mEnterDelegate;
    protected FiniteStateMachine.PushState mPushDelegate;
    protected FiniteStateMachine.PopState mPopDelegate;

    protected IState mStateObject;
    protected string mStateName;
    protected FiniteStateMachine mOwner;
    protected Dictionary<string, FSEvent> mTranslationEvents;

    public FSState(IState obj, FiniteStateMachine owner, string name, FiniteStateMachine.EnterState e, FiniteStateMachine.PushState pu, FiniteStateMachine.PopState po) {
        mStateObject = obj;
        mStateName = name;
        mOwner = owner;
        mEnterDelegate = e;
        mPushDelegate = pu;
        mPopDelegate = po;
        mTranslationEvents = new Dictionary<string, FSEvent>();
    }

    public IState StateObject {
        get {
            return mStateObject;
        }
    }

    public string StateName {
        get {
            return mStateName;
        }
    }

    public FSEvent On(string eventName) {
        FSEvent newEvent = new FSEvent(eventName, null, this, mOwner, mEnterDelegate, mPushDelegate, mPopDelegate);
        mTranslationEvents.Add(eventName, newEvent);
        return newEvent;
    }

    public void Trigger(string name) {
		if (mTranslationEvents.ContainsKey(name))
			mTranslationEvents[name].Execute(null, null, null);
    }

    public void Trigger(string eventName, object param1) {
		if (mTranslationEvents.ContainsKey(eventName))
			mTranslationEvents[eventName].Execute(param1, null, null);
    }

    public void Trigger(string eventName, object param1, object param2) {
		if (mTranslationEvents.ContainsKey(eventName))
			mTranslationEvents[eventName].Execute(param1, param2, null);
    }

    public void Trigger(string eventName, object param1, object param2, object param3) {
		if (mTranslationEvents.ContainsKey(eventName))
			mTranslationEvents[eventName].Execute(param1, param2, param3);
    }


    public FSState On<T>(string eventName, Func<T, bool> action) {
        FSEvent newEvent = new FSEvent(eventName, null, this, mOwner, mEnterDelegate, mPushDelegate, mPopDelegate);
        newEvent.mAction = delegate (object o1, object o2, object o3) {
            T param1;
            try { param1 = (T)o1; } catch { param1 = default(T); }
            action(param1);
            return true;
        };
        mTranslationEvents.Add(eventName, newEvent);
        return this;
    }

    public FSState On<T>(string eventName, Action<T> action) {
        FSEvent newEvent = new FSEvent(eventName, null, this, mOwner, mEnterDelegate, mPushDelegate, mPopDelegate);
        newEvent.mAction = delegate (object o1, object o2, object o3) {
            T param1;
            try { param1 = (T)o1; } catch { param1 = default(T); }
            action(param1);
            return true;
        };
        mTranslationEvents.Add(eventName, newEvent);
        return this;
    }

    public FSState On<T1, T2>(string eventName, Func<T1, T2, bool> action) {
        FSEvent newEvent = new FSEvent(eventName, null, this, mOwner, mEnterDelegate, mPushDelegate, mPopDelegate);
        newEvent.mAction = delegate (object o1, object o2, object o3) {
            T1 param1;
            T2 param2;
            try { param1 = (T1)o1; } catch { param1 = default(T1); }
            try { param2 = (T2)o2; } catch { param2 = default(T2); }
            action(param1, param2);
            return true;
        };
        mTranslationEvents.Add(eventName, newEvent);
        return this;
    }

    public FSState On<T1, T2>(string eventName, Action<T1, T2> action) {
        FSEvent newEvent = new FSEvent(eventName, null, this, mOwner, mEnterDelegate, mPushDelegate, mPopDelegate);
        newEvent.mAction = delegate (object o1, object o2, object o3) {
            T1 param1;
            T2 param2;
            try { param1 = (T1)o1; } catch { param1 = default(T1); }
            try { param2 = (T2)o2; } catch { param2 = default(T2); }
            action(param1, param2);
            return true;
        };
        mTranslationEvents.Add(eventName, newEvent);
        return this;
    }

    public FSState On<T1, T2, T3>(string eventName, Func<T1, T2, T3, bool> action) {
        FSEvent newEvent = new FSEvent(eventName, null, this, mOwner, mEnterDelegate, mPushDelegate, mPopDelegate);
        newEvent.mAction = delegate (object o1, object o2, object o3) {
            T1 param1;
            T2 param2;
            T3 param3;
            try { param1 = (T1)o1; } catch { param1 = default(T1); }
            try { param2 = (T2)o2; } catch { param2 = default(T2); }
            try { param3 = (T3)o3; } catch { param3 = default(T3); }
            action(param1, param2, param3);
            return true;
        };
        mTranslationEvents.Add(eventName, newEvent);
        return this;
    }

    public FSState On<T1, T2, T3>(string eventName, Action<T1, T2, T3> action) {
        FSEvent newEvent = new FSEvent(eventName, null, this, mOwner, mEnterDelegate, mPushDelegate, mPopDelegate);
        newEvent.mAction = delegate (object o1, object o2, object o3) {
            T1 param1;
            T2 param2;
            T3 param3;
            try { param1 = (T1)o1; } catch { param1 = default(T1); }
            try { param2 = (T2)o2; } catch { param2 = default(T2); }
            try { param3 = (T3)o3; } catch { param3 = default(T3); }
            action(param1, param2, param3);
            return true;
        };
        mTranslationEvents.Add(eventName, newEvent);
        return this;
    }
}