using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float healthPoints;              // Enemy HP
    public GameObject thePlayer;            // Gets the player
    public GameObject theMouse;             // Gets the cursor
    public bool targetted = false;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        // CheckDead();
	}
    /*
    public virtual void SetEnemyHealth(float healthChange, bool relative) {
        if (relative) {
            healthPoints += healthChange;
        } else {
            healthPoints = healthChange;
        }
    }
    

    public virtual void CheckDead() {
        if (healthPoints <= 0) {
            Destroy(gameObject);
        }
    }
    */
}
