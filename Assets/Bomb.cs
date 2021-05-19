using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionTimer;
    public GameObject explosion;
    public int range = 1;

    public Bomb(int range)
    {
        this.range = range;
    }

    private void OnEnable()
    {
        GridHandler.instance.GetCellFromPos(transform.position).type = EntityType.Bomb;
        StartCoroutine(Explode());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionTimer);

        foreach (Cell c in GridHandler.instance.CrossCells(this.transform.position, range))
        {
            if (c.entity != null && c.destroyable)
            {
                Destroy(c.entity);               
                c.FreeCell();
                c.type = EntityType.None;
            }

            
            foreach (Player player in GridHandler.instance.players)
            {
                if (GridHandler.instance.GetCellFromPos(player.transform.position) == c)
                {
                    player.TakeDamages(1);
                }
            }

            Instantiate(explosion, c.pos, Quaternion.identity);
        }

        GridHandler.instance.GetCellFromPos(transform.position).type = EntityType.None;
        GridHandler.instance.GetCellFromPos(transform.position).FreeCell();
        Destroy(this.gameObject);
    }

    public void Boom()
    {

    }
}
