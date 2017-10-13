using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    private PlayerController thePlayer;             // Getting the player
    private Animator myAnimator;                    // Animator
    private Collider2D myCollider;                  // Collider
    public bool attacking = false;                  // Check if the player is attacking
    public bool selecting = false;                  // Check if the player is selecting
    public GameObject targetedObject = null;        // Set current targeted object
    public GameObject targetingObject = null;       // Set the hovered over object
    public bool targetingEnemy = false;             // Check if the player is targeting an object
    public static bool cursorExists = false;        // Check if mouse exists
    public bool exitingAfterMovement = false;       // Change level after movement

    void Awake () {
        // Sets the default cursor to invisible
        Cursor.visible = false;
    }

	// Use this for initialization
	void Start () {
        // Check between levels if cursor exists
        if (!cursorExists) {
            cursorExists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(transform.gameObject);
        }
        myAnimator = GetComponent<Animator>();                    // Getting Animator
        thePlayer = FindObjectOfType<PlayerController>();         // Getting the player
        myCollider = GetComponent<Collider2D>();                  // Getting Collider
    }
	
	// Update is called once per frame
	void Update () {
        // Make the cursor follow the real mouse
        transform.position = mouseScreenPositioning(transform.position);
        // Set mouse selecting animation when right click is down
        if (Input.GetMouseButton(1)) {
            selecting = true;
        } else {
            selecting = false;
        }
        // Set parameters in animator
        myAnimator.SetBool("Selecting", selecting);
        myAnimator.SetBool("Attacking", attacking);
    }

    // Right click
    public void OnMouseRightClick() {
        if (Input.GetMouseButtonDown(1)) {
            thePlayer.rightMouseClicked = true;
            thePlayer.playerMoving = true;
            // If there is a target object
            if (targetingObject != null) {
                targetedObject = targetingObject;
                // Check if clicked on an enemy
                if (targetingObject.tag == "Enemy") {
                    // MouseWorldSpace becomes enemy location
                    if (myCollider.IsTouching(targetingObject.GetComponent<CircleCollider2D>())) {
                        targetingEnemy = true;
                        targetedObject.GetComponent<SlimeController>().targeted = true;
                    }
                } else if (targetingObject.tag == "Exit") {
                    // MouseWorldSpace becomes exit location
                    exitingAfterMovement = true;
                    thePlayer.attacking = false;
                }
            } else {
                targetingEnemy = false;
                targetedObject = null;
                thePlayer.engaging = false;
                thePlayer.attacking = false;
            }
            // Otherwise, mouseWorldSpace becomes location clicked on
            thePlayer.mouseWorldSpace = mouseScreenPositioning(thePlayer.mouseWorldSpace);
            thePlayer.lastLocation = thePlayer.transform.position;
        }
    }
    
    // Mouse real coordinates to level coordinates
    public Vector3 mouseScreenPositioning(Vector3 mouseWorldSpace) {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldSpace.z = 0;
        return mouseWorldSpace;
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        // When mouse hovers over slime, activate crosshairs and set target enemy
        if (coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<SlimeController>().crosshairs.SetActive(true);
            attacking = true;
            targetingObject = coll.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        // Disable crosshairs when cursor goes off
        if (coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<SlimeController>().crosshairs.SetActive(false);
            attacking = false;
            targetingObject = null;
        }
    }
}
