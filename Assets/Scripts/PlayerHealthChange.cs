using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthChange : MonoBehaviour {

    public int value;       // Amount of health changed
    public bool relative;   // Relative to current health or set value to current health

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Checks if the player collided with the slime
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name == "Player") {
            other.gameObject.GetComponent<PlayerHealthManager>().setCurrentHeatlh(value, relative);

        }
    }
}
