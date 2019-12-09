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
    public int diamond_rate = 90;
    // Start is called before the first frame update
    void Start()
    {
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
        Vector3 d = new Vector3(diamond_rate,0,0);
        Vector3 start = new Vector3(249, 0.0f, 0.0f);
        diamond.transform.position += (d*Time.deltaTime);
        if (diamond.transform.position.x >= 580.0f)
        {
            diamond.transform.position -= start;
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
