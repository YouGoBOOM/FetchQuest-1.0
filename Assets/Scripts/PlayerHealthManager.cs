﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {

    public float playerMaxHeatlh;                   // Player's maximum health
    public float playerCurrentHealth;               // Player's current health
    public bool overheal = false;                   // Activate the grace period on the overheal
    public float overhealDegradeGraceTimer;         // Amount of time before health begins to degrade
    public float overhealDegradeGraceTimerCounter;  // Counter for overheal degrade grace timer
    public float overhealDegradeTimer;              // Amount of time between health degragation
    public float overhealDegradeTimerCounter;       // Counter for overheal degrade timer
    public float overhealDegradeAmount;             // Amount of health lost during overheal
    public GameObject healthChangeNumbers;          // Getting the damage numbers

    // Use this for initialization
    void Start () {
        // Set current health to full health at beginning of game
        FullHeal();
	}
	
	// Update is called once per frame
	void Update () {
        // Check if player health is greater than max health
        if (playerCurrentHealth > playerMaxHeatlh) {
            Overheal();
        }
        // Unfair stupid healing button for development purposes
        // GET RID OF THIS LATER
        if (Input.GetKeyDown("h")) {
            UnfairStupidHealing();
        }
        // Check if the player health is 0 or lower
        if (playerCurrentHealth <= 0) {
            gameObject.SetActive(false);
        }
	}

    // Set player health
    public void SetCurrentHeatlh(float healthValue, bool relative) {
        if (relative) {
            // Heal or damage value health
            playerCurrentHealth += healthValue;
            // Numbers on health change
            GameObject healthNumbersClone = Instantiate(healthChangeNumbers, transform.position, transform.rotation);
            healthNumbersClone.GetComponent<FloatingNumbers>().healthChange = healthValue;
            // Check if healed and/or overhealed
            if (playerCurrentHealth > playerMaxHeatlh && healthValue > 0) {
                overheal = true;
            }
            // Set color based on healing or damage
            if (healthValue >= 0) {
                healthNumbersClone.GetComponent<FloatingNumbers>().originalColor = Color.green;
            } else if (healthValue < 0) {
                healthNumbersClone.GetComponent<FloatingNumbers>().originalColor = Color.red;
            }
        } else {
            // Hard set health to value
            playerCurrentHealth = healthValue;
        }
    }

    // Heal the player to full health
    public void FullHeal() {
        playerCurrentHealth = playerMaxHeatlh;
    }

    // If overheal, degrade health per second until at full health
    private void Overheal() {
        // Check if overhealed
        if (overheal) {
            // Start grace timer before degragation of health
            overhealDegradeGraceTimerCounter = overhealDegradeGraceTimer;
            overhealDegradeTimerCounter = overhealDegradeTimer;
            // If more overhealing occurs, refresh grace timer
            overheal = false;
        }
        // Check if grace timer is up
        if (overhealDegradeGraceTimerCounter <= 0) {
            // Degrade set health and set interval
            if (overhealDegradeTimerCounter > 0) {
                overhealDegradeTimerCounter -= Time.deltaTime;
            } else {
                playerCurrentHealth -= overhealDegradeAmount;
                overhealDegradeTimerCounter = overhealDegradeTimer;
            }
        } else {
            overhealDegradeGraceTimerCounter -= Time.deltaTime;
        }
    }

    // Unfair heal function to test things
    // REMOVE OR DISABLE AT LATER TIME
    private void UnfairStupidHealing() {
        SetCurrentHeatlh(50, true);
    }
}
