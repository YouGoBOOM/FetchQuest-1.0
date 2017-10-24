using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour {

    public Slider healthBar;                            // Getting the health bar in the HUD
    public Text healthPoints;                           // Getting the health text in the HUD
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
	}
	
	// Update is called once per frame
	void Update () {
        healthBar.maxValue = playerHealthManager.playerMaxHeatlh;   // Max health on bar
        healthBar.value = playerHealthManager.playerCurrentHealth;  // Current health on bar
        // Current health vs max health in text
        healthPoints.text = "" + playerHealthManager.playerCurrentHealth + "/" + playerHealthManager.playerMaxHeatlh;
        level.text = "LVL: " + playerLevelStats.currentLevel;
    }
}
