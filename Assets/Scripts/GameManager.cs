using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string State = "none";
    public int Score = 0;

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
    }

    void Reset()
    {

    }
}

// task list
// get toon material on everything
// pixelation?
// blob shake
// make it walk

// game ideas
// parent holding player's hand so he can't really go anywhere, just squirm around
// player doesn't do their job and just goes exploring instead
// throws waterballoons at cars, shoots potato gun
// particles
// treehouses
// fun = having no power to having lots of power
