using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGrid : MonoBehaviour
{
    //Using a 2D array to represent the grid
    Bubble[,] m_grid;
    bool[,] m_visitedCells;

    BubbleCluster m_bubbleCluster;

    public int GridDimX = 10;
    public int GridDefaultY = 5;
    public float GridSpacing = 1.2f;
    public GameObject BubblePrefab;

    int GridDimY = 20;

    //public bool CalculatedCluster;

    // Start is called before the first frame update
    void Start()
    {
        //Create a 2D array to hold the bubbles, then generate the bubbles in it
        m_grid = new Bubble[GridDimX, GridDimY];

        //Create a grid of visited cells
        m_visitedCells = new bool[GridDimX, GridDimY];

        GenerateBubbles();

        m_bubbleCluster = new BubbleCluster();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindCluster(List<IntVector2> coords, Color color)
    {
        for (int i = coords.Count - 1; i > -1; i--)
        {
            if (!AreCoordsValid(coords[i]) || m_grid[coords[i].x, coords[i].y] == null)
                coords.RemoveAt(i);
        }

        m_bubbleCluster.ClearBubbles();

        ClearVisitedCells();

        List<IntVector2> coordsToVisit = coords;

        while(coordsToVisit.Count > 0)
        {
            int indexToRemove = coordsToVisit.Count - 1;
            IntVector2 currentCoords = coordsToVisit[indexToRemove];
            coordsToVisit.RemoveAt(indexToRemove);

            if (m_visitedCells[currentCoords.x, currentCoords.y])
            {
                continue;
            }

            m_visitedCells[currentCoords.x, currentCoords.y] = true;

            Bubble bubble = m_grid[currentCoords.x, currentCoords.y];
            if (bubble.GetColor() != color)
            {
                continue;
            }

            m_bubbleCluster.AddBubble(bubble);

            AddCoordsIfNeeded(currentCoords, new IntVector2(1, 0), ref coordsToVisit); //right
            AddCoordsIfNeeded(currentCoords, new IntVector2(-1, 0), ref coordsToVisit); //left
            AddCoordsIfNeeded(currentCoords, new IntVector2(0, 1), ref coordsToVisit); //up 
            AddCoordsIfNeeded(currentCoords, new IntVector2(0, -1), ref coordsToVisit); //down
        }

        if (m_bubbleCluster.GetCount() > 0)
        {
            m_bubbleCluster.Fall();
        }
    }

    void AddCoordsIfNeeded(IntVector2 coords, IntVector2 checkDir, ref List<IntVector2> coordsToVisit)
    {
        IntVector2 nextCoords = coords + checkDir;

        if (AreCoordsValid(nextCoords) && m_grid[nextCoords.x, nextCoords.y] != null)
        {
            coordsToVisit.Add(nextCoords);
        }
    }

    //This is a helper function to check if a set of coordinates are valid.  (out of bounds)
    bool AreCoordsValid(IntVector2 coords)
    {
        return (coords.x >= 0 && coords.y >= 0 &&
            coords.x < m_grid.GetLength(0) && coords.y < m_grid.GetLength(1));
    }

    void ClearVisitedCells()
    {
        for (int x = 0; x < GridDimX; ++x)
        {
            for (int y = 0; y < GridDimY; ++y)
            {
                m_visitedCells[x, y] = false;
            }
        }
    }

    //Creates the cubes in the right position and puts them in the grid    
    private void GenerateBubbles()
    {
        for (int x = 0; x < GridDimX; x++)
        {
            for (int y = 0; y < GridDefaultY; y++)
            {
                Vector3 offset = new Vector3(x * GridSpacing, y * GridSpacing, 0.0f);

                GameObject bubble = (GameObject)GameObject.Instantiate(BubblePrefab);

                bubble.transform.position = transform.position - offset;

                bubble.transform.parent = transform;

                bubble.GetComponent<Bubble>().name = "Bubble" + x + y;

                m_grid[x, y] = bubble.GetComponent<Bubble>();
            }
        }
    }

    public IntVector2 GetCell(GameObject obj)
    {
        for (int x = 0; x < GridDimX; x++)
        {
            for (int y = 0; y < GridDimY; y++)
            {
                if (obj.GetComponent<Bubble>() == m_grid[x, y])
                {
                    return new IntVector2(x, y);
                }
            }
        }

        return new IntVector2(0, 0);
    }

    public void AddBubbleToGrid(GameObject bubble, IntVector2 loc)
    {
        if (!AreCoordsValid(loc) && m_grid[loc.x, loc.y] == null)
        {
            Destroy(bubble);
            return;
        }

        bubble.GetComponent<Bubble>().bIsFired = false;
        bubble.GetComponent<Bubble>().bShouldDie = false;

        Vector3 offset = new Vector3(loc.x * GridSpacing, loc.y * GridSpacing, 0.0f);

        bubble.transform.position = transform.position - offset;

        bubble.transform.parent = transform;

        bubble.GetComponent<Bubble>().name = "Bubble" + loc.x + loc.y;

        m_grid[loc.x, loc.y] = bubble.GetComponent<Bubble>();
    }
}