using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void EventFun0();
public delegate void EventFun1<T>(T p1);
public delegate void EventFun2<T, U>(T p1, U p2);
public delegate void EventFun3<T, U, V>(T p1, U p2, V p3);
public delegate void EventFun4<T, U, V, W>(T p1, U p2, V p3, W p4);

public class BaseEvents
{
    public bool isLock = false;
}

public class GenericEvents0 : BaseEvents
{
    public List<EventFun0> eventList = new List<EventFun0>();
    public List<EventFun0> deleteRequests = new List<EventFun0>();
}

public class GenericEvents1<T> : BaseEvents
{
    public List<EventFun1<T>> eventList = new List<EventFun1<T>>();
    public List<EventFun1<T>> deleteRequests = new List<EventFun1<T>>();
}

public class GenericEvents2<T, U> : BaseEvents
{
    public List<EventFun2<T, U>> eventList = new List<EventFun2<T, U>>();
    public List<EventFun2<T, U>> deleteRequests = new List<EventFun2<T, U>>();
}

public class GenericEvents3<T, U, V> : BaseEvents
{
    public List<EventFun3<T, U, V>> eventList = new List<EventFun3<T, U, V>>();
    public List<EventFun3<T, U, V>> deleteRequests = new List<EventFun3<T, U, V>>();
}

public class GenericEvents4<T, U, V, W> : BaseEvents
{
    public List<EventFun4<T, U, V, W>> eventList = new List<EventFun4<T, U, V, W>>();
    public List<EventFun4<T, U, V, W>> deleteRequests = new List<EventFun4<T, U, V, W>>();
}

public class EventSystem
{
    public Dictionary<EventID, BaseEvents> eventDictionary = new Dictionary<EventID, BaseEvents>();

    private static EventSystem eventSystem = null;

    public static EventSystem Instance
    {
        get
        {
            if (eventSystem == null)
            {
                eventSystem = new EventSystem();
            }
            return eventSystem;
        }
    }

    public static void AddListener(EventID eventId, EventFun0 listener)
    {
        BaseEvents baseEvents = null;
        if (!Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            baseEvents = new GenericEvents0();
            Instance.eventDictionary.Add(eventId, baseEvents);
        }
        ((GenericEvents0)baseEvents).eventList.Add(listener);
    }

    public static void AddListener<T>(EventID eventId, EventFun1<T> listener)
    {
        BaseEvents baseEvents = null;
        if (!Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            baseEvents = new GenericEvents1<T>();
            Instance.eventDictionary.Add(eventId, baseEvents);
        }
        ((GenericEvents1<T>)baseEvents).eventList.Add(listener);
    }

    public static void AddListener<T, U>(EventID eventId, EventFun2<T, U> listener)
    {
        BaseEvents baseEvents = null;
        if (!Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            baseEvents = new GenericEvents2<T, U>();
            Instance.eventDictionary.Add(eventId, baseEvents);
        }
        ((GenericEvents2<T, U>)baseEvents).eventList.Add(listener);
    }

    public static void AddListener<T, U, V>(EventID eventId, EventFun3<T, U, V> listener)
    {
        BaseEvents baseEvents = null;
        if (!Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            baseEvents = new GenericEvents3<T, U, V>();
            Instance.eventDictionary.Add(eventId, baseEvents);
        }
        ((GenericEvents3<T, U, V>)baseEvents).eventList.Add(listener);
    }

    public static void AddListener<T, U, V, W>(EventID eventId, EventFun4<T, U, V, W> listener)
    {
        BaseEvents baseEvents = null;
        if (!Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            baseEvents = new GenericEvents4<T, U, V, W>();
            Instance.eventDictionary.Add(eventId, baseEvents);
        }
        ((GenericEvents4<T, U, V, W>)baseEvents).eventList.Add(listener);
    }

    public static void RemoveListener(EventID eventId, EventFun0 listener)
    {
        BaseEvents baseEvents;
        if (Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            GenericEvents0 impEvents = baseEvents as GenericEvents0;
            if (!impEvents.isLock)
            {
                impEvents.eventList.Remove(listener);
            }
            else
            {
                impEvents.deleteRequests.Add(listener);
            }
        }
    }

    public static void RemoveListener<T>(EventID eventId, EventFun1<T> listener)
    {
        BaseEvents baseEvents;
        if (Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            GenericEvents1<T> impEvents = baseEvents as GenericEvents1<T>;
            if (!impEvents.isLock)
            {
                impEvents.eventList.Remove(listener);
            }
            else
            {
                impEvents.deleteRequests.Add(listener);
            }
        }
    }

    public static void RemoveListener<T, U>(EventID eventId, EventFun2<T, U> listener)
    {
        BaseEvents baseEvents;
        if (Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            GenericEvents2<T, U> impEvents = baseEvents as GenericEvents2<T, U>;
            if (!impEvents.isLock)
            {
                impEvents.eventList.Remove(listener);
            }
            else
            {
                impEvents.deleteRequests.Add(listener);
            }
        }
    }

    public static void RemoveListener<T, U, V>(EventID eventId, EventFun3<T, U, V> listener)
    {
        BaseEvents baseEvents;
        if (Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            GenericEvents3<T, U, V> impEvents = baseEvents as GenericEvents3<T, U, V>;
            if (!impEvents.isLock)
            {
                impEvents.eventList.Remove(listener);
            }
            else
            {
                impEvents.deleteRequests.Add(listener);
            }
        }
    }

    public static void RemoveListener<T, U, V, W>(EventID eventId, EventFun4<T, U, V, W> listener)
    {
        BaseEvents baseEvents;
        if (Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            GenericEvents4<T, U, V, W> impEvents = baseEvents as GenericEvents4<T, U, V, W>;
            if (!impEvents.isLock)
            {
                impEvents.eventList.Remove(listener);
            }
            else
            {
                impEvents.deleteRequests.Add(listener);
            }
        }
    }

    public static void Dispatch(EventID eventId)
    {
        BaseEvents baseEvents;
        if (Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            baseEvents.isLock = true;
            GenericEvents0 impEvents = baseEvents as GenericEvents0;
            int count = impEvents.eventList.Count;
            for (int i = 0; i < count; ++i)
            {
                EventFun0 dlgt = impEvents.eventList[i];
                dlgt();
            }
            if (impEvents.deleteRequests.Count > 0)
            {
                for (int i = 0; i < impEvents.deleteRequests.Count; ++i)
                {
                    impEvents.eventList.Remove(impEvents.deleteRequests[i]);
                }
                impEvents.deleteRequests.Clear();
            }
            baseEvents.isLock = false;
        }
    }

    public static void Dispatch<T>(EventID eventId, T obj)
    {
        BaseEvents baseEvents;
        if (Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            baseEvents.isLock = true;
            GenericEvents1<T> impEvents = baseEvents as GenericEvents1<T>;
            int count = impEvents.eventList.Count;
            for (int i = 0; i < count; ++i)
            {
                EventFun1<T> dlgt = impEvents.eventList[i];
                dlgt(obj);
            }
            if (impEvents.deleteRequests.Count > 0)
            {
                for (int i = 0; i < impEvents.deleteRequests.Count; ++i)
                {
                    impEvents.eventList.Remove(impEvents.deleteRequests[i]);
                }
                impEvents.deleteRequests.Clear();
            }
            baseEvents.isLock = false;
        }
    }

    public static void Dispatch<T, U>(EventID eventId, T obj1, U obj2)
    {
        BaseEvents baseEvents;
        if (Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            baseEvents.isLock = true;
            GenericEvents2<T, U> impEvents = baseEvents as GenericEvents2<T, U>;
            int count = impEvents.eventList.Count;
            for (int i = 0; i < count; ++i)
            {
                EventFun2<T, U> dlgt = impEvents.eventList[i];
                dlgt(obj1, obj2);
            }
            if (impEvents.deleteRequests.Count > 0)
            {
                for (int i = 0; i < impEvents.deleteRequests.Count; ++i)
                {
                    impEvents.eventList.Remove(impEvents.deleteRequests[i]);
                }
                impEvents.deleteRequests.Clear();
            }
            baseEvents.isLock = false;
        }
    }

    public static void Dispatch<T, U, V>(EventID eventId, T obj1, U obj2, V obj3)
    {
        BaseEvents baseEvents;
        if (Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            baseEvents.isLock = true;
            GenericEvents3<T, U, V> impEvents = baseEvents as GenericEvents3<T, U, V>;
            int count = impEvents.eventList.Count;
            for (int i = 0; i < count; ++i)
            {
                EventFun3<T, U, V> dlgt = impEvents.eventList[i];
                dlgt(obj1, obj2, obj3);
            }
            if (impEvents.deleteRequests.Count > 0)
            {
                for (int i = 0; i < impEvents.deleteRequests.Count; ++i)
                {
                    impEvents.eventList.Remove(impEvents.deleteRequests[i]);
                }
                impEvents.deleteRequests.Clear();
            }
            baseEvents.isLock = false;
        }
    }

    public static void Dispatch<T, U, V, W>(EventID eventId, T obj1, U obj2, V obj3, W obj4)
    {
        BaseEvents baseEvents;
        if (Instance.eventDictionary.TryGetValue(eventId, out baseEvents))
        {
            baseEvents.isLock = true;
            GenericEvents4<T, U, V, W> impEvents = baseEvents as GenericEvents4<T, U, V, W>;
            int count = impEvents.eventList.Count;
            for (int i = 0; i < count; ++i)
            {
                EventFun4<T, U, V, W> dlgt = impEvents.eventList[i];
                dlgt(obj1, obj2, obj3, obj4);
            }
            if (impEvents.deleteRequests.Count > 0)
            {
                for (int i = 0; i < impEvents.deleteRequests.Count; ++i)
                {
                    impEvents.eventList.Remove(impEvents.deleteRequests[i]);
                }
                impEvents.deleteRequests.Clear();
            }
            baseEvents.isLock = false;
        }
    }
}