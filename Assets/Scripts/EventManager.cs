using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionParams
{
    public float floatParam;
    public int intParam;
    public bool forbid = false;
}
public interface ActionInterceptor
{
    /// <summary>
    /// Intercept event, return true if interceptor should be removed from subscribers.
    /// </summary>
    bool Intercept(Unit u, ActionParams ep);
}
public interface EventHandler
{
    /// <summary>
    /// Handle event, return true if handler should be removed from subscribers.
    /// </summary>
    bool Handle(Unit u);
}
public class EventManager
{
    class TimedHandler : IUpdatable
    {
        string eventName;
        EventHandler handler;
        float time;
        EventManager manager;
        public TimedHandler(string eventName, EventHandler handler, float time, EventManager manager)
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
                manager.InvokeSingleHandler(eventName, handler);
                manager.timedHandlers.Remove(this);
            }
        }
    }

    Dictionary<string, Group<ActionInterceptor>> interceptors = new Dictionary<string, Group<ActionInterceptor>>();
    Dictionary<string, Group<EventHandler>> handlers = new Dictionary<string, Group<EventHandler>>();

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

    public void SubscribeHandler(string eventName, EventHandler handler)
    {
        if (!handlers.ContainsKey(eventName))
        {
            handlers.Add(eventName, new Group<EventHandler>());
        }
        handlers[eventName].Add(handler);
    }
    public void SubscribeInterceptor(string eventName, ActionInterceptor interceptor)
    {
        if (!interceptors.ContainsKey(eventName))
        {
            interceptors.Add(eventName, new Group<ActionInterceptor>());
        }
        interceptors[eventName].Add(interceptor);
    }

    public void SubscribeHandlerWithTimeTrigger(string eventName, EventHandler handler, float time)
    {
        SubscribeHandler(eventName, handler);
        timedHandlers.Add(new TimedHandler(eventName, handler, time, this));
    }

    public void InvokeHandlers(string eventName)
    {
        Debug.Log("Event invoke: " + eventName);
        if (!handlers.ContainsKey(eventName))
            return;
        handlers[eventName].Refresh();
        foreach (var a in handlers[eventName])
        {
            InvokeSingleHandler(eventName, a);
        }
        handlers[eventName].Refresh();
    }
    public void InvokeInterceptors(string eventName, ActionParams ep)
    {
        if (!interceptors.ContainsKey(eventName))
            return;
        interceptors[eventName].Refresh();
        foreach (var a in interceptors[eventName])
        {
            InvokeSingleInterceptor(eventName, ep, a);
        }
        interceptors[eventName].Refresh();
    }

    public void InvokeSingleHandler(string eventName, EventHandler handler)
    {
        Debug.Log("Single handler invoke. Event: " + eventName + ", handler: " + handler.GetType().ToString());
        if (handler.Handle(unit))
            handlers[eventName].Remove(handler);
    }
    public void InvokeSingleInterceptor(string eventName, ActionParams ep, ActionInterceptor interceptor)
    {
        if (interceptor.Intercept(unit, ep))
            interceptors[eventName].Remove(interceptor);
    }

    public void UnsubscribeHandlerCompletely(EventHandler handler)
    {
        foreach (var a in handlers)
            if (a.Value.Contains(handler))
                a.Value.Remove(handler);
    }
    public void UnsubscribeInterceptorCompletely(ActionInterceptor interceptor)
    {
        foreach (var a in interceptors)
            if (a.Value.Contains(interceptor))
                a.Value.Remove(interceptor);
    }

    public void UnsubscribeHandler(string eventName, EventHandler handler)
    {
        if (!handlers.ContainsKey(eventName))
            return;

        foreach (var a in handlers)
            if (a.Value.Contains(handler))
                a.Value.Remove(handler);
    }
    public void UnsubscribeInterceptor(string eventName, ActionInterceptor interceptor)
    {
        if (!interceptors.ContainsKey(eventName))
            return;

        foreach (var a in interceptors)
            if (a.Value.Contains(interceptor))
                a.Value.Remove(interceptor);
    }
}
