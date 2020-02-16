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
        Npc npc;
        Vector3 pos = new Vector3();
        for (int i = 0; i < 100; i ++)
        {
            pos.Set(Random.value * 100, 1, Random.value * 100);
            npc = Instantiate(PrefabManager.instance.Npc, pos, Quaternion.identity, transform);
        }
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
// npc says "hey i got you a burrito" player knocks it on the ground

// the sims - no predefined goal, emergent gameplay (desire to build something and explore experiences)
// portal - puzzles based on something that breaks the laws of physics (desire to explore this strange thing)
// lieve oma - recreating an experience and all the little things that are real and not cliche models
// choplifter - rescue people, have empathy for hostages, pride when saving them.

