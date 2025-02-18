using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientRegisterUIManager : MonoBehaviour
{
    [Header("Transforms")]
    public Transform box;
    public CanvasGroup background;
    public GameObject panel;


    [Header("other scripts")]
    public MenuApiManager _menuApiMan;


  
    private void OnEnable()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;
        clearAllInputFields();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                switchWithTab();
            }
        }
    }


     void clearAllInputFields()
    {
        _menuApiMan.NameField.text = "";
        _menuApiMan.EmailField.text = "";
        _menuApiMan.AgeField.text = "";
        _menuApiMan.LanguageField.text = "";
        _menuApiMan.MobileField.text = "";
        _menuApiMan.AddressField.text = "";
        _menuApiMan.disablityType.text = "";
    }

    void switchWithTab()
    {
        if (_menuApiMan.NameField.isFocused)
            _menuApiMan.EmailField.Select();
        if (_menuApiMan.EmailField.isFocused)
            _menuApiMan.AgeField.Select();
        if (_menuApiMan.AgeField.isFocused)
            _menuApiMan.LanguageField.Select();
        if (_menuApiMan.LanguageField.isFocused)
            _menuApiMan.MobileField.Select();
        if (_menuApiMan.MobileField.isFocused)
            _menuApiMan.AddressField.Select();
        if (_menuApiMan.AddressField.isFocused)
            _menuApiMan.disablityType.Select();
        if (_menuApiMan.disablityType.isFocused)
            _menuApiMan.NameField.Select();

    }

    public void close()
    {
        panel.SetActive(false);
    }
}
