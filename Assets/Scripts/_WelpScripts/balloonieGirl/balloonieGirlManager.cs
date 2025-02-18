using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class balloonieGirlManager : MonoBehaviour
{
    public string gName = "bloonieGirl";
    public string gameBase = "Frequency";
    public float y;


    [Header("Prefabs  and transofrms")]
    public GameObject player;
    public Rigidbody2D playerRigidBody;

    [Header("Db Scale")]
    public float minDb;
    public float maxDb;
    [Header("Slider Scale")]
    public float playerSliderVal;
    public float playerSliderMin;
    public float playerSliderMax;
    [Header("Sprite Scale")]
    public float playerSpriteMin;
    public float playerSpriteMax;


    [Header("Grass containter")]
    public float moveValue;
    public float grassMovementSpeed = 0.0001f;
    public float minValx;
    public float maxValx;
    public float minGrassSliderVal;
    public float maxGrassSliderVal;
    public Transform grassContainer;
    public float valYForGrassContainer;



    [Header("inertia")]
    public float inertiaVal;
    public float inertiaValDeduction;
    public bool shouldInertia;
    
  
   

    [Header("Game variables")]
    public float targetFreq = 150;
    public float targetLoudness = 1;
    public Vector2 addedForceOnCrossingTargetFreq = new Vector2(0, 5f);
    public float moveSpeed = 1f;

    [Header("other Scripts")]
    public Audio_sampler_Final _audioSampler;
    public resultScreen gameOverUI;
    public topBar _topBar;

   

    [Header("AudioSampler")]
    public float audioFreqScale_Min = 0f;
    public float audioFreqScale_Max = 800f;
    public float bloonieGirlYScale_Min = -2.5f;
    public float bloonieGirlYScale_Max = -3.5f;

    [Header("Result")]
    public float elapsedTime;
    float timeStarted;

    //cummulative duration
    bool countCummulative = false;
    public float currentCommulativeDuration;

    //accousticData
    List<float> averagePitch = new List<float>();
    List<float> averageLoudness = new List<float>();
    
    float durationOfSuccessFullAttempts;

    //numoftrials
    public int numOfTrails;
    public float grade = 0;
    public float badAttempts;
    int goodAttempts;
    public Text numOflevelsText;
   
    

    private void Start()
    {
        

        gameOverUI._gameLog.setGameDataPath();
        gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;
        
        
    }
    private void Update()
    {
        //if (Application.targetFrameRate != 60)
        //    Application.targetFrameRate = 60;
        if (_topBar.gamePaused)
            return;

        elpasedTimeCounter();
        calculateSliderVal();
        moveGrassAccordingToValue(moveValue);
        controlPlayerMovement();
        moveGrassOnRecivingSound();

    }

    void calculateSliderVal()
    {
        playerSliderVal = convertBetweenTwoScales(_audioSampler.dbVal, minDb, maxDb, playerSliderMin, playerSliderMax);
        if (playerSliderVal > playerSliderMax)
            playerSliderVal = playerSliderMax;
        if (playerSliderVal < playerSliderMin)
            playerSliderVal = playerSliderMin;
    }

    public void controlPlayerMovement()
    {
        float y = convertBetweenTwoScales(playerSliderVal, playerSliderMin, playerSliderMax, playerSpriteMin, playerSpriteMax);
        player.transform.position = new Vector3(player.transform.position.x, y, player.transform.position.z);
    }


    public void moveGrassAccordingToValue(float moveValue)
    {
        
        float newValueX = convertBetweenTwoScales(moveValue, minGrassSliderVal, maxGrassSliderVal, minValx, maxValx);
        grassContainer.position = new Vector3(newValueX, valYForGrassContainer, 0);
    }
 
    void moveGrassOnRecivingSound()
    {
        if (_audioSampler.dbVal > minDb)
        {
            
            countCummulative = true;
            calculatePitchAndLoudness_update();
            calculateMoveVal();
        }
        else
        {
            shouldInertia = true;
            //calculateInertia();
            countCummulative = false;
        }
    }

   

    
   

    bool keepDoinInertia;
    void calculateInertia()
    {
        if (player.transform.position.y < -2.2)
        {
            player.transform.LeanMoveLocalX(-5, 0.2f);
            player.LeanRotate(new Vector3(0, 0, 0), 0.2f);
            return;
        }

        if (!shouldInertia)
           return;

         inertiaVal = grassMovementSpeed;
        if (inertiaVal > 0)
        {
            moveValue += inertiaVal;
            inertiaVal -= inertiaValDeduction;
            
        }
        shouldInertia = false;

    }
    

    void calculateMoveVal()
    {
        moveValue = moveValue + 1 * grassMovementSpeed;
        if (moveValue > 1.1f && !gameisOver)
            GameOver();
    }

    void elpasedTimeCounter()
    {
        if (countCummulative == true)
            currentCommulativeDuration += Time.deltaTime;
    }
    void calculatePitchAndLoudness_update()
    {
        averagePitch.Add(_audioSampler.pitchVal);
        averageLoudness.Add(_audioSampler.dbVal);
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

    bool gameisOver = false;

    //Setting ui

  


    //GAME OVER
    public void GameOver()
    {
        gameisOver = true;

        _topBar.gameOverRecording();
        _topBar.gameObject.SetActive(false);
        _audioSampler.saveRecording();
        gameOverUI.gameObject.SetActive(true);
        gameOverUI.gameId = gName;

        grade = calculateGrade();
        gameOverUI._grade = grade.ToString();
        numOflevelsText.text = getCompletedLevel().ToString();

        gameOverUI._gameBase = gameBase;
        gameOverUI._ElapsedTime = fetchElapsedTime();
        gameOverUI._cummulativeDurationOfSounds = fetchCommulativeDuration();


        gameOverUI._NumOfTrials = _topBar.GetTrailCont.ToString();
        //gameOverUI._loundNessTarget = targetLoudness.ToString();

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
        _topBar.pauseGame();
        _audioSampler.PlayItBack();
    }

    public void stopPlayingRecording()
    {
        _audioSampler._playBackAudioSource.Stop();
        _topBar.PlayGame();
    }

    public void ResetGame()
    {
        _topBar.gameObject.SetActive(true);
        _topBar.time = 0f;
        _topBar.increaseTrial();



        gameOverUI.gameObject.SetActive(false);

        

        

        averageLoudness.Clear();
        averagePitch.Clear();

        elapsedTime = 0;


       badAttempts = 0;
       goodAttempts = 0;
   

        gameisOver = false;
        moveValue = 0f;
    }

    int getCompletedLevel()
    {
        int lastLevel = 0;

        if(moveValue < 0.3f)
            lastLevel = 1;
        if (moveValue > 0.3f && moveValue < 0.5f)
            lastLevel = 2;
        if (moveValue > 0.5f && moveValue < 0.75f)
            lastLevel = 3;
        if(moveValue> 0.75f)
            lastLevel = 4;


        return lastLevel;
    }

    float calculateNumOfGrassPassed()
    {
        float numofGrassPassed = 0;

        if (moveValue > 0.09f)
            numofGrassPassed = 1;
        if (moveValue > 0.13f)
            numofGrassPassed = 2;
        if (moveValue > 0.2f)
            numofGrassPassed = 3;
        if (moveValue > 0.27f)
            numofGrassPassed = 4;
        if (moveValue > 0.34f)
            numofGrassPassed = 5;
        if (moveValue > 0.375f)
            numofGrassPassed = 6;
        if (moveValue > 0.425f)
            numofGrassPassed = 7;
        if (moveValue > 0.47f)
            numofGrassPassed = 8;
        if (moveValue > 0.57f)
            numofGrassPassed = 9;
        if (moveValue > 0.62f)
            numofGrassPassed = 10;
        if (moveValue > 0.68f)
            numofGrassPassed = 11;
        if (moveValue > 0.74f)
            numofGrassPassed = 12;
        if (moveValue > 0.84f)
            numofGrassPassed = 13;
        if (moveValue > 0.89f)
            numofGrassPassed = 14;
        if (moveValue > 0.95f)
            numofGrassPassed = 15;
        if (moveValue > 1.01f)
            numofGrassPassed = 16;


        return numofGrassPassed;
    }
    float calculateGrade()
    {
        float currentGrade = 0;
        
        currentGrade = (badAttempts / calculateNumOfGrassPassed()) * 10;
        currentGrade = 10 - currentGrade;
        return currentGrade;
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
        
        TimeSpan time = TimeSpan.FromSeconds((int)currentCommulativeDuration);
        return time.ToString("hh':'mm':'ss"); 
    }

    float meanPitch;
    string fetchAveragePitch()
    {
        float allVal = 0;

        for (int i = 0; i < averagePitch.Count; i++)
            allVal += averagePitch[i];

        meanPitch = allVal / averagePitch.Count;

        return meanPitch.ToString();
    }

    float meanLoudness;
    string fetchAverageLoudness()
    {
        float allVal = 0;

        for (int i = 0; i < averageLoudness.Count; i++)
            allVal += averageLoudness[i];

        meanLoudness = allVal / averageLoudness.Count;
        return meanLoudness.ToString();
    }

    string fetchDurationOfSuccessfullAtempts()
    {
        TimeSpan time = TimeSpan.FromSeconds(durationOfSuccessFullAttempts);
        return time.ToString("hh':'mm':'ss");
    }

    string fetchStadDevPitch()
    {
        float sum = 0;
        for (int i = 0; i < averagePitch.Count; i++)
            sum += (averagePitch[i] - meanPitch) * (averagePitch[i] - meanPitch);

        float val = sum / averagePitch.Count;

        return Math.Sqrt(val).ToString();
    }


    string fetchStadDevLoudnes()
    {
        float sum = 0;
        for (int i = 0; i < averageLoudness.Count; i++)
            sum += (averageLoudness[i] - meanLoudness) * (averageLoudness[i] - meanLoudness);

        float val = sum / averageLoudness.Count;

        return Math.Sqrt(val).ToString();
    }
}
