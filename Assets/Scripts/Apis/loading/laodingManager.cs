 using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections;
using SimpleJSON;
using UnityEngine.Networking;

public class laodingManager : MonoBehaviour
{
    private static laodingManager instance;

    public static laodingManager GetInstance
    {
        get
        {
            if(instance != null)
            return instance;
            else 
                return null;
        }
    }

    [Header("info")]
    public bool isNewLogin;

    [Header("LoadingBar")]
    public Image lbFill;
    public Text lbText;

    [Header("other scripts")]
    public patientDataManager _patientDataManager;
    public SearchPatientGameGraphLogger _graphLogger;
    public patientGameDataLog _gameLog;


    private void Awake()
    {
        instance = this;
    }

    public void LoadingInit()
    {
        //if (isNewLogin)
        //{
        //    DownloadAllPatientData();
        //}
        //else
        //{

        //}

        DownloadAllPatientData();
    }
  

    public void DownloadAllPatientData()
    {
        _patientDataManager.UpdatePaitentData();
    }
    int i = 0;
    public void goThroughListAndCollectEveryData()
    {
        i = 0;
        _gameLog.gameDataPath = Application.dataPath + PermanentData.RECENT_GAME_LOG_FILE_NAME;
        for (int i = 0; i < _patientDataManager.patients.Count; i++)
        {
            StartCoroutine(getGameDataOfPatient_coroutine(_patientDataManager.patients[i].id));
        }

        
    }


    IEnumerator getGameDataOfPatient_coroutine(string patientId)
    {
        Debug.Log("getting game data for paitent: " + patientId);
        string uri = "https://vsdoc65.onrender.com/gameData?patientId=" + patientId;
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

                    JSONNode node = JSON.Parse(request.downloadHandler.text);
                  
                }
            }
        }
    }
}
