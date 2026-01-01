using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
    private Dictionary<Type, List<object>> _receivers;

    public void Initialize()
    {
        _receivers = new Dictionary<Type, List<object>>();
    }

    public void Subscribe<T>(Action<T> receiver) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (_receivers.ContainsKey(eventType))
        {
            _receivers[eventType].Add(receiver);
        }
        else
        {
            _receivers.Add(eventType, new List<object>() { receiver });
        }
    }
    public void Subscribe<T>(Action receiver) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (_receivers.ContainsKey(eventType))
        {
            _receivers[eventType].Add(receiver);
        }
        else
        {
            _receivers.Add(eventType, new List<object>() { receiver });
        }
    }

    public void UnSubscribe<T>(Action<T> receiver) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (_receivers.ContainsKey(eventType))
        {
            _receivers[eventType].Remove(receiver);
        }
        else
        {
            Debug.LogErrorFormat("Попытка отписать несуществующего подписчика!");
        }
    }
    public void UnSubscribe<T>(Action receiver) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (_receivers.ContainsKey(eventType))
        {
            _receivers[eventType].Remove(receiver);
        }
        else
        {
            Debug.LogErrorFormat("Попытка отписать несуществующего подписчика!");
        }
    }

    public void Invoke<T>(T @event) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (_receivers.ContainsKey(eventType))
        {
            foreach (var receiver in _receivers[eventType])
            {
                if (receiver is Action<T> typedReceiver)
                {
                    typedReceiver.Invoke(@event);
                }
                else if (receiver is Action untypedReceiver)
                {
                    untypedReceiver.Invoke();
                }
            }
        }
    }
}