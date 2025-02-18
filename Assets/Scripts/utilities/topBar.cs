using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class topBar : MonoBehaviour
{
    public static topBar instance;
    public static int trailCount = 0;

    public int GetTrailCont { get { return trailCount; } }

    public bool gameStarted = false;
    public Text timerText;
    public Text trailText;
    public float time = 0f;
    public float recordingTime = 0f;

    public bool gamePaused = false;

    public Audio_sampler_Final _audioSampler;

    [Header("UI")]
    public GameObject recordButton;
    private void Start()
    {
        
        instance = this;
        
        trailCount++;
        setTrial(trailCount);
        Time.timeScale = 1;

    }
    public void increaseTrial()
    {
        trailCount++;
        setTrial(trailCount);
    }

    void setTrialPlayerPref()
    {
        int trialNum;
        if (PlayerPrefs.HasKey("Trial"))
        {
            trialNum = PlayerPrefs.GetInt("Trial");
            trialNum++;
            PlayerPrefs.SetInt("Trial", trialNum);
            
        }
        else
        {
            PlayerPrefs.SetInt("Trial", 0);
            setTrial(0);
        }
    }

    

    private void Update()
    {
        
        recordingTime = recordingTime + Time.deltaTime;
        upadateTimer();
      
    }

    float seconds;
    float minutes;
    void upadateTimer()
    {
        if (!gameStarted)
            return;
        if (gamePaused)
            return;
        time = time + Time.deltaTime;
        
        seconds = MathF.Floor(time % 60);
         minutes = MathF.Floor(time / 60);
        
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void setTrial(int trialNum)
    {
        trailText.text = "Trail: " + trialNum.ToString();
    }
    public void pauseGame()
    {
        //Time.timeScale = 0;
        gamePaused = true;
        _audioSampler.secPauses.Add(recordingTime);
        
    }

    public void PlayGame()
    {
        //Time.timeScale = 1;
        gamePaused = false;
        _audioSampler.secPlays.Add(recordingTime);
        
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void startRecording()
    {
        _audioSampler.secStart = recordingTime;
    }

    public void stopRecording()
    {   
        _audioSampler.secEnd = recordingTime;
        //recordButton.GetComponent<Button>().interactable = false;

    }

    public void gameOverRecording()
    {
        if (_audioSampler.secEnd == 0)
            _audioSampler.secEnd = recordingTime;
    }
    public void setNumOfTrails(int num)
    {
        PlayerPrefs.SetInt("Trial", num);
    }
}
