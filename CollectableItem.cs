using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public bool Tomato;
    public bool Carrot;
    public GameObject player, enemy;
    public AudioSource audioSource;
    public AudioClip clip, pop;
    Player playerScript;
    Collider2D playerCollider;


    private void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();
        playerCollider = player.GetComponent<Collider2D>();
        audioSource.PlayOneShot(clip);
    }

    private void Update()
    {
        UpdateRespawn();
    }

    private void UpdateRespawn()
    {
        float deltaX = player.transform.position.x - transform.position.x;
        float deltaY = player.transform.position.y - transform.position.y;
        float distance = Mathf.Sqrt(Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2));
        if (distance > 13)
        {
            Instantiate(enemy, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerCollider)
        {
            if (Tomato)
            {
                playerScript.tomatoes++;
            }
            if (Carrot)
            {
                playerScript.carrots++;
            }
            AudioSource.PlayClipAtPoint(pop, new Vector3(0, 0, 0));
            Destroy(gameObject);
        }
        
    }
}
