using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class bloonBlowManager : MonoBehaviour
{
    public static bloonBlowManager instance;
    public string gName = "BloonBlow";
    public string gameBase = "Loudness";


    [Header("Game Starter screen")]
    public GameObject gameStarteScreenPanel;
    public InputField[] targets;
    public Text showdb;

    [Header("Game variables")]
    public float targetFreq = 150;
    public float targetLoudness = 1;
    public float targetLoudness2;
    public float targetLoudness3;
    bool firstTargetAchived = false;
    bool secondTargeAchived = false;
    bool thirdTargetAchived = false;


    [Header("BallonVals")]
    public float ballonVal = 0f;
    public float speedForThresshold = 0.001f;
    public float speedFor2ndTarget = 0.002f;
    public float speedForFinalTarget = 0.003f;
    public float speedInertia = -0.0005f;



    [Header("Prefabs  and transofrms")]
    public GameObject balloonInc;
    public GameObject balloonDec;
    public GameObject[] ballons;
    public GameObject[] balloonsDecreasing;
    public GameObject _topBar;
    public GameObject circleBallon;


    [Header("AudioSampler")]
    public float audioFreqScale_Min = 0f;
    public float audioFreqScale_Max = 800f;
    public float loudnessScale_Min = 0f;
    public float loudnessScale_Max = 50f;
    public float[] balloonScale_Min;
    public float[] balloonScale_Max;

    [Header("otherScripts")]
    public Audio_sampler_Final _audioSampler;
    public resultScreen gameOverUI;
    public topBar topbarcomponent;





    [Header("Result")]
    float timeStarted;
    public float elapsedTime;

    //cummulative duration
    bool countCummulative = false;
    float currentCommulativeDuration;

    //accousticData
    List<float> averagePitch = new List<float>();
    List<float> averageLoudness = new List<float>();
    bool countDurationOfAttmpts = false;
    float durationOfSuccessFullAttempts;

    //numoftrials
    public int numOfTrails;

    
    private void Start()
    {

        instance = this;
       
        gameStarteScreenPanel.SetActive(true);

        gameOverUI._gameLog.setGameDataPath();
        gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;

        topbarcomponent = _topBar.GetComponent<topBar>();


      
        gameStarteScreenPanel.gameObject.SetActive(false);
        topbarcomponent.gameStarted = true;

    }

    private void Update()
    {
        
        
        elpasedTimeCounter();
        
        circleExpand();
    }

    void showDbText()
    {
        if(gameStarteScreenPanel.activeSelf)
        showdb.text = _audioSampler.dbVal.ToString();
    }

   
    public void StartGame()
    {
        targetLoudness = float.Parse( targets[0].text);
        targetLoudness2 = float.Parse(targets[1].text);
        targetLoudness3 = float.Parse(targets[2].text);
        gameStarteScreenPanel.gameObject.SetActive(false);
        topbarcomponent.gameStarted = true;
    }

    public void reselecleTargets()
    {
        gameStarteScreenPanel.gameObject.SetActive(true);
        topbarcomponent.gameStarted = false;
    }



    //BloonBlow
  

    void circleBlow()
    {
        if (gameStarteScreenPanel.gameObject.activeSelf)
            return;

        if (topbarcomponent.gamePaused)
            return;

        if (gameOver)
            return;

        if (_audioSampler.dbVal < targetLoudness)
        {
            calculateBallonVal(speedInertia);

        }
        else if (_audioSampler.dbVal > targetLoudness && _audioSampler.dbVal < targetLoudness2)
        {

            calculateBallonVal(speedForThresshold);

            firstTargetAchived = true;

        }
        else if (_audioSampler.dbVal > targetLoudness2 && _audioSampler.dbVal < targetLoudness3)
        {

            calculateBallonVal(speedFor2ndTarget);

            secondTargeAchived = true;
        }
        else if (_audioSampler.dbVal > targetLoudness3)
        {

            calculateBallonVal(speedForFinalTarget);
            thirdTargetAchived = true;
        }

        if (_audioSampler.dbVal > targetLoudness)
        {

            countCummulative = true;
            countDurationOfAttmpts = true;
            //calculatePitchAndLoudness_update();
        }

        float val = convertBetweenTwoScales(ballonVal, 0, 1, 1, 9);

        if(val < 9)
        {
            val = 9;
        }

        
        circleBallon.transform.LeanScale(new Vector3(val, val, 1), 0.1f).setEaseOutBack();

       
    }

    void circleExpand()
    {
        if (gameStarteScreenPanel.gameObject.activeSelf)
            return;

        if (topbarcomponent.gamePaused)
            return;

        if (gameOver)
            return;


        if(_audioSampler.dbVal > targetLoudness)
        {
            
            float val = convertBetweenTwoScales(_audioSampler.dbVal, targetLoudness, targetLoudness3, 1, 9);
            if (val > 9)
            {
                val = 9;
            }

            circleBallon.transform.localScale = new Vector3(val, val, val);
            countCummulative = true;
            countDurationOfAttmpts = true;
        }
        else 
        {

            countCummulative = false;
            countDurationOfAttmpts = false;
            calculatePitchAndLoudness_update();

        }



    }

    void blowBalloon()
    {

        if (gameStarteScreenPanel.gameObject.activeSelf)
            return;

        if (topbarcomponent.gamePaused)
            return;

        if (gameOver)
            return;




        //float val = convertBetweenTwoScales(_audioSampler.dbVal, loudnessScale_Min, loudnessScale_Max, balloonScale_Min, balloonScale_Max);

        //ballon.transform.LeanScale(new Vector3(val, val, 1), 0.1f);
        if (_audioSampler.dbVal < targetLoudness)
        {
            balloonInc.SetActive(false);
            balloonDec.SetActive(true);
            //setBallonActive(0);
        }
        else if (_audioSampler.dbVal > targetLoudness && _audioSampler.dbVal < targetLoudness2)
        {
            float val = convertBetweenTwoScales(_audioSampler.dbVal, targetLoudness, targetLoudness2, balloonScale_Min[0], balloonScale_Max[0]);
            if (val > 0.04 && val < 0.055)
            {
                setBallonActive(0);
                ballons[0].transform.LeanScale(new Vector3(val, val, 1), 2f).setEaseOutBack();
            }
            if (val > 0.055 && val < 0.07)
            {
                setBallonActive(1);
                ballons[1].transform.LeanScale(new Vector3(val, val, 1), 0.5f).setEaseOutBack();
            }
            firstTargetAchived = true;

        }
        else if (_audioSampler.dbVal > targetLoudness2 && _audioSampler.dbVal < targetLoudness3)
        {
            float val = convertBetweenTwoScales(_audioSampler.dbVal, targetLoudness, targetLoudness2, balloonScale_Min[1], balloonScale_Max[1]);

            if (val > 0.07 && val < 0.12)
            {
                setBallonActive(2);
                ballons[2].transform.LeanScale(new Vector3(val, val, 1), 0.1f).setEaseOutBack();
            }
            if(val > 0.12)
            {
                setBallonActive(3);
                ballons[3].transform.LeanScale(new Vector3(val, val, 1), 0.1f).setEaseOutBack();
            }
            secondTargeAchived = true;
        }
        else if(_audioSampler.dbVal > targetLoudness3)
        {
            float val = convertBetweenTwoScales(_audioSampler.dbVal, targetLoudness, targetLoudness2, balloonScale_Min[2], balloonScale_Max[2]);
            setBallonActive(4);
            ballons[4].transform.LeanScale(new Vector3(val, val, 1), 0.1f).setEaseOutBack();
            thirdTargetAchived= true;
        }

        if (_audioSampler.dbVal > targetLoudness)
        {
            balloonInc.SetActive(true);
            balloonDec.SetActive(false);
            countCummulative = true;
            countDurationOfAttmpts = true;
            calculatePitchAndLoudness_update();
        }
            
    }

    //ballon blow gradual

    void exapndBallon()
    {
        if (gameStarteScreenPanel.gameObject.activeSelf)
            return;

        if (topbarcomponent.gamePaused)
            return;

        if (gameOver)
            return;

        if (_audioSampler.dbVal < targetLoudness)
        {
            calculateBallonVal(speedInertia);
            
        }
        else if (_audioSampler.dbVal > targetLoudness && _audioSampler.dbVal < targetLoudness2)
        {
            
            calculateBallonVal(speedForThresshold);
            
            firstTargetAchived = true;

        }
        else if (_audioSampler.dbVal > targetLoudness2 && _audioSampler.dbVal < targetLoudness3)
        {
           
            calculateBallonVal(speedFor2ndTarget);
           
            secondTargeAchived = true;
        }
        else if (_audioSampler.dbVal > targetLoudness3)
        {
            
            calculateBallonVal(speedForFinalTarget);
            thirdTargetAchived = true;
        }

        if (_audioSampler.dbVal > targetLoudness)
        {
            
            countCummulative = true;
            countDurationOfAttmpts = true;
            //calculatePitchAndLoudness_update();
        }
        balloonBlowGraduall();
    }

    void calculateBallonVal(float speed)
    {
        ballonVal = ballonVal + 1* speed;
        if(ballonVal < 0)
            ballonVal = 0;
        if (ballonVal > 1)
            ballonVal = 1;
    }

    
    void balloonBlowGraduall()
    {
       

        if(ballonVal > 0f && ballonVal <= 0.2f)
        expandBallonAccordingToVal(0, 0, 0.2f, 0);

        if (ballonVal > 0.2f && ballonVal <= 0.4f)
            expandBallonAccordingToVal(1, 0.2f, 0.4f, 1);

        if (ballonVal > 0.4f && ballonVal <= 0.6f)
            expandBallonAccordingToVal(2, 0.4f, 0.6f, 2);

        if (ballonVal > 0.6f && ballonVal <= 0.8f)
            expandBallonAccordingToVal(3, 0.6f, 0.8f, 3);

        if (ballonVal > 0.8f)
            expandBallonAccordingToVal(4, 0.8f, 1, 4);

    }

    void expandBallonAccordingToVal(int ballonIndex, float valMin, float valMax, int minMaxIndex)
    {
        float val = convertBetweenTwoScales(ballonVal, valMin, valMax, balloonScale_Min[minMaxIndex], balloonScale_Max[minMaxIndex]);

        setBalloonDecActive(ballonIndex);
        balloonsDecreasing[ballonIndex].transform.LeanScale(new Vector3(val, val, 1), 0.1f).setEaseOutBack();
    }

    void setBallonActive(int index)
    {
        for (int i = 0; i < ballons.Length; i++)
        {
            if (i == index)
                ballons[i].SetActive(true);
            else
                ballons[i].SetActive(false);
        }
    }

    void setBalloonDecActive(int index)
    {
        for (int i = 0; i < ballons.Length; i++)
        {
            if (i == index)
                balloonsDecreasing[i].SetActive(true);
            else
                balloonsDecreasing[i].SetActive(false);
        }
    }

   
   

    public float convertBetweenTwoScales(float oldVal, float firstScaleMin, float firstScaleMax, float secondScaleMin, float secondScaleMax)
    {
        float firstScaleLength = firstScaleMax - firstScaleMin;
        float secondScaleLength = secondScaleMax - secondScaleMin;

        //shift to origin
        float originShift = oldVal - firstScaleMin;
        //normalize
        float normalizedVal = originShift / firstScaleLength;
        //upscale 
        float upscaleVal = normalizedVal * secondScaleLength;
        //shft form origin
        float newVal = upscaleVal + secondScaleMin;

        return newVal;
    }

    //RESULTS
    void elpasedTimeCounter()
    {
        if (countCummulative == true)
            currentCommulativeDuration += Time.deltaTime;


        if (countDurationOfAttmpts == true)
            durationOfSuccessFullAttempts += Time.deltaTime;

    }

    void calculatePitchAndLoudness_update()
    {
        averagePitch.Add(_audioSampler.pitchVal);
        averageLoudness.Add(_audioSampler.dbVal);
    }
    //GAME OVER
    bool gameOver = false;
    public void GameOver()
    {
        gameOver = true;
        //Time.timeScale = 0;

        topbarcomponent.gameOverRecording();
        _topBar.gameObject.SetActive(false);
        _audioSampler.saveRecording();
        gameOverUI.gameObject.SetActive(true);
        gameOverUI.gameId = gName;

        gameOverUI._ElapsedTime = fetchElapsedTime();
        gameOverUI._cummulativeDurationOfSounds = fetchCommulativeDuration();

        gameOverUI._grade = calculateGrade().ToString();
        gameOverUI._NumOfTrials = topbarcomponent.GetTrailCont.ToString();
        gameOverUI._loundNessTarget = targetLoudness3.ToString();
        gameOverUI._gameBase = gameBase;

        gameOverUI._meanPitch = fetchAveragePitch();
        gameOverUI._meanLoudness = fetchAverageLoudness();

        gameOverUI._StdDevPitch = fetchStadDevPitch();
        gameOverUI._StdDevLoudness = fetchStadDevLoudnes();

        gameOverUI._RangePitchLow = ((int)averagePitch.Min()).ToString();
        gameOverUI._RangePitchHigh = ((int)averagePitch.Max()).ToString();
        gameOverUI._RangeLoudnessLow = ((int)averageLoudness.Min()).ToString();
        gameOverUI._RangeLoudnessHigh = ((int)averageLoudness.Max()).ToString();

        gameOverUI._AudioId = _audioSampler.fileName;
        gameOverUI.showResultScreen();
        gameOverUI.shouludStore = true;
        gameOverUI.addToLocalData();

    }


    public void playRecordint()
    {
        topbarcomponent.pauseGame();
        _audioSampler.PlayItBack();
    }

    public void stopPlayingRecording()
    {
        _audioSampler._playBackAudioSource.Stop();
        topbarcomponent.PlayGame();
    }
    float calculateGrade()
    {
        float grade = 0;
        if (!secondTargeAchived)
        {
            grade = 3;
        }
        if (!thirdTargetAchived)
        {
            grade = 6;
        }
        else
        {
            grade = 10;
        }
        return grade;
    }

    string fetchElapsedTime()
    {
        elapsedTime = Time.time - timeStarted;
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        //Debug.Log(time.ToString("hh':'mm':'ss"));
        return time.ToString("hh':'mm':'ss");
    }
    string fetchCommulativeDuration()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentCommulativeDuration);
        return time.ToString("hh':'mm':'ss");
    }

    string fetchAveragePitch()
    {
        float allVal = 0;

        for (int i = 0; i < averagePitch.Count; i++)
            allVal += averagePitch[i];

        return (allVal / averagePitch.Count).ToString();
    }
    string fetchAverageLoudness()
    {
        float allVal = 0;

        for (int i = 0; i < averageLoudness.Count; i++)
            allVal += averageLoudness[i];

        return (allVal / averageLoudness.Count).ToString();
    }

    string fetchDurationOfSuccessfullAtempts()
    {
        TimeSpan time = TimeSpan.FromSeconds(durationOfSuccessFullAttempts);
        return time.ToString("hh':'mm':'ss");
    }

    string fetchStadDevPitch()
    {
        float allVal = 0;

        for (int i = 0; i < averagePitch.Count; i++)
            allVal += averagePitch[i];

        float mean = allVal / averagePitch.Count;


        float sum = 0;
        for (int i = 0; i < averagePitch.Count; i++)
            sum += (averagePitch[i] - mean) * (averagePitch[i] - mean);

        float val = sum / averagePitch.Count;

        return Math.Sqrt(val).ToString();
    }
    string fetchStadDevLoudnes()
    {
        float allVal = 0;

        for (int i = 0; i < averageLoudness.Count; i++)
            allVal += averageLoudness[i];

        float mean = allVal / averageLoudness.Count;


        float sum = 0;
        for (int i = 0; i < averageLoudness.Count; i++)
            sum += (averageLoudness[i] - mean) * (averageLoudness[i] - mean);

        float val = sum / averageLoudness.Count;

        return Math.Sqrt(val).ToString();
    }
}
