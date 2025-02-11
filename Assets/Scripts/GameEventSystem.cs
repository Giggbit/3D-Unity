using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{
    private static readonly Dictionary<string, List<Action<string, object>>> listeners = new();

    public static void AddListener(Action<string, object> action, string type) { 
        if(! listeners.ContainsKey(type)) { 
            listeners[type] = new();
        }
        listeners[type].Add(action);
    }

    public static void RemoveListener(Action<String, object> action, string type) { 
        if(listeners.ContainsKey(type)) {
            listeners[type].Remove(action);
        }
    }

    public static void EmitEvent(string type, object payload) { 
        if(listeners.ContainsKey(type)) { 
            foreach(var action in listeners[type]) {
                action(type, payload);
            }
        }
    }
}
