using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed = 5.0f;    // this controls the speed of the constant rightward motion
    public GameObject player;// This is a reference to the player, remember to drag and drop player object
    public Material noteMaterial; // this is a public variable containing the note materail
    private GameObject[] Notes = new GameObject[4]; // these will keep references to the note objects
//    private int numNotes = 0; // this keeps track of how many notes have been placed
    private bool[] isInCol = new bool[] {false, false, false, false}; // this keeps track of the collumns which already have notes
    public AudioClip audioClip;    // The track associated with this note
    private AudioSource aS;        // Audio Source reference, used to play, pause, and manage the audio
	void Start () {
        aS = gameObject.GetComponent<AudioSource>();
        addClip(audioClip);
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
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
            
            // calculate the position to place the note and place one if necessary 1st collumn
            if(player.transform.position.x < -6.65f && !isInCol[0]){
                Notes[0] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                Notes[0].transform.position = new Vector3(-6.45f, player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
                isInCol[0] = true;
                Notes[0].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                Notes[0].GetComponent<Renderer>().material = noteMaterial; // apply the material
//                numNotes++;// increase record of notes placed
            }
            else if(player.transform.position.x < -6.65f && isInCol[0]){
                Notes[0].transform.position = new Vector3(-6.45f, player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
            }
            // calculate the position to place the note and place one if necessary 2st collumn
            else if(player.transform.position.x < 0f && player.transform.position.x > -6.65f && !isInCol[1]){
                Notes[1] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                Notes[1].transform.position = new Vector3(0.0f, player.transform.position.y, player.transform.position.z);
                isInCol[1] = true;
                Notes[1].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                Notes[1].GetComponent<Renderer>().material = noteMaterial; // apply the material
//                numNotes++;// increase record of notes placed
            }
            else if(player.transform.position.x < 0f && player.transform.position.x > -6.65f && isInCol[1]){
                Notes[1].transform.position = new Vector3(0.0f, player.transform.position.y, player.transform.position.z);
            }
            // calculate the position to place the note and place one if necessary 3rd collumn
            else if(player.transform.position.x < 6.65f && player.transform.position.x > 0.0f && !isInCol[2]){
                Notes[2] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                Notes[2].transform.position = new Vector3(6.45f, player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
                isInCol[2] = true;
                Notes[2].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                Notes[2].GetComponent<Renderer>().material = noteMaterial; // apply the material
//                numNotes++;// increase record of notes placed
            }
            else if(player.transform.position.x < 6.65f && player.transform.position.x > 0.0f && isInCol[2]){
                Notes[2].transform.position = new Vector3(6.45f, player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
            }
            // calculate the position to place the note and place one if necessary 4th collumn
            else if(player.transform.position.x < 13.3f && player.transform.position.x > 6.65f && !isInCol[3]){
                Notes[3] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                Notes[3].transform.position = new Vector3(12.8f, player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
                isInCol[3] = true;
                Notes[3].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                Notes[3].GetComponent<Renderer>().material = noteMaterial; // apply the material
//                numNotes++;// increase record of notes placed
            }
            else if(player.transform.position.x < 13.3f && player.transform.position.x > 6.65f && isInCol[3]){
                Notes[3].transform.position = new Vector3(12.8f, player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
            }
        }
        // place audio clip play sounds
        if(player.transform.position.x > -6.7f && player.transform.position.x < -6.5f && isInCol[0]){
            playClip();
        }
        if(player.transform.position.x > -0.05f && player.transform.position.x < 0.05f && isInCol[1]){
            playClip();
        }
        if(player.transform.position.x > 6.5f && player.transform.position.x < 6.7f && isInCol[2]){
            playClip();
        }
        if(player.transform.position.x > 12.8f && player.transform.position.x < 13.0f && isInCol[0]){
            playClip();
        }
	}
    public void addClip(AudioClip inputClip) {
        audioClip = inputClip;
        aS.clip = audioClip;
    }

    // Checks if you have an inputted clip and plays it
    public void playClip() {
        if(aS.clip != null) {
            aS.Play();
        } else {
            print("no audio clip buddy :( Try using the addClip function with an AudioClip");
        }
    }
}
