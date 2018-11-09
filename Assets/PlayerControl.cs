using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float speed = 5.0f;    // this controls the speed of the constant rightward motion
    public GameObject player;// This is a reference to the player, remember to drag and drop player object
    public Material noteMaterial; // this is a public variable containing the note materail
    private GameObject[] Notes = new GameObject[8]; // these will keep references to the note objects
//    private int numNotes = 0; // this keeps track of how many notes have been placed
    private bool[] isInCol = new bool[] {false, false, false, false, false, false, false, false}; // this keeps track of the collumns which already have notes
    public AudioClip topSound;      // the track associated with the top row
    public AudioClip middleSound;    // The track associated with the middle row
    public AudioClip bottomSound;    // the track associated with the bottom sound
    private AudioSource[] aS = new AudioSource[3];        // Audio Source reference, used to play, pause, and manage the audio

    ///////////////////////////////
    //LOOK BELOW TO MODIFY VALUES//
    ///////////////////////////////
    // below is a set of private constant variables to make script modification easier
    private const float vertMove = 6.666666666666f; // constant variable to control player motion
    private const float staffEdge = 18.0f;          // location of the edge of the staff
    private float[] colEdge = new float[] {-13.5f, -9.0f, -4.5f, 0.0f, 4.5f, 9.0f, 13.5f, 18.0f}; // location where notes can be placed
    private float[] noteOffset = new float[] {0.6f, 0.4f, 0.2f, 0.0f, -0.2f, -0.4f, -0.6f, -0.8f};
	void Start () {
        for(int i = 0; i < 3; i++){
            aS[i] = gameObject.GetComponent<AudioSource>();
        }
        addClip(topSound, 0);
        addClip(middleSound, 1);
        addClip(bottomSound, 2);
        
	}
	
	// Update is called once per frame
	void Update () {
      // constant motion is handled below, notice it is multiplies by speed
       player.transform.position += Vector3.right * speed * Time.deltaTime;
        // this is the right-left world wrap
        if(player.transform.position.x > staffEdge) {
            player.transform.position = new Vector3(-staffEdge, player.transform.position.y, player.transform.position.z);
        }
        // This allows the player to move up
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + vertMove);
        }
        // this allow the palyer to move down
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - vertMove);
        }

        // this section is the world wrap from top to bottom
        if(player.transform.position.z > vertMove){
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -vertMove);
        }

        // this section is the world wrap from bottom to top
        if(player.transform.position.z < -vertMove){
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, vertMove);
        }
        // This should create a not on the screen, I hope
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
            // calculate the position and place note if necessary in collumns 2 through 8
            for(int i = 1; i < 8; i++){
                if(player.transform.position.x < colEdge[i] && player.transform.position.x > colEdge[i-1] && !isInCol[i]){
                    Notes[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                    Notes[i].transform.position = new Vector3(colEdge[i] + noteOffset[i], player.transform.position.y, player.transform.position.z);
                    isInCol[i] = true;
                    Notes[i].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                    Notes[i].GetComponent<Renderer>().material = noteMaterial; // apply the material
    //                numNotes++;// increase record of notes placed
                }
                else if(player.transform.position.x < colEdge[i] && player.transform.position.x > colEdge[i-1] && isInCol[i]){
                    Notes[i].transform.position = new Vector3(colEdge[i] + noteOffset[i], player.transform.position.y, player.transform.position.z);
                }
            }
            
            // calculate the position to place the note and place one if necessary 1st collumn
            if(player.transform.position.x < colEdge[0] && !isInCol[0]){
                Notes[0] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                Notes[0].transform.position = new Vector3(colEdge[0] + noteOffset[0], player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
                isInCol[0] = true;
                Notes[0].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                Notes[0].GetComponent<Renderer>().material = noteMaterial; // apply the material
//                numNotes++;// increase record of notes placed
            }
            else if(player.transform.position.x < colEdge[0] && isInCol[0]){
                Notes[0].transform.position = new Vector3(colEdge[0] + noteOffset[0], player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
            }

        }
        // place audio clip play sounds
        for(int i = 0; i < 8; i++){
            if(player.transform.position.x > colEdge[i]+noteOffset[i]-0.05f && player.transform.position.x < colEdge[i]+noteOffset[i]+0.05f && isInCol[i]){
                if(Notes[i].transform.position.z > 1.0f){
                    playClip(0);
                }
                else if(Notes[i].transform.position.z < -1.0f){
                    playClip(2);
                }
                else{
                    playClip(1);
                }
            }
        }
	}
    public void addClip(AudioClip inputClip, int index) {
        switch (index){
            case 0:
                topSound = inputClip;
                aS[0].clip = topSound;
                break;
            case 1:
                middleSound = inputClip;
                aS[1].clip = middleSound;
                break;
            case 2:
                bottomSound = inputClip;
                aS[2].clip = bottomSound;
                break;
            default:
                print("Error: addClip case default");
                break;
        }
//        audioClip = inputClip;
//        aS.clip = audioClip;
    }

    // Checks if you have an inputted clip and plays it
    public void playClip(int index) {
        if(aS[index].clip != null) {
            aS[index].Play();
        } else {
            print("no audio clip buddy :( Try using the addClip function with an AudioClip");
        }
    }
}
