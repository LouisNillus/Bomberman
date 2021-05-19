using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionTimer;
    public GameObject explosion;
    public int range = 1;
    public Color32 blinkColor;
    public AnimationClip animationClip;
    public Animator animator;
    SpriteRenderer sr;

    public Bomb(int range)
    {
        this.range = range;
    }

    private void OnEnable()
    {       
        sr = GetComponent<SpriteRenderer>();
        GridHandler.instance.currentBombs.Add(this);
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

    public void StopExplosion()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }


    public IEnumerator Explode()
    {
        float time = 0f;
        float duration = explosionTimer - animationClip.length;

        while (time < duration)
        {
            sr.color = Color.Lerp(Color.white, blinkColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }


        animator.SetTrigger("Bomb");

        //sr.color = Color.white;
        yield return new WaitForSeconds(animationClip.length);

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

            if(c.type != EntityType.UnbreakableWall) Instantiate(explosion, c.pos, Quaternion.identity);
        }

        GridHandler.instance.GetCellFromPos(transform.position).type = EntityType.None;
        GridHandler.instance.GetCellFromPos(transform.position).FreeCell();
        GridHandler.instance.currentBombs.Remove(this);
        Destroy(this.gameObject);
    }
}
