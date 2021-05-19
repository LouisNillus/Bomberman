using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;

    [Range(0,10)]
    public int HP;
    [HideInInspector]
    public int initHP;

    [Range(0,10)]
    public int wallsRemaining;
    [HideInInspector]
    public int initWalls;

    public Color32 pickupColor;
    public Color32 damagesColor;


    [Range(0, 10)]
    public int bombRange = 2;
    [HideInInspector]
    public int initRange;

    public PlayerKeys keys;
    public GameObject bombPrefab;
    public GameObject wallPrefab;

    public Direction playerDirection;

    public Direction PlayerDirection
    {
        get => playerDirection;
        set
        {
            playerDirection = value;
            if (playerDirection == Direction.Up) animator.Play("Back");
            if (playerDirection == Direction.Down) animator.Play("Face");
            if (playerDirection == Direction.Right) animator.Play("Right");
            if (playerDirection == Direction.Left) animator.Play("Left");
        }
    }

    public Animator animator;

    GridHandler gh;


    // Start
    void Start()
    {
        initHP = HP;
        initWalls = wallsRemaining;
        initRange = bombRange;

        gh = GridHandler.instance;
        //id = int.Parse(Random.Range(0, 99).ToString() + Random.Range(0, 99).ToString());
        gh.players.Add(this);   
        
        PlayerDirection = Direction.Down;
    }

    // Update
    void Update()
    {
        if(Input.GetKeyDown(keys.keyRight) &&  gh.PreventTeleport(gh.GetCellFromPos(transform.position), gh.NextCell(transform.position, Direction.Right)) == false)
        {
            if (PlayerDirection == Direction.Right)
            {
                gh.GetCellFromPos(this.transform.position).FreeCell();
                this.transform.position = gh.GoToNextCell(transform.position, Direction.Right).pos;
                gh.CheckPlayer();
            }
            else PlayerDirection = Direction.Right;
        }
        else if (Input.GetKeyDown(keys.keyLeft) && gh.PreventTeleport(gh.GetCellFromPos(transform.position), gh.NextCell(transform.position, Direction.Left)) == false)
        {
            if (PlayerDirection == Direction.Left)
            {
                gh.GetCellFromPos(this.transform.position).FreeCell();
                this.transform.position = gh.GoToNextCell(transform.position, Direction.Left).pos;
                gh.CheckPlayer();
            }
            else PlayerDirection = Direction.Left;
        }
        else if (Input.GetKeyDown(keys.keyUp))
        {
            if (PlayerDirection == Direction.Up)
            {
                gh.GetCellFromPos(this.transform.position).FreeCell();
                this.transform.position = gh.GoToNextCell(transform.position, Direction.Up).pos;
                gh.CheckPlayer();
            }
            else PlayerDirection = Direction.Up;
        }
        else if (Input.GetKeyDown(keys.keyDown))
        {
            if (PlayerDirection == Direction.Down)
            {
                gh.GetCellFromPos(this.transform.position).FreeCell();
                this.transform.position = gh.GoToNextCell(transform.position, Direction.Down).pos;
                gh.CheckPlayer();
            }
            else PlayerDirection = Direction.Down;
        }

        if(Input.GetKeyDown(keys.bomb))
        {
            if (gh.NextCell(this.transform.position, PlayerDirection).type == EntityType.None && gh.PreventTeleport(gh.GetCellFromPos(transform.position), gh.NextCell(transform.position, PlayerDirection)) == false)
            {
                Instantiate(bombPrefab, gh.NextCell(this.transform.position, PlayerDirection).pos, Quaternion.identity).GetComponent<Bomb>().range = bombRange;
            }
        }

        if (Input.GetKeyDown(keys.wall) && wallsRemaining > 0)
        {
            if (gh.NextCell(this.transform.position, PlayerDirection).type == EntityType.None && gh.PreventTeleport(gh.GetCellFromPos(transform.position), gh.NextCell(transform.position, PlayerDirection)) == false)
            {
                wallsRemaining--;
                gh.SetWall(gh.NextCell(this.transform.position, PlayerDirection));
            }
        }
    }
    public void TakeDamages(int amount)
    {
        HP -= amount;
        StartCoroutine(DamagesFeedback(0.5f));
        CheckDeath();
    }

    public void CheckDeath()
    {
        if (HP <= 0)
        {
            string winner = "Player ";
            foreach(Player p in gh.players)
            {
                if (p.id != id) winner = "Player " + p.id.ToString();
            }
            StartCoroutine("EndGame", winner);
        }
    }

    public IEnumerator EndGame(string p_winner)
    {
        yield return new WaitForSeconds(2.0f);
        EndMenu.instance.ShowEndMenu(p_winner);
    }

    public void ResetStats()
    {
        HP = initHP;
        bombRange = initRange;
        wallsRemaining = initWalls;
    }

    public IEnumerator DamagesFeedback(float duration)
    {
        GetComponent<SpriteRenderer>().color = damagesColor;
        yield return new WaitForSeconds(duration);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public IEnumerator PickupFeedback(float duration)
    {
        GetComponent<SpriteRenderer>().color = pickupColor;
        yield return new WaitForSeconds(duration);
        GetComponent<SpriteRenderer>().color = Color.white;
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
