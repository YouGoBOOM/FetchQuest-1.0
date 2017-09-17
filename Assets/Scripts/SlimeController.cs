using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour {
    
    public float moveSpeed;                 // Slime speed
    private Rigidbody2D slimeRigidbody;     // Getting rigidbody of slime
    private bool moving = false;            // Check if slime is moving
    public float timeBetweenMove;           // Time in between random movements
    private float timeBetweenMoveCounter;   // Counts down the time in between random movements
    public float timeToMove;                // Time taken to move
    private float timeToMoveCounter;        // Counts down the time taken to move
    private Vector3 direction;              // Direction of movement

	// Use this for initialization
	void Start () {
        //Setting counters at set times
        timeBetweenMoveCounter = timeBetweenMove;
        timeToMoveCounter = timeToMove;
	}
	
	// Update is called once per frame
	void Update () {
		// Checks if slime is moving
        if (moving) {
            // Begin countdown
            timeToMoveCounter -= Time.deltaTime;
            // Slime is moving
            transform.position = Vector3.MoveTowards(transform.position, direction, moveSpeed * Time.deltaTime);
            // Check if countdown at 0
            if (timeToMoveCounter < 0f) {
                // Stop moving
                moving = false;
                // Set countdown
                timeBetweenMoveCounter = timeBetweenMove;
            }
        } else {
            // Begin countdown
            timeBetweenMoveCounter -= Time.deltaTime;
            // Check if countdown at 0
            if (timeBetweenMoveCounter < 0f) {
                // Start moving
                moving = true;
                // Set countdown
                timeToMoveCounter = timeToMove;
                // Pick new random location to move
                direction = new Vector3(Random.Range(-1f, 1f) * moveSpeed + transform.position.x, Random.Range(-1f, 1f) * moveSpeed + transform.position.y, transform.position.z);
            }
        }
	}
}
