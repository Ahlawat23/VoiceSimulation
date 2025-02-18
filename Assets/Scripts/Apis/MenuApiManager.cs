using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using SimpleJSON;
using TMPro;


public class MenuApiManager : MonoBehaviour
{

    [Header("Doctor login")]
    public InputField doctorIdField;
    public InputField passwordField;
    public Button signInButton;
   
    

    [Header("LodingBar")]
    public Text loadingText;
    public Image LoadingfillBarImg;

    [Header("NewPatientRegistration")]
    public InputField NameField;
    public InputField EmailField;
    public InputField AgeField;
    public InputField LanguageField;
    public InputField MobileField;
    public InputField AddressField;
    public InputField disablityType;


    [Header("PatientDesk")]
    public TMP_InputField patientIdField;


    [Header("Other scripts")]
    public patientDataManager _patientDataManager;
    public loginUIHandler _loginUIHandler;
    public progressBar _loadingManager;
    public patientGameDataLog _gameLog;
    public SearchPatientGameGraphLogger _graphLogerr;



    #region ButtonFuntiohnsToStartCoroutines
    public void LoginDoctor()
    {

        if (doctorIdField.text == String.Empty)
        {
            menuManager.instance._errorBox.showDialougeBox("Please Enter a Docotor id.");

        }
        else
        {
            if (passwordField.text == String.Empty)
            {
                menuManager.instance._errorBox.showDialougeBox("Password cannot be blank.");
            }
               
            else
            {
                StartCoroutine(doctorLogin_Coroutine());
                signInButton.interactable = false;
            }
                
        }


    }
    public void RegisterNewPatient()
    {

        if (validInfoForNewPatientReg())
            StartCoroutine(newPatientRegistration_coroutine());
        //dummyNewPatientReg();
    }
    public void GetAllThePatients()
    {
        StartCoroutine(getPatients_Couroutine());
    }
    public void OpenPatientDesk()
    {
        openPatientDeskWithID(patientIdField.text);
    }
    public void GetGameLinks(string cotegrie)
    {
        StartCoroutine(getallGameLinks_coroutine(cotegrie));
    }


    #endregion


    //DOCTOR LOGIN

     public string personRole;
    public string loginFullName;
    IEnumerator doctorLogin_Coroutine()
    {

        //setting connecting bar
        Debug.Log("Logging in docotor" + " uri :" + getUriByLoginType(_loginUIHandler.loginType) + " and the field : " + _loginUIHandler.getLoginType()) ;
        _loginUIHandler.startLoadingBar();
        _loadingManager.setLoadingTest("Trying to log you in...");

        string uri = getUriByLoginType(_loginUIHandler.loginType);
         

        WWWForm form = new WWWForm();
        form.AddField(_loginUIHandler.getLoginType(), doctorIdField.text);
        form.AddField("password", passwordField.text);

        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {

                Debug.Log(request.error);
                Debug.Log(request.downloadHandler.text);

                _loginUIHandler.deactivateLoadingBarForSignIN();
                if (request.error.Contains("Service Unavailable"))
                {
                    menuManager.instance._errorBox.showDialougeBox("Please try again later.", true);
                }
                if (request.error.Contains("Not Found"))
                {
                    menuManager.instance._errorBox.showDialougeBox("404 not found. Please try again later.", true);
                }
                else
                {
                    JSONNode node = JSON.Parse(request.downloadHandler.text);
                    menuManager.instance._errorBox.showDialougeBox("Cause: " + node["msg"].ToString(), true);


                }
                signInButton.interactable = true;



            }
            else
            {
                _loginUIHandler.deactivateLoadingBarForSignIN();
                //Debug.Log(request.downloadHandler.text);
                //{ "status":true,
                //"msg":"User login successfull",
                //"data":{
                //"userId":"63ac18de969a0e52f1a8cf4f",
                //"fullname":"Manikant Sharma",
                //"username":"Manikant@123",
                //"adminid":"6392dd4c17e79446a1f681e8",
                //"email":"gubu100@gmail.com",
                //"phone":"8889915428",
                //"role":"Doctor",
                //"count":"45",
                //"token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyaWQiOiI2M2FjMThkZTk2OWEwZTUyZjFhOGNmNGYiLCJmdWxsbmFtZSI6Ik1hbmlrYW50IFNoYXJtYSIsImFkbWluaWQiOiI2MzkyZGQ0YzE3ZTc5NDQ2YTFmNjgxZTgiLCJlbWFpbCI6Imd1YnUxMDBAZ21haWwuY29tIiwicGhvbmUiOiI4ODg5OTE1NDI4Iiwicm9sZSI6IkRvY3RvciIsImNvdW50IjoiNDUiLCJpYXQiOjE2NzY5OTEyMzJ9.MCysKLY7Y0dDBFgN11sc1nqPUAe2OQtgCUZwXfWJdfQ"} }
                JSONNode node = JSON.Parse(request.downloadHandler.text);
                

                if (node["msg"].ToString().Contains("User login successfull"))
                {
                    string docid = node["data"]["userId"];
                    //Debug.Log(docid);
                    bool isNewLogin = PermanentData.setDoctorId(docid);
                    PermanentData.setAdminId(node["data"]["adminid"]);

                    personRole = getPreNameFix(node["data"]["role"]);
                    loginFullName = node["data"]["fullname"];
                    
                        

                    _loadingManager.setLoadingTest("Fetching patients");
                     StartCoroutine(getPatients_Couroutine());
                }
                else
                {
                    menuManager.instance._errorBox.showDialougeBox(node["msg"].ToString());
                }


            }
        }
    }

    IEnumerator getPatients_Couroutine()
    {
        Debug.Log("Getting all the patients");
        string uri = "http://vsadmin-env.eba-7n23mnc3.ap-south-1.elasticbeanstalk.com/docPatient?doc_id=" + PermanentData.getDoctorId();
        //Debug.Log(uri);

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {

                Debug.Log(request.error);
                Debug.Log(request.downloadHandler.text);
                signInButton.interactable = true;
            }
            else
            {

                //Debug.Log(request.downloadHandler.text);
                //{ "status":true,
                //        "data":{ 
                //        "email":"sushrut@gmail.com",
                //            "phone":"9988776655",
                //            "role":"Doctor","patientData":
                //            [{ 
                //            "patientFullName":"tomharry",
                //                "email":"tomharry@gmail.com",
                //                "phone":"9565671751",
                //                "age":"23",
                //                "nativeLanaguage":"hindi",
                //                "address":"delhi",
                //                "disabalityType":"Physically",
                //                "progress":[]}, 
                //                { "patientFullName":"rashu",
                //                "email":"rashu@gmail.com",
                //                "phone":"7388449046",
                //                "age":"25",
                //                "nativeLanaguage":"hindi",
                //                "address":"delhi south",
                //                "disabalityType":"Physically",
                //                "progress":[]},}

               _patientDataManager.genrateTheTextData(request.downloadHandler.text.ToString());
                _patientDataManager.fetchPatientData();
                fetchAllGameData();
            }
        }
    }
    int gameDataPatientCount = 0;
    int gameDataCounter = 0;
    bool[] checkCompletation;
    void fetchAllGameData()
    {
        gameDataPatientCount = 0;
        gameDataCounter = 0;
        _loadingManager.setLoadingTest("Fetching patient game data " + gameDataPatientCount + "/" + _patientDataManager.patients.Count);
        checkCompletation = new bool[_patientDataManager.patients.Count];
        for (int i = 0; i < _patientDataManager.patients.Count; i++)
        {
            StartCoroutine(getGameDataOfPatient_coroutine(_patientDataManager.patients[i].id, i));
        }
    }

    IEnumerator getGameDataOfPatient_coroutine(string patientId, int index)
    {
        Debug.Log("getting game data for paitent: " + patientId);
        string uri = "http://vsadmin-env.eba-7n23mnc3.ap-south-1.elasticbeanstalk.com/gameData?patientId=" + patientId;
        //Debug.Log(uri);

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {

                Debug.Log(request.error);
                Debug.Log(request.downloadHandler.text);
                signInButton.interactable = true;

            }
            else
            {
                signInButton.interactable = true;
                Debug.Log(request.downloadHandler.text);
                {
                    //"status":true,
                    //"msg":"game details",
                    //"data":[{
                    //"_id":"63ac1b650beb9b7b41264230",
                    //"gameId":"",
                    //"patientId":"63ac19d20beb9b7b41264229",
                    //"overralrating":"2.5",
                    //"date":"28-12-2022 16:03:08",
                    //"elapsedTime":"00:03:24",
                    //"cumulativeDurationOfSounds":"00:00:49",
                    //"MeanPitch":"664.7736",
                    //"meanLoudness":"-1.295372",
                    //"stdDevPitch":"103.385895131308",
                    //"stdDevLoudness":"4.14949803488628",
                    //"rangepitchMin":"162.1705",
                    //"rangepitchMax":"887.2689",
                    //"rangeLoudnessMin":"-14.73676",
                    //"rangeLoudnessmax":"14.74582",
                    //"audioId":"newClip0"},

                    _loadingManager.setLoadingTest("Fetching patient game data : " + index + "/" + _patientDataManager.patients.Count);
                    JSONNode node = JSON.Parse(request.downloadHandler.text);
                    //Debug.Log("LOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOK !!!!!!!");
                    //Debug.Log(node);
                    //Debug.Log(node["data"]["gameId"]);

                    int i = 0;

                    while (node["data"][i] != null)
                    {
                        _gameLog.addGameDataToTheList((gameDataCounter++).ToString(),
                      node["data"][i]["gameId"],
                      node["data"][i]["patientId"],
                      node["data"][i]["overralrating"],
                      node["data"][i]["gamebase"],
                      node["data"][i]["date"],
                      node["data"][i]["elapsedTime"],
                      node["data"][i]["nloudnessTarget"],
                      node["data"][i]["nnumberOfTrials"],
                      node["data"][i]["cumulativeDurationOfSounds"],
                      node["data"][i]["MeanPitch"],
                      node["data"][i]["meanLoudness"],
                      node["data"][i]["stdDevPitch"],
                      node["data"][i]["stdDevLoudness"],
                      node["data"][i]["rangepitchMin"],
                      node["data"][i]["rangepitchMax"],
                      node["data"][i]["rangeLoudnessMin"],
                      node["data"][i]["rangeLoudnessmax"],
                      node["data"][i]["audioId"]
                      );

                        i++;
                    }
                    
                    _gameLog.gameLogPath = Application.dataPath + PermanentData.RECENT_GAME_LOG_FILE_NAME;
                    
                    _gameLog.reWriteAllGameData();

                    checkCompletation[index] = true;

                    if(isAllPatientsDataDownloaded())
                    {
                        signInButton.interactable = true;
                        menuManager.instance.updatestartMenuState(startMenuState.OpeningMenu);
                       
                        _graphLogerr.startGraph();
                    }

                }
            }
        }
    }

    bool isAllPatientsDataDownloaded()
    {
        for (int i = 0; i < checkCompletation.Length; i++)
        {
            if (!checkCompletation[i])
                return false;
        }

        return true;
    }

    string getUriByLoginType(int index)
    {
        if (index == 0)
        {
            return "http://vsadmin-env.eba-7n23mnc3.ap-south-1.elasticbeanstalk.com/login";
        }
        if (index == 1)
        {
            return "http://vsadmin-env.eba-7n23mnc3.ap-south-1.elasticbeanstalk.com/usernamelogin";
        }
        if(index == 2)
        {
            return "http://vsadmin-env.eba-7n23mnc3.ap-south-1.elasticbeanstalk.com/phonelogin";
        }

        return null;
    }
    

    string getPreNameFix(string role)
    {
        string preName = "";
        switch (role)
        {
            case "Admin":
                preName = "Dr.";
                break;
            case "Sub-Admin":
                preName = "Dr.";
                break;
            case "Doctor":
                preName = "Dr.";
                break;
            case "Patient":
                preName = "Dr.";
                break;
            case "Therapist":
                preName = "Dr.";
                break;
            default:
                preName = "";
                break;

        }

        return preName;
    }

    //NEW PATIENT REGISTRATION
    IEnumerator newPatientRegistration_coroutine()
    {
        Debug.Log("Registring The Patient");

        string uri = "http://vsadmin-env.eba-7n23mnc3.ap-south-1.elasticbeanstalk.com/createPatient";


        Debug.Log(PermanentData.getDoctorId());
        Debug.Log(EmailField.text);
        WWWForm form = new WWWForm();
        form.AddField("DocId", PermanentData.getDoctorId());
        form.AddField("adminid", PermanentData.getAdminId());
        form.AddField("patientFullName", NameField.text);
        form.AddField("email", EmailField.text);
        form.AddField("phone", MobileField.text);
        form.AddField("age", AgeField.text);
        form.AddField("nativeLanaguage", LanguageField.text);
        form.AddField("address", AddressField.text);
        form.AddField("disabalityType", disablityType.text);

        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {

                Debug.Log(request.error);
                Debug.Log(request.downloadHandler.text);
                if (request.error.Contains("Service Unavailable"))
                {
                    menuManager.instance._errorBox.showDialougeBox("Service Unavailable right now. Please try again later");
                }
                else
                {
                    JSONNode node = JSON.Parse(request.downloadHandler.text);
                    menuManager.instance._errorBox.showDialougeBox("Cause: " + node["messgage"].ToString(), true);
                }
            }
            else
            {

                Debug.Log(request.downloadHandler.text);
                JSONNode node = JSON.Parse(request.downloadHandler.text);

                //inserted data successfully
                //{ "status":true,
                //        "msg":"Patient Succesfully Created",
                //        "data":{ 
                //        "DocId":"6369e4c894d295189ab69acd",
                //            "patientFullName":"fdgfdg",
                //            "email":"dsfsd@gmail.com",
                //            "phone":"7894561230",
                //            "age":"32",
                //            "nativeLanaguage":"dfsdf",
                //            "address":"sdag",
                //            "disabalityType":"Physically",
                //            "progress":[],
                //            "_id":"636dd6c97b8bc0f0caada6f8",
                //            "createdAt":"2022-11-11T04:59:53.903Z",
                //            "updatedAt":"2022-11-11T04:59:53.903Z",
                //            "__v":0} }

                //if(request.downloadHandler.text == "inserted data successfully")
                //{
                //    _patientDataManager.UpdatePaitentData();
                //}


                //TODO
                //store the patient locally
                //make this patient current patient

                _patientDataManager.UpdatePaitentData();
                PermanentData.setCurrentPatientId(node["data"]["_id"], true);
                menuManager.instance.updatestartMenuState(startMenuState.PatientDesk);
            }
        }
    }

    int id = 0;

    bool validInfoForNewPatientReg()
    {
        if (NameField.text != String.Empty)
        {
            if (EmailField.text != String.Empty)
            {
                if (AgeField.text != String.Empty)
                {
                    if (LanguageField.text != String.Empty)
                    {
                        if (MobileField.text != String.Empty)
                        {
                            if (AddressField.text != String.Empty)
                            {
                                if (disablityType.text != String.Empty)
                                {
                                    return true;
                                }
                                else
                                {
                                    menuManager.instance._errorBox.showDialougeBox("Please specify the bloodgroup");
                                    return false;
                                }
                            }
                            else
                            {
                                menuManager.instance._errorBox.showDialougeBox("Please enter address");
                                return false;
                            }
                        }
                        else
                        {
                            menuManager.instance._errorBox.showDialougeBox("Please enter the mobile no.");
                            return false;
                        }
                    }
                    else
                    {
                        menuManager.instance._errorBox.showDialougeBox("Please enter a native language");
                        return false;
                    }
                }
                else
                {
                    menuManager.instance._errorBox.showDialougeBox("Please specify age");
                    return false;
                }
            }
            else
            {
                menuManager.instance._errorBox.showDialougeBox("Please enter a email");
                return false;
            }
        }
        else
        {
            menuManager.instance._errorBox.showDialougeBox("Please enter a name");
            return false;
        }
    }
    public void openPatientDeskWithID(string id)
    {
        if (_patientDataManager.patientExit(id))
        {
            PermanentData.setCurrentPatientId(id, true);
            menuManager.instance.updatestartMenuState(startMenuState.PatientDesk);
        }
        else
        {
            menuManager.instance._errorBox.showDialougeBox("Patient with id: " + id + " dosent exist in our database");
        }
    }


   
    //GET GAME LINKS
    IEnumerator getallGameLinks_coroutine(string cotegrie)
        {
            //cotegrie can be HASI, PCT, CFANV, LL
            Debug.Log("getting all the game links");
            string uri = "http://vsadmin-env.eba-7n23mnc3.ap-south-1.elasticbeanstalk.com/gamelist?gamecategories=" + cotegrie;


            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {

                    Debug.Log(request.error);
                    Debug.Log(request.downloadHandler.text);

                }
                else
                {

                    Debug.Log(request.downloadHandler.text);
                    JSONNode node = JSON.Parse(request.downloadHandler.text);
                }
            }
        }
    #region creating the pdf doc
        ////Create a new PDF document.

        //PdfDocument document = new PdfDocument();

        ////Add a page to the document.

        //PdfPage page = document.Pages.Add();

        ////Create PDF graphics for the page.

        //PdfGraphics graphics = page.Graphics;

        ////Set the standard font.

        //PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

        ////Draw the text.

        //graphics.DrawString("This pdf is genrated now!!", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 0));

        ////Creating the stream object

        //MemoryStream stream = new MemoryStream();

        ////Save the document into memory stream

        //document.Save(stream);

        ////If the position is not set to '0' then the PDF will be empty.

        //stream.Position = 0;

        ////Close the document.

        //File.WriteAllBytes("Sample.pdf", stream.ToArray());

        //System.Diagnostics.Process.Start("Sample.pdf", @"C:\Program Files (x86)\Adobe\Acrobat DC\Acrobat\AcroRd32.exe");
        #endregion

}



