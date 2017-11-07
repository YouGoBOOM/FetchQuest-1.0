using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : MonoBehaviour {

    public GameObject thePlayer;                        // Gets the player
    public GameObject theMouse;                         // Gets the cursor
    public bool targeted = false;                       // Check if the enemy is targeted
    public GameObject crosshairs;                       // Getting the crosshairs
    public float moveSpeed;                             // Enemy speed
    private Rigidbody2D enemyRigidbody;                 // Rigidbody for the enemy
    private CircleCollider2D enemyCollider;             // Collider for the enemy
    private PolygonCollider2D solidCollider;            // Collider for solid layer
    private bool moving = false;                        // Check if enemy is moving
    public float timeBetweenMove;                       // Time in between random movements
    public float timeBetweenMoveCounter;                // Counts down the time in between random movements
    public float timeToMove;                            // Time taken to move
    public float timeToMoveCounter;                     // Counts down the time taken to move
    private Vector3 direction;                          // Direction of movement
    public float deathTimer;                            // Time taken to respawn
    public float attackTimer;                           // Attack timer of enemy
    public float attackTimerCounter;                    // Counter for the attack timer
    public bool attacking = false;                      // Check if the enemy is attacking
    public bool attackCooldown = false;                 // Sets the attack cooldown
    public int attackDamage;                            // Amount of damage enemy does per hit
    public int defense;                                 // Amount of defense the enemy has
    public int critChance;                              // Amount of critical hit chance
    public float critMultiplier;                        // Amount of critical hit
    public int reduceDamageChance;                      // Amount of damage reduction chance
    public float reduceDamageMultiplier;                // Amount of damage reduction
    public int missChance;                              // Amount of miss chance
    public float attackRange;                           // Attack range of enemy
    public bool engaging = false;                       // Check if the enemy is engaging the player
    public bool resetHostile = false;                   // Resets hostile state
    public bool hardResetHostile = false;               // Check if first reset in a while
    public float hostileTimer;                          // Time to cool down hostile state
    public float hostileTimerCounter;                   // Counter for the hostile timer
    public float distanceFromPlayer;                    // Distance from player

    // Use this for initialization
    void Start () {
        enemyCollider = GetComponent<CircleCollider2D>();            // Getting enemy Circle Collider 2D
        enemyRigidbody = GetComponent<Rigidbody2D>();                // Getting enemy Rigidbody
        // Getting solid layers collider
        solidCollider = GameObject.FindGameObjectWithTag("Solid").GetComponent<PolygonCollider2D>();
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        crosshairs = transform.GetChild(0).gameObject;               // Crosshairs
        crosshairs.SetActive(false);                                 // Disable the crosshairs
        // Getting the player
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        // Getting the mouse
        theMouse = FindObjectOfType<MouseController>().gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        CheckHostile();
        // Checks if the enemy is not attacking
        if (!engaging) {
            Moving();
        // Check if the enemy is attacking
        } else {
            AttackPlayer();
        }
    }

    // When destroyed, untarget itself
    void OnDestroy () { 
        thePlayer.GetComponent<PlayerController>().engaging = false;
        targeted = false;
        theMouse.GetComponent<MouseController>().targetedObject = null;
        theMouse.GetComponent<MouseController>().targetingEnemy = false;
        thePlayer.GetComponent<PlayerController>().enemyDied = true;
    }

    // Function that allows enemy to move
    private void Moving() {
        // Checks if enemy is moving
        if (moving) {
            // Begin countdown
            timeToMoveCounter -= Time.deltaTime;
            // Move
            enemyRigidbody.velocity = direction;
            // Check if countdown at 0
            if (timeToMoveCounter < 0f) {
                // Stop moving
                moving = false;
                // Set countdown
                timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
            }
        } else {
            // Don't move
            enemyRigidbody.velocity = Vector2.zero;
            // Begin countdown
            timeBetweenMoveCounter -= Time.deltaTime;
            // Check if countdown at 0
            if (timeBetweenMoveCounter < 0f) {
                // Start moving
                moving = true;
                // Set countdown
                timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
                // Pick new random location to move
                //direction = new Vector3(Random.Range(-1f, 1f) * moveSpeed + transform.position.x, Random.Range(-1f, 1f) * moveSpeed + transform.position.y, transform.position.z);
                direction = new Vector2(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed);
            }
        }
        
        // Check if the player died
        // TODO: Need this code snippet for later maybe
        /*
        if (reloading) {
            deathTimer -= Time.deltaTime;
            if(deathTimer < 0) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                thePlayer.gameObject.SetActive(true);
            }
        }
        */
    }

    // Auto-attack the player
    // TODO: SET THIS TO AUTO ATTACK FUNCTION
    public void AttackPlayer() {
        PlayerLevelStats playerStats = thePlayer.GetComponent<PlayerLevelStats>();
        EnemyStatsManager enemyStats = gameObject.GetComponent<EnemyStatsManager>();
        if (distanceFromPlayer <= attackRange) {
            // Don't move if in attack range
            enemyRigidbody.velocity = Vector2.zero;
            // Check if one of the 2 halves is over
            if (attackTimerCounter <= 0) {
                // Set the counter back to the max
                attackTimerCounter = attackTimer;
                // If attack is not on cooldown after one half
                if (!attackCooldown) {
                    int i = enemyStats.enemyLevel;
                    float enemyDamageAfterMultipliers = attackDamage;
                    // Check if player is higher level than enemy
                    if (enemyStats.enemyLevel > playerStats.currentLevel) {
                        while (i > playerStats.currentLevel) {
                            // Multiply damage by 150%
                            enemyDamageAfterMultipliers *= 1.5f;
                            i--;
                        }
                    } else if (enemyStats.enemyLevel < playerStats.currentLevel) {
                        while (i < playerStats.currentLevel) {
                            // Reduce damage by 50%
                            enemyDamageAfterMultipliers *= 0.5f;
                            i++;
                        }
                    }
                    // Equation for the damage reduction per defense
                    float damageReductionPercentage = 100f - Mathf.Pow(10f, 2 - 0.0030103f * playerStats.defense);
                    enemyDamageAfterMultipliers -= enemyDamageAfterMultipliers * (damageReductionPercentage / 100);
                    // Crit chance
                    enemyDamageAfterMultipliers = CheckChance(enemyDamageAfterMultipliers, critChance, critMultiplier);
                    // Reduce damage chance
                    enemyDamageAfterMultipliers = CheckChance(enemyDamageAfterMultipliers, playerStats.reduceDamageChance, -playerStats.reduceDamageMultiplier);
                    // Miss chance
                    enemyDamageAfterMultipliers = CheckChance(enemyDamageAfterMultipliers, playerStats.missChance, -100);
                    // Deal damage
                    thePlayer.GetComponent<PlayerHealthManager>().SetCurrentHeatlh(-Mathf.RoundToInt(enemyDamageAfterMultipliers), true);
                    // Set the cooldown to true
                    attackCooldown = true;
                } else {
                    // Set the cooldown to false
                    attackCooldown = false;
                }
            }
            // Countdown time
            attackTimerCounter -= Time.deltaTime;
        } else {
            // Enemy is moving
            //transform.position = Vector3.MoveTowards(transform.position, thePlayer.transform.position, moveSpeed * Time.deltaTime);
            enemyRigidbody.velocity = new Vector2((thePlayer.transform.position.x - transform.position.x) / distanceFromPlayer * moveSpeed, (thePlayer.transform.position.y - transform.position.y) / distanceFromPlayer * moveSpeed);
        }
    }

    // Check if the enemy is still hostile with the player
    public void CheckHostile() {
        // Check if the hostile state timer is refreshed
        if (resetHostile) {
            hostileTimerCounter = hostileTimer;
            resetHostile = false;
            engaging = true;
            // Reset if haven't reset in a while
            if (!hardResetHostile) {
                attackTimerCounter = attackTimer;
                attackCooldown = true;
                timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
                timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
                hardResetHostile = true;
            }
        }
        // Countdown hostile timer
        if (engaging == true) {
            hostileTimerCounter -= Time.deltaTime;
            // Set to docile if hostile state over
            if (hostileTimerCounter <= 0) {
                engaging = false;
                hardResetHostile = false;
            }
            // Find distance from player
            distanceFromPlayer = (transform.position - thePlayer.transform.position).magnitude;
        }
    }

    // Check if chance is less than roll
    private float CheckChance(float value, int chance, float multiplier)    {
        float randomRoll = Random.Range(0, 101);
        if (randomRoll <= chance) {
            value += value * (multiplier / 100);
        }
        return value;
    }
}
