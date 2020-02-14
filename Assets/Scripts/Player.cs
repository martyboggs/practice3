using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float VeloInfluence = 0.3f;
    private float velocity = 0.2f;
    private Vector3 realDir;
    private Vector3 realDirXZ;
    private Vector3 slowDir;
    private CharacterController controller;
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
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        realDir = ArrowKeys();

        // facing slowDir has a delay
        realDirXZ.Set(realDir.x, 0, realDir.z);
        slowDir = Vector3.RotateTowards(slowDir, realDirXZ, 0.1f, 0.1f);
        transform.LookAt(transform.position + slowDir);

        // gravity
        realDir.y += -2 * Time.deltaTime;

        // move in direction of keys
        if (up || down || left || right) {
            controller.Move(realDir * velocity);
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

        if (up || down || left || right) {
            realDir.x = 0;
            realDir.z = 0;
            if (up) {
                realDir += Vector3.forward;
            }
            if (down) {
                realDir += Vector3.back;
            }
            if (left) {
                realDir += Vector3.left;
            }
            if (right) {
                realDir += Vector3.right;
            }
        }

        return realDir;
    }
}
