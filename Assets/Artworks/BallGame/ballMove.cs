using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;


public class ballMove : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    Animator animator;
    void Start()
    {
        actions.Add("KAAA", Forward);
        actions.Add("next", Up);
        actions.Add("down", Down);
        actions.Add("back", Back);


        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        animator = GetComponent<Animator>();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Forward()
    {
        transform.Translate(7, 0, 0);
    }

    private void Down()
    {
        //transform.Translate(-1, 0, 0);
    }

    private void Up()
    {
        //transform.Translate(0, 1, 0);
        //animator.Play("move");
    }

    private void Back()
    {
        //transform.Translate(0, -1, 0);
    }
}



