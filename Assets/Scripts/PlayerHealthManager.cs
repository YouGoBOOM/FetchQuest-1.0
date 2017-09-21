using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {

    public int playerMaxHeatlh;            // Player's maximum health
    public int playerCurrentHealth;        // Player's current health

	// Use this for initialization
	void Start () {
        // Set current health to full health at beginning of game
        playerCurrentHealth = playerMaxHeatlh;
	}
	
	// Update is called once per frame
	void Update () {
		// Check if the player health is 0 or lower
        if (playerCurrentHealth <= 0) {
            gameObject.SetActive(false);
        }
	}

    public void setCurrentHeatlh(int healthValue, bool relative) {
        if (relative) {
            // Heal or damage value health
            playerCurrentHealth += healthValue;
        } else {
            // Hard set health to value
            playerCurrentHealth = healthValue;
        }
    }
}
