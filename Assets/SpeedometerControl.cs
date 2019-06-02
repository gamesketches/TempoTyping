using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedometerControl : MonoBehaviour {

	Image speedometerImage;
	InputReader inputReader;

	// Use this for initialization
	void Start () {
		speedometerImage = GetComponent<Image>();	
		inputReader = GameObject.Find("TextDisplay").GetComponent<InputReader>();
	}
	
	// Update is called once per frame
	void Update () {
		float diff = inputReader.targetBPM - inputReader.WPM;
		transform.rotation = Quaternion.Euler(0, 0, diff);
	}
}
