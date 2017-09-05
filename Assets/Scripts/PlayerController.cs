using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Initializing Variables
    public float moveSpeed = 4;     // Walking Movement Speed
    public bool rightMouseClicked = false;
    public Vector3 mouseWorldSpace;
    private Animator animator;      // Animator
    
    // Use this for initialization
	void Start () {
        // Getting Animator
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        // Right click movement
        OnMouseRightClick();
        if (rightMouseClicked == true && transform.position != mouseWorldSpace) {
            transform.position = Vector3.MoveTowards(transform.position, mouseWorldSpace, moveSpeed * Time.deltaTime);
        } else if (transform.position == mouseWorldSpace) {
            rightMouseClicked = false;
        }

    }

    // Right click screen to world positioning
    private void OnMouseRightClick() {
        if (Input.GetMouseButtonDown(1)) {
            rightMouseClicked = true;
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldSpace.z = 0;
        }
        
    }
}
