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

        GridHandler.instance.GetCellFromPos(transform.position).type = EntityType.None;
        Destroy(this.gameObject);
    }

    public void Boom()
    {

    }
}
