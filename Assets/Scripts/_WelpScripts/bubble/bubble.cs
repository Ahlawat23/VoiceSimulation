using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Windows.Speech;
using System;
using System.Text;
using System.Linq;

public class bubble : MonoBehaviour
{

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();


    public Rigidbody2D rb;
    public Transform glassLid;
    public TextMeshPro text;

    private void Update()
    {
       
        if(transform.rotation.z > 0.3f || transform.rotation.z < -0.3f)
        {
          text.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void intialize(string keyword)
    {
       
        actions.Add(keyword, endGame);
        rb = GetComponent<Rigidbody2D>();
        text = transform.GetChild(1).GetComponent<TextMeshPro>();
       
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        text.text = keyword;

        
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());
        actions[args.text].Invoke();
    }


    public IEnumerator drop(float timeToMove)
    {
        rb.simulated = false;
        Vector3 finalPos = new Vector3(glassLid.position.x + UnityEngine.Random.Range(-4, 4), glassLid.position.y + 1, glassLid.position.z);
        rb.gameObject.transform.LeanMove(finalPos, timeToMove);
        yield return new WaitForSeconds(timeToMove);
        rb.simulated = true;
    }

    void endGame()
    {
        if (this.gameObject != null)
        {
            bubbleManager.instance.wordsSaid++;
            this.gameObject.SetActive(false);

        }
        
    }


}
