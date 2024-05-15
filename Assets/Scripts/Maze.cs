using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Maze : MonoBehaviour
{
    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north;
        public GameObject south;
        public GameObject east;
        public GameObject west;
    }

    public GameObject wall;
    public GameObject floor;
    public float wallLength = 1.0f;
    public float floorLength = 1.0f;
    public int xSize = 5;
    public int ySize = 5;
    private Vector3 initialPos;
    private GameObject wallHolder;
    private GameObject floorHolder;
    private Cell[] cells;
    private int currentCell = 0;
    private int totalCells;
    private int visitedCells = 0;
    private bool startedBuilding = false;
    private int currentNeighbor = 0;
    private List<int> lastCells;
    private int backingUp = 0;
    private int wallToBreak = 0;
    // Start is called before the first frame update
    void Start()
    {
        createFloors();
        CreateWalls();
    }

    void createFloors()
    {
        floorHolder = new GameObject();
        floorHolder.name = "Maze";
        initialPos = new Vector3(-xSize / 2 + floorLength / 2, 0.0f, -ySize / 2 + floorLength / 2);
        Vector3 myPos = initialPos;
        GameObject tempFloor;

        for(int i = 0; i < ySize; i++)
        {
            for(int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + j * floorLength, 0.0f, initialPos.z + i * floorLength - floorLength / 2);
                tempFloor = Instantiate(floor, myPos, Quaternion.identity) as GameObject;
                tempFloor.transform.parent = floorHolder.transform;
            }
        }

    }

    void CreateWalls()
    {
        wallHolder = new GameObject();
        wallHolder.name = "Maze";
        initialPos = new Vector3(-xSize / 2 + wallLength / 2, 0.0f, -ySize / 2 + wallLength / 2);
        Vector3 myPos = initialPos;
        GameObject tempWall;

        for(int i = 0; i < ySize; i++)
        {
            for(int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + j * wallLength - wallLength / 2, 2.5f, initialPos.z + i * wallLength - wallLength / 2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + j * wallLength, 2.5f, initialPos.z + i * wallLength - wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        CreateCells();
    }

    void CreateCells()
    {
        lastCells = new List<int>();
        lastCells.Clear();
        totalCells = xSize * ySize;
        GameObject[] allWalls;
        int children = wallHolder.transform.childCount;
        allWalls = new GameObject[children];
        cells = new Cell[xSize * ySize];
        int eastWestProcess = 0;
        int childProcess = 0;
        int termCount = 0;

        for(int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        for(int cellProcess = 0; cellProcess < cells.Length; cellProcess++)
        {

            if(termCount == xSize)
            {
                eastWestProcess++;
                termCount = 0;
            }

            cells[cellProcess] = new Cell();
            cells[cellProcess].east = allWalls[eastWestProcess];
            cells[cellProcess].south = allWalls[childProcess + (xSize + 1) * ySize];
            eastWestProcess++;
            termCount++;
            childProcess++;
            cells[cellProcess].west = allWalls[eastWestProcess];
            cells[cellProcess].north = allWalls[(childProcess + (xSize + 1) * ySize) + xSize - 1];
        }

        CreateMaze();
    }

    void CreateMaze()
    {
        if(visitedCells < totalCells)
        {
            if (startedBuilding)
            {
                GiveMeNeighbor();
                if (cells[currentNeighbor].visited == false && cells[currentCell].visited == true)
                {
                    BreakWall();
                    cells[currentNeighbor].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbor;
                    if(lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = Random.Range(0, totalCells);
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }

            Invoke("CreateMaze", 0.0f);
        }
    }

    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1:
                Destroy(cells[currentCell].north); break;
            case 2:
                Destroy(cells[currentCell].south); break;
            case 3:
                Destroy(cells[currentCell].east); break;
            case 4:
                Destroy(cells[currentCell].west); break;
        }
    }

    void GiveMeNeighbor()
    {
        int length = 0;
        int[] neighbors = new int[4];
        int[] connectingWall = new int[4];
        int check = 0;

        check = (currentCell + 1) / xSize;
        check -= 1;
        check *= xSize;
        check += xSize;

        

        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neighbors[length] = currentCell + xSize;
                connectingWall[length] = 1;
                length++;
            }
        }
        
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neighbors[length] = currentCell - xSize;
                connectingWall[length] = 2;
                length++;
            }
        }

        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neighbors[length] = currentCell - 1;
                connectingWall[length] = 3;
                length++;
            }
        }

        if(currentCell + 1 < totalCells && currentCell + 1 != check)
        {
            if (cells[currentCell + 1].visited == false)
            {
                neighbors[length] = currentCell + 1;
                connectingWall[length] = 4;
                length++;
            }
        }

        if(length != 0)
        {
            int theChosenOne = Random.Range(0, length);
            currentNeighbor = neighbors[theChosenOne];
            wallToBreak = connectingWall[theChosenOne];
        }
        else
        {
            if(backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}