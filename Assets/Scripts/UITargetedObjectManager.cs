using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITargetedObjectManager : MonoBehaviour {

    public Slider healthBar;                            // Getting the health bar in the HUD
    public Text healthPoints;                           // Getting the text in the HUD
    public Text enemyName;                              // Getting name of enemy
    public MouseController theCursor;                   // Getting the cursor
    public EnemyHealthChange enemyHealthManager;        // Getting the targeted object health manager
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
    }
	
	// Update is called once per frame
	void Update () {
        enemyHealthManager = theCursor.shownEnemy.GetComponent<EnemyHealthChange>();
        healthBar.maxValue = enemyHealthManager.maxHealthPoints;
        healthBar.value = enemyHealthManager.currentHealthPoints;
        healthPoints.text = "" + enemyHealthManager.currentHealthPoints + "/" + enemyHealthManager.maxHealthPoints;
        enemyName.text = "" + enemyHealthManager.enemyName;
	}
}
