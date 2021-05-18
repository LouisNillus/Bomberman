using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;

    [Range(0,10)]
    public int HP;

    public PlayerKeys keys;
    public GameObject bombPrefab;

    GridHandler gh;

    // Start is called before the first frame update
    void Start()
    {
        gh = GridHandler.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keys.keyRight))
        {
            gh.GetCellFromPos(this.transform.position).FreeCell();
            this.transform.position = gh.GoToNextCell(transform.position, Direction.Right).pos;
        }
        if (Input.GetKeyDown(keys.keyLeft))
        {
            gh.GetCellFromPos(this.transform.position).FreeCell();
            this.transform.position = gh.GoToNextCell(transform.position, Direction.Left).pos;
        }
        if (Input.GetKeyDown(keys.keyUp))
        {
            gh.GetCellFromPos(this.transform.position).FreeCell();
            this.transform.position = gh.GoToNextCell(transform.position, Direction.Up).pos;
        }
        if (Input.GetKeyDown(keys.keyDown))
        {
            gh.GetCellFromPos(this.transform.position).FreeCell();
            this.transform.position = gh.GoToNextCell(transform.position, Direction.Down).pos;
        }

        if(Input.GetKeyDown(keys.bomb))
        {
            if (gh.GetCellFromPos(transform.position).type == EntityType.None)
            {
                Instantiate(bombPrefab, this.transform.position, Quaternion.identity);
            }
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            foreach(Cell c in gh.GetAllEmptyCells())
            {
                gh.SetWall(c);
            }
        }
    }
}

[System.Serializable]
public class PlayerKeys
{
    public KeyCode keyUp;
    public KeyCode keyDown;
    public KeyCode keyLeft;
    public KeyCode keyRight;
    public KeyCode bomb;
    public KeyCode wall;
}
