using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public GameObject player, announcementText;
    Player playerScript;
    Collider2D playerCollider;
    SceneLoader sceneLoader;
    AnnouncementText announcementTextScript;

    [Header("Requirement")]
    public int tomatoes;
    public int carrots;
    public int eggs;
    public int wheat;

    [Header("Maximum")]
    public int maxTomatoes;
    public int maxCarrots;
    public int maxEggs;
    public int maxWheat;

    private void Start()
    {
        player = GameObject.Find("Player");
        sceneLoader = FindObjectOfType<SceneLoader>();
        playerScript = player.GetComponent<Player>();
        playerCollider = player.GetComponent<Collider2D>();

        // WTF is this line lol
        announcementTextScript = announcementText.GetComponent<AnnouncementText>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerCollider)
        {
            playerScript.health = playerScript.maxHealth;
            if (playerScript.tomatoes >= tomatoes && playerScript.carrots >= carrots)
            {
                playerScript.isLevelComplete = true;
                sceneLoader.LoadSceneFromDirectReference(3);
            }
            else
            {
                int missingTomatoes = tomatoes - playerScript.tomatoes;
                int missingCarrots = carrots - playerScript.carrots;
                string message = "You need ";
                if (missingCarrots > 0 )
                {
                    if (missingCarrots == 1)
                    {
                        message += "1 more carrot ";
                    }
                    else
                    {
                        message += missingCarrots + " more carrots ";
                    }
                }
                if (missingTomatoes > 0)
                {
                    if (missingCarrots > 0)
                    {
                        message += "and ";
                    }
                    if (missingTomatoes == 1)
                    {
                        message += "1 more tomato ";
                    }
                    else
                    {
                        message += missingTomatoes + " more tomatoes ";
                    }
                }
                message += "to finish the recipe.";
                announcementTextScript.text = message;
            }
        }
    }
        
}
