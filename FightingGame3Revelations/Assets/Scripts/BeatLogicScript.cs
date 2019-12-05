using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatLogicScript : MonoBehaviour
{
	public AudioClip normBeatSound;
	public AudioClip jumpBeatSound;
	public AudioSource audio;
	
	public float beat1Time;
	public float beat2Time;
	public float beat3Time;
	public float beat4Time;
	//private float beatEndTime;
	public float jumpBeatTime;
	
	private int curBeat;
	private bool NeedsToJump;
	
	private float curTimer;
	private float curJumpTimer;
	
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
		curTimer = beat1Time;
		curJumpTimer = jumpBeatTime;
		//beatEndTime = jumpBeatTime - (beat1Time + beat2Time + beat3Time + beat4Time) + (beat1Time / 2); 
		NeedsToJump = true;
		curBeat = 1;
    }

    // Update is called once per frame
    void Update()
    {
        curTimer -= Time.deltaTime;
		curJumpTimer -= Time.deltaTime;
		
		if(curTimer <= 0)
		{
			// Play normal beat, get ready to play next one.
			if(curBeat < 5)
			{
                audio.PlayOneShot (normBeatSound, 1.0f);
			}
			
			// Increment the beat we are playing currently
			curBeat = (curBeat + 1) % 5;
			switch(curBeat)
			{
				case 1:
					// This is the reset of the sequence, so we reset everything accordingly
					curTimer = beat1Time;
					curJumpTimer = jumpBeatTime;
					NeedsToJump = true;
					break;
				case 2:
					curTimer = beat2Time;
					break;
				case 3:
					curTimer = beat3Time;
					break;
				case 4:
					curTimer = beat4Time;
					break;
				case 5:
					//curTimer = beatEndTime;
					break;
				default:
					break;
			}
		}
		// Checks if we are on jump beat, and that we need to play the jump beat.
		if(curJumpTimer <= 0 && NeedsToJump == true)
		{
			// Play jump beat, set the need to play jump beat to false;
		    audio.PlayOneShot (jumpBeatSound, 1.0f);
			
			//curJumpTimer = jumpBeatTime;
			NeedsToJump = false;
		}
    }
	
	public float getJumpTime()
	{
		// How close it is to the "jump beat"
		// 0 is when the jump beat hits
		return curJumpTimer;
	}
	
	public float getMaxJumpTime()
	{
		// Get the max time for jump Beat from the beginning of the beat sequence
		// What the jump beat timer gets reset to each sequence
		return jumpBeatTime;
	}
	
}
