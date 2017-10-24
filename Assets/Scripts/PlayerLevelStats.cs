using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelStats : MonoBehaviour {

    public int currentLevel;                // Current level of player
    public int currentEXP;                  // Current amount of experience
    public int[] requiredEXP;               // Experience required to level up
    public GameObject EXPChangeNumbers;     // Getting the damage numbers

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (currentEXP >= requiredEXP[currentLevel]) {
            currentLevel++;
            currentEXP = 0;
        }
	}

    // Set experience relative to current experience or hard set to value
    public void SetPlayerEXP(int EXPChange, bool relative) {
        // Relative set
        if (relative) {
            currentEXP += EXPChange;
            Vector3 aboveHeadPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            GameObject EXPNumbersClone = Instantiate(EXPChangeNumbers, aboveHeadPosition, transform.rotation);
            EXPNumbersClone.GetComponent<FloatingNumbers>().valueChange = EXPChange;
            EXPNumbersClone.GetComponent<FloatingNumbers>().displayNumber.color = Color.yellow;
            EXPNumbersClone.GetComponent<FloatingNumbers>().checkEXP = true;
            EXPNumbersClone.GetComponent<FloatingNumbers>().movespeed /= 3;
        // Hard set
        } else {
            currentEXP = EXPChange;
        }
    }
}
