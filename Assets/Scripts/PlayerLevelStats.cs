using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelStats : MonoBehaviour {

    public int currentLevel;                // Current level of player
    public int maxLevel;                    // Max level of player
    public bool atMaxLevel;                 // Check if at max level
    public int experience;                  // Current amount of experience
    public int[] requiredEXP;               // Experience required to level up
    public GameObject EXPChangeNumbers;     // Getting the damage numbers
    public int power;                       // Getting the value for the attribute of power
    public int resiliance;                  // Getting the value for the attribute of resiliance
    public int endurance;                   // Getting the value for the attribute of endurance
    public int dexterity;                   // Getting the value for the attribute of dexterity
    public int intellect;                   // Getting the value for the attribute of intellect
    public int luck;                        // Getting the value for the attribute of luck
    public string[] attributeNames;         // Setting attribute names for the player
    public int[] attributes;                // Setting attributes for the player
    public int attack;                      // Getting total player attack
    public int defense;                     // Getting total player defense
    public PlayerController playerController;

    // Use this for initialization
    void Start () {
        experience = 0;
        for (int i = 0; i < attributes.Length; i++) {
            // Hard set each attribute to 0 at the beginning of the game
            attributes[i] = currentLevel;
        }
        maxLevel = requiredEXP.Length - 1;
        // Getting the player controller
        playerController = gameObject.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        // Check if level up if not at max level
        if (experience >= requiredEXP[currentLevel] && !atMaxLevel) {
            LevelUp();
            // Check if at max level
            if (currentLevel == maxLevel) {
                atMaxLevel = true;
            }
        }

        // Unfair stupid experinence button for development purposes
        // GET RID OF THIS LATER
        if (Input.GetKeyDown("g")) {
            UnfairStupidExperienceFarming();
        }

        attack = playerController.weaponDamage;
        defense = playerController.armourDefense;
	}

    // Set experience relative to current experience or hard set to value
    public void SetPlayerEXP(int EXPChange, bool relative) {
        // Relative set
        if (relative) {
            if (!atMaxLevel) {
                experience += EXPChange;
                Vector3 aboveHeadPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                GameObject EXPNumbersClone = Instantiate(EXPChangeNumbers, aboveHeadPosition, transform.rotation);
                EXPNumbersClone.GetComponent<FloatingNumbers>().valueChange = EXPChange;
                EXPNumbersClone.GetComponent<FloatingNumbers>().displayNumber.color = Color.yellow;
                EXPNumbersClone.GetComponent<FloatingNumbers>().checkEXP = true;
                EXPNumbersClone.GetComponent<FloatingNumbers>().movespeed /= 3;
            }
        // Hard set
        } else {
            experience = EXPChange;
        }
    }

    // Things that happen when player levels up
    public void LevelUp() {
        if (experience >= requiredEXP[currentLevel] && !atMaxLevel) {
            experience -= requiredEXP[currentLevel];
            currentLevel++;
        }
        // Give an additional point to each attribute when leveling up
        for (int i = 0; i < attributes.Length; i++) {
            attributes[i]++;
        }
    }

    // Unfair expreience function to test things
    // REMOVE OR DISABLE AT LATER TIME
    private void UnfairStupidExperienceFarming() {
        SetPlayerEXP(50, true);
    }
}
