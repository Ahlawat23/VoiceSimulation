using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameDataScriptleObject : MonoBehaviour
{
    public string gameName;
    public string moduleName;

    [Header("dataStrings")]
    public string counter;
    public string gameId;
    public string patientId;
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
    public string overralRating;

    [Header("text elements")]
    public Text GameName;
    public Text Duration;
    public Text Module;
    public Button showResults;

    [Header("other scripts")]
    public resultScreen _resultUI;


    public void setAllTextElements()
    {
        GameName.text = gameId;
        Duration.text = "Date : " + date + "\n" + "Duration: " + elapsedTime;
        Module.text = moduleName;
    }
    public void showResult()
    {
       

        _resultUI.shouludStore = false;
        _resultUI.gameObject.SetActive(true);


      
        _resultUI.gameObject.SetActive(true);

        _resultUI._ElapsedTime = elapsedTime;
        _resultUI._cummulativeDurationOfSounds = cummulativeDurationOfsounds;


        _resultUI._NumOfTrials = numberOfTrials;
        _resultUI._loundNessTarget = loudnessTarget;

        _resultUI._meanPitch = meanPitch;
        _resultUI._meanLoudness = meanLoudness;

        _resultUI._StdDevPitch = stdDevPitch;
        _resultUI._StdDevLoudness = stdDevLoudness;

        _resultUI._RangePitchLow = rangePitchMin;
        _resultUI._RangePitchHigh = rangePitchMax;
        _resultUI._RangeLoudnessLow = rangeLoudnessMin;
        _resultUI._RangeLoudnessHigh = rangeLoudnessMax;

        _resultUI._grade = overralRating;
        Debug.Log(overralRating);
        _resultUI._AudioId = audioId;
        _resultUI.showResultScreen();
        
      
    }
}
