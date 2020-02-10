using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;
    public Color ButtonColor;
    public Color BackgroundColor;
    public Color TextColor;
    public Color DisabledTextColor;
    public Color FailedTextColor;
    public Color SpecialTextColor;
    public Color OffTextColor;

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
}
