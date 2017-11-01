using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITargetedObjectManager : MonoBehaviour {

    public Slider healthBar;                            // Getting the health bar in the HUD
    public Text healthPoints;                           // Getting the text in the HUD
    public Text enemyName;                              // Getting the name of enemy
    public Text enemyLevel;                             // Getting the level of enemy
    public MouseController theCursor;                   // Getting the cursor
    public EnemyStatsManager enemyStatsManager;         // Getting the targeted object health manager
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
        enemyStatsManager = theCursor.shownEnemy.GetComponent<EnemyStatsManager>();
        healthBar.maxValue = enemyStatsManager.maxHealthPoints;
        healthBar.value = enemyStatsManager.currentHealthPoints;
        healthPoints.text = "" + enemyStatsManager.currentHealthPoints + "/" + enemyStatsManager.maxHealthPoints;
        enemyName.text = "" + enemyStatsManager.enemyName;
        enemyLevel.text = "" + enemyStatsManager.enemyLevel;
	}
}
