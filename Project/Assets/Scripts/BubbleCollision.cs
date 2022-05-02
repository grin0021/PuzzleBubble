using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCollision : MonoBehaviour
{
    SphereGrid m_grid;

    bool active = true;

    void Start()
    {
        m_grid = GameObject.Find("SphereGrid").GetComponent<SphereGrid>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (active)
        {
            IntVector2 hitCoords = m_grid.GetCell(col.gameObject);
            Vector3 pos = transform.position;
            Vector3 hitPos = col.transform.position;
            IntVector2 gridPos = new IntVector2();

            if (pos.x - hitPos.x >= -0.5f && pos.x - hitPos.x <= 0.5f)
            {
                gridPos = new IntVector2(hitCoords.x, hitCoords.y + 1);
                m_grid.AddBubbleToGrid(gameObject, gridPos);
                active = false;
            }
            else
            {
                if (pos.x - hitPos.x < -0.5f)
                {
                    if (hitPos.y - pos.y < 0.75f)
                    {
                        gridPos = new IntVector2(hitCoords.x + 1, hitCoords.y);
                        m_grid.AddBubbleToGrid(gameObject, gridPos);
                        active = false;
                    }
                }
                else
                {
                    if (hitPos.y - pos.y < 0.75f)
                    {
                        gridPos = new IntVector2(hitCoords.x - 1, hitCoords.y);
                        m_grid.AddBubbleToGrid(gameObject, gridPos);
                        active = false;
                    }
                }
            }

            List<IntVector2> searchCoords = new List<IntVector2>();

            if (gridPos != new IntVector2(0, 0))
            {
                searchCoords.Add(new IntVector2(gridPos.x, gridPos.y + 1));
                searchCoords.Add(new IntVector2(gridPos.x, gridPos.y - 1));
                searchCoords.Add(new IntVector2(gridPos.x + 1, gridPos.y));
                searchCoords.Add(new IntVector2(gridPos.x - 1, gridPos.y));

                m_grid.FindCluster(searchCoords, GetComponent<Bubble>().GetColor());
            }
        }
    }
}