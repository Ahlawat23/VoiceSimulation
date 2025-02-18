using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class welcomeChat : MonoBehaviour
{
    public Text chatText;
	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string leadingChar;
	[SerializeField] bool leadingCharBeforeDelay = false;


   
    
    public void showChat(string msg)
    {
        Debug.Log("text called");
		chatText.text = msg;
		StartCoroutine(TypeWriterText(msg));
		animateChatBox();
    }

	public void animateChatBox()
    {

    }
	
	IEnumerator TypeWriterText(string writer)
	{
        chatText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
            if (chatText.text.Length > 0)
            {
                chatText.text = chatText.text.Substring(0, chatText.text.Length - leadingChar.Length);
            }
            chatText.text += c;
            chatText.text += leadingChar;
            yield return new WaitForSeconds(timeBtwChars);
		}

        if (leadingChar != "")
        {
            chatText.text = chatText.text.Substring(0, chatText.text.Length - leadingChar.Length);
        }
    }


}
