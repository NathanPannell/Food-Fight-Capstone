using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public bool isAutoAim, isSetPosition;
    public int previousScene;
    public int currentScene;
    public float awakenDistance;
    public float playerSpeed;
    public Vector3 playerPosition;

    [Header("Player Inventory")]
    public int carrots;
    public int tomatoes;
    public int eggs;
    public int wheat;
    
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Highlander");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void UpdateInventory(int c, int t, int e, int w)
    {
        carrots = c;
        tomatoes = t;
        eggs = e;
        wheat = w;
    }

    public void ResetInventory()
    {
        carrots = 0;
        tomatoes = 0;
        eggs = 0;
        wheat = 0;
    }

    public void SetPlayerPosition(Vector3 position)
    {
        playerPosition = position;
    }
}
