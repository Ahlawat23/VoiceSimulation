using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class resultScreen : MonoBehaviour
{
    [Header("Text elements")]
    public Text clientID;
    public Text Date;
    public Text ElapsedTime;

    public Text loundNessTarget;
    public Text NumOfTrials;
    public Text cummulativeDurationOfSounds;

    public Text Mean1;
    public Text Mean2;
    public Text Mean3;

    public Text StdDev1;
    public Text StdDev2;
    public Text StdDev3;

    public Text Range1;
    public Text Range2;
    public Text Range3;

    public Text recordingFeedBackTest;
    public Text GradeText;
    public Button ResetButton;
    public Text resetButtonText;
    public Text numOflevels;

    [Header("other scripts")]
    public patientGameDataLog _gameLog;
    public Audio_sampler_Final _audioSampler;
    public bool shouludStore = true;

    [Header("Strings")]
    public string currentIdPath;
    public string gameId;
    public string _ElapsedTime;
    public string _grade;
    public string _gameBase;

    public string _loundNessTarget;
    public string _NumOfTrials;
    public string _cummulativeDurationOfSounds;

    public string _meanPitch;
    public string _meanLoudness;
    public string _meanDuration;

    public string _StdDevPitch;
    public string _StdDevLoudness;
    public string _StdDevDuration;

    public string _RangePitchLow;
    public string _RangeLoudnessLow;
    public string _RangeDurationLow;

    public string _RangePitchHigh;
    public string _RangeLoudnessHigh;
    public string _RangeDurationHigh;

    public string _AudioId;

    [Header("additionalBools")]
    public bool isFlappy = false;

    private void Start()
    {
        currentIdPath = _gameLog.gameDataPath + PermanentData.CURRENT_PATIENT_AND_COUNTER_PATH_FOR_SIM;
    }
    public void showResultScreen()
    {
        clientID.text = PermanentData.getCurrentPatientId(currentIdPath);
        DateTime dateTime = DateTime.Now;
        Date.text = dateTime.ToString();

       ElapsedTime.text = _ElapsedTime;

      loundNessTarget.text = _loundNessTarget;
      NumOfTrials.text = _NumOfTrials;
      cummulativeDurationOfSounds.text = _cummulativeDurationOfSounds;

      Mean1.text = _meanPitch;
      Mean2.text = _meanLoudness;
      Mean3.text = _meanDuration;

      StdDev1.text = _StdDevPitch;
      StdDev2.text = _StdDevLoudness;
      StdDev3.text = _StdDevDuration;

      Range1.text = _RangePitchLow + "..." + _RangePitchHigh;
      Range2.text = _RangeLoudnessLow + "..." + _RangeLoudnessHigh;
      Range3.text = _RangeDurationLow + "..." + _RangeDurationHigh;

      GradeText.text = _grade;

        if (isFlappy)
        {
            StartCoroutine(resutButtonInteractbale_coroutine());
        }


    }

    public void addToLocalData()
    {
        if (!shouludStore)
            return;

        DateTime _date = DateTime.Now;

        //_gameLog.fetchCurrentPatienAndcounter();

        _gameLog.writeNewData(
            _grade,
            _gameLog.fetchLatestCounter().ToString(),
            gameId,
            clientID.text,
            Date.text,
            _gameBase,
            _ElapsedTime,
             _loundNessTarget,
            _NumOfTrials,
             _cummulativeDurationOfSounds,
             _meanPitch,
             _meanLoudness,
             _StdDevPitch, 
             _StdDevLoudness,
             _RangePitchLow,
             _RangePitchHigh,
             _RangeLoudnessLow,
             _RangeLoudnessHigh, 
             _AudioId

            );
    }

   public void closeApp()
    {
        Debug.Log("funtion called");
        if (shouludStore)
        {
            Application.Quit();
        }

        else
        {
            _audioSampler._audioSource.Stop();
            gameObject.SetActive(false);
            menuManager.instance.updatestartMenuState(startMenuState.roster);
        }

    }


    public void print()
    {

    }
    public void reload()
    {
        
        if (shouludStore)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
        
    }

    IEnumerator resutButtonInteractbale_coroutine()
    {
        ResetButton.interactable = false;
        resetButtonText.fontSize = 20;
        yield return new WaitForSeconds(1);
        resetButtonText.text = "You can retry in 7s";
        yield return new WaitForSeconds(1);
        resetButtonText.text = "You can retry in 6s";
        yield return new WaitForSeconds(1);
        resetButtonText.text = "You can retry in 5s";
        yield return new WaitForSeconds(1);
        resetButtonText.text = "You can retry in 4s";
        yield return new WaitForSeconds(1);
        resetButtonText.text = "You can retry in 3s";
        yield return new WaitForSeconds(1);
        resetButtonText.text = "You can retry in 2s";
        yield return new WaitForSeconds(1);
        resetButtonText.text = "You can retry in 1s";
        yield return new WaitForSeconds(1);
        resetButtonText.text = "Retry";
        resetButtonText.fontSize = 35;
        ResetButton.interactable = true;
    }

    public void recordingfeedback()
    {
        //if (!_audioSampler.showRecoding)
        //    return;
        
        _audioSampler.fileName = _AudioId;
        _audioSampler.PlayItBack();
    }
}
