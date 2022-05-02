using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCluster : MonoBehaviour
{
    List<Bubble> m_bubbles = new List<Bubble>();

    public void AddBubble(Bubble bubble)
    {
        m_bubbles.Add(bubble);
    }

    public void ClearBubbles()
    {
        if (m_bubbles.Count > 0)
            m_bubbles.Clear();
    }

    public int GetCount()
    {
        return m_bubbles.Count;
    }

    public void Fall()
    {
        for (int i = 0; i < m_bubbles.Count; i++)
        {
            m_bubbles[i].GetComponent<Collider2D>().enabled = false;
            m_bubbles[i].Dir = new Vector3(0.0f, -1.0f, 0.0f);
            m_bubbles[i].bShouldDie = true;
            m_bubbles[i].bIsFired = true;
        }
    }
}
