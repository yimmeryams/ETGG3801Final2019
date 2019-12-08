using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlatformScript : MonoBehaviour
{
    [SerializeField]
    private GameObject flag;
    [SerializeField]
    private float flagSpeed = 0.0f;         // speed for flag to move
    [SerializeField]
    private float flagMoveY = 0.0f;         // distance for flag to move upwards when triggered

    private Vector3 finalFlagPos = Vector3.zero;

    [HideInInspector]
    public bool playerFinished = false;     // true if player made it to the finish platform; false otherwise


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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerFinished = true;
        }
    }
}
