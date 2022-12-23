using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{

    public static UIController Instance { private set; get; } // Singleton
    [SerializeField] PlayerController player;
    [SerializeField] EventSystem eventSys;

    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject optionsBox;

    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text moneyText;

    [SerializeField] Animator arrowAnim;
    [SerializeField] List<Button> options = new List<Button>();

    Queue<string> sentences; // This is used to display the dialogue texts in order
    public bool playing;
    bool hasOptions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        sentences = new Queue<string>();
    }

    // Freezes the character in place and starts to show the text
    public void StartDialogue(Dialogue dialogue)
    {
        playing = true;
        player.canMove = false;
        sentences.Clear();
        hasOptions = dialogue.hasOptions;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        dialogueBox.SetActive(true);
        NextSentence();
    }

    // Triggers the next sentence
    public void NextSentence()
    {
        arrowAnim.gameObject.SetActive(false);
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var sentence = sentences.Dequeue();
        StopAllCoroutines(); // Ends the animation in case it's running
        StartCoroutine(SentenceAnimation(sentence));
    }

    // Closes the dialogue box and the player can move again
    private void EndDialogue()
    {
        if (hasOptions)
        {
            ShowOptions();
            return;
        }
        playing = false;
        player.canMove = true;
        dialogueBox.SetActive(false);
        optionsBox.SetActive(false);
    }

    // Used to display text char by char
    IEnumerator SentenceAnimation(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.02f);
        }
        arrowAnim.gameObject.SetActive(true);
        arrowAnim.Play(AnimationClips.UIArrowMove);
    }

    // Used to avoid the submit being made automatically, can be improved
    IEnumerator LockOptions()
    {
        yield return new WaitForSeconds(.01f);
        options.ForEach(item =>
        {
            item.interactable = true;
        });
        eventSys.SetSelectedGameObject(options[0].gameObject);
    }

    public void ShowOptions()
    {
        StartCoroutine(LockOptions());
        optionsBox.SetActive(true);
    }

    // Attached to the buttons, tells the player controller what to buy
    public void SelectedOption(int option)
    {
        StopAllCoroutines();
        options.ForEach(item =>
        {
            item.interactable = false;
        });
        hasOptions = false;
        EndDialogue();
        player.BuyCloth(option);
    }

    // Triggered when the player can't afford something
    public void CantBuy()
    {
        dialogueBox.SetActive(true);
        player.canMove = false;
        StartCoroutine(SentenceAnimation("Seems like you cannot afford this right now"));
        StartCoroutine(WaitForSubmit());
    }

    // Complements the method above, this system can be improved
    IEnumerator WaitForSubmit()
    {
        yield return new WaitForSeconds(2);
        dialogueBox.SetActive(false);
        player.canMove = true;
    }

    public void SetMoney()
    {
        moneyText.text = player.money.ToString();
    }
}
