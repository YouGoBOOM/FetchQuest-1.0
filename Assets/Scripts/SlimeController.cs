using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : EnemyController {
    
    public float moveSpeed;                          // Slime speed
    private CircleCollider2D slimeCollider;          // Collider for the slime
    private PolygonCollider2D solidCollider;         // Collider for solid layer
    private bool moving = false;                     // Check if slime is moving
    public float timeBetweenMove;                    // Time in between random movements
    public float timeBetweenMoveCounter;             // Counts down the time in between random movements
    public float timeToMove;                         // Time taken to move
    public float timeToMoveCounter;                  // Counts down the time taken to move
    private Vector3 direction;                       // Direction of movement
    public float deathTimer;                         // Time taken to respawn
    
    public GameObject crosshairs;                    // Getting the crosshairs
    // public bool targetted = false;                   // Check if targetted
    public bool attacking = false;                   // Check if the slime is attacking

    // Use this for initialization
    void Start () {
        slimeCollider = GetComponent<CircleCollider2D>();            // Getting slime Circle Collider 2D
        // Getting solid layers collider
        solidCollider = GameObject.FindGameObjectWithTag("Solid").GetComponent<PolygonCollider2D>();
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        crosshairs = transform.GetChild(0).gameObject;               // Crosshairs
        crosshairs.SetActive(false);                                 // Disable the crosshairs
        // Getting the player
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        theMouse = FindObjectOfType<MouseController>().gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        Moving();
    }

    // When destroyed, untarget itself
    void OnDestroy () {
        thePlayer.GetComponent<PlayerController>().engaging = false;
        targetted = false;
        theMouse.GetComponent<MouseController>().targettedEnemy = null;
        theMouse.GetComponent<MouseController>().targetting = false;
        thePlayer.GetComponent<PlayerController>().enemyDied = true;
    }

    // Function that allows slime to move
    private void Moving() {
        // Checks if slime is moving
        if (!attacking) {
            if (moving) {
                // Begin countdown
                timeToMoveCounter -= Time.deltaTime;
                // FIX ALONG WITH PLAYER COLLISION WITH SOLID
                // Check if slime is colliding with solid layer
                if (!(slimeCollider.IsTouching(solidCollider))) {
                    // Slime is moving
                    transform.position = Vector3.MoveTowards(transform.position, direction, moveSpeed * Time.deltaTime);
                }
                // Check if countdown at 0
                if (timeToMoveCounter < 0f) {
                    // Stop moving
                    moving = false;
                    // Set countdown
                    timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
                }
            } else {
                // Begin countdown
                timeBetweenMoveCounter -= Time.deltaTime;
                // Check if countdown at 0
                if (timeBetweenMoveCounter < 0f) {
                    // Start moving
                    moving = true;
                    // Set countdown
                    timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
                    // Pick new random location to move
                    direction = new Vector3(Random.Range(-1f, 1f) * moveSpeed + transform.position.x, Random.Range(-1f, 1f) * moveSpeed + transform.position.y, transform.position.z);
                }
            }
        } else {

        }
        // Check if the player died
        // Need this code snippet for later maybe
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
}
