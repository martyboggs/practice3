using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour
{
    private Leg LeftLeg;
    private Leg RightLeg;
    private Leg ActiveLeg;
    private Leg InactiveLeg;

    // Start is called before the first frame update
    void Start()
    {
        LeftLeg = Instantiate(PrefabManager.instance.Leg, transform);
        RightLeg = Instantiate(PrefabManager.instance.Leg, transform);
        ActiveLeg = RightLeg;
        InactiveLeg = LeftLeg;

        LeftLeg.rend.SetPosition(1, transform.position);
        RightLeg.rend.SetPosition(1, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // set legs to hips
        LeftLeg.rend.SetPosition(0, transform.position + 0.5f * (Quaternion.AngleAxis(90, Vector3.up) * transform.forward));
        RightLeg.rend.SetPosition(0, transform.position + 0.5f * (Quaternion.AngleAxis(-90, Vector3.up) * transform.forward));

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
}
