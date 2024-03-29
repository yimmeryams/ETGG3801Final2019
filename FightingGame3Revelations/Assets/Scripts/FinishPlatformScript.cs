﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPlatformScript : MonoBehaviour
{
    [SerializeField]
    private GameObject flag;
    [SerializeField]
    private float flagSpeed = 0.0f;         // speed for flag to move
    [SerializeField]
    private float flagMoveY = 0.0f;         // distance for flag to move upwards when triggered

    private Vector3 finalFlagPos = Vector3.zero;
    private bool playerFinished = false;     // true if player made it to the finish platform; false otherwise
    private float timer = 0;
    public float waitTime = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        finalFlagPos = flag.transform.position + new Vector3(0, flagMoveY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFinished)
        {
            if (Vector3.Distance(flag.transform.position, finalFlagPos) > 0.01)
            {
                flag.transform.position = Vector3.MoveTowards(flag.transform.position, finalFlagPos, flagSpeed * Time.deltaTime);
            }
        }

        bool test = IsPlayerFinished();
        if (test)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
                SceneManager.LoadScene("MenuWin");
        }





        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerFinished = true;

            other.GetComponent<AudioSource>().enabled = false;
            GetComponent<AudioSource>().PlayDelayed(0.5f);
        }
    }

    // returns playerFinished; true if player has reached finish platform, false otherwise
    public bool IsPlayerFinished()
    {
        return playerFinished;
    }
}
