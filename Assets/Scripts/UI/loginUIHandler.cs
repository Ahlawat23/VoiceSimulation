using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class loginUIHandler : MonoBehaviour
{
    [Header("Panels")]
    public GameObject SelectLoginTypePanel;
    public GameObject IdPasswordPanel;


    [Header("UI")]
    public Text idFieldPlaceholder;
    public InputField passwordField;
    public GameObject showPasswordSlash;
    public GameObject circleLoadingBar;
    public Text signButtonText;
    public int loginType = 0;
    public Text loadingTExt;

    [Header("change method buttons")]
    public GameObject emailButton;
    public GameObject usernameButton;
    public GameObject phoneButton;

    public void LoginWithEmail()
    {
        loginType = 0;
        idFieldPlaceholder.text = "     Email";

        emailButton.SetActive(false);
        usernameButton.SetActive(true);
        phoneButton.SetActive(true);
    }

    public void LoginWithUserName()
    {
        loginType = 1;
        idFieldPlaceholder.text = "     Username";

        emailButton.SetActive(true);
        emailButton.transform.position = usernameButton.transform.position;
        usernameButton.SetActive(false);
        phoneButton.SetActive(true);
    }

    public void LoginWithMobile()
    {
        loginType = 2;
        idFieldPlaceholder.text = "     Mobile";

        emailButton.SetActive(true);
        emailButton.transform.position = phoneButton.transform.position;
        usernameButton.SetActive(true);
        phoneButton.SetActive(false);
    }

    void openIdPasswardPanel()
    {
        SelectLoginTypePanel.SetActive(false);
        IdPasswordPanel.SetActive(true);
    }

    public void goBack()
    {
        SelectLoginTypePanel.SetActive(true);
        IdPasswordPanel.SetActive(false);
    }

    public string getLoginType()
    {
        switch (loginType)
        {
            case 0:
                return "email";
            case 1:
                return "username";
            case 2:
                return "phone";
            default:
                return "email";

        }


    }

    public void showPassward()
    {
        if (passwordField.contentType == InputField.ContentType.Password)
        {
            passwordField.contentType = InputField.ContentType.Standard;
            showPasswordSlash.SetActive(true);

        }
        else
        {
            passwordField.contentType = InputField.ContentType.Password;
            showPasswordSlash.SetActive(false);
        }


        passwordField.Select();
    }

    public void startLoadingBar()
    {
        circleLoadingBar.SetActive(true);
        //signinbutton changes
        signButtonText.text = "connecting...";
        signButtonText.fontSize = 20;


        emailButton.SetActive(false);
        usernameButton.SetActive(false);
        phoneButton.SetActive(false);
    }

    public void deactivateLoadingBarForSignIN()
    {
        circleLoadingBar.SetActive(false);

        signButtonText.text = "SignIn";
        loadingTExt.text = "";

        if (loginType == 0)
            LoginWithEmail();
        if (loginType == 1)
            LoginWithUserName();
        if (loginType == 2)
            LoginWithMobile();



    }
}
