using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingame_Ui : MonoBehaviour
{
    public bool isPaused = false;
    public bool isOver = false;
    private bool no_error = true;
    private float minutes = 30;
    private float seconds = 59.9999f;
    public int diamond_rate = 50;
    private float diamondStartX;
    private float diamondStartY;
    private float barLeft;
    private float barRight;
    private float barSize;
    private float magicDiamondNumber;
    public float beatDuration = 1.75f;   // could grab this from Player->BeatLogicScript but nah I'm too lazy ~JDP
    // Start is called before the first frame update
    void Start()
    {
        // saves starting position of diamond, place it on the orange circle ~JDP
        diamondStartX = GetComponent<Canvas>().transform.GetChild(3).transform.position.x;
        diamondStartY = GetComponent<Canvas>().transform.GetChild(3).transform.position.y;
        // saves the width, left bound pos, and right bound pos of the beat bar ~JDP
        barSize = GetComponent<Canvas>().transform.GetChild(2).transform.GetComponent<RectTransform>().sizeDelta.x;
        barLeft = GetComponent<Canvas>().transform.GetChild(2).transform.position.x - barSize / 2.0f;
        barRight = GetComponent<Canvas>().transform.GetChild(2).transform.position.x + barSize / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        seconds -= Time.deltaTime;
        if (seconds < 0)
        {
            minutes -= 1;
            seconds = 60;
        }
        if (minutes == -1)
        {
            isOver = true;
        }
        Text time = GameObject.Find("Timer").GetComponent<Text>();
        time.text = minutes + ":" + seconds;
        GameObject diamond = GameObject.Find("Diamond");

        //Vector3 d = new Vector3(diamond_rate,0,0);
        //Vector3 start = new Vector3(249, 0.0f, 0.0f);

        // calculating how far to move the Diamond  ~JDP
        float percentage = Time.deltaTime / beatDuration;
        magicDiamondNumber = percentage * barSize;
        magicDiamondNumber = 0.98f * magicDiamondNumber;
        diamond.transform.position = new Vector3(diamond.transform.position.x + magicDiamondNumber, diamond.transform.position.y, diamond.transform.position.z);
        if (diamond.transform.position.x >= barRight)
        {
            diamond.transform.position = new Vector3(diamond.transform.position.x - barSize, diamond.transform.position.y, diamond.transform.position.z);
        }
        if (isPaused)
        {
            Vector3 show = new Vector3(1, 1, 1);
            GameObject.Find("PauseText").transform.localScale = show;
            GameObject.Find("Continue").transform.localScale = show;
            GameObject.Find("Quit").transform.localScale = show;
            no_error = true;
        }
        else if (no_error)
        {
            Vector3 hide = new Vector3(0, 0, 0);
            GameObject.Find("PauseText").transform.localScale = hide;
            GameObject.Find("Continue").transform.localScale = hide;
            GameObject.Find("Quit").transform.localScale = hide;
            no_error = false;
        }
        if (isOver)
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = true;
        }
    }
    public void start()
    {
        isPaused = false;
    }
    public void stop()
    {
        Application.Quit();
    }
}
