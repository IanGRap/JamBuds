using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public int speed = 5;    // this controls the speed of the constant rightward motion
    public GameObject player;// This is a reference to the player, remember to drag and drop player object
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
	}
}
