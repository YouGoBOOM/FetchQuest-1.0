using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthChange : MonoBehaviour {

    public float currentHealthPoints;           // Set current health points
    public float maxHealthPoints;               // Set maximum health points
    public GameObject damageParticles;          // Getting the damage particles

	// Use this for initialization
	void Start () {
        currentHealthPoints = maxHealthPoints;
	}
	
	// Update is called once per frame
	void Update () {
        CheckDead();
	}

    public void SetEnemyHealth(float healthChange, bool relative) {
        if (relative) {
            currentHealthPoints += healthChange;
            if (healthChange < 0) {
                GameObject damageParticlesClone = Instantiate(damageParticles, transform.position, transform.rotation);
                Destroy(damageParticlesClone, 1.0f);
            }
        } else {
            currentHealthPoints = healthChange;
        }
    }

    public void CheckDead() {
        if (currentHealthPoints <= 0) {
            Destroy(gameObject);
        }
    }

    //public void DamageParticles() {

    //}
}
