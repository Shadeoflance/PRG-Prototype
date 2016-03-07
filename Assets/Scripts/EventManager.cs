using UnityEngine;
using Object = System.Object;
using System.Collections;
using System.Collections.Generic;

public class ActionParams
{
    public Dictionary<string, object> parameters;
    public bool forbid = false;
    public Unit unit;

    public ActionParams()
    {
        parameters = new Dictionary<string, Object>();
    }
    public ActionParams(Unit unit)
    {
        this.unit = unit;
    }

    public object this[string index]
    {
        get
        {
            if (parameters.ContainsKey(index))
                return parameters[index];
            else return null;
        }
        set
        {
            if (parameters.ContainsKey(index))
                parameters[index] = value;
            else parameters.Add(index, value);
        }
    }
}
public interface ActionListener
{
    /// <summary>
    /// Intercept event, return true if interceptor should be removed from subscribers.
    /// </summary>
    bool Handle(ActionParams ap);
}
public class EventManager
{
    class TimedHandler : IUpdatable
    {
        string eventName;
        ActionListener handler;
        float time;
        EventManager manager;
        public TimedHandler(string eventName, ActionListener handler, float time, EventManager manager)
        {
            this.eventName = eventName;
            this.handler = handler;
            this.time = time;
            this.manager = manager;
        }
        public void Update()
        {
            if (time > 0)
                time -= Time.deltaTime;
            else
            {
                ActionParams ap = new ActionParams(manager.unit);
                manager.InvokeSingleHandler(eventName, handler, ap);
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is TimedHandler)
                return (obj as TimedHandler).handler.Equals(handler);
            else if (obj is ActionListener)
                return obj.Equals(handler);
            else return false;
        }
        public override int GetHashCode()
        {
            return handler.GetHashCode();
        }
    }

    Dictionary<string, Group<ActionListener>> interceptors = new Dictionary<string, Group<ActionListener>>();
    Dictionary<string, Group<ActionListener>> handlers = new Dictionary<string, Group<ActionListener>>();

    Group<TimedHandler> timedHandlers = new Group<TimedHandler>();

    Unit unit;

    public EventManager(Unit unit)
    {
        this.unit = unit;
    }

    public void Update()
    {
        timedHandlers.Refresh();
        foreach (var a in timedHandlers)
            a.Update();
    }

    public void SubscribeHandler(string eventName, ActionListener handler)
    {
        if (!handlers.ContainsKey(eventName))
        {
            handlers.Add(eventName, new Group<ActionListener>());
        }
        handlers[eventName].Add(handler);
    }
    public void SubscribeInterceptor(string eventName, ActionListener interceptor)
    {
        if (!interceptors.ContainsKey(eventName))
        {
            interceptors.Add(eventName, new Group<ActionListener>());
        }
        interceptors[eventName].Add(interceptor);
    }

    public void SubscribeHandlerWithTimeTrigger(string eventName, ActionListener handler, float time)
    {
        SubscribeHandler(eventName, handler);
        timedHandlers.Add(new TimedHandler(eventName, handler, time, this));
    }

    public void InvokeHandlers(string eventName, ActionParams ap)
    {
        Debug.Log("Event invoke by " + unit.name + ": " + eventName);
        if (!handlers.ContainsKey(eventName))
            return;
        handlers[eventName].Refresh();
        if(ap == null)
            ap = new ActionParams();
        ap.unit = unit;
        foreach (var a in handlers[eventName])
        {
            InvokeSingleHandler(eventName, a, ap);
        }
        handlers[eventName].Refresh();
    }

    public void InvokeInterceptors(string eventName, ActionParams ap)
    {
        Debug.Log("Action invoke by " + unit.name + ": " + eventName);
        if (!interceptors.ContainsKey(eventName))
            return;
        interceptors[eventName].Refresh();
        if(ap == null)
            ap = new ActionParams();
        ap.unit = unit;
        foreach (var a in interceptors[eventName])
        {
            InvokeSingleInterceptor(eventName, ap, a);
        }
        interceptors[eventName].Refresh();
    }

    public void InvokeSingleHandler(string eventName, ActionListener handler, ActionParams ap)
    {
        Debug.Log("Single handler invoke by " + unit.name + ". Event: " + eventName + ", handler: " + handler.GetType().ToString());
        if (handler.Handle(ap))
        {
            handlers[eventName].Remove(handler);
            ClearTimedEvent(handler);
        }
    }
    private void ClearTimedEvent(ActionListener handler)
    {
        foreach (var a in timedHandlers)
        {
            if (a.Equals(handler))
            {
                timedHandlers.Remove(a);
            }
        }
    }
    public void InvokeSingleInterceptor(string eventName, ActionParams ap, ActionListener interceptor)
    {
        Debug.Log("Single interceptor invoke by " + unit.name + ". Event: " + eventName + ", interceptor: " + interceptor.GetType().ToString());
        if (interceptor.Handle(ap))
            interceptors[eventName].Remove(interceptor);
    }

    public void UnsubscribeHandlerCompletely(ActionListener handler)
    {
        foreach (var a in handlers)
            if (a.Value.Contains(handler))
                a.Value.Remove(handler);
    }
    public void UnsubscribeInterceptorCompletely(ActionListener interceptor)
    {
        foreach (var a in interceptors)
            if (a.Value.Contains(interceptor))
                a.Value.Remove(interceptor);
    }

    public void UnsubscribeHandler(string eventName, ActionListener handler)
    {
        if (!handlers.ContainsKey(eventName))
            return;

        foreach (var a in handlers)
            if (a.Value.Contains(handler))
                a.Value.Remove(handler);
    }
    public void UnsubscribeInterceptor(string eventName, ActionListener interceptor)
    {
        if (!interceptors.ContainsKey(eventName))
            return;

        foreach (var a in interceptors)
            if (a.Value.Contains(interceptor))
                a.Value.Remove(interceptor);
    }
}
