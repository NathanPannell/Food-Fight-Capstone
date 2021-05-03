using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public Player playerScript;
    public Collider2D playerCollider;
    Info info;

    [Header("Components")]
    public Rigidbody2D enemyRigidbody2D;
    public Animator enemyAnimator;
    public GameObject enemyItemDrop;
    public ParticleSystem particles;

    [Header("Variables")]
    public string enemyRegion;
    public float health;
    public float damage;
    public float awakenDistance;
    public float distanceToPlayer;
    public bool isDormant;
    public bool isJumping;

    [Header("Movement Variables")]
    public Vector2 movement;
    public float waitTime;
    public float moveTime;
    public float jumpLimit;
    public float verticalJumpModifier;

    [Header("Initial Randomization (Between 0 & 1)")]
    public float movementVariability;

    private void Start()
    {
        // Initialization of variables
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();
        playerCollider = player.GetComponent<Collider2D>();
        info = GameObject.FindGameObjectWithTag("Highlander").GetComponent<Info>();

        isDormant = true;
        isJumping = false;
        enemyAnimator.SetBool("isDormant", isDormant);

        // Minor randomization of movement variables
        waitTime *= Random.Range( 1 - movementVariability , 1 + movementVariability );
        moveTime *= Random.Range(1 - movementVariability, 1 + movementVariability);
        jumpLimit *= Random.Range(1 - movementVariability, 1 + movementVariability);
        verticalJumpModifier *= Random.Range(1 - movementVariability, 1 + movementVariability);
        awakenDistance = info.awakenDistance + Random.Range(0f, 1f);
    }

    private void Update()
    {
        if (isDormant == true)
        {
            UpdateDormant();
        }
        else
        {
            if (!isJumping && playerScript.region == enemyRegion)
            {
                //isJumping = true;
                CalculateMovement();
                StartCoroutine(MoveToPlayer());
                //isJumping = false;
            }
        }
    }
    private void FixedUpdate()
    {
        UpdateDistance();
        UpdateHealth();
    }
    private void CalculateMovement()
    {
        float deltaX = player.transform.position.x - enemyRigidbody2D.position.x;
        float deltaY = player.transform.position.y - enemyRigidbody2D.position.y;
        movement = new Vector2(deltaX, deltaY);
        if (distanceToPlayer > jumpLimit)
        {
            float ratio = jumpLimit / distanceToPlayer;
            movement *= ratio;
        }
    }

    IEnumerator MoveToPlayer()
    {
        float timePassed = 0;
        isJumping = true;

        movement.y += verticalJumpModifier;
        enemyAnimator.SetInteger("JumpProgress", 1);
        enemyAnimator.SetBool("IsJumping", true);
        while (timePassed < moveTime / 2)
        {
            timePassed += Time.deltaTime;
            transform.position = enemyRigidbody2D.position + movement * Time.deltaTime / moveTime;
            yield return null;
        }

        movement.y -= 2f * verticalJumpModifier;
        enemyAnimator.SetInteger("JumpProgress", 2);
        while (timePassed < moveTime)
        {
            timePassed += Time.deltaTime;
            transform.position = enemyRigidbody2D.position + movement * Time.deltaTime / moveTime;
            yield return null;
        }

        enemyAnimator.SetBool("IsJumping", false);
        enemyAnimator.SetInteger("JumpProgress", 0);
        yield return new WaitForSeconds(waitTime);
        isJumping = false;
    }

    private void UpdateHealth()
    {
        if (health <= 0)
        {
            playerScript.distanceToNearestEnemy = 100f;
            Instantiate(enemyItemDrop, transform.position, transform.rotation);
            Instantiate(particles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == playerCollider)
        {
            playerScript.health -= damage;
        }
    }

    private void UpdateDormant()
    {
        if (distanceToPlayer > 0 && distanceToPlayer <= awakenDistance)
        {
            isDormant = false;
        }
        enemyAnimator.SetBool("isDormant", isDormant);
    }

    private void UpdateDistance()
    {
        float deltaX = player.transform.position.x - enemyRigidbody2D.position.x;
        float deltaY = player.transform.position.y - enemyRigidbody2D.position.y;
        distanceToPlayer = Mathf.Sqrt(Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2));

        // Update player's nearest enemy
        if (playerScript.distanceToNearestEnemy > distanceToPlayer)
        {
            playerScript.nearestEnemy = gameObject;
            playerScript.distanceToNearestEnemy = distanceToPlayer;
        }
    }
}
