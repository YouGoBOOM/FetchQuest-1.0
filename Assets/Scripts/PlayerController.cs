using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Initializing Variables
    public float moveSpeed = 4f;             // Walking Movement Speed
    public bool rightMouseClicked = false;   // Check if right mouse was clicked
    public Vector3 mouseWorldSpace;          // Make target for right click global
    private Animator animator;               // Animator
    private Rigidbody2D playerRigidbody;     // Rigidbody
    private Collider2D playerCollider;       // Collider for player
    public PolygonCollider2D solidCollider;  // Collider for solid layer
    public bool playerMoving = false;        // Check if player is moving
    public float direction;                  // Current direction
    public float lastDirection;              // Last direction when idle
    public Vector3 lastLocation;             // Last location
    public static bool playerExists = false; // Determines if the player already exists
    
    // Use this for initialization
	void Start () {
        // Check between levels if player exists
        if (!playerExists) {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(gameObject);
        }
        
        animator = GetComponent<Animator>();                // Getting Animator
        playerRigidbody = GetComponent<Rigidbody2D>();      // Getting Rigidbody 2D
        playerCollider = GetComponent<Collider2D>();        // Getting player Collider 2D
    }
	
	// Update is called once per frame
	void Update () {

        // Right click movement
        OnMouseRightClick();
        if (transform.position == mouseWorldSpace || playerCollider.IsTouching(solidCollider)) {
            // When at target or at something solid, stop moving
            lastDirection = direction;
            rightMouseClicked = false;
            playerMoving = false;
            // Collision check
            if (playerCollider.IsTouching(solidCollider)) {
                transform.position = Vector3.MoveTowards(transform.position, lastLocation, moveSpeed * Time.deltaTime);
            }
        } else if (rightMouseClicked == true && transform.position != mouseWorldSpace) {
            // Move until at target
            direction = CalculateDirection(mouseWorldSpace.x, mouseWorldSpace.y, transform.position.x, transform.position.y);
            transform.position = Vector3.MoveTowards(transform.position, mouseWorldSpace, moveSpeed * Time.deltaTime);
        }

        // Set parameters in animator
        animator.SetFloat("CurrentDirection", direction);
        animator.SetFloat("LastDirection", lastDirection);
        animator.SetBool("PlayerMoving", playerMoving);
    }

    // Right click screen to world positioning
    private void OnMouseRightClick() {
        if (Input.GetMouseButtonDown(1)) {
            rightMouseClicked = true;
            playerMoving = true;
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldSpace.z = 0;
            lastLocation = transform.position;
        }
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
        // Mathf.Round rounds 0.5 to the even number
        // 10.5 --> 10
        // Created own rounding function
        if (angle % 1f >= 0.5f) {
            angle += 0.5f;
        }
        direction = Mathf.Floor(angle);
        if (direction == 8f) {
            return 0f;
        } else {
            return direction;
        }
    }
}