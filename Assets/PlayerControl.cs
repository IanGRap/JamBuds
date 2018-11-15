using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    // We are going to start out by hardcoding the ability for two animals to play sounds while
    // we take the time to learn how to do this more dynamically

    public float speed = 5.0f;    // this controls the speed of the constant rightward motion
    public GameObject player;// This is a reference to the player, remember to drag and drop player object
    public Material noteMaterial; // this is a public variable containing the note materail
    public Transform staff;
    public bool useSpaces = true; // if true, the notes are placed in the middle of the spaces, not on the lines
    private GameObject[] Notes = new GameObject[8]; // these will keep references to the note objects
    private bool[] isInCol = new bool[] {false, false, false, false, false, false, false, false}; // this keeps track of the collumns which already have notes
    private float[] prevNotePosition = new float[8]; // this will hold the x positions of previous notes so that they can be replayed later
    private bool[] wasUsed = new bool[] {false, false, false, false, false, false, false, false};
    private bool isComplete = false; // this will become true when the player is placing new sounds for the second pass
    public AudioClip[] sounds = new AudioClip[6]; // array to hold the audio clips that will be used
    private AudioSource[] aS;        // Audio Source reference, used to play, pause, and manage the audio

    // these hold the indeces of sounnds with locations of current top middle and bottom
    private int top = 0;
    private int middle = 1;
    private int bottom = 2;

    ///////////////////////////////
    //LOOK BELOW TO MODIFY VALUES//
    ///////////////////////////////
    // below is a set of private constant variables to make script modification easier
    private const float vertMove = 6.666666666666f; // constant variable to control player motion
    private const float staffEdge = 18.0f;          // location of the edge of the staff
    private float[] colEdge = new float[] {-13.5f, -9.0f, -4.5f, 0.0f, 4.5f, 9.0f, 13.5f, 18.0f}; // location where notes can be placed
    private float[] noteOffset = new float[] {0.3f, 0.2f, 0.1f, 0.0f, -0.1f, -0.2f, -0.3f, -0.4f}; // these offset the position of the notes to account for paralax

	void Start () {
        aS = gameObject.GetComponents<AudioSource>();

        // move the position of the notes to the center of the spaces if option is selected
        if(useSpaces){
            for( int i = 0; i < colEdge.Length; i++){
                colEdge[i] = colEdge[i] - 2.25f;
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
      // constant motion is handled below, notice it is multiplies by speed
       player.transform.localPosition += Vector3.right * speed * Time.deltaTime;
        // this is the right-left world wrap
        if(player.transform.localPosition.x > staffEdge) {
            player.transform.localPosition = new Vector3(-staffEdge, player.transform.localPosition.y, player.transform.localPosition.z);
        }
        // This allows the player to move up
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z + vertMove);
        }
        // this allow the palyer to move down
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z - vertMove);
        }

        // this section is the world wrap from top to bottom
        if(player.transform.localPosition.z > vertMove){
            player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, -vertMove);
        }

        // this section is the world wrap from bottom to top
        if(player.transform.localPosition.z < -vertMove){
            player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, vertMove);
        }
        // This should create a not on the screen, I hope
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
            // calculate the Position and place note if necessary in collumns 2 through 8
            for(int i = 1; i < 8; i++){
                if(player.transform.localPosition.x < colEdge[i] && player.transform.localPosition.x > colEdge[i-1] && !isInCol[i]){
                    Notes[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                    Notes[i].transform.position = new Vector3(colEdge[i] + noteOffset[i]+staff.position.x, player.transform.position.y, player.transform.position.z);
                    isInCol[i] = true;
                    Notes[i].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                    Notes[i].GetComponent<Renderer>().material = noteMaterial; // apply the material
    //                numNotes++;// increase record of notes placed
                }
                else if(player.transform.localPosition.x < colEdge[i] && player.transform.localPosition.x > colEdge[i-1] && isInCol[i]){
                    Notes[i].transform.position = new Vector3(colEdge[i] + noteOffset[i]+staff.position.x, player.transform.position.y, player.transform.position.z);
                }
            }
            
            // calculate the position to place the note and place one if necessary 1st collumn
            if(player.transform.localPosition.x < colEdge[0] && !isInCol[0]){
                Notes[0] = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // add a note to the array 
                Notes[0].transform.position = new Vector3(colEdge[0] + noteOffset[0]+staff.position.x, player.transform.position.y, player.transform.position.z); // adjust x position for angle of view
                isInCol[0] = true;
                Notes[0].transform.Rotate(Vector3.right * 90.0f); // rotate the cylinder
                Notes[0].GetComponent<Renderer>().material = noteMaterial; // apply the material
//                numNotes++;// increase record of notes placed
            }
            else if(player.transform.localPosition.x < colEdge[0] && isInCol[0]){
                Notes[0].transform.position = new Vector3(colEdge[0] + noteOffset[0]+staff.position.x, player.transform.position.y, player.transform.localPosition.z); // adjust x position for angle of view
            }

        }
        // place audio clip play sounds
        for(int i = 0; i < 8; i++){
            if(player.transform.localPosition.x > colEdge[i]+noteOffset[i]-0.05f && player.transform.localPosition.x < colEdge[i]+noteOffset[i]+0.05f && isInCol[i]){
                if(Notes[i].transform.localPosition.z > player.transform.parent.position.z + 1.0f){
                    addAndPlay(sounds[top], 0);
                }
                else if(Notes[i].transform.localPosition.z < player.transform.parent.position.z-1.0f){
                    addAndPlay(sounds[bottom], 0);
                }
                else{
                    addAndPlay(sounds[middle], 0);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.N) && !isComplete){
            for(int i = 0; i < Notes.Length; i++){
                if(isInCol[i]){
                    prevNotePosition[i] = Notes[i].transform.localPosition.z;
                }
                wasUsed[i] = isInCol[i];
                isInCol[i] = false;
                Destroy(Notes[i]);
            }
            top += 3;
            middle += 3;
            bottom += 3;
            isComplete = true;
        }

        if(isComplete){
            playBackground();
        }
    }

    public void playBackground(){
        for(int i = 0; i < 8; i++){
            if(player.transform.localPosition.x > colEdge[i]+noteOffset[i]-0.05f && player.transform.localPosition.x < colEdge[i]+noteOffset[i]+0.05f && wasUsed[i]){
                if(prevNotePosition[i] > 1.0f){
                    addAndPlay(sounds[0], 1);
                }
                else if(prevNotePosition[i] < -1.0f){
                    addAndPlay(sounds[2], 1);
                }
                else{
                    addAndPlay(sounds[1], 1);
                }
            }
        }
    }

    public void addAndPlay(AudioClip inputClip, int index){
        aS[index].clip = inputClip;
        if(aS[index].clip != null){
            aS[index].Play();
        }
        else {
            print("no audio clip provided");
        }
    }
}
