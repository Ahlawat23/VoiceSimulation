
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PatientScriptableObject : MonoBehaviour
{
    public string id;
    public string patientFullName;
    public string email;
    public string phone;
    public string age;
    public string nativeLanaguage;
    public string address;
    public string disabalityType;


    [Header("Text elemtents")]
    public Text NameText;
    public Text AgeText;
    public Text IdText;
    public Text phoneText;

    [Header("Expanded Info")]
    public GameObject expandInfoParentObject;
    public Text pe_idText;
    public Text pe_nameText;
    public Text pe_emailtext;
    public Text pe_ageText;
    public Text pe_mobileText;
    public Text pe_addressText;
    public Text pe_languageText;
    public Text pe_DisablityType;

    [Header("Graphs")]
    public graphWindow frequencyGraph;
    public graphWindow LoudnessGraph;
    public graphWindow recognitionGraph;
    

    [Header("sections")]
    public GameObject buttons;
    private RectTransform _rec;
    private CanvasGroup _expandedInfoCanvasGroup;
    private bool isCardExpanded = false;

    private void Start()
    {
        _rec = GetComponent<RectTransform>();
        _expandedInfoCanvasGroup = expandInfoParentObject.GetComponent<CanvasGroup>();
    }

    public void setAllTextElemtents()
    {
        //setting intial texts
      NameText.text ="Name: " + patientFullName;
      AgeText.text = "Age: " + age;
      IdText.text = "ID: " + id;
      phoneText.text = "Mobile no. : " + phone;

        //setting expanded information

      pe_idText.text =  id;
      pe_nameText.text =  patientFullName;
      pe_emailtext.text = email;
      pe_ageText.text =  age;
      pe_mobileText.text = phone;
      pe_addressText.text =  address;
      pe_languageText.text =  nativeLanaguage;
      pe_DisablityType.text =  disabalityType;

    }


    public void PlayerCardOnClick()
    {
        if (isCardExpanded)
            UnExpandPatientInfo();
        else
            ExpandPatientInfo();
    }



   private void ExpandPatientInfo()
    {
        NameText.gameObject.SetActive(false);
        AgeText.gameObject.SetActive(false);
        IdText.gameObject.SetActive(false);
        phoneText.gameObject.SetActive(false);

        expandInfoParentObject.SetActive(true);

        _expandedInfoCanvasGroup.LeanAlpha(1f, 0.6f).setEaseInExpo();
        

       

        _rec.LeanSize(new Vector2(1200, 600), 0.4f).setEaseInExpo();


        buttons.SetActive(true);


        isCardExpanded = true;
    }

    private void UnExpandPatientInfo()
    {
        NameText.gameObject.SetActive(true );
        AgeText.gameObject.SetActive(true);
        IdText.gameObject.SetActive(true);
        phoneText.gameObject.SetActive(true);

        _expandedInfoCanvasGroup.LeanAlpha(0, 0.1f);
        _rec.LeanSize(new Vector2(1000, 150), 0.2f);

        expandInfoParentObject.SetActive(false );
        buttons.SetActive(false);
        isCardExpanded =false;

    }

    public void showgraphs(List<int> elpasedTime, List<int> loudness, List<int> recogniton)
    {
        frequencyGraph.CalculateGraph(elpasedTime);
        LoudnessGraph.CalculateGraph(loudness);
        recognitionGraph.CalculateGraph(recogniton);

    }

    public void openPatientDest()
    {
        menuManager.instance._menuApiManager.openPatientDeskWithID(id);
    }

    public void openRosterMenu()
    {
        menuManager.instance._menuApiManager.openPatientDeskWithID(id);
        menuManager.instance.updatestartMenuState(startMenuState.roster);
    }


}
