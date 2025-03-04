﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCamera : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 10, -30);
    public static MainCamera instance;

    void Awake()
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
        if (Input.GetKeyUp(KeyCode.J)) {
            offset = Quaternion.Euler(0, Player.instance.transform.eulerAngles.y, 0) * new Vector3(0, 10, -30);
        }

        transform.position = Vector3.MoveTowards(
            transform.position, Player.instance.transform.position + offset,
            Vector3.Distance(transform.position - offset, Player.instance.transform.position) * 1.5f * Time.deltaTime
        );
        transform.LookAt(Player.instance.transform);

    }
}
