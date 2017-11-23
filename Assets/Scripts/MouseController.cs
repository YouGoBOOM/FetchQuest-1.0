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
    public bool targetingEnemy = false;                 // Check if the player is targeting an enemy
    public bool targetingNPC = false;                   // Check if the player is targeting an NPC
    public bool targetingSign = false;                  // Check if the player is targeting a sign
    public static bool cursorExists = false;            // Check if mouse exists
    public bool exitingAfterMovement = false;           // Change level after movement
    public GameObject targetedShownObject = null;       // Set shown object clicked on
    public GameObject targetingShownObject = null;      // Set shown object moused over
    public GameObject shownObject = null;               // Set shown object
    public UITargetedObjectManager shownObjectManager;  // Get manager of shown object
    public bool goToObject = false;                     // Check if player should go to object

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
        SetShownObject();
        // Check if shown object is null or a sign
        if (shownObject == null || targetingSign) {
            shownObjectManager.gameObject.SetActive(false);
        } else {
            shownObjectManager.gameObject.SetActive(true);
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
                goToObject = true;
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
                    targetedShownObject = targetedObject;
                    exitingAfterMovement = false;
                    targetingNPC = false;
                    targetingSign = false;
                    // Check if clicked on an exit
                } else if (targetingObject.tag == "Exit") {
                    // MouseWorldSpace becomes exit location
                    exitingAfterMovement = true;
                    targetingEnemy = false;
                    thePlayer.engaging = false;
                    thePlayer.attacking = false;
                    targetedShownObject = null;
                    targetingNPC = false;
                    targetingSign = false;
                    // Check if clicked on NPC or sign
                } else if (targetingObject.tag == "NPC") {
                    // MouseWorldSpace becomes npc location
                    targetedShownObject = targetedObject;
                    thePlayer.attacking = false;
                    exitingAfterMovement = false;
                    targetingNPC = true;
                    // Check if NPC is a sign
                    if (targetingObject.GetComponent<NPCController>().isSign) {
                        targetingSign = true;
                    }
                }
            } else {
                goToObject = false;
                targetingEnemy = false;
                targetedObject = null;
                thePlayer.engaging = false;
                thePlayer.attacking = false;
                targetedShownObject = null;
                exitingAfterMovement = false;
                targetingNPC = false;
                targetingSign = false;
            }
            if (goToObject) {
                // Go to targeting object
                thePlayer.mouseWorldSpace = targetingObject.transform.position;
            } else {
                // Otherwise, mouseWorldSpace becomes location clicked on
                thePlayer.mouseWorldSpace = MouseScreenPositioning(thePlayer.mouseWorldSpace);
            }
        }
    }
    
    // Mouse real coordinates to level coordinates
    public Vector3 MouseScreenPositioning(Vector3 mouseWorldSpace) {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldSpace.z = 0;
        return mouseWorldSpace;
    }

    // Pick proper shown object
    public void SetShownObject() {
        if (targetedShownObject != null || targetingShownObject != null) {
            if (targetedShownObject != null) {
                shownObject = targetedShownObject;
            }
            if (targetingShownObject != null) {
                shownObject = targetingShownObject;
            }
        } else {
            shownObject = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        // When mouse hovers over slime, activate crosshairs and set target enemy
        if (coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<SlimeController>().crosshairs.SetActive(true);
            attacking = true;
            targetingObject = coll.gameObject;
            targetingShownObject = coll.gameObject;
        }
        // Target NPC
        if (coll.gameObject.tag == "NPC") {
            targetingObject = coll.gameObject;
            if (!coll.gameObject.GetComponent<NPCController>().isSign) {
                targetingShownObject = coll.gameObject;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        // Disable crosshairs when cursor goes off
        if (coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<SlimeController>().crosshairs.SetActive(false);
            attacking = false;
            targetingObject = null;
            targetingShownObject = null;
        }
        // Untarget NPC
        if (coll.gameObject.tag == "NPC") {
            targetingObject = null;
            if (!coll.gameObject.GetComponent<NPCController>().isSign) {
                targetingShownObject = null;
            }
        }
    }
}
