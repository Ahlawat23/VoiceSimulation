using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using System.Linq;
using System;
public class patientDataManager : MonoBehaviour
{
    [System.Serializable]
    public class patient
    {
        public string id;
        public string patientFullName;
        public string email;
        public string phone;
        public string age;
        public string nativeLanaguage;
        public string address;
        public string disabalityType;

    }
    public class currentPatientAndCounter
    {
        public string currentPatient;
        public string currentCounter;
    }

    private string ReadText;

    public InputField SearchField;
    public GameObject patientCardPrefab;
    public Transform searchPlaceToSpawCard;

    //name and id;
    public Dictionary<string, string> _patientNamesDictionary = new Dictionary<string, string>();
    public List<patient> patients = new List<patient>();

    //data paths
    public string allPatientDataPath;

    [Header("otherScripts")]
    public SearchPatientGameGraphLogger _graphLogger;
    public laodingManager _loadingManager;

    private void Awake()
    {
        allPatientDataPath = Application.dataPath + PermanentData.ALL_PATIENT_LOG_FILE_NAME;
    }


    public void UpdatePaitentData() => StartCoroutine(getPatients_Couroutine());

    IEnumerator getPatients_Couroutine()
    {
        Debug.Log("Getting all the patients");
        string uri = "http://vsadmin-env.eba-7n23mnc3.ap-south-1.elasticbeanstalk.com/docPatient?doc_id=" + PermanentData.getDoctorId();
        Debug.Log(uri);

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

                genrateTheTextData(request.downloadHandler.text.ToString());
                
            }
        }
    }

    public void genrateTheTextData(string downloadedText)
    {
        File.WriteAllText(allPatientDataPath, downloadedText);   
    }


    //FETCHING THE PATIENT DATA TO FILL THE LIST FOR FURTHER EASY REFRENCES
    JSONNode _patientDataNode;
    public void fetchPatientData()
    {
        Debug.Log("fetching patients name and id...");
        if (!File.Exists(allPatientDataPath))
        {
            UpdatePaitentData();
            return;
        }
            


        _patientNamesDictionary.Clear();
        patients.Clear();

        ReadText = File.ReadAllText(allPatientDataPath);

        _patientDataNode = JSON.Parse(ReadText);


        
     
        
        populatePatientsList();


    }

 

    public void populatePatientsList()
    {
        int i = 0;
        while (_patientDataNode["data"]["patientData"][i] != null)
        {


            //change Phone to id
            
            addPatientToTheList(_patientDataNode["data"]["patientData"][i]["_id"],
                _patientDataNode["data"]["patientData"][i]["patientFullName"],
                _patientDataNode["data"]["patientData"][i]["email"],
                _patientDataNode["data"]["patientData"][i]["phone"],
                _patientDataNode["data"]["patientData"][i]["age"],
                _patientDataNode["data"]["patientData"][i]["nativeLanaguage"],
                _patientDataNode["data"]["patientData"][i]["address"],
                _patientDataNode["data"]["patientData"][i]["disabalityType"]);

            i++;
        }

        
      

    }

    public void addPatientToTheList(string id, string fullName, string email,  string phone, string age, string nativeLanguage, string address, string disablityType)
    {
        patient newPatient = new patient();
        newPatient.id = id;
        newPatient.patientFullName = fullName;
        newPatient.email = email;
        newPatient.phone = phone;
        newPatient.age = age;
        newPatient.nativeLanaguage = nativeLanguage;
        newPatient.address = address;
        newPatient.disabalityType = disablityType;

        //Debug.Log(newPatient.patientFullName);
        patients.Add(newPatient);

        populatePatientDictionary(newPatient.id, newPatient.patientFullName);
    }

    public void populatePatientDictionary(string id, string fullName)
    {
       
            _patientNamesDictionary.Add(id, fullName);
 
    }


    //public string makeJasonStringOfPatients()
    //{
    //    string finalJson = "[";
    //    for (int i = 0; i < patients.Count; i++)
    //    {
    //        string newJson = JsonUtility.ToJson(patients[i]);
    //        finalJson = finalJson + ", " + newJson;
    //    }
    //    finalJson = finalJson + "]";

    //    return finalJson;
    //}


    //SEARCH


    #region SEARCH
    public void search()
    {
        clearSearchView();
        searchByPatientName(SearchField.text);
    }

    public void clearSearchView()
    {
        for(int i = 0; i< searchPlaceToSpawCard.childCount; i++)
        {
            Destroy(searchPlaceToSpawCard.GetChild(i).gameObject);
        }
    }

    private void searchByPatientName(string keyword)
    {

        bool oneSearchFound = false;
        for(int i = 0; i < _patientNamesDictionary.Count; i++)
        {
            
            if (DoesContains(_patientNamesDictionary.ElementAt(i).Value, keyword, System.StringComparison.OrdinalIgnoreCase))
            {
                instantiatePlayerCardInTheSearch(_patientNamesDictionary.ElementAt(i).Key, _patientNamesDictionary.ElementAt(i).Value);
                oneSearchFound = true;
            }
        }

        if (!oneSearchFound)
        {
            menuManager.instance._errorBox.showDialougeBox("Sorry cannot find any match with keyword : " + keyword);
        }
    }

     bool DoesContains(string source, string toCheck, System.StringComparison comp)
    {
        return source?.IndexOf(toCheck, comp) >= 0;
    }

    private void instantiatePlayerCardInTheSearch(string id, string name)
    {
        PatientScriptableObject newPaitentCard = Instantiate(patientCardPrefab, searchPlaceToSpawCard).GetComponent<PatientScriptableObject>();
        newPaitentCard.id = id;
        newPaitentCard.patientFullName = name;

        //setting rest of the details
        patient foundPatient = searchPatientData(id);

        newPaitentCard.email = foundPatient.email;
        newPaitentCard.phone = foundPatient.phone;
        newPaitentCard.age = foundPatient.age;
        newPaitentCard.nativeLanaguage = foundPatient.nativeLanaguage;
        newPaitentCard.address = foundPatient.address;
        newPaitentCard.disabalityType = foundPatient.disabalityType;

        //setting graphs
        _graphLogger.findDataWithID(id);

        newPaitentCard.showgraphs(_graphLogger.fetchFrequencyGradeWithId(), _graphLogger.fetchLoudnessGradeWithId(), _graphLogger.fetchRecogtionGradeWithId());

        newPaitentCard.setAllTextElemtents();
    }

    public patient searchPatientData(string id)
    {
        patient foundPatient = new patient();

        for (int i = 0; i < patients.Count; i++)
        {
            if (patients[i].id == id)
                foundPatient = patients[i];
        }

        return foundPatient;
    }


    #endregion


    public bool patientExit(string id)
    {
        Debug.Log(patients.Count + " patient counts");
        for (int i = 0; i < patients.Count; i++)
        {
            if (patients[i].id == id)
                return true;

            Debug.Log(patients[i].id);
        }

        return false;
    }


   

    //public void writeCurrentPatientAndCounter(string currentPatient)
    //{
    //    currentPatientAndCounter currentData = new currentPatientAndCounter();
    //    currentData.currentPatient = currentPatient;
    //    currentData.currentCounter = getGameDataCounter();

    //    string newJson = JsonUtility.ToJson(currentData);

    //    File.WriteAllText(Application.dataPath + "/" + PermanentData.CURRENT_PATIENT_AND_COUNTER_FILE_NAME, newJson);
    //}

    //string getGameDataCounter()
    //{
    //    string counter = null;

    //    if (!PlayerPrefs.HasKey(PermanentData.GAME_DATA_COUNTER))
    //        PermanentData.setGameDataCounter(0);
              
    //    counter = PermanentData.getGameDataCounter().ToString();

    //    return counter;
    //}
}
