using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridHandler : MonoBehaviour
{

    public static GridHandler instance;

    public int rows;
    public int lines;

    public Cell[] map;
    public GameObject tilePrefab;
    public GameObject wallPrefab;
    public int tileResolution;

    public Bounds bounds;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitMap()
    {
        map = new Cell[lines * rows];

        for (int i = 0; i < lines; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                map[(i * lines) + j] = new Cell(new Vector3(j, i, 0) * TileSize());
                map[(i * lines) + j].tile = Instantiate(tilePrefab, map[(i * lines) + j].pos, Quaternion.identity);
            }
        }

        bounds = GetBounds();
    }

    public Cell GetCellFromPos(Vector3 pos)
    {
        foreach (Cell c in map)
        {
            if (c.pos == pos) return c;
        }

        return null;
    }

    public int GetCellIndexFromPos(Vector3 pos)
    {
        foreach (Cell c in map)
        {
            if (c.pos == pos) return Array.IndexOf(map, c);
        }

        Debug.LogError("Can't find any cell index for this position in grid");
        return map.Length + 1;
    }

    public Cell GoToNextCell(Vector3 position, Direction direction)
    {
        Cell c = null;

        switch (direction)
        {
            case Direction.Up:
                if (GetCellIndexFromPos(position) + rows < map.Length)
                {
                    c = map[GetCellIndexFromPos(position) + rows];
                }
                break;
            case Direction.Down:
                if (GetCellIndexFromPos(position) - rows >= 0)
                {
                    c = map[GetCellIndexFromPos(position) - rows];
                }
                break;
            case Direction.Right:
                if (GetCellIndexFromPos(position) + 1 < map.Length)
                {
                    c = map[GetCellIndexFromPos(position) + 1];
                }
                break;
            case Direction.Left:
                if (GetCellIndexFromPos(position) - 1 >= 0)
                {
                    c = map[GetCellIndexFromPos(position) - 1];
                }
                break;
        }

        if (c == null || c.occupied)
        {
            map[GetCellIndexFromPos(position)].occupied = true;
            return map[GetCellIndexFromPos(position)]; //Si on peut pas aller à la suivante on renvoie le case sur laquelle on est
        }
        else
        {
            c.occupied = true;
            return c;
        }

    }

    public float TileSize()
    {
        return tileResolution / 100f;
    }
    public Bounds GetBounds()
    {
        return new Bounds((lines - 1) * TileSize(), 0, 0, (rows - 1) * TileSize());
    }

    public void SetWall(Cell cell)
    {
        Instantiate(wallPrefab, cell.pos, Quaternion.identity);
        cell.type = EntityType.Wall;
        cell.occupied = true;
    }

    public List<Cell> GetAllEmptyCells(bool includeBombs = false)
    {
        List<Cell> cells = new List<Cell>();

        if (includeBombs)
        {
            foreach(Cell c in map)
            {
                if (c.occupied == false) cells.Add(c);
            }
        }
        else
        {
            foreach (Cell c in map)
            {
                if (c.occupied == false && c.type != EntityType.Bomb) cells.Add(c);
            }
        }

        return cells;
    }

    public List<Cell> CrossCells(Vector3 from, int radius, bool includeSelf = true)
    {
        List<Cell> cells = new List<Cell>();

        foreach(Cell c in map)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)//TO COMPLETE
                {

                }
            }
        }

        return cells;
    }
}

[System.Serializable]
public class Cell
{
    public Vector3 pos = Vector3.zero;
    public GameObject tile;
    public EntityType type;
    public bool occupied = false;

    public Cell(Vector3 pos)
    {
        this.pos = pos;
    }

    public void FreeCell()
    {
        occupied = false;
    }
}

[System.Serializable]
public class Bounds
{
    public float top;
    public float bottom;
    public float left;
    public float right;

    public Bounds(float top, float bottom, float left, float right)
    {
        this.top = top;
        this.bottom = bottom;
        this.left = left;
        this.right = right;
    }
}

public enum Direction {Up, Down, Left, Right}
public enum EntityType {None, Bomb, Wall}