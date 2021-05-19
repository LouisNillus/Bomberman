using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{

    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Cell cell = GridHandler.instance.GetCellFromPos(this.transform.position);
        foreach (Player player in GridHandler.instance.players)
        {
            if (GridHandler.instance.GetCellFromPos(player.transform.position) == cell)
            {
                Instantiate(particles, this.transform.position, Quaternion.identity);
                player.HP++;
                Destroy(this.gameObject);
            }
        }
    }
}
