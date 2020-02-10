using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    public bool planted = true;
    public int timeMoving;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public Vector3 ElbowPoint;
    public LineRenderer rend;
    private float progress;


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Vector3.distance(transform.position, EndPosition) < 0.2f)
        // {
        // }

        if (!planted) {
            if (Time.frameCount - timeMoving < 10) {
                progress = (float) Time.frameCount - timeMoving;
                rend.SetPosition(1, Vector3.Lerp(startPosition, rend.GetPosition(0) + 2 * Player.instance.direction + 0.5f * Vector3.down, progress / 10f));
            } else {
                planted = true;
            }
        }
    }
}
