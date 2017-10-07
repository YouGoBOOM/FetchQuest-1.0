using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthChange : MonoBehaviour {

    public float healthPoints;      // Set health points

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckDead();
	}

    public void SetEnemyHealth(float healthChange, bool relative) {
        if (relative) {
            healthPoints += healthChange;
        } else {
            healthPoints = healthChange;
        }
    }

    public void CheckDead() {
        if (healthPoints <= 0) {
            Destroy(gameObject);
        }
    }
}
