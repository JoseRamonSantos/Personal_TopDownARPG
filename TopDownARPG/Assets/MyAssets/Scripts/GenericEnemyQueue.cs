using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemyQueue<T> where T : class
{
    public Node m_head; 
    public Node m_tail;
    public int m_count;

    public class Node
    {
        public T m_data;
        public Node m_previous;
        public Node m_following;

        public Node(T _t)
        {
            m_previous = null;
            m_following = null;
            m_data = _t;
        }
    }

    public void Enqueue(T _t)
    {
        Node n = new Node(_t);

        if (m_head == null)
        {
            m_head = n;
            m_tail = n;
        }
        else
        {
            m_tail.m_following = n;
            n.m_previous = m_tail;
            m_tail = n;
        }

        m_count++;
    }
    public T PeekTail()
    {
        return m_tail.m_data;
    }

    public T Dequeue()
    {
        if (m_count > 0)
        {
            T headData;
            headData = m_head.m_data;

            if (m_head.m_following != null)
            {
                m_head = m_head.m_following;
            }
            else
            {
                m_head = null;
                m_tail = null;
            }

            m_count--;

            return headData;
        }
        else
        {
            return null;
        }
    }

    public T PeekHead()
    {
        if (m_head != null)
        {
            return m_head.m_data;
        }
        else
        {
            return null;
        }
    }
}
