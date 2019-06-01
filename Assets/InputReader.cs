using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class InputReader : MonoBehaviour {

	public string targetText;
	Text textDisplay;
	Text bpmDisplay;
	int cursor;
	float curTime;
	float BPM;
	public int marginSize;
	public int leadLetters;
	public float targetBPM;
	public float shiftAmount;
	public float smoothTime;
	float currentVelocity;
	AudioSource audioSource;
	AudioMixer mixer;
	// Use this for initialization
	void Awake () {
		textDisplay = GetComponent<Text>();
		bpmDisplay = GameObject.Find("BPMDisplay").GetComponent<Text>();
		BPM = targetBPM;
		audioSource = GetComponent<AudioSource>();
		mixer = Resources.Load<AudioMixer>("AudioMixer"); 
		curTime = 0;
		audioSource.Play();
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(char c in Input.inputString) {
			if(targetText[cursor] == c) {
				cursor++;
			}
		}
		if(cursor > leadLetters) {
			float WPM = (cursor / 5f) / (curTime / 60);
			if(Mathf.Abs(BPM - WPM) > marginSize) {
				if(BPM > WPM) {
					BPM -= marginSize;
				} else {
					BPM += marginSize;
				}
			}
			bpmDisplay.text = "BPM: " + BPM.ToString();
			//BPM = (cursor / 5f) / (curTime / 60) / targetBPM;
			audioSource.pitch = Mathf.SmoothDamp(audioSource.pitch, BPM / targetBPM, ref currentVelocity, smoothTime);
			mixer.SetFloat("pitchBend", 1f / audioSource.pitch);
		}
		textDisplay.text = targetText.Substring(cursor);
		curTime += Time.deltaTime;
		//Debug.Log(BPM);
	}
}
