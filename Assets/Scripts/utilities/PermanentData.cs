using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SimpleJSON;

public static class PermanentData
{
    public class currentPatientAndCounter
    {
        public string currentPatient;
        public string currentCounter;
    }


    private const string DOCTOR_ID = "doctor_id";
    private const string ADMIN_ID = "admin_id";
    public const string GAME_DATA_COUNTER = "gameDataCounter";
    public const string CURRENT_PATIENT_ID = "currentPatientID";



    //static List<PatientScriptableObject> AllThePatients = new List<PatientScriptableObject>();


    //paths
    public const string ALL_SIMULATIONS_PATH = "/allSimulations";
    public const string SIMULATION_EXE_NAME = "Stack.exe";
    public const string ALL_MENUFILEPATH_FORSIM = "/Stack_Data/";
    public const string GAME_LOG_PATH_FOR_SIMULATIONS = ALL_MENUFILEPATH_FORSIM + RECENT_GAME_LOG_FILE_NAME;
    public const string CURRENT_PATIENT_AND_COUNTER_PATH_FOR_SIM = ALL_MENUFILEPATH_FORSIM + CURRENT_PATIENT_AND_COUNTER_FILE_NAME;

    public const string RECENT_GAME_LOG_FILE_NAME = "/recentGameDataList.text";
    public const string ALL_PATIENT_LOG_FILE_NAME = "/AllPatients.text";
    public const string CURRENT_PATIENT_AND_COUNTER_FILE_NAME = "currentPatientAndCounter.text";


    //gameID
    public const string GAME_ID_PREF_KEY = "gameIDPrefKey";
    public const string GAME_ID_FILE_NAME = "/thisGameId.text";
    //for currentPatient and counter storage


    public static bool setDoctorId(string value)
    {


        if (!PlayerPrefs.HasKey(DOCTOR_ID))
        {
            PlayerPrefs.SetString(DOCTOR_ID, value);
            return true;
        }
        else
        {
            if (PlayerPrefs.GetString(DOCTOR_ID) == value)
                return false;
            else
                return true;
        }



    }

    public static string getDoctorId()
    {
        return PlayerPrefs.GetString(DOCTOR_ID);
    }

    public static void setAdminId(string value)
    {
        PlayerPrefs.SetString(ADMIN_ID, value);
    }

    public static string getAdminId()
    {
        return PlayerPrefs.GetString(ADMIN_ID);
    }


    public static void setGameDataCounter(int value)
    {
       
        PlayerPrefs.SetInt(GAME_DATA_COUNTER, value);
    }

    public static int getGameDataCounter()
    {
        if(PlayerPrefs.HasKey(GAME_DATA_COUNTER))
        {
            return PlayerPrefs.GetInt(GAME_DATA_COUNTER);
        }
        else
        {
            setGameDataCounter(0);
            return PlayerPrefs.GetInt(GAME_DATA_COUNTER);
        }
        
    }


    #region current patient id and counter
    public static void setCurrentPatientId(string value, bool write = false)
    {
        PlayerPrefs.SetString(CURRENT_PATIENT_ID, value);

        //genrate the currentuser text file and fetch the counter and all
        if(write)
        writeCurrentPatientAndCounter(value);
    }

    public static void writeCurrentPatientAndCounter(string currentPatient)
    {

        currentPatientAndCounter currentData = new currentPatientAndCounter();
        currentData.currentPatient = currentPatient;
        currentData.currentCounter = "0";

        string newJson = JsonUtility.ToJson(currentData);

        File.WriteAllText(Application.dataPath + "/" + CURRENT_PATIENT_AND_COUNTER_FILE_NAME, newJson);
    }

    public static string getCurrentPatientId(string path)
    {
        fetchCurrentPatientandCounter(path);
        return PlayerPrefs.GetString(CURRENT_PATIENT_ID);
    }

    public static void fetchCurrentPatientandCounter(string path)
    {
        if (!File.Exists(path))
            return;

       string patientAndCounter = File.ReadAllText(path);

        JSONNode node = JSON.Parse(patientAndCounter);

        //taken form class made above
        setCurrentPatientId(node["currentPatient"]);
        setGameDataCounter(int.Parse(node["currentCounter"]));
    }

    #endregion

    #region gameId

    public static void setGameId(string path, string value, bool write = false)
    {

        if (write)
        {
            File.WriteAllText(path, value);
        }
        else
        {
            PlayerPrefs.SetString(GAME_ID_PREF_KEY, value);
        }
            
    }

    public static string getCurrntGameID(string path)
    {
        fetchCurrentPatientandCounter(path);
        return PlayerPrefs.GetString(GAME_ID_PREF_KEY);
    }

    public static void fetchCurrentgameIDr(string path)
    {
        if (!File.Exists(path))
            return;

        string gameId = File.ReadAllText(path);
        PlayerPrefs.SetString(GAME_ID_PREF_KEY, gameId);   
    }

    #endregion

    //public static void addPatients(PatientScriptableObject newPatient)
    //{
    //    AllThePatients.Add(newPatient);
    //    PlayerPrefs.SetString(newPatient.id, newPatient.name);
    //}

    //public static PatientScriptableObject getPatientAtElement(int index)
    //{
    //    return AllThePatients[index];
    //}

    //public static void PopulateList()
    //{
    //    string[] assetNames = AssetDatabase.FindAssets("the",  new[] { "Assets/Scripts/utilities" });
    //    //foreach(string soname in assetNames)
    //    //{
    //    //    Debug.Log(soname);
    //    //}
    //    AllThePatients.Clear();
    //    foreach (string SOName in assetNames)
    //    {
    //        var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
    //        var character = AssetDatabase.LoadAssetAtPath<PatientScriptableObject>(SOpath);
    //        AllThePatients.Add(character);
    //    }
    //    Debug.Log(AllThePatients.Count);
    //}
}
