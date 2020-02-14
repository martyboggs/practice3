using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 circling;
    private Vector3 direction;
    private float velocity;
    private CharacterController controller;
    public string State = "none";


    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        velocity = Random.value / 10f;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % Mathf.Floor(velocity * 10000f) > 150) {
            State = "circle";
        } else {
            State = "follow";
        }

        if (State == "circle") {
            circling.Set(
                10 * Mathf.Cos(0.015f * Time.frameCount),
                0,
                10 * Mathf.Sin(0.015f * Time.frameCount)
            );
            transform.LookAt(pos + circling);
        } else if (State == "follow") {
            transform.LookAt(Player.instance.transform.position);
        }
        direction.x = transform.forward.x;
        direction.z = transform.forward.z;
        // gravity
        direction.y += -2 * Time.deltaTime;

        controller.Move(direction * velocity);
    }
}
