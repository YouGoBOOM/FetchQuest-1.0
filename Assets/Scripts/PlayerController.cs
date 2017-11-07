using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Declaring Variables
    public float moveSpeed;                     // Walking Movement Speed
    public bool rightMouseClicked = false;      // Check if right mouse was clicked
    public Vector3 mouseWorldSpace;             // Make target for right click global
    private Animator animator;                  // Animator
    private Rigidbody2D myRigidbody;            // Rigidbody
    private Collider2D playerCollider;          // Collider for player
    public PolygonCollider2D solidCollider;     // Collider for solid layer
    public bool playerMoving = false;           // Check if player is moving
    public float deltaX;                        // Getting the change in x direction
    public float deltaY;                        // Getting the change in y direction
    public float magnitude;                     // Getting the magnitude of the direction
    public float angle;                         // Getting the angle of the player
    public float direction;                     // Current direction
    public float lastDirection;                 // Last direction when idle
    public bool attacking = false;              // Check if player is attacking
    public float attackTimer;                   // Attack timer of player
    public float attackTimerCounter;            // Counter for the attack timer
    public bool attackCooldown = false;         // Sets the attack cooldown
    public static bool playerExists = false;    // Check if the player already exists
    private MouseController theCursor;          // Getting the mouse
    private PlayerLevelStats playerStats;       // Getting the player stats
    public float distanceFromEnemy;             // Distance from target enemy
    public float attackRange;                   // Distance from target enemy
    public int weaponDamage;                    // Amount of damage the weapon deals
    public int armourDefense;                   // Amount of defense from the armour
    public int weaponCritChance;                // Amount of critical hit chance from the weapon
    public float weaponCritMultiplier;          // Amount of critical hit from the weapon
    public int armourReduceDamageChance;        // Amount of damage reduction chance
    public float armourReduceDamageMultiplier;  // Amount of damage reduction
    public int armourMissChance;                // Amount of miss chance
    public bool engaging = false;               // Check if player is engaging
    public bool enemyDied = false;              // Check if the enemy died
    public bool firstChecker = false;           // Check if this is the first time this is called
    public GameObject currentExit;              // The current exit the player is going towards
    public string startPoint;                   // The specific start point

    // Use this for initialization
    void Start () {
        // Check between levels if player exists
        if (!playerExists) {
            playerExists = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        
        animator = GetComponent<Animator>();                // Getting Animator
        myRigidbody = GetComponent<Rigidbody2D>();          // Getting Rigidbody
        playerCollider = GetComponent<Collider2D>();        // Getting player Collider 2D
        theCursor = FindObjectOfType<MouseController>();    // Getting the mouse
        // Getting the player level stats
        playerStats = gameObject.GetComponent<PlayerLevelStats>();
        // Getting solid layers collider
        solidCollider = GameObject.FindGameObjectWithTag("Solid").GetComponent<PolygonCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        // Right click movement
        theCursor.OnMouseRightClick();
        // Check if an enemy is targeted
        if (theCursor.targetingEnemy) {
            // Get the distance from the targeted enemy
            distanceFromEnemy = (transform.position - theCursor.targetedObject.transform.position).magnitude;
            if (distanceFromEnemy > attackRange) {
                // If player targeted enemy, walk towards enemy until at attack range
                mouseWorldSpace = theCursor.targetedObject.transform.position;
                rightMouseClicked = true;
                playerMoving = true;
                engaging = false;
            } else {
                // Attack enemy when within range
                StopMoving();
                AttackEnemy(theCursor.targetedObject);
                mouseWorldSpace = transform.position;
                direction = CalculateDirection(theCursor.targetedObject.transform.position.x - transform.position.x, theCursor.targetedObject.transform.position.y - transform.position.y);
            }
        }
        // When the targeted enemy dies, reset attack animation
        ResetAttackAnimationOnEnemyDeath();
        if (rightMouseClicked == true) {
            // Move until at target
            MovingToTarget(mouseWorldSpace);
        }
        
        // Set parameters in animator
        animator.SetFloat("CurrentDirection", direction);
        animator.SetFloat("LastDirection", lastDirection);
        animator.SetBool("PlayerMoving", playerMoving);
        animator.SetBool("PlayerAttacking", attacking);
    }
    
    // Calculates direction of movement
    // Returns direction as float
    // 0 = right, 1 = up-right, 2 = up, 3 = up-left, 4 = left, 5 = down-left, 6 = down, 7 = down-right
    private float CalculateDirection(float deltaX, float deltaY) {
        if (deltaX > 0) {
            angle = 2 * Mathf.PI + Mathf.Atan2(deltaY, deltaX);
        } else if (deltaX < 0) {
            angle = 2 * Mathf.PI + Mathf.Atan2(deltaY, deltaX);
        } else {
            if (deltaY > 0f) {
                angle = Mathf.PI / 2;
            } else {
                angle = 3 * Mathf.PI / 2;
            }
        }
        if (angle >= 2 * Mathf.PI) {
            angle -= 2 * Mathf.PI;
        }
        angle /= (Mathf.PI / 4);
        angle = Mathf.RoundToInt(angle);
        if (angle == 8f) {
            return 0f;
        } else {
            return angle;
        }
    }

    // Move player to target
    private void MovingToTarget(Vector3 target) {
        deltaX = target.x - transform.position.x;                   // Find delta x
        deltaY = target.y - transform.position.y;                   // Find delta y
        magnitude = (target - transform.position).magnitude;        // Find the magnitude
        // Check if not engaging
        if (!engaging) {
            // If distance longer than one frame worth of movement
            if (magnitude > moveSpeed * Time.deltaTime) {
                // Move towards it
                myRigidbody.velocity = new Vector2((deltaX / magnitude) * moveSpeed, (deltaY / magnitude) * moveSpeed);
            } else {
                StopMoving();
            }
            direction = CalculateDirection(deltaX, deltaY);             // Find the direction
        }
        
    }

    // Stop moving
    private void StopMoving() {
        lastDirection = direction;
        rightMouseClicked = false;
        playerMoving = false;
        myRigidbody.velocity = Vector2.zero;
        if (theCursor.exitingAfterMovement) {
            theCursor.exitingAfterMovement = false;
            currentExit.GetComponent<Exit>().MoveToLevel();
        }
    }

    // Attack an enemy
    private void AttackEnemy(GameObject enemy) {
        // Reduce amount of getting component
        SlimeController targetedEnemyController = enemy.GetComponent<SlimeController>();
        EnemyStatsManager targetedEnemyStats = enemy.GetComponent<EnemyStatsManager>();
        // 3 stages to attack, first half windup, instance of damage, second half cooldown
        if (!engaging) {
            attackTimerCounter = attackTimer;
            engaging = true;
            attackCooldown = false;
            attacking = false;
        }
        // Check if one of the 2 halves is over
        if (attackTimerCounter <= 0) {
            // Set the counter back to the max
            attackTimerCounter = attackTimer;
            // If attack is not on cooldown after one half
            if (!attackCooldown) {
                // Check what player is targeting
                if (targetedEnemyController.targeted == true && engaging) {
                    int i = playerStats.currentLevel;
                    float playerDamageAfterMultipliers = playerStats.attack;
                    // Add power to damage
                    playerDamageAfterMultipliers += playerStats.power;
                    // Check if player is higher level than enemy
                    if (playerStats.currentLevel > targetedEnemyStats.enemyLevel) {
                        while (i > targetedEnemyStats.enemyLevel) {
                            // Multiply damage by 150%
                            playerDamageAfterMultipliers *= 1.5f;
                            i--;
                        }
                    } else if (playerStats.currentLevel < targetedEnemyStats.enemyLevel) {
                        while (i < targetedEnemyStats.enemyLevel) {
                            // Reduce damage by 50%
                            playerDamageAfterMultipliers *= 0.5f;
                            i++;
                        }
                    }
                    // Equation for the damage reduction per defense
                    float damageReductionPercentage = 100f - Mathf.Pow(10f, 2 - 0.0030103f * targetedEnemyController.defense);
                    playerDamageAfterMultipliers -= playerDamageAfterMultipliers * (damageReductionPercentage / 100);
                    // Crit chance
                    playerDamageAfterMultipliers = CheckChance(playerDamageAfterMultipliers, weaponCritChance, weaponCritMultiplier);
                    // Reduce damage chance
                    playerDamageAfterMultipliers = CheckChance(playerDamageAfterMultipliers, targetedEnemyController.reduceDamageChance, -targetedEnemyController.reduceDamageMultiplier);
                    // Miss chance
                    playerDamageAfterMultipliers = CheckChance(playerDamageAfterMultipliers, targetedEnemyController.missChance, -100);
                    // Deal damage
                    targetedEnemyStats.SetEnemyHealth(-Mathf.RoundToInt(playerDamageAfterMultipliers), true);
                    // Make the enemy hostile
                    targetedEnemyController.resetHostile = true;
                }
                // Set the cooldown to true
                attackCooldown = true;
                // Set animation to attacking
                attacking = true;
            } else {
                // Set the cooldown to false
                attackCooldown = false;
                // Set the animation to idling
                attacking = false;
            }
        }
        // Countdown time
        attackTimerCounter -= Time.deltaTime;
    }

    // Reset attack animation on enemy death
    private void ResetAttackAnimationOnEnemyDeath() {
        // Check if enemy died
        if (enemyDied) {
            // Check if the timer has been set
            if (!firstChecker) {
                attackTimerCounter = attackTimer;
                firstChecker = true;
            }
            attackTimerCounter -= Time.deltaTime;
            // Check if the counter has counted down compeletely
            if (attackTimerCounter <= 0) {
                // Reset everything
                attacking = false;
                firstChecker = false;
                enemyDied = false;
                attackTimerCounter = 0;
            }
        }
    }

    // Check if chance is less than roll
    private float CheckChance(float value, int chance, float multiplier) {
        float randomRoll = Random.Range(0, 101);
        if (randomRoll <= chance) {
            value += value * (multiplier / 100);
        }
        return value;
    }
}