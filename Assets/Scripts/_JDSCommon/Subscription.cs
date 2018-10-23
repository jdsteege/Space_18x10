using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class SubscriptionBase
{

    protected bool isMuted = false;

    public void Mute()
    {
        isMuted = true;
    }

    public void Unmute()
    {
        isMuted = false;
    }
    
}

public class Subscription : SubscriptionBase
{

    //
    public delegate void CallMethod();

    private CallMethod Receivers = delegate
    {
    };

    //
    public Subscription()
    {
    }

    public Subscription(CallMethod c)
    {
        Subscribe(c);
    }

    //
    public void Subscribe(CallMethod c)
    {
        Unsubscribe(c);
        Receivers += c;
    }

    public void Unsubscribe(CallMethod c)
    {
        Receivers -= c;
    }

    public void ClearSubscribers()
    {
        Receivers = delegate
        {
        };
    }

    public void Send()
    {
        if(isMuted)
        {
            return;
        }

        Receivers();
    }

}

public class Subscription<T0> : SubscriptionBase
{

    //
    public delegate void CallMethod(T0 t0);

    private CallMethod Receivers = delegate
    {
    };

    //
    public Subscription()
    {
    }

    public Subscription(CallMethod c)
    {
        Subscribe(c);
    }

    //
    public void Subscribe(CallMethod c)
    {
        Receivers -= c;
        Receivers += c;
    }

    public void Send(T0 t0)
    {
        if(isMuted)
        {
            return;
        }

        Receivers(t0);
    }
	
}

public class Subscription<T0, T1> : SubscriptionBase
{

    //
    public delegate void CallMethod(T0 t0, T1 t1);

    private CallMethod Receivers = delegate
    {
    };

    //
    public Subscription()
    {
    }

    public Subscription(CallMethod c)
    {
        Subscribe(c);
    }

    //
    public void Subscribe(CallMethod c)
    {
        Receivers -= c;
        Receivers += c;
    }

    public void Send(T0 t0, T1 t1)
    {
        if(isMuted)
        {
            return;
        }

        Receivers(t0, t1);
    }

}

public class Subscription<T0, T1, T2> : SubscriptionBase
{

    //
    public delegate void CallMethod(T0 t0, T1 t1, T2 t2);

    private CallMethod Receivers = delegate
    {
    };

    //
    public Subscription()
    {
    }

    public Subscription(CallMethod c)
    {
        Subscribe(c);
    }

    //
    public void Subscribe(CallMethod c)
    {
        Receivers -= c;
        Receivers += c;
    }

    public void Send(T0 t0, T1 t1, T2 t2)
    {
        if(isMuted)
        {
            return;
        }

        Receivers(t0, t1, t2);
    }

}

public class Subscription<T0, T1, T2, T3> : SubscriptionBase
{

    //
    public delegate void CallMethod(T0 t0, T1 t1, T2 t2, T3 t3);

    private CallMethod Receivers = delegate
    {
    };

    //
    public Subscription()
    {
    }

    public Subscription(CallMethod c)
    {
        Subscribe(c);
    }

    //
    public void Subscribe(CallMethod c)
    {
        Receivers -= c;
        Receivers += c;
    }

    public void Send(T0 t0, T1 t1, T2 t2, T3 t3)
    {
        if(isMuted)
        {
            return;
        }

        Receivers(t0, t1, t2, t3);
    }

}
