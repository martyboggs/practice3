using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float VeloInfluence = 0.3f;
    public Leg LeftLeg;
    public Leg RightLeg;
    private Leg ActiveLeg;
    private Leg InactiveLeg;
    private float velocity = 0.2f;
    public Vector3 direction;
    private Vector3 desiredDirection;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ActiveLeg = RightLeg;
        InactiveLeg = LeftLeg;
    }

    // Update is called once per frame
    void Update()
    {
        desiredDirection = ArrowKeys();

        direction = Vector3.RotateTowards(direction, desiredDirection, 0.3f, 0.1f);

        // facing direction has a delay
        transform.LookAt(transform.position + direction);

        // move in direction of keys
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a") || Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d") ||
        Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w") || Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s")) {
            transform.Translate(desiredDirection * velocity, Space.World);
        }

        // set legs to hips
        LeftLeg.rend.SetPosition(0, transform.position + 0.5f * Vector3.left);
        RightLeg.rend.SetPosition(0, transform.position + 0.5f * Vector3.right);

        // take step
        if (ActiveLeg.planted && InactiveLeg.planted && Vector3.Distance(transform.position, ActiveLeg.rend.GetPosition(1)) > 2)
        {
            TakeStep(ActiveLeg);
            if (ActiveLeg == RightLeg) {
                ActiveLeg = LeftLeg;
                InactiveLeg = RightLeg;
            } else {
                ActiveLeg = RightLeg;
                InactiveLeg = LeftLeg;
            }
        }
    }

    void TakeStep(Leg leg)
    {
        leg.planted = false;
        leg.timeMoving = Time.frameCount;
        leg.startPosition = leg.rend.GetPosition(1);
    }

    Vector3 ArrowKeys()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a"))
        {
            desiredDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d"))
        {
            desiredDirection += Vector3.right;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w"))
        {
            desiredDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s"))
        {
            desiredDirection += Vector3.back;
        }
        desiredDirection = Vector3.Normalize(desiredDirection);
        return desiredDirection;
    }
}
