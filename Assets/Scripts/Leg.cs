using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    public bool planted = true;
    public int timeMoving;
    public Vector3 startPosition;
    public Vector3 endPosition;
    [HideInInspector]
    public LineRenderer rend;
    private float progress;
    private Vector3 nextStepXZ;
    public float nextStepY;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // high hill, height - 5.5 - player.height
        if (!planted) {
            if (Time.frameCount - timeMoving < 10) {
                progress = (float) Time.frameCount - timeMoving;
                nextStepXZ = rend.GetPosition(0) + 2 * transform.forward;
                nextStepXZ.y = nextStepY;
                rend.SetPosition(1, Vector3.Lerp(startPosition, nextStepXZ, progress / 10f));
            } else {
                planted = true;
            }
        }
    }
}
