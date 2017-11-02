using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour {

    public Slider healthBar;                            // Getting the health bar in the HUD
    public Text healthPoints;                           // Getting the health text in the HUD
    public Slider experienceBar;                        // Getting the exp bar in the HUD
    public Text experiencePoints;                       // Gettin gthe exp text in the HUD
    public PlayerHealthManager playerHealthManager;     // Getting the player health manager
    public Text level;                                  // Getting the level text in the HUD
    public PlayerLevelStats playerLevelStats;           // Getting the player level stats
    public static bool UIExists;                        // Check if UI already exists

	// Use this for initialization
	void Start () {
		// Check between levels if UI exists
        if (!UIExists) {
            UIExists = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        gameObject.GetComponent<Canvas>().sortingLayerName = "HUD";
        gameObject.GetComponent<Canvas>().sortingOrder = 1;
    }
	
	// Update is called once per frame
	void Update () {
        // Health values
        healthBar.maxValue = playerHealthManager.playerMaxHeatlh;
        // If overkilled, set value to 0
        if (playerHealthManager.playerCurrentHealth < 0) {
            healthBar.value = 0;
            healthPoints.text = "0/" + playerHealthManager.playerMaxHeatlh;
        // Otherwise, set value to current health
        } else {
            healthBar.value = playerHealthManager.playerCurrentHealth;
            healthPoints.text = "" + playerHealthManager.playerCurrentHealth + "/" + playerHealthManager.playerMaxHeatlh;
        }
        // If not at max level
        if (!playerLevelStats.atMaxLevel) {
            experienceBar.maxValue = playerLevelStats.requiredEXP[playerLevelStats.currentLevel];
            experienceBar.value = playerLevelStats.experience;
            experiencePoints.text = "EXP: " + playerLevelStats.experience + " / " + playerLevelStats.requiredEXP[playerLevelStats.currentLevel];
            level.text = "LVL: " + playerLevelStats.currentLevel;
        // Otherwise, at max level
        } else {
            experienceBar.maxValue = 1;
            experienceBar.value = 1;
            experiencePoints.text = "MAX LEVEL REACHED";
            level.text = "LVL: MAX";
        } 
    }
}
