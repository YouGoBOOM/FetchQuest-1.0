using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    private PlayerController thePlayer;                 // Getting the player
    private Animator myAnimator;                        // Animator
    public bool attacking = false;                      // Check if the player is attacking
    public bool selecting = false;                      // Check if the player is selecting
    public GameObject targetedObject = null;            // Set current targeted object
    public GameObject targetingObject = null;           // Set the hovered over object
    public GameObject oldTargetedObject = null;         // Set old targeted object
    public bool targetingEnemy = false;                 // Check if the player is targeting an object
    public static bool cursorExists = false;            // Check if mouse exists
    public bool exitingAfterMovement = false;           // Change level after movement
    public GameObject targetedShownEnemy = null;        // Set shown enemy clicked on
    public GameObject targetingShownEnemy = null;       // Set shown enemy moused over
    public GameObject shownEnemy = null;                // Set shown enemy
    public UITargetedObjectManager shownEnemyManager;   // Get manager of shown enemy  

    void Awake () {
        // Sets the default cursor to invisible
        Cursor.visible = false;
    }

	// Use this for initialization
	void Start () {
        // Check between levels if cursor exists
        if (!cursorExists) {
            cursorExists = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        // On load, these turn into missing, so set to null
        targetedObject = null;
        targetingObject = null;

        myAnimator = GetComponent<Animator>();                    // Getting Animator
        thePlayer = FindObjectOfType<PlayerController>();         // Getting the player
        // Getting the UI Targeted Object Manager
    }
	
	// Update is called once per frame
	void Update () {
        // Make the cursor follow the real mouse
        transform.position = MouseScreenPositioning(transform.position);
        // Set mouse selecting animation when right click is down
        if (Input.GetMouseButton(1)) {
            selecting = true;
        } else {
            selecting = false;
        }
        // Set shown enemy
        SetShownEnemy();
        // Set activeness of object
        if (shownEnemy == null) {
            shownEnemyManager.gameObject.SetActive(false);
        } else {
            shownEnemyManager.gameObject.SetActive(true);
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
                thePlayer.attacking = false;
                oldTargetedObject = targetedObject;
                targetedObject = targetingObject;
                // Check if the old clicked object was an enemy
                if (oldTargetedObject != null) {
                    if (oldTargetedObject.tag == "Enemy") {
                        // Untarget old enemy
                        oldTargetedObject.GetComponent<SlimeController>().targeted = false;
                    }
                }
                // Check if clicked on an enemy
                if (targetingObject.tag == "Enemy") {
                    // MouseWorldSpace becomes enemy location
                    targetingEnemy = true;
                    targetedObject.GetComponent<SlimeController>().targeted = true;
                    targetedShownEnemy = targetedObject;
                } else if (targetingObject.tag == "Exit") {
                    // MouseWorldSpace becomes exit location
                    exitingAfterMovement = true;
                    targetingEnemy = false;
                    thePlayer.engaging = false;
                    thePlayer.attacking = false;
                    targetedShownEnemy = null;
                }
            } else {
                targetingEnemy = false;
                targetedObject = null;
                thePlayer.engaging = false;
                thePlayer.attacking = false;
                targetedShownEnemy = null;
            }
            // Otherwise, mouseWorldSpace becomes location clicked on
            thePlayer.mouseWorldSpace = MouseScreenPositioning(thePlayer.mouseWorldSpace);
            thePlayer.lastLocation = thePlayer.transform.position;
        }
    }
    
    // Mouse real coordinates to level coordinates
    public Vector3 MouseScreenPositioning(Vector3 mouseWorldSpace) {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldSpace.z = 0;
        return mouseWorldSpace;
    }

    // Pick proper shown enemy
    public void SetShownEnemy() {
        if (targetedShownEnemy != null || targetingShownEnemy != null) {
            if (targetedShownEnemy != null) {
                shownEnemy = targetedShownEnemy;
            }
            if (targetingShownEnemy != null) {
                shownEnemy = targetingShownEnemy;
            }
        } else {
            shownEnemy = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        // When mouse hovers over slime, activate crosshairs and set target enemy
        if (coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<SlimeController>().crosshairs.SetActive(true);
            attacking = true;
            targetingObject = coll.gameObject;
            targetingShownEnemy = coll.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        // Disable crosshairs when cursor goes off
        if (coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<SlimeController>().crosshairs.SetActive(false);
            attacking = false;
            targetingObject = null;
            targetingShownEnemy = null;
        }
    }
}
