using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubscriptionQueue<T>
{
	
    Queue<T> queue;

    public SubscriptionQueue(Subscription<T> provider)
    {
        queue = new Queue<T>();

        provider.Subscribe(Enqueue);
    }

    public void Enqueue(T t)
    {
        Debug.Log(t.ToString());

        queue.Enqueue(t);
    }

    public bool IsEmpty()
    {
        return queue.Count <= 0;
    }

    public T Dequeue()
    {
        if(IsEmpty())
        {
            return default(T);
        }

        return queue.Dequeue();
    }
	
}
