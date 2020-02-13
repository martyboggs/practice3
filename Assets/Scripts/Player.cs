using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float VeloInfluence = 0.3f;
    private float velocity = 0.2f;
    public Vector3 direction;
    private Vector3 desiredDirection;
    bool up;
    bool down;
    bool left;
    bool right;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        desiredDirection = ArrowKeys();

        direction = Vector3.RotateTowards(direction, desiredDirection, 0.3f, 0.1f);

        // facing direction has a delay
        transform.LookAt(transform.position + direction);

        // move in direction of keys
        if (up || down || left || right) {
            transform.Translate(desiredDirection * velocity, Space.World);
        }

    }

    Vector3 ArrowKeys()
    {
        // 1 a
        // 2
        // 3 y
        // 4
        // 5 // r bumper
        // 6 // select
        // 7 //start
        // 8 left stick
        // 9 right stickclick
        // 10
        // 11
        // 15

        float dhaxis = Input.GetAxis("XboxDpadHorizontal");
        float dvaxis = Input.GetAxis("XboxDpadVertical");
        up = Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w") || dvaxis < 0;
        down = Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s") || dvaxis > 0;
        left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a") || dhaxis < 0;
        right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d") || dhaxis > 0;

        if (up)
        {
            desiredDirection += Vector3.forward;
        }
        if (down)
        {
            desiredDirection += Vector3.back;
        }
        if (left)
        {
            desiredDirection += Vector3.left;
        }
        if (right)
        {
            desiredDirection += Vector3.right;
        }
        desiredDirection = Vector3.Normalize(desiredDirection);
        return desiredDirection;
    }
}
