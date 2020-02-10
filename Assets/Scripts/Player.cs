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
    public Vector3 direction = Vector3.forward;
    private Vector3 lastPosition;
    private float velocity = 0.2f;

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
        direction = Vector3.Normalize(transform.position - lastPosition);
        lastPosition = transform.position;

        ArrowKeys();

        LeftLeg.rend.SetPosition(0, transform.position + 0.5f * Vector3.left);
        RightLeg.rend.SetPosition(0, transform.position + 0.5f * Vector3.right);

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

    void ArrowKeys()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * velocity, Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * velocity, Space.World);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * velocity, Space.World);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * velocity, Space.World);
        }
    }
}
