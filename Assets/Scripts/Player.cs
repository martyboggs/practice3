using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float VeloInfluence = 0.3f;
    [HideInInspector]
    public string State = "none";
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
        realDirXZ = ArrowKeys();

        // facing slowDir has a delay
        slowDir = Vector3.RotateTowards(slowDir, realDirXZ, 0.1f, 0.1f);
        transform.LookAt(transform.position + slowDir);

        // gravity
        realDir.y += -2 * Time.deltaTime;

        // move in direction of keys
        if (State == "none") {
            if (up || down || left || right) {
                realDir.x = realDirXZ.x;
                realDir.z = realDirXZ.z;
                controller.Move(realDir * velocity);
            }
        } else if (State == "talking") {

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
            realDirXZ.Set(0, 0, 0);
            if (up) {
                realDirXZ += Vector3.forward;
            }
            if (down) {
                realDirXZ += Vector3.back;
            }
            if (left) {
                realDirXZ += Vector3.left;
            }
            if (right) {
                realDirXZ += Vector3.right;
            }
        }

        return Vector3.Normalize(realDirXZ);
    }
}
