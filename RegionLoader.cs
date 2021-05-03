using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionLoader : MonoBehaviour
{
    GameObject player;
    Collider2D playerCollider;
    Player playerScript;
    
    public string RegionName;

    private void Start()
    {
        // Initialization of variables
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<Collider2D>();
        playerScript = player.GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerCollider)
        {
            playerScript.region = RegionName;
        }
    }
}
