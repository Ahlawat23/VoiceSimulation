using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RosterManager : MonoBehaviour
{
    public Text gameLodPath;
   public patientDataManager _patientData;
   public patientGameDataLog _gameLog;
   public resultScreen  _resultUI;

    [Header("prefabs and trnsform")]
    public GameObject gameCardPrefab;
    public Transform placeToSpawnGameCard;


    public List<patientGameDataLog.gameData> _CurrentPatientGameData = new List<patientGameDataLog.gameData>();


    public void OnEnable()
    {
        showRoasterOfPatient(PermanentData.getCurrentPatientId(Application.dataPath + "/" + PermanentData.CURRENT_PATIENT_AND_COUNTER_FILE_NAME));
    }

   
    public void showRoasterOfPatient(string id)
    {
        clearRaosterList();
        _gameLog.isGame = false;
        _gameLog.fetchGameDataFromTextFile();
        gameLodPath.text = Application.dataPath + "/" + PermanentData.RECENT_GAME_LOG_FILE_NAME;
        getGameDataFromTheGameLog(id);
        renderAllgameLogData();
    }
    void clearRaosterList()
    {
        _CurrentPatientGameData.Clear();
        for (int i = 0; i < placeToSpawnGameCard.childCount; i++)
        {
            Destroy(placeToSpawnGameCard.GetChild(i).gameObject);
        }
    }
    public void getGameDataFromTheGameLog(string id)
    {
        Debug.Log("patient id : " + id);

        for (int i = 0; i < _gameLog.gameDataList.Count; i++)
        {
            Debug.Log("patient id  :" + _gameLog.gameDataList[i].patientId + " to be compared with : " + id) ;
            if (_gameLog.gameDataList[i].patientId == id)
            {
                _CurrentPatientGameData.Add(_gameLog.gameDataList[i]);
            }
        }
    }

    public void printPatientList()
    {
        for (int i = 0; i < _CurrentPatientGameData.Count; i++)
        {
            Debug.Log(_CurrentPatientGameData[i]);
        }
    }

    public void renderAllgameLogData()
    {
        Debug.Log("enter it");
        Debug.Log(_CurrentPatientGameData.Count);
        for (int i = _CurrentPatientGameData.Count - 1; i >= 0; --i)
        {
           
            instantiateGameCard(_CurrentPatientGameData[i]);
        }
    }

    private void instantiateGameCard(patientGameDataLog.gameData resultVar)
    {
        gameDataScriptleObject newGameData = Instantiate(gameCardPrefab, placeToSpawnGameCard).GetComponent<gameDataScriptleObject>();
        newGameData.gameName = resultVar.gameId;
        newGameData.moduleName = "HASI";
        newGameData.overralRating = resultVar.overralrating;
        newGameData._resultUI = _resultUI;
        newGameData.counter = resultVar.counter;
        newGameData.gameId = resultVar.gameId;
        newGameData.patientId = resultVar.patientId;
        newGameData.date = resultVar.date;
        newGameData.elapsedTime = resultVar.elapsedTime;
        newGameData.loudnessTarget = resultVar.loudnessTarget;
        newGameData.numberOfTrials = resultVar.numberOfTrials;
        newGameData.cummulativeDurationOfsounds = resultVar.cummulativeDurationOfsounds;
        newGameData.meanPitch = resultVar.meanPitch;
        newGameData.meanLoudness = resultVar.meanLoudness;
        newGameData.stdDevPitch = resultVar.stdDevPitch;
        newGameData.stdDevLoudness = resultVar.stdDevLoudness;
        newGameData.rangePitchMin = resultVar.rangePitchMin;
        newGameData.rangePitchMax = resultVar.rangePitchMax;
        newGameData.rangeLoudnessMin = resultVar.rangeLoudnessMin;
        newGameData.rangeLoudnessMax = resultVar.rangeLoudnessMax;
        newGameData.audioId = resultVar.audioId;

        newGameData.setAllTextElements();



    }

    

}
