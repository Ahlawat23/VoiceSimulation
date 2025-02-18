using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class boyStoneManager : MonoBehaviour
{
    public static boyStoneManager instance;
    public string gName;
    public string gameBase;

    public List<string> testList = new List<string>();

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    [Header("KeyWords")]
    public List<string> keywords = new List<string>();
    public Text keywordText;

    [Header("Transoform and prefabs")]
    public List<apple> apples = new List<apple>();
    public List<Transform> finalPos = new List<Transform>();

    [Header("Game variables")]
    public int postionIndex = 0;
    public float timeToDrop = 2f;
    public float targetFreq = 150;
    public float targetLoudness = 1;
    public bool isSecondlevel = false;



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
    public int numOfGoodAttmpts;
    public int numOfBadAttmpts;

    void Start()
    {
        instance = this;
        keywordText.text = keywords[0];

        testList = keywords.Distinct().ToList();

        for (int i = 0; i < testList.Count; i++)
        {
            actions.Add(testList[i], dropTheApple);
        }


        if (!isSecondlevel)
        {
            keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
            keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
            keywordRecognizer.Start();
        }



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

    void dropTheApple()
    {

        StartCoroutine(appleDropper_coroutine());
        StartCoroutine(endMovingApple_coroutine());
    }

    public Animator _handAnimator;

    IEnumerator appleDropper_coroutine()
    {
        _handAnimator.SetTrigger("handAnime");
        
        yield return new WaitForSeconds(1f);
        translateAppleTo(postionIndex);
        yield return new WaitForSeconds(timeToDrop);

    }
    void translateAppleTo(int index)
    {
        apple _apple = apples[index];
        _apple.finalPos = finalPos[index];
        _apple.timeToDrop = timeToDrop;
        _apple.dropApple();

    }

    IEnumerator endMovingApple_coroutine()
    {
        yield return new WaitForSeconds(timeToDrop + 1f);

        numOfGoodAttmpts++;

        postionIndex++;
        stopVarCounting();

        if (postionIndex == apples.Count)
           GameOver();


    }

    public GameObject truck;
    public float xTruck = 13;
    public string nextLevelName;
    IEnumerator nextLevel()
    {
        truck.transform.LeanMoveLocalX(xTruck, 1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(nextLevelName);
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
            numOfBadAttmpts++;
            StartCoroutine(timeUnsuccessfullAttempt_coroutine());
        }
    }

    IEnumerator timeUnsuccessfullAttempt_coroutine()
    {
        shouldCountUnScuccesfull = false;
        yield return new WaitForSeconds(1);
        shouldCountUnScuccesfull = true;
    }

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
        float grade = 0;
        if (_topBar.time <= 75)
        {
            grade = 10;
        }
        else
        {
            float sec = _topBar.time - 75;
            grade = 10 - (sec / 15);
            if (grade < 0)
                grade = 0;
        }


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
