using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingNumbers : MonoBehaviour {

    public float movespeed;             // Speed of the floating number
    public float valueChange;           // Amount of health lost or gained
    public Text displayNumber;          // Text box with the floating numbers numbers
    public float fadeOutTimer;          // Time required to fade out
    public float stillTimer;            // Time to keep the number still
    public float entireTimer;           // Both timers
    public float numberTimerCounter;    // Timer for the number existance
    public Color originalColor;         // Original color of text
    public bool checkEXP;               // Check if gaining EXP

    // Use this for initialization
    void Start () {
        // Set text to value change
        if (checkEXP) {
            displayNumber.text = "" + valueChange + " EXP";
        } else {
            if (valueChange != 0) {
                displayNumber.text = "" + valueChange;
            } else {
                displayNumber.text = "Miss";
            }
        }
        // Set original color to current color
        originalColor = displayNumber.color;
        // Set entire timer to still timer plus fade out timer
        entireTimer = stillTimer + fadeOutTimer;
        // Set timer counter to entire timer
        numberTimerCounter = entireTimer;
    }
	
	// Update is called once per frame
	void Update () {
        // 2 stages: keep still and then float up and disappear
        numberTimerCounter -= Time.deltaTime;
        // If counter done stage 1, float numbers
        if (numberTimerCounter <= fadeOutTimer) {
            displayNumber.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, (fadeOutTimer - numberTimerCounter) / fadeOutTimer));
            transform.position = new Vector3(transform.position.x, transform.position.y + movespeed * Time.deltaTime, transform.position.z);
        }
        // If counter done stage 2, destroy object
        if (numberTimerCounter <= 0) {
            Destroy(gameObject);
        }
    }

    
}
