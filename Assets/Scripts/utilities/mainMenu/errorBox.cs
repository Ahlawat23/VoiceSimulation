using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class errorBox : MonoBehaviour
{
    public Transform box;
    public CanvasGroup background;
    public Text dialougeText;

    [Header("docotLogin")]
    public Image backgroundImage;
    public RectTransform okButton;
    public RectTransform continueOfflineButton;

    [Header("vectors")]
    public Vector2 okButtonPosToGoOffline;
    public Vector2 ContinueButPosToGoOFfline;
    public Color backGroundColorToGoOffline;
    private void OnEnable()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;


    }

    public void showDialougeBox(string inputText, bool goOfflinePropmt = false)
    {
        dialougeText.text = inputText;
        gameObject.SetActive(true);

        if (goOfflinePropmt)
            loginDoctorInterNetError();
        
    }

    public void loginDoctorInterNetError()
    {
        continueOfflineButton.gameObject.SetActive(true);
        backgroundImage.color = backGroundColorToGoOffline;


        okButton.anchoredPosition = okButtonPosToGoOffline;
        continueOfflineButton.anchoredPosition = ContinueButPosToGoOFfline;
        

    }

    public void closeDialogBox()
    {
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo();
        StartCoroutine(watiforsec());
    }

    IEnumerator watiforsec()
    {
        yield return new WaitForSeconds(1);
        background.gameObject.SetActive(false);
    }

   
}
