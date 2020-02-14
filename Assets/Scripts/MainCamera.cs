using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCamera : MonoBehaviour
{
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pos.Set(
            Player.instance.transform.position.x,
            Player.instance.transform.position.y + 10,
            Player.instance.transform.position.z - 30
        );
        transform.position = Vector3.MoveTowards(transform.position, pos, 0.15f);
    }
}
