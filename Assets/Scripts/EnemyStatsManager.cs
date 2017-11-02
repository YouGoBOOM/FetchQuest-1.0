using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : MonoBehaviour {

    public string enemyName;                    // Name of enemy
    public int enemyLevel;                      // Enemy level
    public int currentHealthPoints;             // Set current health points
    public int maxHealthPoints;                 // Set maximum health points
    public GameObject damageParticles;          // Getting the damage particles
    public GameObject healthChangeNumbers;      // Getting the damage numbers
    public PlayerLevelStats playerLevelStats;   // Getting the player level stats
    public int EXPGain;                         // EXP gained from killing enemy
    public bool resetHealthRegen;               // Check if health regen got reset
    public float healthRegenGraceTimer;         // Amount of time before health begins to degrade
    public float healthRegenGraceTimerCounter;  // Counter for overheal degrade grace timer
    public float healthRegenTimer;              // Amount of time between health degragation
    public float healthRegenTimerCounter;       // Counter for overheal degrade timer
    public float healthRegenPercentage;         // Percentage of health regen
    public bool maxHealthBasedRegen;            // Check if regen based on max or missing health

    // Use this for initialization
    void Start () {
        currentHealthPoints = maxHealthPoints;  // Set current health to maximum
        playerLevelStats = FindObjectOfType<PlayerLevelStats>();
        healthRegenPercentage /= 100;           // Change percentage to decimal
	}
	
	// Update is called once per frame
	void Update () {
        CheckDead();
        // Check if not engaging and hurt
        if (!(gameObject.GetComponent<SlimeController>().engaging) && currentHealthPoints < maxHealthPoints) {
            RegenHealth();
        }
	}

    // Set enemy health relative to current health or hard set to value
    public void SetEnemyHealth(int healthChange, bool relative) {
        // Relative set
        if (relative) {
            currentHealthPoints += healthChange;
            // If taking damage, produce damage particle and numbers
            if (healthChange <= 0) {
                if (healthChange != 0) {
                    GameObject damageParticlesClone = Instantiate(damageParticles, transform.position, transform.rotation);
                    Destroy(damageParticlesClone, 1.0f);
                }
                GameObject healthNumbersClone = Instantiate(healthChangeNumbers, transform.position, transform.rotation);
                healthNumbersClone.GetComponent<FloatingNumbers>().valueChange = healthChange;
                healthNumbersClone.GetComponent<FloatingNumbers>().displayNumber.color = Color.red;
                resetHealthRegen = true;
            }
        // Hard set
        } else {
            currentHealthPoints = healthChange;
        }
    }

    // Check if enemy is dead
    public void CheckDead() {
        if (currentHealthPoints <= 0) {
            int EXPGainAfterModifier = EXPGainLevelDifferenceModifier(EXPGain);
            playerLevelStats.SetPlayerEXP(EXPGainAfterModifier, true);
            Destroy(gameObject);
        }
    }

    // Enemy regenerates health after not engaging for a while
    public void RegenHealth() {
        if (resetHealthRegen) {
            // Start grace timer before regeneration of health
            healthRegenGraceTimerCounter = healthRegenGraceTimer;
            healthRegenTimerCounter = healthRegenTimer;
            resetHealthRegen = false;
        }
        // Check if grace timer is up
        if (healthRegenGraceTimerCounter <= 0) { 
            if (healthRegenTimerCounter > 0) {
                healthRegenTimerCounter -= Time.deltaTime;
            } else {
                int healthRegenInstance = HealthRegenInstanceAmount(healthRegenPercentage, maxHealthBasedRegen);
                SetEnemyHealth(healthRegenInstance, true);
                healthRegenTimerCounter = healthRegenTimer;
            }
        } else {
            healthRegenGraceTimerCounter -= Time.deltaTime;
        }
    }

    // Calculate the amount of health regenerated each tick based on max or missing health
    private int HealthRegenInstanceAmount(float percentage, bool relativeToMaxHealth) {
        if (relativeToMaxHealth) {
            return Mathf.Max(Mathf.RoundToInt((maxHealthPoints) * percentage), 1);
        } else {
            return Mathf.Max(Mathf.RoundToInt((maxHealthPoints - currentHealthPoints) * percentage), 1);
        }
    }

    // Calculate the amount of experience gained with a level modifier
    private int EXPGainLevelDifferenceModifier(int EXPGainWithoutModifier) {
        float EXPGainWithModifier = EXPGainWithoutModifier;
        int i = playerLevelStats.currentLevel;
        // Increase experience gained by 50% per level if player lower level than enemy
        if (playerLevelStats.currentLevel < enemyLevel) {
            while (i < enemyLevel) {
                EXPGainWithModifier *= 1.5f;
                i++;
            }
        // Decrease experience gained by 80% per level if player higher level than enemy
        } else if (playerLevelStats.currentLevel > enemyLevel) {
            while (i > enemyLevel) {
                EXPGainWithModifier *= 0.2f;
                i--;
            }
        }
        return Mathf.RoundToInt(EXPGainWithModifier);
    }
}
