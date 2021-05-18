using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCenterCamera : MonoBehaviour
{
    GridHandler gh;

    // Start
    void Start()
    {
        gh = GridHandler.instance;
        Camera.main.transform.position = new Vector3((gh.rows / 2) * gh.TileSize() - (gh.TileSize() / 2), (gh.lines / 2) * gh.TileSize() - (gh.TileSize()/2), -10);
    }
}
