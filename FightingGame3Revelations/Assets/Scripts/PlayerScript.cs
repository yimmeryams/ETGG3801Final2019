using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [HideInInspector]
    public int coinCount = 0;
    //private Text coinText;
    private bool dying = false;
    public float deathTimer = 3.0f;

    public float move_speed = 50.0f;
    public float look_speed = 100.0f;
    public float deathHeight = -150.0f;
    public float beatSensitivity = 0.85f;
    
    public int health = 6;
    [HideInInspector]
    public int currentHealth;

    private float jump_time = 0.0f;
    public float jump_max = 2.0f;
    private float jump_max_orig;
    public int jump_force = 40000;
    public float jump_force_mult = 50.0f;
    private float jump_force_mult_orig;
   
    private float current_cam_dist;

    public float camera_distance = 6.0f;
    public float camera_distance_launcher = 106.0f;
    
    public float camera_vertical_mult = 0.1f;
    public float camera_launcher_mult = 50.0f;

    private float jumpy;
    private float jumpyMax;

    private Rigidbody rb;
    private Camera cam;
    private GameObject skull;
    private Animator anim;

    private bool canJump = false;
    //private bool jumping = false;
    private Vector3 desired_camera_position;
    private Vector3 start_camera_position;
    
    private SkinnedMeshRenderer smr;
    [HideInInspector]
    public bool playerControl = true;
    
    // camera yaw and pitch
    // cam_trans.localRotation = Quat(curcampitch, Vec3.right)
    // this.transform.rotation = Quat(curcamyaw, Vec3.up)

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        Transform temp = this.transform.GetChild(0);
        skull = temp.gameObject;
        cam = skull.gameObject.GetComponentInChildren<Camera>();
        temp = this.transform.GetChild(1);
        smr = temp.GetComponentInChildren<SkinnedMeshRenderer>();
        anim = temp.GetComponent<Animator>();
        //coinText = FindObjectOfType<Canvas>().transform.GetChild(0).GetComponent<Text>();
        currentHealth = health;
        jump_force_mult_orig = jump_force_mult;
        jump_max_orig = jump_max;
    }

    // Update is called once per frame
    void Update()
    {
        // I used these:
        // https://forum.unity.com/threads/first-person-movement.495804/#post-3222990
        // https://forum.unity.com/threads/rotate-the-camera-around-the-object.47353/#post-301177
        // http://answers.unity.com/answers/667520/view.html
        // https://answers.unity.com/questions/1087351/limit-vertical-rotation-of-camera.html

        if (playerControl)
        {
            if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
            {
                rb.MovePosition(transform.position - (transform.forward * Input.GetAxis("Vertical") * move_speed * Time.deltaTime) - (transform.right * Input.GetAxis("Horizontal") * move_speed * Time.deltaTime));
                anim.SetTrigger("WalkStartTrigger");
                anim.ResetTrigger("WalkEndTrigger");
            }
            else
            {
                anim.SetTrigger("WalkEndTrigger");
                anim.ResetTrigger("WalkStartTrigger");
            }

            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * look_speed * Time.deltaTime, 0)));
            //rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("JoyMouseX") * look_speed * Time.deltaTime, 0)));

            cam.transform.Translate(Vector3.down * Input.GetAxis("Mouse Y") * look_speed * Time.deltaTime * camera_vertical_mult);
            //cam.transform.Translate(Vector3.down * Input.GetAxis("JoyMouseY") * look_speed * Time.deltaTime * camera_vertical_mult);
            if (cam.transform.localPosition.z < 0)
            {
                cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, -cam.transform.localPosition.z);
                //cam.transform.localRotation = Quaternion.Euler(cam.transform.localRotation.x, 180, 0);
            }
            cam.transform.LookAt(skull.transform);

            current_cam_dist = Vector3.Distance(cam.transform.position, skull.transform.position);
            if (current_cam_dist > camera_distance)
            {
                cam.transform.Translate(Vector3.forward * (current_cam_dist - camera_distance));
            }
            else if (current_cam_dist < camera_distance)
            {
                cam.transform.Translate(Vector3.back * (camera_distance - current_cam_dist));
            }

        }
        if (transform.position.y <= deathHeight)
        {
            print("deathHeight");
            PlayerDeath();
        }

        if (Input.GetKey(KeyCode.M))
        {

            coinCount += 100;
            move_speed = 250;
            MoneyUpdate();
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            currentHealth = health * 2;
            HealthUpdate();
        }
        if (Input.GetKey(KeyCode.Minus))
        {
            currentHealth -= 1;
            if (currentHealth < 1)
                currentHealth = 1;
            HealthUpdate();
        }
        if (Input.GetKey(KeyCode.Equals))
        {
            currentHealth += 1;
            HealthUpdate();
        }

        if (dying)
        {
            playerControl = false;
            transform.Rotate(new Vector3(15 * Time.deltaTime, 30 * Time.deltaTime, 45 * Time.deltaTime), Space.Self);
            deathTimer -= Time.deltaTime;
            anim.SetTrigger("WalkEndTrigger");
            anim.ResetTrigger("WalkStartTrigger");
            if (deathTimer < 0)
                MainMenu();
        }
    }

    private void FixedUpdate()
    {
        if (playerControl)
        {
            if (Input.GetAxis("Jump") != 0)
            {
                if (canJump & (jump_time < jump_max))
                {
                    
                    if (jump_time == 0)
                    {
                        jumpy = Mathf.Abs(this.GetComponent<BeatLogicScript>().getJumpTime());
                        jumpyMax = this.GetComponent<BeatLogicScript>().getMaxJumpTime();
                        //print(jumpy);
                        print(1 - jumpy / jumpyMax);
                        jump_force_mult = (1 - jumpy / jumpyMax) * jump_force_mult_orig * 4;
                        if ((1 - jumpy / jumpyMax) < beatSensitivity)
                        {
                            jump_force_mult = jump_force_mult / 20.0f;
                            jump_max = jump_max_orig / 5.0f;
                        }
                        else
                        {
                            jump_max = jump_max_orig;
                        }


                        rb.AddForce(new Vector3(0, jump_force  +  jump_force_mult * Time.deltaTime * jump_force, 0));
                        jump_time += Time.deltaTime;
                    }
                    else
                    {
                        jump_time += Time.deltaTime;
                        if (jump_time < jump_max)
                        {
                            rb.AddForce(new Vector3(0,  jump_force_mult * Time.deltaTime * jump_force, 0));
                        }
                        else
                        {
                            rb.AddForce(new Vector3(0, jump_force_mult * (Time.deltaTime - (jump_time - jump_max)) * jump_force, 0));
                        }
                    }
                }
            }
            else if (jump_time != 0.0f)
            {
                jump_time = jump_max;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jump_time = 0.0f;
            canJump = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jump_time = 0.0f;
            canJump = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MissileEnemy")
        {
            if (playerControl)
            {
                print("MissileEnemy");
                PlayerDeath();
            }
        }
    }

    void PlayerDeath()
    {
        dying = true;
        
    }

    void MainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    void HitByExplosion()
    {
        if (playerControl)
        {
            print("HitByExplosion");
            PlayerDeath();
        }
    }

    void MoneyUpdate()
    {
        //coinText.text = "Money: $" + coinCount;
    }

    void HealthUpdate()
    {
        GameObject.Find("HealthBarInner").SendMessage("Resize", (1.0f * currentHealth / health));
        if (currentHealth <= 0)
            PlayerDeath();
    }

    void TurnOffGravity()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
    }
    void TurnOnGravity()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
