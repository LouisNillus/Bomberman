using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{

    public static GridHandler instance;

    public int rows;
    public int lines;

    public Cell[] map;
    public GameObject tilePrefab;
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
                map[(i * lines) + j].go = Instantiate(tilePrefab, map[(i * lines) + j].pos, Quaternion.identity);
            }
        }

        bounds = GetBounds();
    }



    public float TileSize()
    {
        return tileResolution / 100f;
    }
    public Bounds GetBounds()
    {
        return new Bounds((lines-1) * TileSize(), 0, 0, (rows-1) * TileSize());
    }
}

[System.Serializable]
public class Cell
{
    public Vector3 pos = Vector3.zero;
    public bool test;
    public GameObject go;

    public Cell(Vector3 pos)
    {
        this.pos = pos;
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