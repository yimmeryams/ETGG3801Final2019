using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPlatformScript : MonoBehaviour
{
    [SerializeField]
    private bool cyclicalPath = false;      // true: platform will navigate through the list and cycle back (ex: 1->2->3->1->etc.)
                                            // false: platform will navigate to the next or previous element in list (ex: 1->2->3->2->1->etc.)
    [SerializeField]
    private Vector3 spinRate = Vector3.zero;    // specifies any rotation of the platform (Vector3.zero signifies no rotation)
    [SerializeField]
    private Vector3[] path;                 // list of Vector3 positions to navigate to
    [SerializeField]
    private float platformSpeed = 0.0f;     // speed of platform when navigating
    [SerializeField]
    private int startingPoint = 0;          // index of Vector3 position platform will start from (becomes lastPoint)
    [SerializeField]
    private int platformDirection = 1;      // 1 if moving right through the list (start -> end), -1 if moving left through the list (end -> start)

    private int lastPoint = 0;              // index of Vector3 position platform was last at
    private int targetPoint = 1;            // index of Vector3 position platform is currently navigating to


    // Start is called before the first frame update
    void Start()
    {
        lastPoint = startingPoint;

        // linear
        if (!cyclicalPath)
        {
            // last point
            if (lastPoint == path.Length - 1)
            {
                targetPoint = lastPoint - 1;
                platformDirection = -1;
            }
            // first point
            else if (lastPoint == 0)
            {
                targetPoint = 1;
                platformDirection = 1;
            }
            // in the middle
            else
            {
                targetPoint = lastPoint + platformDirection;
            }
        }
        // cyclical
        else
        {
            targetPoint = (lastPoint + platformDirection) % path.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // rotate platform (if one is given)
        if (spinRate != Vector3.zero)
        {
            transform.Rotate(transform.right, spinRate.x * platformSpeed * Time.deltaTime);
            transform.Rotate(transform.up, spinRate.y * platformSpeed * Time.deltaTime);
            transform.Rotate(transform.forward, spinRate.z * platformSpeed * Time.deltaTime);
        }

        // navigate platform path
        if (path.Length > 0)
        {
            float step = platformSpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, path[targetPoint], step);

            if (Vector3.Distance(transform.position, path[targetPoint]) <= 0.025)
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
    }

    void OnDrawGizmos()
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
