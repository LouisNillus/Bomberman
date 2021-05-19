using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    private void OnDestroy()
    {
        GridHandler.instance.GetCellFromPos(this.transform.position).type = EntityType.None;
        //GridHandler.instance.GetCellFromPos(this.transform.position).FreeCell();
    }
}
