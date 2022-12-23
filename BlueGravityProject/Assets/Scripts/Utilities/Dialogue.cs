using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class stores the basic information of the dialogue system
[System.Serializable]
public class Dialogue
{
    public string name;
    [TextArea] public string[] sentences;
    public bool hasOptions;
}
