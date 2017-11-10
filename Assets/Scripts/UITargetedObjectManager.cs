using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITargetedObjectManager : MonoBehaviour {

    public Slider healthBar;                            // Getting the health bar in the HUD
    public Text healthPoints;                           // Getting the text in the HUD
    public Text objectName;                             // Getting the name of enemy
    public Text objectLevel;                             // Getting the level of enemy
    public MouseController theCursor;                   // Getting the cursor
    public EnemyStatsManager enemyStatsManager;         // Getting the targeted enemy health manager
    public NPCController NPCController;                 // Getting the targeted NPC controller
    public static bool UIExists;                        // Check if UI already exists

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
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
        UpdateTargetedObjectManager();
    }

    public void UpdateTargetedObjectManager() {
        // Check if the cursor is showing an enemy
        if (theCursor.shownObject.tag == "Enemy") {
            enemyStatsManager = theCursor.shownObject.GetComponent<EnemyStatsManager>();
            healthBar.fillRect.GetComponentInChildren<Image>().color = Color.red;
            healthBar.maxValue = enemyStatsManager.maxHealthPoints;
            // If overkilled, show current health as 0
            if (enemyStatsManager.currentHealthPoints <= 0) {
                healthBar.value = 0;
                healthPoints.text = "0/" + enemyStatsManager.maxHealthPoints;
            // Otherwise show current health
            } else {
                healthBar.value = enemyStatsManager.currentHealthPoints;
                healthPoints.text = "" + enemyStatsManager.currentHealthPoints + "/" + enemyStatsManager.maxHealthPoints;
            }
            objectName.text = "" + enemyStatsManager.enemyName;
            objectLevel.text = "" + enemyStatsManager.enemyLevel;
        }
        // Check if the cursor is showing an NPC
        if (theCursor.shownObject.tag == "NPC") {
            NPCController = theCursor.shownObject.GetComponent<NPCController>();
            healthBar.fillRect.GetComponentInChildren<Image>().color = Color.green;
            healthBar.maxValue = 1;
            healthBar.value = 1;
            healthPoints.text = "Friendly";
            objectName.text = "" + NPCController.NPCName;
            objectLevel.text = "" + NPCController.NPCLevel;
        }
    }
}
