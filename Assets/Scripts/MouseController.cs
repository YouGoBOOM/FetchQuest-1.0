using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    private PlayerController thePlayer;             // Getting the player
    private Animator animator;                      // Animator
    private bool mouseExists = false;               // Check if the mouse exists
    public bool attacking = false;                  // Check if the player is attacking
    public bool selecting = false;                  // Check if the player is selecting
    public SpriteRenderer mouseSpriteRenderer;      // Sprite Renderer

    void Awake () {
        // Sets the default cursor to invisible
        Cursor.visible = false;
    }

	// Use this for initialization
	void Start () {
        // Check if the mouse exists
        if (!mouseExists) {
            mouseExists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(gameObject);
        }
        animator = GetComponent<Animator>();                    // Getting Animator
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();   // Getting Sprite Renderer
        thePlayer = FindObjectOfType<PlayerController>();       // Getting the player
        
    }
	
	// Update is called once per frame
	void Update () {
        
        // Make the cursor follow the real mouse
        transform.gameObject.transform.position = mouseScreenPositioning(transform.gameObject.transform.position);
        // Set parameters in animator
        animator.SetBool("Selecting", selecting);
        animator.SetBool("Attacking", attacking);

    }

    // Right click
    public void OnMouseRightClick() {
        if (Input.GetMouseButtonDown(1)) {
            thePlayer.rightMouseClicked = true;
            thePlayer.playerMoving = true;
            thePlayer.mouseWorldSpace = mouseScreenPositioning(thePlayer.mouseWorldSpace);  
            thePlayer.lastLocation = transform.position;
        }
    }
    
    // Mouse real coordinates to level coordinates
    private Vector3 mouseScreenPositioning(Vector3 mouseWorldSpace) {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldSpace.z = 0;
        return mouseWorldSpace;
    }
}
