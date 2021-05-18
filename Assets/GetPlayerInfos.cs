﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetPlayerInfos : MonoBehaviour
{

    TextMeshProUGUI value;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        value = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        value.text = "x" + player.wallsRemaining.ToString();
    }
}