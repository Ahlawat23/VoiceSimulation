using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;


public class bllonieUI : MonoBehaviour
{
    public Label Date;
    public Label ElapsedTime;
    public Label cummulativeDurationOfSounds;

    public Label Mean1;
    public Label Mean2;
    public Label Mean3;

    public Label StdDev1;
    public Label StdDev2;
    public Label StdDev3;

    public Label Range1Low;
    public Label Range2Low;
    public Label Range3Low;

    public Label Range1High;
    public Label Range2High;
    public Label Range3High;


    public Label clientID;

    public Label loundNessTarget;

    public Label NumOfTrials;

    public Label rightCummulativeDurSoun;

    public Label Pitch;
    public Label Loudness;
    public Label DurationOfSuccessFullAttempts;

   
    public Button PrintButton;
    public Button ResetButton;
    public Button playItBackButton;
    public Button ExitButton;


    public patientGameDataLog _gameLog;
    public Audio_sampler_Final _audioSampler;
    public bool shouludStore=  true;

    
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Date = root.Q<Label>("Date");
        ElapsedTime = root.Q<Label>("ElapsedTime");
        
        cummulativeDurationOfSounds = root.Q<Label>("cummulativeDurationOfSounds");

        Mean1 = root.Q<Label>("Mean1");
        Mean2 = root.Q<Label>("Mean2");
        Mean3 = root.Q<Label>("Mean3");

        StdDev1 = root.Q<Label>("StdDev1");
        StdDev2 = root.Q<Label>("StdDev2");
        StdDev3 = root.Q<Label>("StdDev3");

        Range1Low = root.Q<Label>("Range1Low");
        Range2Low = root.Q<Label>("Range2Low");
        Range3Low = root.Q<Label>("Range3Low");

        Range1High = root.Q<Label>("Range1High");
        Range2High = root.Q<Label>("Range2High");
        Range3High = root.Q<Label>("Range3High");


        clientID = root.Q<Label>("clientID");

        loundNessTarget = root.Q<Label>("loundNessTarget");

        NumOfTrials = root.Q<Label>("NumOfTrials");

        

        Pitch = root.Q<Label>("Pitch");

        Loudness = root.Q<Label>("Loudness");

        DurationOfSuccessFullAttempts = root.Q<Label>("DurationOfSuccessFullAttempts");

        PrintButton = root.Q<Button>("Print");
        ResetButton = root.Q<Button>("Reset");
        playItBackButton = root.Q<Button>("PlayItBack");
        ExitButton = root.Q<Button>("Exit");

        PrintButton.clickable.clicked += print;
        ResetButton.clickable.clicked += reload;
        playItBackButton.clickable.clicked += recordingfeedback;
        ExitButton.clickable.clicked += closeApp;
        

    }

    public void setElapsedTime(string time)
    {
        ElapsedTime.text = time;
        
    }

    public void setComulativerDurationOfSound(string time)
    {
        cummulativeDurationOfSounds.text = time;
       
    }

    

    public void setDurationOfSuccessFullAttempts(string time)
    {
        DurationOfSuccessFullAttempts.text = time;
        
    }


    public void setNumOfTrial(string num)
    {
        NumOfTrials.text = num;
    }
    public void setNumOfTrial(string num, string outOf)
    {
        NumOfTrials.text = num + "/" + outOf;
    }


    public void setLoudnessTarget(string dbVal)
    {
        loundNessTarget.text = dbVal;
    }

    public void setMean(string pitch, string loudness, string time)
    {
        Mean1.text = pitch;
        Mean2.text = loudness;
        Mean3.text = time;
    }

    public void setStdDev(string pitch, string loudness, string time)
    {
        StdDev1.text = pitch;
        StdDev2.text = loudness;
        StdDev3.text = time;
    }

    public void setRangePitch(string low, string high)
    {
        Range1Low.text = low;
        Range1High.text = high;
    }

    public void setRangeLoudness(string low, string high)
    {
        Range2Low.text = low;
        Range2High.text = high;
    }

    public void setRangeTime(string low, string high)
    {
        Range3Low.text = low;
        Range3High.text = high;
        addToLocalData();
    }

    public void addToLocalData()
    {
        if (!shouludStore)
            return;

        DateTime _date = DateTime.Now;

        //_gameLog.fetchCurrentPatienAndcounter();

        _gameLog.writeNewData(
            "gameBase",
            "overrallRating",
            _gameLog.fetchLatestCounter().ToString(),
            "id",
            PermanentData.getCurrentPatientId(_gameLog.gameDataPath + PermanentData.CURRENT_PATIENT_AND_COUNTER_PATH_FOR_SIM),
            _date.ToString(),
            ElapsedTime.text,
             loundNessTarget.text,
             NumOfTrials.text,
             cummulativeDurationOfSounds.text,
             Mean1.text, 
             Mean2.text,
             StdDev1.text,
             StdDev2.text,
             Range1Low.text, 
             Range2High.text,
             Range2Low.text,
             Range2High.text, 
             Range2High.text

            ) ;

        
    }

     void closeApp()
    {
        Debug.Log("funtion called");
        if (shouludStore)
        {
            Application.Quit();
        }
            
        else
        {
            gameObject.SetActive(false);
            menuManager.instance.updatestartMenuState(startMenuState.roster);
        }
            
    }

    void print()
    {

    }
    void reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void recordingfeedback()
    {
        
        _audioSampler.PlayItBack();
    }
}
