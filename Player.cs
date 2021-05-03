using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Add announcement for entering new region
    
    [Header("Objects")]
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer spriteRenderer, lbSpriteRenderer;
    public GameObject nearestEnemy, carrotTextObject, tomatoTextObject, flagPole, lifeBar, lifeBarParent, weapon;
    public Collider2D playerCollider2D;
    public Info info;
    FlagPole flagPoleScript;
    TextMeshProUGUI carrotText, tomatoText;
    public AnnouncementText announcementText;
    public SceneLoader sceneLoader;

    [Header("Variables")]
    public string region;
    public float distanceToNearestEnemy, moveSpeed, health, maxHealth, percentage;
    public Vector2 movement, movementToEnemy;
    public bool isLevelComplete;

    [Header("Inventory")]
    public int tomatoes;
    public int carrots;
    public int eggs;
    public int wheat;

    private void Start()
    {
        region = "";

        playerCollider2D.isTrigger = false;

        distanceToNearestEnemy = 100;

        flagPoleScript = flagPole.GetComponent<FlagPole>();

        announcementText = GameObject.Find("Announcement Text").GetComponent<AnnouncementText>();

        lbSpriteRenderer = lifeBar.GetComponent<SpriteRenderer>();

        info = GameObject.FindGameObjectWithTag("Highlander").GetComponent<Info>();
        
        if (info.isSetPosition)
        {
            transform.position = info.playerPosition;
            info.isSetPosition = false;
        }

        carrotText = carrotTextObject.GetComponent<TextMeshProUGUI>();
        carrotText.color = new Color(0, 0, 0, 1);
        tomatoText = tomatoTextObject.GetComponent<TextMeshProUGUI>();
        tomatoText.color = new Color(0, 0, 0, 1);

        health = maxHealth;

        tomatoes = info.tomatoes;
        carrots = info.carrots;
        eggs = info.eggs;
        wheat = info.wheat;
        moveSpeed = info.playerSpeed;


    }

    private void Update()
    {
        if (health > 0)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            UpdateSprite();
            UpdateNearestEnemy();
            UpdateScore();
        }
        UpdateHealth();
        UpdateLifebar();
        info.UpdateInventory(carrots, tomatoes, eggs, wheat);
    }

    private void UpdateHealth()
    {
        // On death ...
        if (health <= 0)
        {
            region = "dead";
            announcementText.text = "Game Over";
            anim.SetBool("isDead", true);
            spriteRenderer.flipX = false;
            spriteRenderer.sortingLayerName = "Foreground Decoration";
            weapon.SetActive(false);
            movement = new Vector2(0, 0);
            playerCollider2D.isTrigger = true;
            StartCoroutine(LoadStartMenu());
        }
    }

    private void UpdateSprite()
    {
        anim.SetFloat("Speed", Mathf.Abs(movement.x) + Mathf.Abs(movement.y));
        if (movement.x > 0.01)
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x < -0.01)
        {
            spriteRenderer.flipX = true;
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed);
    }

    private void UpdateNearestEnemy()
    {
        if (nearestEnemy)
        {
            float deltaX = nearestEnemy.transform.position.x - rb.position.x;
            float deltaY = nearestEnemy.transform.position.y - rb.position.y;
            distanceToNearestEnemy = Mathf.Sqrt(Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2));

            // Creates a Vector2 for use by weapon item. The hypotenuse is one so move speed scaling is easier.
            if (distanceToNearestEnemy > 1)
            {
                float ratio = 1 / distanceToNearestEnemy;
                deltaX *= ratio;
                deltaY *= ratio;
            }
            movementToEnemy = new Vector2(deltaX, deltaY);
        }
        else
        {
            
            // Broaden search for new nearest enemy
            distanceToNearestEnemy = 100f;

            // Align default position of knife to current orientation of player (facing right or left)
            if (spriteRenderer.flipX)
            {
                movementToEnemy = new Vector2(-1, 0);
            }
            else
            {
                movementToEnemy = new Vector2(1, 0);
            }
            
        }
    }

    private void UpdateScore()
    {
        carrotText.text = carrots.ToString();
        tomatoText.text = tomatoes.ToString();

        // Updating color of text when enough items are collected to beat a level.
        if (carrots >= flagPoleScript.carrots)
        {
            carrotText.color = Color.green;
        }
        else
        {
            carrotText.color = Color.black;
        }

        if (tomatoes >= flagPoleScript.tomatoes)
        {
            tomatoText.color = Color.green;
        }
        else
        {
            tomatoText.color = Color.black;
        }

        if (flagPoleScript.maxCarrots == carrots && flagPoleScript.maxTomatoes == tomatoes)
        {
            announcementText.text = "We have all of the ingredients for this recipe. Time to head home!";
        }
    }

    private void UpdateLifebar()
    {
        percentage = health / maxHealth;
        if (percentage >= 0.67f)
        {
            lbSpriteRenderer.color = Color.green;
        }
        else if (percentage >= 0.34f)
        {
            lbSpriteRenderer.color = Color.yellow;
        }
        else if (percentage > 0f)
        {
            lbSpriteRenderer.color = Color.red;
        }
        else
        {
            lifeBarParent.SetActive(false);
        }
        lifeBar.transform.localScale = new Vector3(percentage * 0.94f, 0.7f);
        lifeBar.transform.localPosition = new Vector3((1 - percentage) * (-0.47f), 0f);
    }

    IEnumerator LoadStartMenu()
    {
        yield return new WaitForSeconds(3);
        sceneLoader.LoadSceneFromDirectReference(1);
    }
}
