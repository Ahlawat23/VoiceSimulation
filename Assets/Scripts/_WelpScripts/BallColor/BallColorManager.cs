using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;
using System.Text;
using UnityEngine.UI;
using TMPro;

public class BallColorManager : MonoBehaviour
{
    public static BallColorManager instance;
    public string gName = "BallColorLevel1";
    public string gameBase = "recognition";

    public List<string> testList = new List<string>();

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    [Header("KeyWords")]
    [Tooltip("Order: blue, red, green, orange, yellow")]
    public List<string> keywords = new List<string>();
    public List<TextMeshPro> keywordText = new List<TextMeshPro>();
    public List<InputField> KeywordField = new List<InputField>();

    [Header("Transoform and prefabs")]
    public List<GameObject> balls = new List<GameObject>();
    public List<Transform> bluePoints = new List<Transform>();
    public List<Transform> redPoints = new List<Transform>();
    public List<Transform> greenPoints = new List<Transform>();
    public List<Transform> orangePoints = new List<Transform>();
    public List<Transform> yellowPoints = new List<Transform>();

    public Dictionary<string, List<Transform>> firstPoints = new Dictionary<string, List<Transform>>();
    public Dictionary<string, List<Transform>> finalPoints = new Dictionary<string, List<Transform>>();

    [Header("Game variables")]
    public List<int> postionIndex = new List<int>();
    public List<bool> isKeywordFinished = new List<bool>();

    public float timeToReachNextPoint = 2f;
    public float targetFreq = 150;
    public float targetLoudness = 1;
    public float jumpHeight = 1;

    public List<float> timestamps;




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
    public float numOfGoodAttmpts;
    public float totalAttmpts;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        gameOverUI._gameLog.setGameDataPath();
        //gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;



    }

    public void startGame()
    {
        setAllKeywordsFromText();
        setKeywordText();
        addActions();
        addfinalPoints();
        addFirstPoints();



        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        Time.timeScale = 1;
    }

    void setAllKeywordsFromText()
    {
        for (int i = 0; i < KeywordField.Count; i++)
        {
            keywords[i] = KeywordField[i].text;
        }
    }

    void setKeywordText()
    {
        for (int i = 0; i < keywordText.Count; i++)
        {
            keywordText[i].text = keywords[i];
        }
    }
    void addActions()
    {
        actions.Add(keywords[0], moveBlueBallFurther);
        actions.Add(keywords[1], moveRedBallFurther);
        actions.Add(keywords[2], moveGreenBallFurther);
        actions.Add(keywords[3], moveOrangeBallFurther);
        actions.Add(keywords[4], moveYellowBallFurther);
    }
    void addFirstPoints()
    {
        firstPoints.Add(keywords[0], fetchFirstPoints( bluePoints));
        firstPoints.Add(keywords[1], fetchFirstPoints( redPoints));
        firstPoints.Add(keywords[2], fetchFirstPoints( greenPoints));
        firstPoints.Add(keywords[3], fetchFirstPoints( orangePoints));
        firstPoints.Add(keywords[4], fetchFirstPoints( yellowPoints));
    }
    void addfinalPoints()
    {
        finalPoints.Add(keywords[0], fetchFinalPoints( bluePoints));
        finalPoints.Add(keywords[1], fetchFinalPoints( redPoints));
        finalPoints.Add(keywords[2], fetchFinalPoints( greenPoints));
        finalPoints.Add(keywords[3], fetchFinalPoints( orangePoints));
        finalPoints.Add(keywords[4], fetchFinalPoints( yellowPoints));
    }
    List<Transform> fetchFirstPoints( List<Transform> listOfPoints)
    {
        List<Transform> fistPoints = new List<Transform>();
        for (int i = 0; i < listOfPoints.Count; i++)
        {
            fistPoints.Add(listOfPoints[i]);
        }

        fistPoints.RemoveAt(fistPoints.Count - 1);

        return fistPoints;
    }
    List<Transform> fetchFinalPoints(List<Transform> listOfPoints)
    {
        List<Transform> finalPoints = new List<Transform>();
        for (int i = 0; i < listOfPoints.Count; i++)
        {
            finalPoints.Add(listOfPoints[i]);
        }

        finalPoints.RemoveAt(0);

        return finalPoints;
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());
        actions[args.text].Invoke();
    }

    void moveBlueBallFurther()
    {
        Debug.Log("Blue called");
        moveBallOne(0);
    } 
    void moveRedBallFurther()
    {
        Debug.Log("red called");
        moveBallOne(1);
    }
    void moveGreenBallFurther()
    {
        Debug.Log("green called");
        moveBallOne(2);
    }
    void moveOrangeBallFurther()
    {
        Debug.Log("orange called");
        moveBallOne(3);
    }
    void moveYellowBallFurther()
    {
        Debug.Log("yellow called");
        moveBallOne(4);
    }

    void moveBallOne(int posIndex)
    {
        if (isKeywordFinished[posIndex])
            return;

        string dictionaryString = keywords[posIndex];
        Debug.Log(dictionaryString);
        Transform firstTrans = firstPoints[dictionaryString][postionIndex[posIndex]];
        
        Vector3 firstPos = firstTrans.position;


        Quaternion rotation = Quaternion.Euler(new Vector3(0, firstTrans.localRotation.eulerAngles.y, 0));
        balls[posIndex].transform.SetPositionAndRotation(firstPos, rotation);

        StartCoroutine(translateBallTo(balls[posIndex], firstTrans, finalPoints[dictionaryString][postionIndex[posIndex]]));
        Debug.Log(finalPoints[dictionaryString][postionIndex[posIndex]].name);

        postionIndex[posIndex]++;
        startVarCounting();


        StartCoroutine(endMovingBall_coroutine(posIndex, dictionaryString));
    }

    IEnumerator translateBallTo(GameObject ball, Transform from, Transform to)
    {
        Vector3 midpoint = (from.position + to.position) / 2;
        midpoint = new Vector3(midpoint.x, midpoint.y + jumpHeight, midpoint.z);

        Vector3 preMidPoint = new Vector3(midpoint.x - 0.8f, midpoint.y - 0.2f, midpoint.z);
        Vector3 postMidPoint = new Vector3(midpoint.x + 0.8f, midpoint.y - 0.2f, midpoint.z);

        ball.transform.LeanMove(preMidPoint, timeToReachNextPoint / 4);
        yield return new WaitForSeconds(timeToReachNextPoint / 4);

        ball.transform.LeanMove(midpoint, timeToReachNextPoint / 4);

        yield return new WaitForSeconds(timeToReachNextPoint / 4);

        ball.transform.LeanMove(postMidPoint, timeToReachNextPoint / 4);
        yield return new WaitForSeconds(timeToReachNextPoint / 4);

        ball.transform.LeanMove(to.position, timeToReachNextPoint / 4);
    }

    IEnumerator endMovingBall_coroutine(int indexForPostionArray, string finalDictionayString )
    {
        yield return new WaitForSeconds(timeToReachNextPoint);

        timestamps.Add(_topBar.time);
        numOfGoodAttmpts++;

        Debug.Log("final counts : " + finalPoints[finalDictionayString].Count);
        Debug.Log("postionIndex : " + postionIndex[indexForPostionArray]);
        if (postionIndex[indexForPostionArray] == finalPoints[finalDictionayString].Count)
            sustainFurtherMovement(indexForPostionArray);
        


    }

    void sustainFurtherMovement(int keywordIndex)
    {
        isKeywordFinished[keywordIndex] = true;

        if (areAllKeywordFinished())
        {
            GameOver();
        }
            
    }

    bool areAllKeywordFinished()
    {
        bool gameOver = true;
        for (int i = 0; i < isKeywordFinished.Count; i++)
        {
            if (isKeywordFinished[i] == false)
                gameOver = false;
        }

        return gameOver;
    }

    //timer
    #region Timer
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
            totalAttmpts++;
            StartCoroutine(timeUnsuccessfullAttempt_coroutine());
        }
    }

    IEnumerator timeUnsuccessfullAttempt_coroutine()
    {
        shouldCountUnScuccesfull = false;
        yield return new WaitForSeconds(1);
        shouldCountUnScuccesfull = true;
    }
    #endregion

    #region GAMEOVER
    //GAME OVER
    public void GameOver()
    {
        Time.timeScale = 0;



        _topBar.gameObject.SetActive(false);
        _audioSampler.saveRecording();
        gameOverUI.gameObject.SetActive(true);
        gameOverUI.gameId = gName;

        gameOverUI._ElapsedTime = fetchElapsedTime();
        gameOverUI._cummulativeDurationOfSounds = fetchCommulativeDuration();

        gameOverUI._gameBase = "recognise"; 
        gameOverUI._grade = calculateGrade().ToString();
        gameOverUI._NumOfTrials = _topBar.GetTrailCont.ToString();
        gameOverUI._loundNessTarget = targetLoudness.ToString();

        gameOverUI._meanPitch = fetchAveragePitch();
        gameOverUI._meanLoudness = fetchAverageLoudness();

        gameOverUI._StdDevPitch = fetchStadDevPitch();
        gameOverUI._StdDevLoudness = fetchStadDevLoudnes();

        if (averagePitch.Count > 0 && averageLoudness.Count > 0)
        {
            gameOverUI._RangePitchLow = averagePitch.Min().ToString();
            gameOverUI._RangePitchHigh = averagePitch.Max().ToString();
            gameOverUI._RangeLoudnessLow = averageLoudness.Min().ToString();
            gameOverUI._RangeLoudnessHigh = averageLoudness.Max().ToString();

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
