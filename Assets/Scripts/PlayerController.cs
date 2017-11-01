﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Declaring Variables
    public float moveSpeed;                  // Walking Movement Speed
    public bool rightMouseClicked = false;   // Check if right mouse was clicked
    public Vector3 mouseWorldSpace;          // Make target for right click global
    private Animator animator;               // Animator
    private Collider2D playerCollider;       // Collider for player
    public PolygonCollider2D solidCollider;  // Collider for solid layer
    public bool playerMoving = false;        // Check if player is moving
    public float direction;                  // Current direction
    public float lastDirection;              // Last direction when idle
    public Vector3 lastLocation;             // Last location
    public bool attacking = false;           // Check if player is attacking
    public float attackTimer;                // Attack timer of player
    public float attackTimerCounter;         // Counter for the attack timer
    public bool attackCooldown = false;      // Sets the attack cooldown
    public static bool playerExists = false; // Check if the player already exists
    private MouseController theCursor;       // Getting the mouse
    public float distanceFromEnemy;          // Distance from target enemy
    public float attackRange;                // Distance from target enemy
    public int playerDamage;                 // Amount of damage the player deals
    public bool engaging = false;            // Check if player is engaging
    public bool enemyDied = false;           // Check if the enemy died
    public bool firstChecker = false;        // Check if this is the first time this is called
    public GameObject currentExit;           // The current exit the player is going towards
    public string startPoint;                // The specific start point
    
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
        playerCollider = GetComponent<Collider2D>();        // Getting player Collider 2D
        theCursor = FindObjectOfType<MouseController>();    // Getting the mouse
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
                playerMoving = true;
                engaging = false;
            } else {
                // Attack enemy when within range
                lastDirection = direction;
                playerMoving = false;
                AttackEnemy(theCursor.targetedObject);
                mouseWorldSpace = transform.position;
                direction = CalculateDirection(theCursor.targetedObject.transform.position.x, theCursor.targetedObject.transform.position.y, transform.position.x, transform.position.y);
            }
        }
        // When the targeted enemy dies, reset attack animation
        ResetAttackAnimationOnEnemyDeath();
        if (transform.position == mouseWorldSpace || playerCollider.IsTouching(solidCollider)) {
            // Stop moving if at target or touching solid
            if (!engaging) {
                StopMoving();
            }
        } else if (rightMouseClicked == true && transform.position != mouseWorldSpace) {
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
    private float CalculateDirection(float targetLocationX, float targetLocationY, float currentLocationX, float currentLocationY) {
        float angle;
        float direction;
        float deltaX = targetLocationX - currentLocationX;
        float deltaY = targetLocationY - currentLocationY;
        if (targetLocationX > currentLocationX) {
            angle = 2 * Mathf.PI + Mathf.Atan2(deltaY, deltaX);
        } else if (targetLocationX < currentLocationX) {
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
        direction = Mathf.RoundToInt(angle);
        if (direction == 8f) {
            return 0f;
        } else {
            return direction;
        }
    }

    // Move player to target
    private void MovingToTarget(Vector3 target) {
        direction = CalculateDirection(target.x, target.y, transform.position.x, transform.position.y);
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    // Stop moving
    private void StopMoving() {
        lastDirection = direction;
        rightMouseClicked = false;
        playerMoving = false;
        // Collision check
        if (playerCollider.IsTouching(solidCollider))
        {
            transform.position = Vector3.MoveTowards(transform.position, lastLocation, moveSpeed * Time.deltaTime);
        }
        if (theCursor.exitingAfterMovement) {
            theCursor.exitingAfterMovement = false;
            currentExit.GetComponent<Exit>().MoveToLevel();
        }
    }

    // Attack an enemy
    private void AttackEnemy(GameObject enemy) {
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
                if (enemy.GetComponent<SlimeController>().targeted == true && engaging) {
                    // Deal damage
                    enemy.GetComponent<EnemyStatsManager>().SetEnemyHealth(-playerDamage, true);
                    // Make the enemy hostile
                    enemy.GetComponent<SlimeController>().resetHostile = true;
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
}