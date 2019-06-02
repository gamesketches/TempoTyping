using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour {

	public static float travelSpeed = 3;
	InputReader inputReader;
	Animator animator;

	// Use this for initialization
	void Start () {
		inputReader = GameObject.Find("TextDisplay").GetComponent<InputReader>();
		animator = GetComponent<Animator>();
		animator.SetInteger("Selection", Random.Range(0,9));
	}
	
	// Update is called once per frame
	void Update () {
		float rate = inputReader.BPM / inputReader.targetBPM;
		transform.Translate( -rate * travelSpeed * Time.deltaTime, 0, 0);
		animator.speed = rate;
		if(transform.position.x < -100) Destroy(this.gameObject);
	}
}
