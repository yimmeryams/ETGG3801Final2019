using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField]
    private bool cyclicalPath = false;      // true: platform will navigate through the list and cycle back (ex: 1->2->3->1->etc.)
                                            // else: platform will navigate to the next or previous element in list (ex: 1->2->3->2->1->etc.)
    [SerializeField]
    private Vector3[] path;                 // list of Vector3 positions to navigate to
    [SerializeField]
    private float platformSpeed = 0.0f;     // speed of platform when navigating

    private int lastPoint = 0;              // index of Vector3 position platform was last at
    private int targetPoint = 1;            // index of Vector3 position platform is currently navigating to
    private int platformDirection = 1;      // 1 if moving right through the list, -1 if moving left through the list


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = platformSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, path[targetPoint], step);

        if (Vector3.Distance(transform.position, path[targetPoint]) <= 0.05)
        {
            if (!cyclicalPath)
            {
                if (targetPoint == 0)
                {
                    platformDirection = 1;
                }
                else if (targetPoint == path.Length - 1)
                {
                    platformDirection = -1;
                }

                lastPoint = targetPoint;
                targetPoint += platformDirection;
            }
            else
            {
                targetPoint = Mathf.Abs(targetPoint + platformDirection) % path.Length;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // draw wire spheres at path positions
        Gizmos.color = Color.red;

        foreach (Vector3 pos in path)
        {
            Gizmos.DrawWireSphere(pos, 0.5f);
        }

        // draw path
        Gizmos.color = Color.magenta;

        if (!cyclicalPath)
        {
            for (int i = 0; i < path.Length - 1; i++)
            {
                // current path platform is using
                if (i == lastPoint && i + 1 == targetPoint ||
                    i == targetPoint && i + 1 == lastPoint)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(path[i], path[i + 1]);

                    Gizmos.color = Color.magenta; // reset color
                }
                else
                {
                    Gizmos.DrawLine(path[i], path[i + 1]);
                }
            }
        }
        else
        {
            for (int i = 0; i < path.Length; i++)
            {
                // not end of path list
                if (i != path.Length - 1)
                {
                    Gizmos.DrawLine(path[i], path[i + 1]);
                }
                // end of path list (draw line from end to start)
                else
                {
                    Gizmos.DrawLine(path[i], path[0]);
                }
            }
        }
    }
}
