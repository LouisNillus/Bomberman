using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GridHandler : MonoBehaviour
{

    public static GridHandler instance;

    public int rows;
    public int lines;

    public Cell[] map;
    public GameObject tilePrefab;
    public GameObject wallPrefab;
    public GameObject pressurePlate;
    public GameObject unbreakableWall;
    public int tileResolution;

    public List<Player> players = new List<Player>();

    public Bounds bounds;

    public List<TextAsset> mapsFiles = new List<TextAsset>();

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
        //cellsDebug = CrossCells(player1.gameObject.transform.position, 3);

        if(Input.GetKeyDown(KeyCode.Y)) ReadMap();
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
        Cell c = NextCell(position, direction);

        if (c == null || c.occupied)
        {
            map[GetCellIndexFromPos(position)].occupied = true;            
            return map[GetCellIndexFromPos(position)]; //Si on peut pas aller à la suivante on renvoie la tuile sur laquelle on est
        }
        else
        {
            c.occupied = true;
            return c;
        }


    }

    public List<Cell> GetAllCellsOfType(EntityType type, bool checkDestroyable = true)
    {
        List<Cell> result = new List<Cell>();
        foreach(Cell c in map)
        {
            if(checkDestroyable)
            {
                if (c.type == type && c.destroyable) result.Add(c);
            }
            else
            {
                if(c.type == type) result.Add(c);
            }
        }

        return result;
    }

    public List<Cell> GetRandomCells(List<Cell> cells, int count)
    {
        List<Cell> result = cells;

        for (int i = (cells.Count-1); i > count; i--)
        {
            result.RemoveAt(Random.Range(0, (result.Count-1)));
        }

        return result;
    }

    public Cell NextCell(Vector3 position, Direction direction, int range = 1)
    {
        Cell c = null;

        switch (direction)
        {
            case Direction.Up:
                if (GetCellIndexFromPos(position) + (rows*range) < map.Length)
                {
                    c = map[GetCellIndexFromPos(position) + (rows*range)];
                }
                break;
            case Direction.Down:
                if (GetCellIndexFromPos(position) - (rows*range) >= 0)
                {
                    c = map[GetCellIndexFromPos(position) - (rows * range)];
                }
                break;
            case Direction.Right:
                if (GetCellIndexFromPos(position) + (1*range) < map.Length)
                {
                    c = map[GetCellIndexFromPos(position) + (1 * range)];
                }
                break;
            case Direction.Left:
                if (GetCellIndexFromPos(position) - (1*range) >= 0)
                {
                    c = map[GetCellIndexFromPos(position) - (1 * range)];
                }
                break;
        }

        return c;
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
        if(cell.occupied == false)
        {
            cell.entity = Instantiate(wallPrefab, cell.pos, Quaternion.identity);
            cell.type = EntityType.Wall;
            cell.occupied = true;
        }
    }

    public void SetUnbreakableWall(Cell cell)
    {
        cell.entity = Instantiate(unbreakableWall, cell.pos, Quaternion.identity);
        cell.type = EntityType.Wall;
        cell.occupied = true;
        cell.destroyable = false;
    }

    public void SetPressurePlate(Cell cell)
    {
        cell.entity = Instantiate(pressurePlate, cell.pos, Quaternion.identity);
        cell.type = EntityType.PressurePlate;
        cell.occupied = false;
        cell.destroyable = false;
    }

    public void CheckPlayer()
    {
        foreach(Cell c in map)
        foreach (Player p in players)
        {
            if (p.transform.position == c.pos)
            {
                c.player = p;
            }
            else
            {
                c.player = null;
            }
        }
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

        if (radius < 1) radius = 1;

        bool upBlock = false;
        bool downBlock = false;
        bool rightBlock = false;
        bool leftBlock = false;

        for (int i = 1; i <= radius; i++)
        {
            if (NextCell(from, Direction.Up, i) != null)
            {
                if(upBlock == false) cells.Add(NextCell(from, Direction.Up, i));
                if (NextCell(from, Direction.Up, i).type == EntityType.Wall) upBlock = true;
            }
            if (NextCell(from, Direction.Down, i) != null) 
            {
                if (downBlock == false) cells.Add(NextCell(from, Direction.Down, i));
                if (NextCell(from, Direction.Down, i).type == EntityType.Wall) downBlock = true;
            }
            if (NextCell(from, Direction.Right, i) != null)
            {
                if (rightBlock == false && PreventTeleport(GetCellFromPos(from), NextCell(from, Direction.Right, i)) == false) cells.Add(NextCell(from, Direction.Right, i));
                if (NextCell(from, Direction.Right, i).type == EntityType.Wall) rightBlock = true;
            }
            if (NextCell(from, Direction.Left, i) != null)
            {
                if (leftBlock == false && PreventTeleport(GetCellFromPos(from), NextCell(from, Direction.Left, i)) == false) cells.Add(NextCell(from, Direction.Left, i));
                if (NextCell(from, Direction.Left, i).type == EntityType.Wall) leftBlock = true;
            }
        }       

        if (includeSelf) cells.Add(GetCellFromPos(from));

        return cells;
    }

    public bool PreventTeleport(Cell from, Cell to)
    {
        if (to == null) return true;

        if ((from.pos.x < to.pos.x && from.pos.y > to.pos.y) || (from.pos.x > to.pos.x && from.pos.y < to.pos.y)) return true;
        else return false;
    }

    public bool ExistingIndex(int index)
    {
        return (index >= 0 && index < map.Length);
    }

    public void ReadMap()
    {
        TextAsset data = mapsFiles[UnityEngine.Random.Range(0, mapsFiles.Count)];

        string[] _lines = data.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        if(lines != _lines.Length) Debug.LogError("Lines count doesn't match with map file");

        for (int i = _lines.Length-1; i >= 0; i--)
        {
            string[] _rows = _lines[i].Split(',');

            if (rows != _rows.Length) Debug.LogError("Rows count doesn't match with map file");

            for (int j = 0; j < _rows.Length; j++)
            {
                Cell c = map[((_lines.Length - 1 - i) * _rows.Length) + j];

                switch (_rows[j])
                {
                    case "0":
                        break;
                    case "1":
                        SetWall(c);
                        break;
                    case "2":
                        SetUnbreakableWall(c);
                        break;
                    case "P":
                        SetPressurePlate(c);
                        break;
                    case "X":
                        players[0].transform.position = c.pos;
                        break;
                    case "Y":
                        players[1].transform.position = c.pos;
                        break;
                }
            }
        }
    }
}

[System.Serializable]
public class Cell
{
    public Vector3 pos = Vector3.zero;
    public GameObject tile;
    public GameObject entity;
    public Player player;
    public EntityType type;
    public bool occupied = false;
    public bool destroyable = true;

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
public enum EntityType {None, Bomb, Wall, PressurePlate}