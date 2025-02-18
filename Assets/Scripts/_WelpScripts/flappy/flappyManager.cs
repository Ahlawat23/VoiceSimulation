using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class flappyManager : MonoBehaviour
{
    public static flappyManager instance;
    public string gName = "Flappy";
    public string gameBase = "Frequency";

    public float y;
    public float upscale;

    [Header("Game variables")]
    public float targetFreq = 150;
    public float targetLoudness = 1;
    public Vector2 addedForceOnCrossingTargetFreq = new Vector2(0, 5f);

    public float minTimeToSpawnPipes = 2f;
    public float maxTimeToSpawnPipes = 5f;
    public float timeForPipes = 7f;

    public float minHeight = 0.2f;
    public float maxHeight = 0.2f;

    [Header("Prefabs  and transofrms")]
    public GameObject player;
    public Rigidbody2D playerRigidBody;
    public GameObject[] pipePrefab;
    public Transform pipeSpawnerTransform;
    public Transform finalPostionForPipe;

    [Header("AudioSampler")]
    public float audioFreqScale_Min = 0f;
    public float audioFreqScale_Max = 800f;
    public float planeYScale_Min = -2.5f;
    public float planelYScale_Max = -3.5f;

    [Header("otherScripts")]
    public Audio_sampler_Final _audioSampler;
    public resultScreen gameOverUI;
    public topBar _topBar;
    public levelPopUp _levelPopUp;


    [Header("ui")]
    public GameObject starterScreen;
    [Header("Level flash")]
    public GameObject levelshowcasePanel;
    public Text levelCountText;
    public float numOfHurdlesCrossed;
    public int level = 1;

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
    public float badAttempts;
    //public int fpstarget = 30;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {





        gameOverUI._gameLog.setGameDataPath();
        gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;


        timeStarted = Time.time;
        Time.timeScale = 1;

    }

    private void Update()
    {
        playerJump();
        elpasedTimeCounter();
        //if (Application.targetFrameRate != fpstarget)
        //    Application.targetFrameRate = fpstarget;

    }

    void genratePipes()
    {

        pipes newPipe = Instantiate(pipePrefab[0], pipeSpawnerTransform.position, Quaternion.identity).GetComponent<pipes>();


        newPipe.finalPostion = finalPostionForPipe;
        newPipe.timeToReachFinalPos = timeForPipes;
        newPipe.MoveGrassToPlayer();


        Invoke("genratePipes", UnityEngine.Random.Range(minTimeToSpawnPipes, maxTimeToSpawnPipes));
    }

    public void startGame()
    {
        starterScreen.SetActive(false);
        showLevel(1);

    }
    public void showLevel(int levelNum)
    {

        if (levelNum == 5)
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
    public int numOftimesInvoked = 0;
    void levelOneGenrateGrass()
    {

        numOftimesInvoked++;
        pipes newPipe = Instantiate(pipePrefab[0], pipeSpawnerTransform.position, Quaternion.identity).GetComponent<pipes>();

        newPipe.grassheight = getgrasHeight();
        newPipe.finalPostion = finalPostionForPipe;
        newPipe.timeToReachFinalPos = timeForPipes;
        newPipe.MoveGrassToPlayer();


        Debug.Log("lvel one started" + numOftimesInvoked);
       
        if (numOftimesInvoked < 4 && !isgameover)
            Invoke("levelOneGenrateGrass", UnityEngine.Random.Range(minTimeToSpawnPipes, maxTimeToSpawnPipes));


    }

   


    public void startLevel2()
    {
        levelTwoGenrateGrass();
    }
    void levelTwoGenrateGrass()
    {
        numOftimesInvoked++;
        pipes newPipe = Instantiate(pipePrefab[1], pipeSpawnerTransform.position, Quaternion.identity).GetComponent<pipes>();

        newPipe.grassheight = getgrasHeight();
        newPipe.finalPostion = finalPostionForPipe;
        newPipe.timeToReachFinalPos = timeForPipes;
        newPipe.MoveGrassToPlayer();

        if (numOftimesInvoked < 8 && !isgameover)
            Invoke("levelTwoGenrateGrass", UnityEngine.Random.Range(minTimeToSpawnPipes, maxTimeToSpawnPipes));
    }

    public void startLevel3()
    {
        levelThreeGenrateGrass();
    }
    void levelThreeGenrateGrass()
    {
       
        numOftimesInvoked++;
        pipes newPipe = Instantiate(pipePrefab[2], pipeSpawnerTransform.position, Quaternion.identity).GetComponent<pipes>();

        newPipe.grassheight = getgrasHeight();
        newPipe.finalPostion = finalPostionForPipe;
        newPipe.timeToReachFinalPos = timeForPipes;
        newPipe.MoveGrassToPlayer();

        if (numOftimesInvoked < 12 && !isgameover)
            Invoke("levelThreeGenrateGrass", UnityEngine.Random.Range(minTimeToSpawnPipes, maxTimeToSpawnPipes));
    }

    public void startLevel4()
    {
        levelFourGenrateGrass();
    }
    void levelFourGenrateGrass()
    {
        numOftimesInvoked++;
        pipes newPipe = Instantiate(pipePrefab[3], pipeSpawnerTransform.position, Quaternion.identity).GetComponent<pipes>();

        newPipe.grassheight = getgrasHeight();
        newPipe.finalPostion = finalPostionForPipe;
        newPipe.timeToReachFinalPos = timeForPipes;
        newPipe.MoveGrassToPlayer();

        if (numOftimesInvoked < 16 && !isgameover)
            Invoke("levelFourGenrateGrass", UnityEngine.Random.Range(minTimeToSpawnPipes, maxTimeToSpawnPipes));
    }

    float getgrasHeight()
    {
        float height = 0;
        switch (numOftimesInvoked)
        {
            case 0:
                height = 0.1f;
                break;
            case 1:
                height = 0.1f;
                break;
            case 2:
                height = 0.15f;
                break;
            case 3:
                height = 0.2f;
                break;
            case 4:
                height = 0.2f;
                break;
            case 5:
                height = 0.9f;
                break;
            case 6:
                height = 1f;
                break;
            case 7:
                height = 1f;
                break;
            case 8:
                height = 1.1f;
                break;
            case 9:
                height = 0.35f;
                break;
            case 10:
                height = 0.4f;
                break;
            case 11:
                height = 0.45f;
                break;
            case 12:
                height = 0.5f;
                break;
            case 13:
                height = 0.5f;
                break;
            case 14:
                height = 0.6f;
                break;
            case 15:
                height = 0.7f;
                break;
            case 16:
                height = 0.75f;
                break ;
            default:
                height = 0;
                break;
        }
        return height;
    }


    //PLAYER JUMP
    void playerJump()
    {
        playerJumpByForce();
    }

    void playerJumpByForce()
    {
        if (_topBar.gamePaused)
            return;

        if (_audioSampler.pitchVal > 0)
        {
            countCummulative = true;
            countDurationOfAttmpts = false;
        }
        if (_audioSampler.pitchVal > targetFreq)
        {
            playerRigidBody.AddForce(addedForceOnCrossingTargetFreq*3, ForceMode2D.Force);
            countCummulative = true;
            countDurationOfAttmpts = true;
            calculatePitchAndLoudness_update();
        }
        else
        {
            //playerRigidBody.AddForce(-addedForceOnCrossingTargetFreq / 2);
            player.transform.LeanMoveLocalX(-3.75f, 0.2f);
            player.LeanRotate(new Vector3(0, 0, 0), 0.2f);
            countCummulative = false;
            countDurationOfAttmpts = false;
        }
    }

    void playerJumbByFreqTrack()
    {
        y = convertBetweenTwoScales(_audioSampler.pitchVal, audioFreqScale_Min, audioFreqScale_Max, planeYScale_Min, planelYScale_Max);

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
        upscale = upscaleVal;
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
    public bool isgameover = false;
    public void GameOver()
    {
        //Time.timeScale = 0;
        isgameover = true;

      
        _topBar.gameObject.SetActive(false);
        _topBar.gameStarted = false;
        _levelPopUp.isGameOver = true;
        levelshowcasePanel.gameObject.SetActive(false);
        _audioSampler.saveRecording();
        gameOverUI.gameObject.SetActive(true);

        gameOverUI.gameId = gName;
        gameOverUI._gameBase = gameBase;

        gameOverUI._grade = calculateGrade().ToString();
        gameOverUI._ElapsedTime = fetchElapsedTime();
        gameOverUI._cummulativeDurationOfSounds = fetchCommulativeDuration();


        gameOverUI._NumOfTrials = _topBar.GetTrailCont.ToString();
        gameOverUI._loundNessTarget = targetLoudness.ToString();

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

        starterScreen.SetActive(true);

        _levelPopUp.isGameOver = false;
        _levelPopUp.resetGame();

        averageLoudness.Clear();
        averagePitch.Clear();

        elapsedTime = 0;
        numOftimesInvoked = 0;
        numOfHurdlesCrossed = 0;
        badAttempts = 0;
        level = 1;
        isgameover = false;
    }
    float calculateGrade()
    {
        float currentGrade = 0;
        currentGrade = (badAttempts / numOfHurdlesCrossed) * 10;
        currentGrade = 10 - currentGrade;
        return currentGrade;
    }

    string fetchElapsedTime()
    {
        elapsedTime = Time.time - timeStarted;
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        Debug.Log(time.ToString("hh':'mm':'ss"));
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
