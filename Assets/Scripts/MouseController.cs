using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    private PlayerController thePlayer;             // Getting the player
    private Animator myAnimator;                    // Animator
    private Collider2D myCollider;                  // Collider
    public bool attacking = false;                  // Check if the player is attacking
    public bool selecting = false;                  // Check if the player is selecting
    public GameObject targettedEnemy = null;        // Set current targetted enemy
    public bool targetting = false;                 // Check if the player is targetting an enemy
    public static bool cursorExists = false;        // Check if mouse exists

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
        // Set parameters in animator
        myAnimator.SetBool("Selecting", selecting);
        myAnimator.SetBool("Attacking", attacking);

    }

    // Right click
    public void OnMouseRightClick() {
        if (Input.GetMouseButtonDown(1)) {
            thePlayer.rightMouseClicked = true;
            thePlayer.playerMoving = true;
            // If there is a target enemy
            if (targettedEnemy != null) {
                // If clicked on the enemy, mouseWorldSpace becomes enemy location
                if (myCollider.IsTouching(targettedEnemy.GetComponent<CircleCollider2D>())) {
                    targetting = true;
                    targettedEnemy.GetComponent<SlimeController>().targetted = true;
                } else {
                    targetting = false;
                }
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
            targettedEnemy = coll.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        // Disable crosshairs when cursor goes off
        if (coll.gameObject.tag == "Enemy") {
            coll.gameObject.GetComponent<SlimeController>().crosshairs.SetActive(false);
            attacking = false;
        }
    }
}
