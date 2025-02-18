using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class rabbitManager : MonoBehaviour
{
    public static rabbitManager instance;
    public string gName;
    public string gamebase;

    public List<string> testList = new List<string>();

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    [Header("KeyWords")]
    public List<string> keywords = new List<string>();
    public Text keywordText;

    [Header("Transoform and prefabs")]
    public GameObject rabbit;
    public List<Transform> firstPoints = new List<Transform>();
    public List<Transform> finalPoints = new List<Transform>();

    [Header("Game variables")]
    public int postionIndex = 0;
    public float timeToReachPointCar = 2f;
    public float targetFreq = 150;
    public float targetLoudness = 1;
    public List<float> timestamps;

    [Header("startSCreen")]
    public InputField[] wordsToSay;

    [Header("otherScripts")]
    public Audio_sampler_Final _audioSampler;
    public resultScreen gameOverUI;
    public topBar _topBar;
    public Animator _rabbitAnimator;
    public blinkWord _blinkWord;

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
    public float numOfGoodAttmpts;
    public float tottalAttmpts;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        //keywordText.text = keywords[0];

        //testList = keywords.Distinct().ToList();

        //for (int i = 0; i < testList.Count; i++)
        //{
        //    actions.Add(testList[i], moveRabbitFurther);
        //}



        //keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        //keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        //keywordRecognizer.Start();

        //gameOverUI._gameLog.setGameDataPath();
        //gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;


    }

    public void startGame()
    {
        keywords.Clear();
        for (int i = 0; i < wordsToSay.Length; i++)
        {
            keywords.Add(wordsToSay[i].text);
            _blinkWord.keywordText[i].text = wordsToSay[i].text;
        }

       

        keywordText.text = keywords[0];

        testList = keywords.Distinct().ToList();

        for (int i = 0; i < testList.Count; i++)
        {
            actions.Add(testList[i], moveRabbitFurther);
        }



        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        gameOverUI._gameLog.setGameDataPath();
        gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;

        Time.timeScale = 1;
        _topBar.gameStarted = true;
        _blinkWord.switchText(0);
    }

    public void reselectWords()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    string recgWord;
    private void RecognizedSpeech(PhraseRecognizedEventArgs args)
    {
        if (_topBar.gamePaused)
            return;
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());
        recgWord = args.text;
        actions[args.text].Invoke();
    }

    int indexOfRecognisedWord()
    {
        int index = 0;
        for (int i = postionIndex; i < wordsToSay.Length; i++)
        {

            if (wordsToSay[i].text == recgWord)
            {
                index = i;
            }
        }
        return index;
    }

    void moveRabbitFurther()
    {
        if (indexOfRecognisedWord() != postionIndex)
            return;
        Vector3 firstPos = firstPoints[postionIndex].position;
        
        Quaternion rotation = Quaternion.Euler(new Vector3(0, firstPoints[postionIndex].localRotation.eulerAngles.y + 180f, 0));
        rabbit.transform.SetPositionAndRotation(firstPos, rotation);

        translateRabbitTo(finalPoints[postionIndex]);

        postionIndex++;
        startVarCounting();
        _rabbitAnimator.SetBool("run", true);

        StartCoroutine(endMovingRabbit_coroutine());
    }

    void translateRabbitTo(Transform to)
    {
        rabbit.transform.LeanMove(to.position, timeToReachPointCar);
    }

    IEnumerator endMovingRabbit_coroutine()
    {
        yield return new WaitForSeconds(timeToReachPointCar);
        timestamps.Add(_topBar.time);

        numOfGoodAttmpts++;

        _rabbitAnimator.SetBool("run", false);
        stopVarCounting();

        if (postionIndex == finalPoints.Count)
            GameOver();
        else
            _blinkWord.switchText(postionIndex);

    }

    //timer

    private void Update()
    {
        elpasedTimeCounter();
        countTimeAvg();
        countUnsuccessfullAttmpts();
    }
    void elpasedTimeCounter()
    {
        if (countCummulative == true)
            currentCommulativeDuration += Time.deltaTime;


        if (countDurationOfAttmpts == true)
            durationOfSuccessFullAttempts += Time.deltaTime;

    }

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


    bool shouldCountUnScuccesfull = true;
    void countUnsuccessfullAttmpts()
    {

        if (_audioSampler.pitchVal > 200 && !shouldCountTimeAvg && shouldCountUnScuccesfull)
        {
            tottalAttmpts++;
            StartCoroutine(timeUnsuccessfullAttempt_coroutine());
        }
    }

    IEnumerator timeUnsuccessfullAttempt_coroutine()
    {
        shouldCountUnScuccesfull = false;
        yield return new WaitForSeconds(1);
        shouldCountUnScuccesfull = true;
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

        gameOverUI._ElapsedTime = fetchElapsedTime();
        gameOverUI._cummulativeDurationOfSounds = fetchCommulativeDuration();

        gameOverUI._gameBase = gamebase;
        gameOverUI._grade = calculateGrade().ToString();
        gameOverUI._NumOfTrials = "1";
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
        float grade = 10;

        for (int i = 0; i < timestamps.Count; i++)
        {



            float cal = (timestamps[i] - (i == 0 ? 0 : timestamps[i - 1])) / 15;
            cal = cal < 1 ? 0 : cal;
            grade = grade - cal;

        }


        return grade < 0 ? 0 : grade;
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
