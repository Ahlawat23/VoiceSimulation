using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class bloonieGirlManager : MonoBehaviour
{
    public static bloonieGirlManager instance;

    public float y;
    public string gName = "bloonieGirl";

    [Header("Game variables")]
    public float targetFreq = 150;
    public float targetLoudness = 1;
    public Vector2 addedForceOnCrossingTargetFreq = new Vector2(0, 5f);

    [Header("Grass")]
    public float minmumTimeToSpawnGrass = 0.1f;
    public float maxmumTimeToSpawnGrass = 3f;
    public float timeForGrass = 3f;
    public float minmumGrassHeight;
    public float maximumGrassheight;

    

    [Header("Prefabs  and transofrms")]
    public GameObject player;
    Rigidbody2D playerRigidBody;
    public GameObject[] grassPrefab;
    public Transform grassSpawnerTransform;
    public Transform finalPostionForGrass;

    [Header("UI transorfoms")]
    public GameObject _topBar;
    
    [Header("menu")]
    public GameObject startScreen;
    public Text timerText;

    [Header("Level flash")]
    public GameObject levelshowcasePanel;
    public Text levelCountText;
    public float numOfHurdlesCrossed;
    public int level = 1;

    [Header("AudioSampler")]
    public float audioFreqScale_Min = 0f;
    public float audioFreqScale_Max = 800f;
    public float bloonieGirlYScale_Min = -2.5f;
    public float bloonieGirlYScale_Max = -3.5f;

    [Header("otherScripts")]
    public Audio_sampler_Final _audioSampler;
    public resultScreen gameOverUI;
    public levelPopUp _levelPopUp;
    





    [Header("Result")]
    public float elapsedTime;
    float timeStarted;

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
    public float grade = 0;
    int badAttempts;
    int goodAttempts;
    public Text numOflevelsText;

    private void Start()
    {
       
        instance = this;

        playerRigidBody = player.GetComponent<Rigidbody2D>();

        gameOverUI._gameLog.setGameDataPath();
        gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;

    }


   
    
    private void Update()
    {

        playerJump();
        elpasedTimeCounter();
        
    }

    public void startGame()
    {
        startScreen.SetActive(false);
        showLevel(1);
   
    }
    public void showLevel(int levelNum)
    {

        if(levelNum == 5)
        {
            GameOver();
            return;
        }

        levelCountText.text = levelNum.ToString();
        setLevelBoolForLevelPopUP(levelNum);
        levelshowcasePanel.SetActive(true);
        
      
        
    }

    void setLevelBoolForLevelPopUP(int levelNum)
    {
        for (int i = 0; i < _levelPopUp.isLevelFinished.Length; i++)
        {
            if (levelNum - 1 == i)
            {
                _levelPopUp.isLevelFinished[i] = true;
            }
            else
            {
                _levelPopUp.isLevelFinished[i] = false;
            }

        }
    }

    public void startLevelOne()
    {
        levelOneGenrateGrass();
        timeStarted = Time.time;
        
        topBar.instance.gameStarted = true;
    }
    int numOftimesInvoked = 0;
    void levelOneGenrateGrass()
    {
        numOftimesInvoked++;
        Grass newGrass = Instantiate(grassPrefab[0], grassSpawnerTransform.position, Quaternion.identity).GetComponent<Grass>();

        newGrass.grassheight = UnityEngine.Random.Range(minmumGrassHeight, maximumGrassheight);
        newGrass.finalPostion = finalPostionForGrass;
        newGrass.timeToReachFinalPos = timeForGrass;
        newGrass.MoveGrassToPlayer();

        if (numOftimesInvoked < 4)
            Invoke("levelOneGenrateGrass", UnityEngine.Random.Range(minmumTimeToSpawnGrass, maxmumTimeToSpawnGrass));
        
           
    }


    public void startLevel2()
    {
        levelTwoGenrateGrass();
    }
    void levelTwoGenrateGrass()
    {
        numOftimesInvoked++;
        Grass newGrass = Instantiate(grassPrefab[1], grassSpawnerTransform.position, Quaternion.identity).GetComponent<Grass>();

        //newGrass.grassheight = UnityEngine.Random.Range(minmumGrassHeight, maximumGrassheight);
        newGrass.finalPostion = finalPostionForGrass;
        newGrass.timeToReachFinalPos = timeForGrass;
        newGrass.MoveGrassToPlayer();

        if (numOftimesInvoked < 8)
            Invoke("levelTwoGenrateGrass", UnityEngine.Random.Range(minmumTimeToSpawnGrass, maxmumTimeToSpawnGrass));
    }

    public void startLevel3()
    {
        levelThreeGenrateGrass();
    }
    void levelThreeGenrateGrass()
    {
        numOftimesInvoked++;
        Grass newGrass = Instantiate(grassPrefab[2], grassSpawnerTransform.position, Quaternion.identity).GetComponent<Grass>();

        //newGrass.grassheight = UnityEngine.Random.Range(minmumGrassHeight, maximumGrassheight);
        newGrass.finalPostion = finalPostionForGrass;
        newGrass.timeToReachFinalPos = timeForGrass;
        newGrass.MoveGrassToPlayer();

        if (numOftimesInvoked < 12)
            Invoke("levelThreeGenrateGrass", UnityEngine.Random.Range(minmumTimeToSpawnGrass, maxmumTimeToSpawnGrass));
    }

    public void startLevel4()
    {
        levelFourGenrateGrass();
    }
    void levelFourGenrateGrass()
    {
        numOftimesInvoked++;
        Grass newGrass = Instantiate(grassPrefab[3], grassSpawnerTransform.position, Quaternion.identity).GetComponent<Grass>();

        //newGrass.grassheight = UnityEngine.Random.Range(minmumGrassHeight, maximumGrassheight);
        newGrass.finalPostion = finalPostionForGrass;
        newGrass.timeToReachFinalPos = timeForGrass;
        newGrass.MoveGrassToPlayer();

        if (numOftimesInvoked < 16)
            Invoke("levelFourGenrateGrass", UnityEngine.Random.Range(minmumTimeToSpawnGrass, maxmumTimeToSpawnGrass));
    }




    int grassPrefabCount = 0;
    int getGrassPrefabCount()
    {
        grassPrefabCount++;
        if(grassPrefabCount == grassPrefab.Length)
        {
            grassPrefabCount =0;
        }
        return grassPrefabCount;
    }


    //PLAYER JUMP
    void playerJump()
    {
        playerJumpByForce();
    }

    void playerJumpByForce()
    {
        if(_audioSampler.pitchVal > 0)
        {
            countCummulative = true;
            countDurationOfAttmpts = false;
        }
        if (_audioSampler.pitchVal > targetFreq)
        {
            
            playerRigidBody.AddForce(addedForceOnCrossingTargetFreq*3);
            countCummulative = true;
            countDurationOfAttmpts = true;
            calculatePitchAndLoudness_update();
        }
        else
        {
            //playerRigidBody.AddForce(-addedForceOnCrossingTargetFreq / 3);
            countCummulative = false;
            countDurationOfAttmpts = false;
        }
    }

    void playerJumbByFreqTrack()
    {
        y = convertBetweenTwoScales(_audioSampler.pitchVal, audioFreqScale_Min, audioFreqScale_Max, bloonieGirlYScale_Min, bloonieGirlYScale_Max);

        player.transform.LeanMoveLocalY(y, 0.1f);
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
    public int overralRating;
    void elpasedTimeCounter()
    {    
        if(countCummulative == true)
            currentCommulativeDuration  += Time.deltaTime;


        if (countDurationOfAttmpts == true)
            durationOfSuccessFullAttempts  += Time.deltaTime;

    }

    void calculatePitchAndLoudness_update()
    {
        averagePitch.Add(_audioSampler.pitchVal);
        averageLoudness.Add(_audioSampler.dbVal);
    }
    //GAME OVER
    public void GameOver()
    {
        Time.timeScale = 0;

        _topBar.SetActive(false);
        _audioSampler.saveRecording();
        gameOverUI.gameObject.SetActive(true);
        gameOverUI.gameId = gName;

        calucalateGrade();
        gameOverUI._grade = grade.ToString();
        numOflevelsText.text = level.ToString();

        gameOverUI._ElapsedTime = fetchElapsedTime();
        gameOverUI._cummulativeDurationOfSounds = fetchCommulativeDuration();


        gameOverUI._NumOfTrials = numOfHurdlesCrossed.ToString();
        gameOverUI._loundNessTarget = targetLoudness.ToString();

        gameOverUI._meanPitch = fetchAveragePitch();
        gameOverUI._meanLoudness = fetchAverageLoudness();

        gameOverUI._StdDevPitch = fetchStadDevPitch();
        gameOverUI._StdDevLoudness = fetchStadDevLoudnes();

        gameOverUI._RangePitchLow = averagePitch.Min().ToString();
        gameOverUI._RangePitchHigh = averagePitch.Max().ToString();
        gameOverUI._RangeLoudnessLow = averageLoudness.Min().ToString();
        gameOverUI._RangeLoudnessHigh = averageLoudness.Max().ToString();

        gameOverUI._AudioId = _audioSampler.fileName;
        gameOverUI.showResultScreen();
        gameOverUI.shouludStore = true;
        gameOverUI.addToLocalData();

        
    }

    void calucalateGrade()
    {
        Debug.Log("calculating grade");
        Debug.Log(level);
        switch (level)
        {
            case 1:
                grade = (numOfHurdlesCrossed / 4) * 10;
                Debug.Log("case 1 active");
                Debug.Log(numOfHurdlesCrossed / 4);
                break;
            case 2:
                grade = (numOfHurdlesCrossed / 8) * 10;
                break;
            case 3:
                grade = (numOfHurdlesCrossed / 12) * 10;
                break;
            case 4:
                grade = (numOfHurdlesCrossed / 16) * 10;
                break;
            case 5:
                grade = 10;
                break;
        }
    }

    string fetchElapsedTime()
    {
        elapsedTime = Time.time - timeStarted;
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        Debug.Log(time.ToString("hh':'mm':'ss"));
        return time.ToString("hh':'mm':'ss");
    }

    string fetchOverralRating()
    {
        overralRating = goodAttempts / (badAttempts + goodAttempts);
        return overralRating.ToString();
    }

    string fetchCommulativeDuration()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentCommulativeDuration);
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
