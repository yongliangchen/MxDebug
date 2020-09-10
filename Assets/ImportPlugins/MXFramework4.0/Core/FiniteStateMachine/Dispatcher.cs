using System;
using System.Collections;
using System.Collections.Generic;
namespace EventSystem {
    public class Dispatcher {  
        public void Trigger(string eventName) {
            Call(eventName, null, null, null);
        }

        public void Dispatch(string eventName) {
            Dispatched d = (mFreeDispatch.Count == 0) ? new Dispatched() : mFreeDispatch.Pop();
            mDispatched.Enqueue(d.Set(eventName));
        }

        public int On(string eventName, Func<bool> action) {
            return Register(eventName, delegate(object arg1, object arg2, object arg3) {
                return(action());
            });
        }

        public int On(string eventName, Action action) {
            return Register(eventName, delegate(object arg1, object arg2, object arg3) {
                action();
                return(true);
            });
        }

        public void Trigger(string eventName, object param1) {
            Call(eventName, param1, null, null);
        }

        public void Dispatch(string eventName, object param1) {
            Dispatched d = (mFreeDispatch.Count == 0) ? new Dispatched() : mFreeDispatch.Pop();
            mDispatched.Enqueue(d.Set(eventName, param1));
        }

        public int On<T>(string eventName, Func<T,bool> action) {
            return Register(eventName, delegate(object arg1, object arg2, object arg3) {
                T  param1;
                try { param1 = (T)arg1; } catch { param1 = default(T); }
                return(action(param1));
            });
        }

        public int On<T>(string eventName, Action<T> action) {
            return Register(eventName, delegate(object arg1, object arg2, object arg3) {
                T  param1;
                try { param1 = (T)arg1; } catch { param1 = default(T); }
                action(param1);
                return true;
            });
        }

        public void Trigger(string eventName, object param1, object param2) {
            Call(eventName, param1, param2, null);
        }

        public void Dispatch(string eventName, object param1, object param2) {
            Dispatched d = (mFreeDispatch.Count == 0) ? new Dispatched() : mFreeDispatch.Pop();
            mDispatched.Enqueue(d.Set(eventName, param1, param2));
        }

        public int On<T1,T2>(string eventName, Func<T1,T2,bool> action) {
            return Register(eventName, delegate(object arg1, object arg2, object arg3) {
                T1  param1;
                T2  param2;
                try { param1 = (T1)arg1; } catch { param1 = default(T1); }
                try { param2 = (T2)arg2; } catch { param2 = default(T2); }
                return(action(param1, param2));
            });
        }

        public int On<T1,T2>(string eventName, Action<T1,T2> action) {
            return Register(eventName, delegate(object arg1, object arg2, object arg3) {
                T1  param1;
                T2  param2;
                try { param1 = (T1)arg1; } catch { param1 = default(T1); }
                try { param2 = (T2)arg2; } catch { param2 = default(T2); }
                action(param1, param2);
                return true;
            });
        }

        public void Trigger(string eventName, object param1, object param2, object param3) {
            Call(eventName, param1, param2, param3);
        }

        public void Dispatch(string eventName, object param1, object param2, object param3){
            Dispatched d = (mFreeDispatch.Count == 0) ? new Dispatched() : mFreeDispatch.Pop();
            mDispatched.Enqueue(d.Set(eventName, param1, param2, param3));
        }

        public int On<T1,T2,T3>(string eventName, Func<T1,T2,T3,bool> action) {
            return Register(eventName, delegate(object arg1, object arg2, object arg3) {
                T1  param1;
                T2  param2;
                T3  param3;
                try { param1 = (T1)arg1; } catch { param1 = default(T1); }
                try { param2 = (T2)arg2; } catch { param2 = default(T2); }
                try { param3 = (T3)arg3; } catch { param3 = default(T3); }
                return(action(param1, param2, param3));
            });
        }

        public int On<T1,T2,T3>(string eventName, Action<T1,T2,T3> action) {
            return Register(eventName, delegate(object arg1, object arg2, object arg3) {
                T1  param1;
                T2  param2;
                T3  param3;
                try { param1 = (T1)arg1; } catch { param1 = default(T1); }
                try { param2 = (T2)arg2; } catch { param2 = default(T2); }
                try { param3 = (T3)arg3; } catch { param3 = default(T3); }
                action(param1, param2, param3);
                return true;
            });
        }

        public bool Cancel(int listenerID) {
            return mRegistered.Remove(listenerID);
        }

        public void DispatchPending() {
            while(mDispatched.Count > 0) {
                Dispatched d = mDispatched.Dequeue();
                Call(d.mEventName, d.mArg1, d.mArg2, d.mArg3);
                mFreeDispatch.Push(d);
            }
        }

        int Register(string eventName, Func<object,object,object,bool> action) {
            int   listenID = ++mNextListenID;
            Listen  listen = (mFreeListen.Count == 0) ? new Listen() : mFreeListen.Pop();
            listen.mID = listenID;
            listen.mAction = action;
            mRegistered.Add(listenID, listen);
            List<int>  eventList;
            if(!mRegisteredEvents.TryGetValue(eventName, out eventList))
                eventList = mRegisteredEvents[eventName] = new List<int>();
            eventList.Add(listenID);
            return listenID;
        }

        void Call(string eventName, object arg1, object arg2, object arg3) {
            List<int> listenerList;
            if(mRegisteredEvents.TryGetValue(eventName, out listenerList)) {
                for(int i = listenerList.Count - 1; i >= 0; --i) {
                    Listen listener;
                    if(mRegistered.TryGetValue(listenerList[i], out listener)) {
                        if(!listener.mAction(arg1, arg2, arg3)) {
                            mRegistered.Remove(listenerList[i]);
                            mFreeListen.Push(listener);
                            listenerList.RemoveAt(i);
                        }
                    }
                    else {
                        listenerList.RemoveAt(i);
                    }
                }
                if(listenerList.Count == 0) {
                    mRegisteredEvents.Remove(eventName);
                }
            }
        }

        class Listen {
            public int        mID;
            public Func<object,object,object,bool> mAction;
        }

        class Dispatched {
            public Dispatched Set(string eventName, object arg1=null, object arg2=null, object arg3=null) {
                mEventName = eventName;
                mArg1 = arg1;
                mArg2 = arg2;
                mArg3 = arg3;
                return this;
            }
            public string mEventName;
            public object mArg1, mArg2, mArg3;
        }

        Dictionary<string,List<int>>  mRegisteredEvents = new Dictionary<string, List<int>>();
        Dictionary<int,Listen>    mRegistered = new Dictionary<int, Listen>();
        Stack<Listen>      mFreeListen = new Stack<Listen>();
        Stack<Dispatched>     mFreeDispatch = new Stack<Dispatched>();
        Queue<Dispatched>     mDispatched = new Queue<Dispatched>();
        int         mNextListenID = 4711;
    }
}