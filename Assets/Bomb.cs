using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionTimer;
    public int range = 1;

    public Bomb(float explosionTimer)
    {
        this.explosionTimer = explosionTimer;
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
        Boom();

        foreach (Cell c in GridHandler.instance.CrossCells(this.transform.position, range))
        {
            if (c.entity != null && c.destroyable)
            {
                Destroy(c.entity);
                c.FreeCell();
            }
        }

        GridHandler.instance.GetCellFromPos(transform.position).type = EntityType.None;
        Destroy(this.gameObject);
    }

    public void Boom()
    {

    }
}
