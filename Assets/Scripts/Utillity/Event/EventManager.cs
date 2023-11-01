using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Event
{
    public class EventManager:MonoBehaviourSingle<EventManager>
    {
        private Dictionary<string, Action<object[]>> _dictionary = new Dictionary<string, Action<object[]>>(100);
        private Queue<string> _actionIndex = new Queue<string>(10);
        private Queue<object[]> _actions = new Queue<object[]>(10);
        private EventManager()
        {
            
        }

        public void SendApplication(string index,object[] param)
        {
            if (_actionIndex.Count >= 10)
            {
                Debug.LogError("send wrong!");
            }
            else
            {
                _actionIndex.Enqueue(index);
                _actions.Enqueue(param);
            }
        }

        public void RegisterEvent(string eventName,Action<object[]> action)
        {
            if (_dictionary.ContainsKey(eventName))
            {
                Debug.LogError("already have this event");
            }
            else
            {
                _dictionary.Add(eventName,action);
            }
        }

        public void UnRegisterEvent(string index)
        {
            if(_dictionary.ContainsKey(index))
                _dictionary.Remove(index);
        }

        private void UnRegisterAllEvent()
        {
            _dictionary.Clear();
        }

        private void GetMessage()
        {
            if (_actionIndex.Count > 0)
            {
                if (_dictionary.ContainsKey(_actionIndex.Peek()))
                {
                    _dictionary[_actionIndex.Dequeue()].Invoke(_actions.Dequeue());
                }
                else
                {
                    Debug.LogError("Not Found This Action!");
                }
            }
        }
        
        private void LateUpdate()
        {
            GetMessage();
        }
    }
}