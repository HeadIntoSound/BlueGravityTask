using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the main actions of the NPC, for now it's only talking 
public class NPCController : MonoBehaviour
{
    public Dialogue dialogue;

    public void Talk()
    {

        if (!UIController.Instance.playing)
        {
            UIController.Instance.StartDialogue(dialogue);
        }
        else
        {
            UIController.Instance.NextSentence();
        }

    }
}
