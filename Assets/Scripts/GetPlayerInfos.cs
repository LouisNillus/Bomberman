using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetPlayerInfos : MonoBehaviour
{

    TextMeshProUGUI value;
    public Player player;

    public bool hp;
    public bool range;

    // Start
    void Start()
    {
        value = GetComponent<TextMeshProUGUI>();
    }

    // Update
    void Update()
    {
        if(hp) value.text = "x" + player.HP.ToString();
        else if(range) value.text = player.bombRange.ToString();
        else value.text = "x" + player.wallsRemaining.ToString();
    }
}
