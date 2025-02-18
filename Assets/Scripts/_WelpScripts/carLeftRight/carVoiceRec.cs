using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;
using System.Text;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class carVoiceRec : MonoBehaviour
{
    public static carVoiceRec instance;
    public string gName = "CarLevel1";
    public string gamebase = "Recogintion";

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    
    [Header("KeyWords")] 
    public string keywordRight;
    public string keywordLeft;
    public TextMeshProUGUI rightText;
    public TextMeshProUGUI leftText;

    [Header("Transoform and prefabs")]
    public Transform carSprite;
    public List<GameObject> obstacles = new List<GameObject>();
    public List<Transform> obstaclesSpawPoint = new List<Transform>();
    public List<Transform> finalPosPoint = new List<Transform>();

    [Header("Game variables")]
    public float obstSpawnSpeedMin = 2;
    public float obstSpawnSpeedMax = 5;
    public float obstTimeToFinal = 5f;
    public float targetFreq = 150;
    public float targetLoudness = 1;
    public float numOfBlinks = 8;
    public float secBetweenBlinks = 0.2f;

    [Header("startSCreen")]
    public InputField wordsToSayRight;
    public InputField wordsToSayLeft;

    public bool isStartScreen = false;
    public bool isFirstLevel = false;


    [Header("otherScripts")]
    public Audio_sampler_Final _audioSampler;
    public resultScreen gameOverUI;
    public topBar _topBar;

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
    public float toatalNumOfObs;
    public float numOfObsCarshed;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!isStartScreen)
            return;

        //actions.Add(keywordRight, MoveCarRight);
        //actions.Add(keywordLeft, MoveCarLeft);
       
        
        //keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        //keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        //keywordRecognizer.Start();

        //genrateObstacleForFirstTime();

        //gameOverUI._gameLog.setGameDataPath();
        //gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;
    }

    public void startGame()
    {
        Time.timeScale = 1;

        keywordLeft = wordsToSayLeft.text;
        keywordRight = wordsToSayRight.text;

        rightText.text = keywordRight;
        leftText.text = keywordLeft;

        actions.Add(keywordRight, MoveCarRight);
        actions.Add(keywordLeft, MoveCarLeft);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();


        genrateObstacleForFirstTime();

        gameOverUI._gameLog.setGameDataPath();
        gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;
        _topBar.gameStarted = true;
    }
    public void pauseGame()
    {
        Time.timeScale = 0;
      

    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        

    }
    public void reselectWords()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs args)
    {
        if (_topBar.gamePaused)
            return;
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());
        actions[args.text].Invoke();
    }

    private void MoveCarRight()
    {
        if (carSprite.transform.position.x > 7)
            return;
        carSprite.transform.Translate(4, 0,0);
        startVarCounting();
    }

    private void MoveCarLeft()
    {
        if (carSprite.transform.position.x < -8)
            return;
        carSprite.transform.Translate(-4, 0, 0);
        startVarCounting();
    }

  
  
    //OBSTACLES
    void genrateObstacles()
    {
        
            int obstIndex = UnityEngine.Random.Range(0, obstacles.Count);
        int obstSpawIndex = UnityEngine.Random.Range(0, obstaclesSpawPoint.Count);
        float spawnTime = UnityEngine.Random.Range(obstSpawnSpeedMin, obstSpawnSpeedMax);

        obstacle newObs = Instantiate(obstacles[obstIndex], obstaclesSpawPoint[obstSpawIndex].position, Quaternion.identity).GetComponent<obstacle>();
        newObs.finalPostion = finalPosPoint[obstSpawIndex];
        newObs.timeToReachFinalPos = obstTimeToFinal;
        newObs.moveObstacle();
       
        Invoke("genrateObstacles", spawnTime);
    }

    async System.Threading.Tasks.Task genrateObstacleForFirstTime()
    {
        await System.Threading.Tasks.Task.Delay(2000);
        obstacle newObs =  Instantiate(obstacles[0], obstaclesSpawPoint[obstaclesSpawPoint.Count / 2].position, Quaternion.identity).GetComponent<obstacle>();
        newObs.finalPostion = finalPosPoint[obstaclesSpawPoint.Count / 2];
        newObs.timeToReachFinalPos = obstTimeToFinal;
        newObs.moveObstacle();

        float spawnTime = UnityEngine.Random.Range(obstSpawnSpeedMin, obstSpawnSpeedMax);
        Invoke("genrateObstacles", spawnTime);
    }

    #region TIMER
    //RESULTS

    private void Update()
    {
        elpasedTimeCounter();
        countTimeAvg();
    }
    void elpasedTimeCounter()
    {
        if (countCummulative == true)
            currentCommulativeDuration += Time.deltaTime;


        if (countDurationOfAttmpts == true)
            durationOfSuccessFullAttempts += Time.deltaTime;

    }

    //timer
    float timeAvgConter = 1;
    bool shouldCountTimeAvg = false;

    void countTimeAvg()
    {
        if (shouldCountTimeAvg)
        {
            timeAvgConter -= Time.deltaTime;
            averageCounter();
        }

        else
        {
            timeAvgConter = 1;
        }
    }

    void averageCounter()
    {
        averagePitch.Add(_audioSampler.pitchVal);
        averageLoudness.Add(_audioSampler.dbVal);

        if (timeAvgConter <= 0)
            stopVarCounting();

            
    }

    void startVarCounting()
    {
        shouldCountTimeAvg = true;
        countCummulative = true;
        countDurationOfAttmpts = true;
    }

    void stopVarCounting()
    {
        shouldCountTimeAvg = false;
        countCummulative = false;
        countDurationOfAttmpts = false;
    }
    #endregion

    public void onColistion()
    {
        numOfObsCarshed++;
        carSprite.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(blink_coroutine());
    }

   
    IEnumerator blink_coroutine()
    {
       
        for (int i = 0; i < numOfBlinks; i++)
        {
            
            carSprite.gameObject.SetActive(false);
            yield return new WaitForSeconds(secBetweenBlinks);
            carSprite.gameObject.SetActive(true);
            yield return new WaitForSeconds(secBetweenBlinks);

        }
        yield return new WaitForSeconds(secBetweenBlinks);
        carSprite.GetComponent<BoxCollider2D>().enabled = true;
    }
    public GameObject resetbutton;
    #region GAMEOVER
    //GAME OVER
    public void GameOver()
    {
        Time.timeScale = 0;

        resetbutton.SetActive(false);
        _topBar.gameObject.SetActive(false);
        _audioSampler.saveRecording();
        gameOverUI.gameObject.SetActive(true);
        gameOverUI.gameId = gName;

        gameOverUI._grade = calculateGrade().ToString();

        gameOverUI._ElapsedTime = fetchElapsedTime();
        gameOverUI._cummulativeDurationOfSounds = fetchCommulativeDuration();
        gameOverUI._gameBase = gamebase;




        gameOverUI._NumOfTrials = _topBar.GetTrailCont.ToString();
        gameOverUI._loundNessTarget = targetLoudness.ToString();

        gameOverUI._meanPitch = fetchAveragePitch();
        gameOverUI._meanLoudness = fetchAverageLoudness();

        gameOverUI._StdDevPitch = fetchStadDevPitch();
        gameOverUI._StdDevLoudness = fetchStadDevLoudnes();

        if (averagePitch.Count > 0 && averageLoudness.Count > 0)
        {
            gameOverUI._RangePitchLow = ((int)averagePitch.Min()).ToString();
            gameOverUI._RangePitchHigh = ((int)averagePitch.Max()).ToString();
            gameOverUI._RangeLoudnessLow = ((int)averageLoudness.Min()).ToString();
            gameOverUI._RangeLoudnessHigh = ((int)averageLoudness.Max()).ToString();

        }

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
    float calculateGrade()
    {
        float grade = 0;
        grade = ((toatalNumOfObs - numOfObsCarshed) / toatalNumOfObs)*10;
        return grade;
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
    #endregion 
}
