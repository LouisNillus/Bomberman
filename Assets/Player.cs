using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;

    [Range(0,10)]
    public int HP;

    [Range(0,5)]
    public int wallsRemaining;

    public PlayerKeys keys;
    public GameObject bombPrefab;
    public GameObject wallPrefab;

    public Direction playerDirection = Direction.Down;

    GridHandler gh;

    // Start
    void Start()
    {
        gh = GridHandler.instance;
        id = int.Parse(Random.Range(0, 99).ToString() + Random.Range(0, 99).ToString());
        gh.players.Add(this);
    }

    // Update
    void Update()
    {
        if(Input.GetKeyDown(keys.keyRight) &&  gh.PreventTeleport(gh.GetCellFromPos(transform.position), gh.NextCell(transform.position, Direction.Right)) == false)
        {
            if (playerDirection == Direction.Right)
            {
                gh.GetCellFromPos(this.transform.position).FreeCell();
                this.transform.position = gh.GoToNextCell(transform.position, Direction.Right).pos;
                gh.CheckPlayer();
            }
            else playerDirection = Direction.Right;
        }
        else if (Input.GetKeyDown(keys.keyLeft) && gh.PreventTeleport(gh.GetCellFromPos(transform.position), gh.NextCell(transform.position, Direction.Left)) == false)
        {
            if (playerDirection == Direction.Left)
            {
                gh.GetCellFromPos(this.transform.position).FreeCell();
                this.transform.position = gh.GoToNextCell(transform.position, Direction.Left).pos;
                gh.CheckPlayer();
            }
            else playerDirection = Direction.Left;
        }
        else if (Input.GetKeyDown(keys.keyUp))
        {
            if (playerDirection == Direction.Up)
            {
                gh.GetCellFromPos(this.transform.position).FreeCell();
                this.transform.position = gh.GoToNextCell(transform.position, Direction.Up).pos;
                gh.CheckPlayer();
            }
            else playerDirection = Direction.Up;
        }
        else if (Input.GetKeyDown(keys.keyDown))
        {
            if (playerDirection == Direction.Down)
            {
                gh.GetCellFromPos(this.transform.position).FreeCell();
                this.transform.position = gh.GoToNextCell(transform.position, Direction.Down).pos;
                gh.CheckPlayer();
            }
            else playerDirection = Direction.Down;
        }

        if(Input.GetKeyDown(keys.bomb))
        {
            if (gh.NextCell(this.transform.position, playerDirection).type == EntityType.None && gh.PreventTeleport(gh.GetCellFromPos(transform.position), gh.NextCell(transform.position, playerDirection)) == false)
            {
                Instantiate(bombPrefab, gh.NextCell(this.transform.position, playerDirection).pos, Quaternion.identity);
            }
        }

        if (Input.GetKeyDown(keys.wall))
        {
            if (gh.NextCell(this.transform.position, playerDirection).type == EntityType.None && gh.PreventTeleport(gh.GetCellFromPos(transform.position), gh.NextCell(transform.position, playerDirection)) == false)
            {
                gh.SetWall(gh.NextCell(this.transform.position, playerDirection));
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
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
