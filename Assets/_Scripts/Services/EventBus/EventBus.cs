using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : IDisposable
{
    private Dictionary<Type, List<object>> _receivers;
    private Dictionary<Type, List<object>> _requestReceivers;

    public void Initialize()
    {
        _receivers = new Dictionary<Type, List<object>>();
        _requestReceivers = new Dictionary<Type, List<object>>();
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
            Debug.LogErrorFormat("Попытка отписать несуществующего подписчика! " + receiver.GetType());
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

    // ===== События с возвращаемым значением) =====
    //
    //ИСПОЛЬЗОВАНИЕ: 
    // short _currentBonesCount = _eventBus.Request<DogWasInteracted, short>(new DogWasInteracted());
    // _eventBus.Subscribe<DogWasInteracted, short>(GetBonesCount);

    public void Subscribe<T, TResult>(Func<T, TResult> receiver) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (_requestReceivers.ContainsKey(eventType))
        {
            _requestReceivers[eventType].Add(receiver);
        }
        else
        {
            _requestReceivers.Add(eventType, new List<object>() { receiver });
        }
    }

    public void Subscribe<T, TResult>(Func<TResult> receiver) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (_requestReceivers.ContainsKey(eventType))
        {
            _requestReceivers[eventType].Add(receiver);
        }
        else
        {
            _requestReceivers.Add(eventType, new List<object>() { receiver });
        }
    }

    public void UnSubscribe<T, TResult>(Func<T, TResult> receiver) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (_requestReceivers.ContainsKey(eventType))
        {
            _requestReceivers[eventType].Remove(receiver);
        }
        else
        {
            Debug.LogErrorFormat("Попытка отписать несуществующего подписчика запроса!");
        }
    }

    public void UnSubscribe<T, TResult>(Func<TResult> receiver) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (_requestReceivers.ContainsKey(eventType))
        {
            _requestReceivers[eventType].Remove(receiver);
        }
        else
        {
            Debug.LogErrorFormat("Попытка отписать несуществующего подписчика запроса!");
        }
    }

    public TResult Request<T, TResult>(T @event) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        
        if (_requestReceivers.TryGetValue(eventType, out var receivers) && receivers.Count > 0)
        {
            var receiver = receivers[0];
            if (receiver is Func<T, TResult> typedReceiver)
            {
                return typedReceiver.Invoke(@event);
            }
            else if (receiver is Func<TResult> untypedReceiver)
            {
                return untypedReceiver.Invoke();
            }
        }
        
        throw new InvalidOperationException($"Нет подписчиков на запрос типа {typeof(T).Name}");
    }

    public List<TResult> RequestAll<T, TResult>(T @event) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        var results = new List<TResult>();
        
        if (_requestReceivers.TryGetValue(eventType, out var receivers))
        {
            foreach (var receiver in receivers)
            {
                if (receiver is Func<T, TResult> typedReceiver)
                {
                    results.Add(typedReceiver.Invoke(@event));
                }
                else if (receiver is Func<TResult> untypedReceiver)
                {
                    results.Add(untypedReceiver.Invoke());
                }
            }
        }
        
        return results;
    }

    public TResult Request<T, TResult>() where T : struct, IEvent
    {
        Type eventType = typeof(T);
        
        if (_requestReceivers.TryGetValue(eventType, out var receivers) && receivers.Count > 0)
        {
            var receiver = receivers[0];
            if (receiver is Func<T, TResult> typedReceiver)
            {
                return typedReceiver.Invoke(default);
            }
            else if (receiver is Func<TResult> untypedReceiver)
            {
                return untypedReceiver.Invoke();
            }
        }
        
        throw new InvalidOperationException($"Нет подписчиков на запрос типа {typeof(T).Name}");
    }

    public void Dispose()
    {
        _receivers?.Clear();
        _requestReceivers?.Clear();
    }
}