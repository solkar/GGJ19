
using System;

public static class EventBus
{
    #region breaking .NET's events

    public class Event
    {
        public Action evt = () => { };

        public void Invoke()
        {
            evt();
        }
    }

    public class Event<T>
    {
        public Action<T> evt = _ => { };

        public void Invoke(T data)
        {
            evt(data);
        }
    }

    public class Event<T, T2>
    {
        public Action<T, T2> evt = (x, y) => { };

        public void Invoke(T d1, T2 d2)
        {
            evt(d1, d2);
        }
    }

    #endregion

    public static Event OnEnterHouse = new Event();
    public static Event OnExitHouse = new Event();
    
    #region Player Events
    
    public static Event OnPlayerDamage = new Event();
    public static Event OnPlayerAttack = new Event();
    
    #endregion
}
