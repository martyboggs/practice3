using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;

public class Npc : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 circling;
    private Vector3 direction;
    private float velocity;
    private CharacterController controller;
    public string State = "none";
    public List<string> conv = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        conv.Add("0hello");
        conv.Add("1hi");
        pos = transform.position;
        velocity = Random.value * 5 + 5;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (State == "circle") {
            circling.Set(
                10 * Mathf.Cos(0.015f * Time.frameCount),
                0,
                10 * Mathf.Sin(0.015f * Time.frameCount)
            );
            transform.LookAt(pos + circling);
            ChangeState();
        } else if (State == "follow") {
            transform.LookAt(Player.instance.transform.position);
            ChangeState();
        } else if (State == "talking") {

        } else {
            ChangeState();
        }

        direction.x = transform.forward.x;
        direction.z = transform.forward.z;
        // gravity
        direction.y += -2 * Time.deltaTime;

        if (State != "talking") {
            controller.Move(direction * velocity * Time.deltaTime);
        }
    }

    void ChangeState()
    {
        if (Time.frameCount % Mathf.Floor(velocity * 10000f) > 150) {
            State = "circle";
        } else {
            State = "follow";
        }
    }
}
