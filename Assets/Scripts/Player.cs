using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fix foot placement
// add juice
// what do i want to demonstrate?
//    job pickig apples... game sucks... price of stuff in store is too high
//    sell to people, don't get caught, put in packages, market the product

public class Player : MonoBehaviour
{
    public static Player instance;
    public float VeloInfluence = 0.3f;
    [HideInInspector]
    public string State = "none";
    [HideInInspector]
    public GameObject TalkingTo;
    private float velocity = 15f;
    private Vector3 realDir;
    private Vector3 realDirXZ;
    private Vector3 slowDir;
    private CharacterController controller;
    private bool jumping = false;
    int talkTime = 0;
    bool up;
    bool down;
    bool left;
    bool right;
    private Vector3 forward;

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
        // move in direction of keys
        if (State == "none") {
            realDirXZ = ArrowKeys();
            // jump
            if (!jumping && Input.GetKeyUp(KeyCode.Space)) {
                jumping = true;
                realDir.y = 3;
            }
            // facing slowDir has a delay
            slowDir = Vector3.RotateTowards(slowDir, realDirXZ, 0.1f, 0.1f);
            transform.LookAt(transform.position + slowDir);
        } else if (State == "talking") {
            realDirXZ = Vector3.zero;
        }

        // gravity
        realDir.y += -9.8f * Time.deltaTime;

        realDir.x = realDirXZ.x;
        realDir.z = realDirXZ.z;
        controller.Move(realDir * velocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Time.frameCount > talkTime && !TalkingTo && hit.collider.gameObject.tag == "Npc") {
            talkTime = Time.frameCount + 300;
            TalkingTo = hit.collider.gameObject;
            Npc npc = TalkingTo.GetComponent<Npc>();
            npc.State = "talking";
            State = "talking";
            Physics.autoSimulation = false;
            Speech.instance.Talk(npc);
            Speech.instance.gameObject.SetActive(true);
            Speech.instance.transform.position = TalkingTo.transform.position + 10 * Vector3.up;
        }

        if (hit.collider.gameObject.tag == "Terrain") {
            realDir.y = 0;
            jumping = false;
        }

        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We don't want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * 10.0f;
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

        realDirXZ.x = 0;
        realDirXZ.z = 0;
        if (up || down || left || right) {
            // unit vector from camera to player
            forward = Player.instance.transform.position - MainCamera.instance.transform.position;
            if (up) {
                realDirXZ += forward;
            }
            if (down) {
                realDirXZ += Quaternion.Euler(0, 180, 0) * forward;
            }
            if (left) {
                realDirXZ += Quaternion.Euler(0, -90, 0) * forward;
            }
            if (right) {
                realDirXZ += Quaternion.Euler(0, 90, 0) * forward;
            }
        }
        realDirXZ.y = 0;

        return Vector3.Normalize(realDirXZ);
    }
}
