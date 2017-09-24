using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : MonoBehaviour {
    
    public float moveSpeed;                          // Slime speed
    private Rigidbody2D slimeRigidbody;              // Getting rigidbody of slime
    private CircleCollider2D slimeCollider;          // Collider for the slime
    private PolygonCollider2D solidCollider;         // Collider for solid layer
    private bool moving = false;                     // Check if slime is moving
    public float timeBetweenMove;                    // Time in between random movements
    private float timeBetweenMoveCounter;            // Counts down the time in between random movements
    public float timeToMove;                         // Time taken to move
    private float timeToMoveCounter;                 // Counts down the time taken to move
    private Vector3 direction;                       // Direction of movement
    public float deathTimer;                         // Time taken to respawn
    private bool reloading;                          // Checks to see if the player is respawning
    private GameObject player;                       // Gets the player
    private GameObject crosshairs;                   // Getting the crosshairs
    private MouseController theMouse;                // Getting the mouse controller

    // Use this for initialization
    void Start () {
        slimeRigidbody = GetComponent<Rigidbody2D>();           // Getting Rigidbody 2D
        slimeCollider = GetComponent<CircleCollider2D>();       // Getting slime Circle Collider 2D
        // Getting solid layers collider
        solidCollider = GameObject.FindGameObjectWithTag("Solid").GetComponent<PolygonCollider2D>();
        randomTimeCounters(-1);
        crosshairs = transform.GetChild(0).gameObject;               // Getting the exit hover
        crosshairs.SetActive(false);                                 // Disable the exit hover
        theMouse = FindObjectOfType<MouseController>();     // Getting the mouse
    }
	
	// Update is called once per frame
	void Update () {
        Moving();
	}

    // Function that allows slime to move
    private void Moving() {
        // Checks if slime is moving
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
                randomTimeCounters(0);
            }
        } else {
            // Begin countdown
            timeBetweenMoveCounter -= Time.deltaTime;
            // Check if countdown at 0
            if (timeBetweenMoveCounter < 0f) {
                // Start moving
                moving = true;
                // Set countdown
                randomTimeCounters(1);
                // Pick new random location to move
                direction = new Vector3(Random.Range(-1f, 1f) * moveSpeed + transform.position.x, Random.Range(-1f, 1f) * moveSpeed + transform.position.y, transform.position.z);
            }
        }
        // Check if the player died
        if(reloading) {
            deathTimer -= Time.deltaTime;
            if(deathTimer < 0) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                player.gameObject.SetActive(true);
            }
        }
    }

    // FIX THIS SO IT'S NICER LATER
    // Set counter to random range of around the set times
    private void randomTimeCounters(int counterChosen) {
        if (counterChosen == 0) {
            timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        } else if (counterChosen == 1) {
            timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        } else {
            timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
            timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D coll) {
        // When mouse hovers over slime, activate crosshairs
        if (coll.gameObject.name == "Cursor") {
            crosshairs.SetActive(true);
            theMouse.attacking = true;
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        // Disable crosshairs when cursor goes off
        if (coll.gameObject.name == "Cursor") {
            crosshairs.SetActive(false);
            theMouse.attacking = false;
        }
    }
}
