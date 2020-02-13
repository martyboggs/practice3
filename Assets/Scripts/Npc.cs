using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 circling;
    private Vector3 direction;
    private float speed;
    public string State = "none";

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        speed = Random.value / 10f;
    }

    // Update is called once per frame
    void Update()
    {
        direction = Player.instance.transform.position - transform.position;
        if (Time.frameCount % Mathf.Floor(speed * 10000f) > 150) {
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
        transform.position += speed * transform.forward;
    }
}
