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
	public float WPM;
	public float BPM;
	public int marginSize;
	public int leadLetters;
	public int leadingWhiteSpace;
	public float targetBPM;
	public float shiftAmount;
	public float smoothTime;
	float currentVelocity;
	Queue<float> charsEntered;
	AudioSource audioSource;
	AudioMixer mixer;
	int scratchCursor;
	// Use this for initialization
	void Awake () {
		charsEntered = new Queue<float>();
		scratchCursor = -1;
		textDisplay = GetComponent<Text>();
		bpmDisplay = GameObject.Find("BPMDisplay").GetComponent<Text>();
		BPM = targetBPM;
		audioSource = GetComponent<AudioSource>();
		mixer = Resources.Load<AudioMixer>("AudioMixer"); 
		TextAsset textFile = Resources.Load<TextAsset>("text");
		targetText = textFile.text.Replace("\n", " ");
		curTime = 0;
		for(int i = 0; i < leadingWhiteSpace; i++) {
			targetText = string.Concat(" ", targetText);
		}
		leadLetters += leadingWhiteSpace;
		cursor += leadingWhiteSpace;
		audioSource.Play();
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(char c in Input.inputString) {
			if(targetText[cursor] == c) {
				cursor++;
				charsEntered.Enqueue(curTime);
				scratchCursor = -1;
			}
			else if(scratchCursor > 0) {
				if(targetText[scratchCursor] == c) {
					audioSource.time += WPM / 60;
				}
				else if(targetText[scratchCursor - 1] == c) {
					audioSource.time -= WPM / 60;
				}
			}
			/*else if(cursor > 0 && targetText[cursor - 1] == c) {
				audioSource.time -= WPM / 60;
				curTime -= Time.deltaTime;
				scratchCursor = cursor;
				cursor++;
				Debug.Log(WPM / 60 / 60);
			}*/
		}
		if(charsEntered.Count > 0){
			while(curTime - charsEntered.Peek() > 60) {
				charsEntered.Dequeue();
			}
		}
		if(cursor > leadLetters) {
			WPM = (charsEntered.Count / 5f) / (curTime / 60);//(cursor / 5f) / (curTime / 60);
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
		if(scratchCursor < 0) {
			curTime += Time.deltaTime;
			textDisplay.text = "<color=#ff0000>" + targetText.Substring(cursor - leadingWhiteSpace, leadingWhiteSpace) + "</color>" + targetText.Substring(cursor);
		} else {
			textDisplay.text = "<color=#ff000>" + targetText.Substring(scratchCursor - 1 - leadingWhiteSpace, leadingWhiteSpace) + "</color>" + targetText.Substring(cursor);
		}
		//Debug.Log(BPM);
	}
}
