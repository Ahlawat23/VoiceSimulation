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

public class DuckManager : MonoBehaviour
{
    public static DuckManager instance;
    public string gName = "DuckLevel1";
    public string NextLevelName;
    public string gamebase = "Recogintion";

    public List<string> testList = new List<string>();

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    [Header("KeyWords")]
    public List<string> keywords = new List<string>();
    public Text keywordText;

    [Header("Transoform and prefabs")]
    public GameObject Duck;
    public GameObject endDuck;
    public List<Transform> firstPoints = new List<Transform>();
    public List<Transform> finalPoints = new List<Transform>();

    [Header("Game variables")]
    public int postionIndex = 0;
    public float timeToReachPointCar = 2f;
    public float targetFreq = 150;
    public float targetLoudness = 1;
    public float jumpHeight = 1;
    public List<float> timestamps;

    [Header("startSCreen")]
    public InputField[] wordsToSay;
    public bool isStartScreen = false;
    public bool isFirstLevel = false;
    public bool isSecondLevel = false;


    [Header("otherScripts")]
    public Audio_sampler_Final _audioSampler;
    public resultScreen gameOverUI;
    public topBar _topBar;
    public blinkWord _blinkWord;


    [Header("Result")]
    public GameObject resetbutton;
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
    public float totaleNumOfAttmpts;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (isStartScreen)
            return;
        
        

        testList = keywords.Distinct().ToList();

        for (int i = 0; i < testList.Count; i++)
        {
            actions.Add(testList[i], jumpDuckFurther);
        }



        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Medium);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        gameOverUI._gameLog.setGameDataPath();
        gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;


    }

    public void startGame()
    {
        Time.timeScale = 1;
        keywords.Clear();
        for (int i = 0; i < wordsToSay.Length; i++)
        {
            keywords.Add(wordsToSay[i].text);
            _blinkWord.keywordText[i].text = wordsToSay[i].text;
        }

        if (isFirstLevel)
        {
            keywords.Add(wordsToSay[0].text);
            keywords.Add(wordsToSay[0].text);
            _blinkWord.keywordText[0].text = wordsToSay[0].text;
            _blinkWord.keywordText[1].text = wordsToSay[0].text;
            _blinkWord.keywordText[2].text = wordsToSay[0].text;

        }

       

        testList = keywords.Distinct().ToList();

        for (int i = 0; i < testList.Count; i++)
        {
            actions.Add(testList[i], jumpDuckFurther);
        }



        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();


        gameOverUI._gameLog.setGameDataPath();
        gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;

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
        if (isFirstLevel)
            return postionIndex;
        
        for (int i = postionIndex; i < wordsToSay.Length; i++)
        {
            
            if (wordsToSay[i].text == recgWord)
            {
                index = i;
            }
        }
        return index;
    }

    void jumpDuckFurther()
    {
        if (indexOfRecognisedWord() != postionIndex)
            return;
        Vector3 firstPos = firstPoints[postionIndex].position;

        Quaternion rotation = Quaternion.Euler(new Vector3(0, firstPoints[postionIndex].localRotation.eulerAngles.y + 180f, 0));
        Duck.transform.SetPositionAndRotation(firstPos, rotation);

        StartCoroutine(translateDuckTo(firstPoints[postionIndex], finalPoints[postionIndex]));

        postionIndex++;
        startVarCounting();
       

        StartCoroutine(endMovingRabbit_coroutine());
    }

    IEnumerator translateDuckTo(Transform from, Transform to)
    {
        Vector3 midpoint = (from.position + to.position) / 2;
        midpoint = new Vector3(midpoint.x, midpoint.y + jumpHeight, midpoint.z);

        Duck.transform.LeanMove(midpoint, timeToReachPointCar / 2);

        yield return new WaitForSeconds(timeToReachPointCar / 2);

        Duck.transform.LeanMove(to.position, timeToReachPointCar / 2);
    }

    IEnumerator endMovingRabbit_coroutine()
    {
        yield return new WaitForSeconds(timeToReachPointCar);

        timestamps.Add(_topBar.time);
        numOfGoodAttmpts++;

       
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
    bool shouldCountTimeAvg = true;

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

        if (_audioSampler.pitchVal > 200 && shouldCountUnScuccesfull)
        {
            Debug.Log("entered");
            totaleNumOfAttmpts++;
            shouldCountUnScuccesfull = false;
            StartCoroutine(timeUnsuccessfullAttempt_coroutine());
        }
    }

    IEnumerator timeUnsuccessfullAttempt_coroutine()
    {
       
            
            yield return new WaitForSeconds(1);
            shouldCountUnScuccesfull = true;
     
          
    }

    #region GAMEOVER
    //GAME OVER
    public void GameOver()
    {
        StartCoroutine(playDuckEatAnime_Coroutine());
       

    }

    IEnumerator playDuckEatAnime_Coroutine()
    { 
        Duck.SetActive(false);
        endDuck.SetActive(true);
        yield return new WaitForSeconds(1);
        theRestGameOver();
    }

    void theRestGameOver()
    {
        Time.timeScale = 0;

        resetbutton.SetActive(false);
        showNextLevel();
        _topBar.gameObject.SetActive(false);
        _audioSampler.saveRecording();
        gameOverUI.gameObject.SetActive(true);
        gameOverUI.gameId = gName;

        gameOverUI._gameBase = gamebase;
        gameOverUI._ElapsedTime = fetchElapsedTime();
        gameOverUI._cummulativeDurationOfSounds = fetchCommulativeDuration();

        gameOverUI._grade = calculateGrad().ToString();
        gameOverUI._NumOfTrials = _topBar.GetTrailCont.ToString(); ;
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

    public Image nextLevelButton;
    public Text nextLevleButtonText;
    void showNextLevel()
    {
        if (gName == "")
        {
            nextLevleButtonText.text = "print";
            nextLevelButton.color = Color.white;
            return;
        }

    }

    public void NextLevel()
    {
        if(!(NextLevelName == ""))
        SceneManager.LoadScene(NextLevelName);
    }
     
    float calculateGrad()
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
