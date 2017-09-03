using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Initializing Variables
    public float moveSpeed = 4;     // Walking Movement Speed
    private Animator animator;      // Animator
    
    // Use this for initialization
	void Start () {
        // Getting Animator
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
        // Horizontal 
        if (Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") < 0) {
            // Horizontal Movement
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
        }

        // Vertical
        if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Vertical") < 0) {
            // Vertical Movement
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
        }

        // Animator
        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));

    }
}
