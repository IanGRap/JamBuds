﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public int speed = 5;    // this controls the speed of the constant rightward motion
    public GameObject player;// This is a reference to the player, remember to drag and drop player object
    public Material noteMaterial; // this is a public variable containing the note materail
    private GameObject[] Notes = new GameObject[4]; // these will keep references to the note objects
    private int noteIndex = 0; // this keeps track of how many notes have been placed
    private bool[] isInCol = new bool[] {false, false, false, false}; // this keeps track of the collumns which already have notes
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
      // constant motion is handled below, notice it is multiplies by speed
       player.transform.position += Vector3.right * speed * Time.deltaTime;
        // this is the right-left world wrap
        if(player.transform.position.x > 13.0f) {
            player.transform.position = new Vector3(-13.0f, player.transform.position.y, player.transform.position.z);
        }
        // This allows the player to move up
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 5.5f);
        }
        // this allow the palyer to move down
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 5.5f);
        }

        // this section is the world wrap from top to bottom
        if(player.transform.position.z > 5.5f){
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -5.5f);
        }

        // this section is the world wrap from bottom to top
        if(player.transform.position.z < -5.5f){
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 5.5f);
        }
        // This should create a not on the screen, I hope
        if(Input.GetKeyDown(KeyCode.Return) && noteIndex < 4){
            
            // calculate the position to place the note and place one if necessary 1st collumn
            if(player.transform.position.x < -6.65f && !isInCol[0]){
                Notes[noteIndex] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                Notes[noteIndex].transform.position = new Vector3(-6.45f, player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
                isInCol[0] = true;
                Notes[noteIndex].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                Notes[noteIndex].GetComponent<Renderer>().material = noteMaterial; // apply the material
                noteIndex++;// increase record of notes placed
            }
            // calculate the position to place the note and place one if necessary 2st collumn
            else if(player.transform.position.x < 0f && player.transform.position.x > -6.65f && !isInCol[1]){
                Notes[noteIndex] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                Notes[noteIndex].transform.position = new Vector3(0.0f, player.transform.position.y, player.transform.position.z);
                isInCol[1] = true;
                Notes[noteIndex].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                Notes[noteIndex].GetComponent<Renderer>().material = noteMaterial; // apply the material
                noteIndex++;// increase record of notes placed
            }
            // calculate the position to place the note and place one if necessary 3rd collumn
            else if(player.transform.position.x < 6.65f && player.transform.position.x > 0.0f && !isInCol[2]){
                Notes[noteIndex] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                Notes[noteIndex].transform.position = new Vector3(6.45f, player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
                isInCol[2] = true;
                Notes[noteIndex].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                Notes[noteIndex].GetComponent<Renderer>().material = noteMaterial; // apply the material
                noteIndex++;// increase record of notes placed
            }
            // calculate the position to place the note and place one if necessary 4th collumn
            else if(player.transform.position.x < 13.3f && player.transform.position.x > 6.65f &&!isInCol[3]){
                Notes[noteIndex] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                Notes[noteIndex].transform.position = new Vector3(12.8f, player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
                isInCol[3] = true;
                Notes[noteIndex].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                Notes[noteIndex].GetComponent<Renderer>().material = noteMaterial; // apply the material
                noteIndex++;// increase record of notes placed
            }
            // place it at the correct position
            
        }
	}
}
