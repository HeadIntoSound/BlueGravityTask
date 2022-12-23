using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class stores the information that's going to intervene in a transaction
public class BuyCloth : MonoBehaviour
{
    public Clothing hat;
    public Clothing cloth;

    public Dialogue dialogue;
    [TextArea][SerializeField] string ownHat;
    [TextArea][SerializeField] string ownCloth;
    [TextArea][SerializeField] string changeClothes;

    private void Start()
    {
        SetPrices();
    }

    // Formats the string to the price in the dialogue
    private void SetPrices()
    {
        var result = String.Format(dialogue.sentences[dialogue.sentences.Length - 1], hat.price, cloth.price, hat.price + cloth.price);
        dialogue.sentences[dialogue.sentences.Length - 1] = result;
    }

    // If some text is showing, moves to the next, otherwise it starts the conversation
    public void BuyText()
    {
        if (!UIController.Instance.playing)
            UIController.Instance.StartDialogue(dialogue);
        else
            UIController.Instance.NextSentence();
    }

    // Sets each clothing owned variable if bought
    public void SetOwned(int option)
    {
        if (option == 0 || option == 2)
        {
            dialogue.sentences[dialogue.sentences.Length - 1] = String.Format(ownHat, cloth.price);
            hat.owned = true;
        }

        if (option == 1 || option == 2)
        {
            dialogue.sentences[dialogue.sentences.Length - 1] = String.Format(ownCloth, hat.price);
            cloth.owned = true;
        }
        if (hat.owned && cloth.owned)
        {
            dialogue.sentences[dialogue.sentences.Length - 1] = changeClothes;
        }
    }
}
