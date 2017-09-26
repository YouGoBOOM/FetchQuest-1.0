using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour {

    public MouseController theCursor;       // Getting the mouse controller

	// Use this for initialization
	void Start () {
        theCursor = FindObjectOfType<MouseController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            Destroy(other.gameObject);
            theCursor.targetting = false;
        }
    }
    
    
}
