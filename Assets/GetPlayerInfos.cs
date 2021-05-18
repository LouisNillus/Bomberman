using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetPlayerInfos : MonoBehaviour
{

    TextMeshProUGUI value;
    public Player player;

    // Start
    void Start()
    {
        value = GetComponent<TextMeshProUGUI>();
    }

    // Update
    void Update()
    {
        value.text = "x" + player.wallsRemaining.ToString();
    }
}
