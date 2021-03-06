﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour {

    public float NPCMoveSpeed;                  // Getting the move speed of the NPC
    public int currentStop;                     // Find which step the NPC is on
    public Vector3[] stops;                     // Getting an array of steps
    public float[] waitTimeDuringStops;         // Getting how much time waiting at a stop
    public float waitTimeDuringStopsCounter;    // Counter for wait time counter
    public bool isWalking;                      // Check if the NPC is walking
    public PlayerController playerController;   // Getting the player controller
    public Rigidbody2D NPCRigidbody;            // Getting the NPC rigidbody
    public bool loopStops;                      // Check if NPC loops through stops array
    public bool reverseStops;                   // Check if NPC reverse through stops array
    public bool isReversing;                    // Check if NPC is reversing through stops array
    public bool endWalkingSequence;             // Check if NPC walking sequence is ended
    public bool pauseWalkingForDialogue;        // Check if NPC paused walking for dialogue

	// Use this for initialization
	void Start () {
        // Getting the player controller
		playerController = FindObjectOfType<PlayerController>();
        // Getting the NPC rigidbody
        NPCRigidbody = GetComponent<Rigidbody2D>();
        // Chck if there are stops
        if (stops.Length != 0) {
            // Setting the wait time counter to the wait time at the current stop
            waitTimeDuringStopsCounter = waitTimeDuringStops[currentStop];
            // Set starting position to current stop
            transform.position = stops[currentStop];
        }
        // Set initial velocity to zero
        NPCRigidbody.velocity = Vector2.zero;
    }
	
	// Update is called once per frame
	void Update () {
        CheckSortingLayerOrder();
        Moving();
    }

    // Check position relative to player
    private void CheckSortingLayerOrder() {
        // Check if player above
        if (playerController.gameObject.transform.position.y > transform.position.y) {
            // Put NPC in front
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        // Check if player below
        } else {
            // Put player in front
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }

    // Move towards the stops
    private void Moving() {
        // Check if there isn't a stop
        if (stops.Length != 0) {
            // Check if not paused for dialogue
            if (!pauseWalkingForDialogue) {
                // Check if not at the end
                if (!endWalkingSequence) {
                    // Check if not walking
                    if (!isWalking) {
                        // Count down counter
                        if (waitTimeDuringStopsCounter > 0) {
                            waitTimeDuringStopsCounter -= Time.deltaTime;
                        } else {
                            // Check if stops should reverse
                            if (reverseStops) {
                                // Check if going forward through stops
                                if (!isReversing) {
                                    // Increase current stop
                                    currentStop++;
                                    // If at last stop, reverse 
                                    if (currentStop == stops.Length - 1) {
                                        isReversing = true;
                                    }
                                    // Check if going reverse through stops
                                } else {
                                    // Decrease current stop
                                    currentStop--;
                                    // If at first stop, go forward
                                    if (currentStop == 0) {
                                        isReversing = false;
                                    }
                                }
                                // Check if stops should loop
                            } else if (loopStops) {
                                currentStop++;
                                // Loop back if at end
                                if (currentStop == stops.Length) {
                                    currentStop = 0;
                                }
                            } else {
                                // Stop at end
                                if (currentStop != stops.Length - 1) {
                                    currentStop++;
                                } else {
                                    endWalkingSequence = true;
                                }
                            }
                            // Set stop counter to next stop
                            waitTimeDuringStopsCounter = waitTimeDuringStops[currentStop];
                            // Set NPC walking
                            isWalking = true;
                        }
                        NPCRigidbody.velocity = Vector2.zero;
                        // Check if walking
                    } else {
                        // Move to stop
                        transform.position = Vector3.MoveTowards(transform.position, stops[currentStop], NPCMoveSpeed * Time.deltaTime);
                        // Check if at stop
                        if (transform.position == stops[currentStop]) {
                            // Set NPC not walking
                            isWalking = false;
                        }
                    }
                }
            }
        // Otherwise don't move
        } else {
            NPCRigidbody.velocity = Vector2.zero;
        }
    }
}
