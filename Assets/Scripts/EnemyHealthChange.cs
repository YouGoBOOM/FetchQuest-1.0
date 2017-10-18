using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthChange : MonoBehaviour {

    public float currentHealthPoints;           // Set current health points
    public float maxHealthPoints;               // Set maximum health points
    public GameObject damageParticles;          // Getting the damage particles
    public GameObject damageNumbers;            // Getting the damage numbers

	// Use this for initialization
	void Start () {
        currentHealthPoints = maxHealthPoints;  // Set current health to maximum
	}
	
	// Update is called once per frame
	void Update () {
        CheckDead();
	}

    // Set enemy health relative to current health or hard set to value
    public void SetEnemyHealth(float healthChange, bool relative) {
        // Relative set
        if (relative) {
            currentHealthPoints += healthChange;
            // If taking damage, produce damage particle and numbers
            if (healthChange < 0) {
                GameObject damageParticlesClone = Instantiate(damageParticles, transform.position, transform.rotation);
                Destroy(damageParticlesClone, 1.0f);
                GameObject damageNumbersClone = Instantiate(damageNumbers, transform.position, transform.rotation);
                damageNumbersClone.GetComponent<FloatingNumbers>().healthChange = healthChange;
            }
        // Hard set
        } else {
            currentHealthPoints = healthChange;
        }
    }

    // Check if enemy is dead
    public void CheckDead() {
        if (currentHealthPoints <= 0) {
            Destroy(gameObject);
        }
    }
}
