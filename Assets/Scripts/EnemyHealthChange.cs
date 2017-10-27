using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthChange : MonoBehaviour {

    public string enemyName;                    // Get name of enemy
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

    // Use this for initialization
    void Start () {
        currentHealthPoints = maxHealthPoints;  // Set current health to maximum
        playerLevelStats = FindObjectOfType<PlayerLevelStats>();
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
            if (healthChange < 0) {
                GameObject damageParticlesClone = Instantiate(damageParticles, transform.position, transform.rotation);
                Destroy(damageParticlesClone, 1.0f);
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
            playerLevelStats.SetPlayerEXP(EXPGain, true);
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
                // Regen 10% of missing health or 1 hp, whichever is more
                currentHealthPoints += Mathf.Max(Mathf.RoundToInt((maxHealthPoints - currentHealthPoints) * 0.1f), 1);
                healthRegenTimerCounter = healthRegenTimer;
            }
        } else {
            healthRegenGraceTimerCounter -= Time.deltaTime;
        }
    }
}
