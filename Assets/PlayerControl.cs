using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    private int speed = 5;
	// Use this for initialization
    public Transform transform;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.right * speed * Time.deltaTime;
	}
}
