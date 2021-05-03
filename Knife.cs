using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [Header("Objects")]
    public GameObject player;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;
    Info info;
    Player playerScript;
    Collider2D playerCollider;

    [Header("Variables")]
    public Vector2 movement;
    public float speed;
    public float damage;
    public bool isMoving;

    private void Start()
    {
        // Initialization of variables
        info = GameObject.Find("Info").GetComponent<Info>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();
        playerCollider = player.GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (isMoving)
        {
            UpdateMovement();
        }
        else
        {
            if (info.isAutoAim)
            {
                AutoAim();
            }
            else
            {
                MouseAim();
            }

            if (Input.GetMouseButtonDown(0))
            {
                CreateInstance();
            }
        }
    }

    private void MouseAim()
    {
        movement.x = Input.mousePosition.x - 960;
        movement.y = Input.mousePosition.y - 540;
        CalculateDirection(movement.x, movement.y);
        float hyp = Mathf.Sqrt(Mathf.Pow(movement.x, 2) + Mathf.Pow(movement.y, 2));
        if (hyp > 1)
        {
            float ratio = 1 / hyp;
            movement *= ratio;
        }
    }

    private void UpdateMovement()
    {
        // ignoring barriers, the knife moves in a straight line
        transform.position = rb.position + movement * speed * Time.deltaTime;
    }

    private void CreateInstance()
    {
        audioSource.Play();
        GameObject instance = Instantiate(gameObject, transform.position, transform.rotation);
        Knife instanceScript = instance.GetComponent<Knife>();
        instanceScript.isMoving = true;
    }

    private void AutoAim()
    {
        movement = playerScript.movementToEnemy;
        CalculateDirection(movement.x, movement.y);
    }

    private void CalculateDirection(float deltaX, float deltaY)
    {
        float theta = Mathf.Atan(Mathf.Abs(deltaY / deltaX)) * 180 / Mathf.PI;
        if (deltaX < 0)
        {
            if (deltaY < 0)
            {
                theta += 180f;
            }
            else if (deltaY > 0)
            {
                theta = 180f - theta;
            }
            else if (deltaY == 0)
            {
                theta = 180f;
            }
        }
        else if (deltaX > 0)
        {
            if (deltaY < 0)
            {
                theta = 360 - theta;
            }
            else if (deltaY == 0)
            {
                theta = 0f;
            }
        }
        else if (deltaX == 0)
        {
            if (deltaY > 0)
            {
                theta = 90f;
            }
            else if (deltaY < 0)
            {
                theta = 270f;
            }
            else if (deltaY == 0)
            {
                theta = 0f;
            }
        }

        // Rotate sprite
        transform.eulerAngles = new Vector3(0f, 0f, theta);

        // Flip sprite to be right-side up
        if (theta < 270 && theta > 90)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != playerCollider && isMoving)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy && enemy.isDormant == false)
            {
                enemy.health -= damage;
                Destroy(gameObject);
            }
            
            Damage damageScript = collision.GetComponent<Damage>();
            if (damageScript)
            {
                damageScript.health -= damage;
                Destroy(gameObject);
            }
        }
    }
}
