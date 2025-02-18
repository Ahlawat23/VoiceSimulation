using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class tabPanlesManager : MonoBehaviour
{
    public GameObject[] panels;

    public Text[] buttonTexts;
    public Image[] buttonImages;

    [Header("asthetic vars")]
    public Color selectedtextColour;
    public Color deselectedTextColour;
    public Color selecetedButtonColour;
    public Color deselectedButtonColour;

    public void firstButton() => setButtonActive(0);
    public void secondButton() => setButtonActive(1);
    public void thirdButton() => setButtonActive(2);



    void setButtonActive(int index)
    {
        setPanel(index);
        setTextColor(index);
        setButtonImageColor(index);
    }

    void setPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if(index==i)
                panels[i].SetActive(true);
            else
                panels[i].SetActive(false);
        }
    }

    void setTextColor(int index)
    {
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            if (index == i)
                buttonTexts[i].color = selectedtextColour;
            else
                buttonTexts[i].color = deselectedTextColour;
        }

       
    }

    void setButtonImageColor(int index)
    {
        for (int i = 0; i < buttonImages.Length; i++)
        {
            if (index == i)
                buttonImages[i].color = selecetedButtonColour;
            else
                buttonImages[i].color = deselectedButtonColour;
        }
    }
}
