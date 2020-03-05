using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speech : MonoBehaviour
{
    public static Speech instance;
    public Text text;
    public RectTransform panel;
    private int letterWidth = 400;
    private int convIndex;
    private int charIndex;
    private List<string> conv = new List<string>();
    public string State = "none";
    private Npc TalkingTo;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (State == "blurbing") {
            panel.sizeDelta = new Vector2(letterWidth * conv[convIndex].Length, 1);
            // panel.SetSizeWithCurrentAnchors();
            State = "talking";
        } else if (State == "talking") {
            text.text = conv[convIndex].Substring(1, charIndex);
            if (conv[convIndex].Substring(0, 1) == "0") {
                text.color = Color.blue;
            } else {
                text.color = Color.black;
            }
            if (charIndex < conv[convIndex].Length - 1) {
            } else {
                State = "paused";
            }
            charIndex++;
        } else if (State == "paused") {
            if (Input.GetKeyUp(KeyCode.Return)) {
                convIndex++;
                charIndex = 1;
                text.text = "";
                if (convIndex < conv.Count) {
                    State = "blurbing";
                } else {
                    State = "none";
                    gameObject.SetActive(false);
                    Player.instance.State = "none";
                    Player.instance.TalkingTo = null;
                    TalkingTo.State = "none";
                    Physics.autoSimulation = true;
                }
            }
        }
    }

    public void Talk(Npc npc)
    {
        convIndex = 0;
        charIndex = 1;
        TalkingTo = npc;
        conv = npc.conv;
        State = "blurbing";
    }
}
