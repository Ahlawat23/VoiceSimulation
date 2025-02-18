using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using System.Linq;
public class patientGameDataLog : MonoBehaviour
{
    public class gameData
    {
        public string counter;
        public string gamebase;
        public string gameId;
        public string patientId;
        public string overralrating;
        public string date;
        public string elapsedTime;
        public string loudnessTarget;
        public string numberOfTrials;
        public string cummulativeDurationOfsounds;
        public string meanPitch;
        public string meanLoudness;
        public string stdDevPitch;
        public string stdDevLoudness;
        public string rangePitchMin;
        public string rangePitchMax;
        public string rangeLoudnessMin;
        public string rangeLoudnessMax;
        public string audioId;
       
    }
    public bool isGame = true;
    public Text dataPath;
    public Text debugText;  
    private string ReadText;

    public List<gameData> gameDataList =  new List<gameData>();
    public string gameDataPath;
    public string gameLogPath;
    public string currentPatient_counter;

    public void setGameDataPath()
    {
        gameDataPath = Application.dataPath;
        string newGameData = null;
        int pos = gameDataPath.IndexOf(PermanentData.ALL_SIMULATIONS_PATH);
        if (pos >= 0)
        {
            // String after founder  
            newGameData = gameDataPath.Remove(pos);


        }
        gameDataPath = newGameData;
        gameLogPath = gameDataPath + PermanentData.GAME_LOG_PATH_FOR_SIMULATIONS;
        
    }

    //fetching data
    public void fetchGameDataFromTextFile()
    {

        gameDataList.Clear();


        if (!isGame)
            gameLogPath = Application.dataPath + PermanentData.RECENT_GAME_LOG_FILE_NAME;

        if (!File.Exists(gameLogPath))
        {  
            return;
        }
       
            

        ReadText = File.ReadAllText(gameLogPath);

        JSONNode node = JSON.Parse(ReadText);

       
        int i = 0;
        while (node[i] != null)
        {
            //Debug.Log(node[i]);
            addGameDataToTheList(
                node[i]["counter"], 
                node[i]["gameId"],
                node[i]["patientId"],
                node[i]["overralrating"],
                node[i]["gamebase"],
                node[i]["date"], 
                node[i]["elapsedTime"],
                node[i]["loudnessTarget"],
                node[i]["numberOfTrials"],
                node[i]["cummulativeDurationOfsounds"],
                node[i]["meanPitch"],
                node[i]["meanLoudness"],
                node[i]["stdDevPitch"],
                node[i]["stdDevLoudness"],
                node[i]["rangePitchMin"],
                node[i]["rangePitchMax"],
                node[i]["rangeLoudnessMin"],
                node[i]["rangeLoudnessMax"],
                node[i]["audioId"]
                );

            Debug.Log("in patient game data log " + node[i]["overralrating"].ToString());
            i++;
        }
        if (node[i - 1] !=  null)
        PermanentData.setGameDataCounter(i - 1);

        if (i == 0)
            PermanentData.setGameDataCounter(0);



    }
    public void addGameDataToTheList( string ncounter, string ngameId, string npatientId, string overrallrating, string gamebase, string ndate, string nelapsedTime, string nloudnessTarget, string nnumberOfTrials, string ncummulativeDurationOfsounds, string nmeanPitch, string nmeanLoudness, string nstdDevPitch, string nstdDevLoudness,string nrangePitchMin,string nrangePitchMax,string nrangeLoudnessMin, string nrangeLoudnessMax, string audioId )
    {
        gameData newGameData = new gameData();
        newGameData.counter = ncounter;
        newGameData.gameId = ngameId;
        newGameData.patientId = npatientId;
        newGameData.overralrating = overrallrating;
        newGameData.gamebase = gamebase;
        newGameData.date = ndate;
        newGameData.elapsedTime = nelapsedTime;
        newGameData.loudnessTarget = nloudnessTarget;
        newGameData.numberOfTrials = nnumberOfTrials;
        newGameData.cummulativeDurationOfsounds = ncummulativeDurationOfsounds;
        newGameData.meanPitch = nmeanPitch;
        newGameData.meanLoudness = nmeanLoudness;
        newGameData.stdDevPitch = nstdDevPitch;
        newGameData.stdDevLoudness = nstdDevLoudness;
        newGameData.rangePitchMin = nrangePitchMin;
        newGameData.rangePitchMax = nrangePitchMax;
        newGameData.rangeLoudnessMin = nrangeLoudnessMin;
        newGameData.rangeLoudnessMax = nrangeLoudnessMax;
        newGameData.audioId = audioId;
        

        gameDataList.Add(newGameData);
     }

    
   

   


    #region externalFuntion
    public void writeNewData(string overallRating, string ncounter, string ngameId, string npatientId, string ndate, string ngameBase, string nelapsedTime, string nloudnessTarget, string nnumberOfTrials, string ncummulativeDurationOfsounds, string nmeanPitch, string nmeanLoudness, string nstdDevPitch, string nstdDevLoudness, string nrangePitchMin, string nrangePitchMax, string nrangeLoudnessMin, string nrangeLoudnessMax, string audioId)
    {
        isGame = true;
        fetchGameDataFromTextFile();
        gameData newGameData = new gameData();
        newGameData.overralrating = overallRating;
        newGameData.counter = ncounter;
        newGameData.gameId = ngameId;
        newGameData.patientId = npatientId;
        newGameData.date = ndate;
        newGameData.gamebase = ngameBase;
        newGameData.elapsedTime = nelapsedTime;
        newGameData.loudnessTarget = nloudnessTarget;
        newGameData.numberOfTrials = nnumberOfTrials;
        newGameData.cummulativeDurationOfsounds = ncummulativeDurationOfsounds;
        newGameData.meanPitch = nmeanPitch;
        newGameData.meanLoudness = nmeanLoudness;
        newGameData.stdDevPitch = nstdDevPitch;
        newGameData.stdDevLoudness = nstdDevLoudness;
        newGameData.rangePitchMin = nrangePitchMin;
        newGameData.rangePitchMax = nrangePitchMax;
        newGameData.rangeLoudnessMin = nrangeLoudnessMin;
        newGameData.rangeLoudnessMax = nrangeLoudnessMax;
        newGameData.audioId = audioId;

        gameDataList.Add(newGameData);

        reWriteAllGameData();
        StartCoroutine(storeDataOnline_coroutine(
            newGameData.overralrating,
            newGameData.patientId,
            newGameData.gameId,
            newGameData.date,
            newGameData.gamebase,
            newGameData.elapsedTime,
            newGameData.loudnessTarget,
            newGameData.numberOfTrials,
            newGameData.cummulativeDurationOfsounds,
            newGameData.meanPitch,
            newGameData.meanLoudness,
            newGameData.stdDevPitch,
            newGameData.stdDevLoudness,
            newGameData.rangePitchMin,
            newGameData.rangePitchMax,
            newGameData.rangeLoudnessMin,
            newGameData.rangeLoudnessMax,
            newGameData.audioId
            ));
    }
    
    

    IEnumerator storeDataOnline_coroutine( string nOverralrating, string npatientId, string gameId, string ndate, string gameBase, string nelapsedTime, string nloudnessTarget, string nnumberOfTrials, string ncummulativeDurationOfsounds,
        string nmeanPitch, string nmeanLoudness, string nstdDevPitch, string nstdDevLoudness, string nrangePitchMin, string nrangePitchMax, string nrangeLoudnessMin, string nrangeLoudnessMax, string audioId)
    {
       
        debugText.text = "Uploading the data... please wait";

        string uri = "http://vsadmin-env.eba-7n23mnc3.ap-south-1.elasticbeanstalk.com/storeData";


        Debug.Log(PermanentData.getDoctorId());
        WWWForm form = new WWWForm();
        form.AddField("gameId", gameId);
        form.AddField("overralrating", nOverralrating);
        form.AddField("patientId", npatientId);
        form.AddField("date", ndate);
        form.AddField("gamebase", gameBase);

        form.AddField("LoudnessTarget", nloudnessTarget);
        form.AddField("elapsedTime", nelapsedTime);
        form.AddField("NumberOfTrials", nnumberOfTrials);
        form.AddField("cumulativeDurationOfSounds", ncummulativeDurationOfsounds);

        form.AddField("MeanPitch", nmeanPitch);
        form.AddField("meanLoudness", nmeanLoudness);
        form.AddField("stdDevPitch", nstdDevPitch);
        form.AddField("stdDevLoudness", nstdDevLoudness);

        form.AddField("rangepitchMin", nrangePitchMin);
        form.AddField("rangepitchMax", nrangePitchMax);
        form.AddField("rangeLoudnessMin", nrangeLoudnessMin);
        form.AddField("rangeLoudnessmax", nrangeLoudnessMax);
        form.AddField("audioId", audioId);
        


        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {

                Debug.Log(request.error);
                Debug.Log(request.downloadHandler.text);
                debugText.text = request.error;
            }
            else
            {

                Debug.Log(request.downloadHandler.text);
                JSONNode node = JSON.Parse(request.downloadHandler.text);
                debugText.text = "Uploaded now you can exit.";
                //{ "status":true,
                //"data":{
                //"_id":"63a82eeaa5589cbd82615483",
                //"DocId":"63a811c7ef31f5b49ceb83f8",
                //"patientFullName":"mona",
                //"email":"dskflsdj@gmail.com",
                //"phone":"7896956412",
                //"age":"12",
                //"nativeLanaguage":"jkdsfhjdks",
                //"address":"gfdgdfg",
                //"disabalityType":"fdgdfgfdg",
                //"progress":[{
                //"gamebase":"frequency",
                //"gameId":"",
                //"patientId":"63a82eeaa5589cbd82615483",
                //"overralrating":"7.5","date":"04-01-2023 11:38:48",
                //"elapsedTime":"00:00:06",
                //"cumulativeDurationOfSounds":"00:00:01",
                //"MeanPitch":"500.7177",
                //"meanLoudness":"-15.97634",
                //"stdDevPitch":"157.685053736475",
                //"stdDevLoudness":"5.51917392575682",
                //"rangepitchMin":"233.7524",
                //"rangepitchMax":"943.8699",
                //"rangeLoudnessMin":"-33.22623",
                //"rangeLoudnessmax":"-5.344912",
                //"audioId":"newClip432",
                //"_id":"63b517f004b5ef8b40c70675",
                //"createdAt":"2023-01-04T06:08:48.968Z",
                //"updatedAt":"2023-01-04T06:08:48.968Z",
                //"__v":0}],
                //"createdAt":"2022-12-25T11:07:22.326Z",
                //"updatedAt":"2022-12-25T11:07:22.326Z","__v":0} }


            }
        }
    }

  
    public void reWriteAllGameData()
    {
        genrateTheTextData(makeJasonStringOfGameData());
    }
    public void genrateTheTextData(string downloadedText)
    {
        File.WriteAllText(gameLogPath, downloadedText);
    }
    public string makeJasonStringOfGameData()
    {
        string finalJson = "[";
        for (int i = 0; i < gameDataList.Count; i++)
        {
            string newJson = JsonUtility.ToJson(gameDataList[i]);
            finalJson = finalJson + ", " + newJson;
        }
        finalJson = finalJson + "]";

        return finalJson;
    }
    #endregion


    public int fetchLatestCounter()
    {
        int counter = 0;
        if (PlayerPrefs.HasKey(PermanentData.GAME_DATA_COUNTER))
        {
            counter = PermanentData.getGameDataCounter();
            counter++;

            PermanentData.setGameDataCounter(counter);
        }
        else
        {
            PermanentData.setGameDataCounter(0);

            
        }
        //Debug.Log(counter);
        return counter;

        
    }

    //string patientAndCounter;
    //public void fetchCurrentPatienAndcounter()
    //{
    //    if (!File.Exists(gameDataPath + PermanentData.CURRENT_PATIENT_AND_COUNTER_PATH_FOR_SIM))
    //        return;

    //    patientAndCounter = File.ReadAllText(gameDataPath + PermanentData.CURRENT_PATIENT_AND_COUNTER_PATH_FOR_SIM);

    //    JSONNode node = JSON.Parse(patientAndCounter);

    //    //taken form patientDataManager.currentPatientAndCounter
    //    PermanentData.setCurrentPatientId(node["currentPatient"]);
    //    PermanentData.setGameDataCounter(int.Parse(node["currentCounter"]));
    //}

   
}
