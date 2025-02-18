using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class balloonNumberManager : MonoBehaviour
{

    public static balloonNumberManager instance;
    public string gName;
    public string gameBase;

    public List<string> testList = new List<string>();

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    [Header("KeyWords")]
    public List<string> keywords = new List<string>();
    public Text keywordText;

    [Header("OtherScripts")]
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
    public float totaleNumOfAttmpts;

    public void Awake()
    {
        instance = this;


    }

    private void Start()
    {
        gameOverUI._gameLog.setGameDataPath();
        //gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;
        StartCoroutine(getBalloon(positionIndex));
        StartGame();
    }

    public void StartGame()
    {
        

        

        testList = keywords.Distinct().ToList();

        for (int i = 0; i < testList.Count; i++)
        {
            actions.Add(testList[i], balloonMove);
        }


        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
        Time.timeScale = 1;

    }

    int recgWord;

    private void RecognizedSpeech(PhraseRecognizedEventArgs args)
    {
        if (_topBar.gamePaused)
            return;
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());
        recgWord = matchString(args.text);
        actions[args.text].Invoke();
    }

    int matchString(string str)
    {
        for (int i = 0; i < keywords.Count; i++)
        {
            
            if (keywords[i] == str)
            {
                return i;
            }
        }

        return -1;
    }



    public List<GameObject> balloonList = new List<GameObject>();
    public Transform middle;

    int positionIndex = 0;
    void balloonMove()
    {
        

        if(positionIndex== recgWord + 1)
        StartCoroutine(getBalloon(positionIndex));


    }

    IEnumerator getBalloon(int index)
    {
        if(index == 0)
        {
            balloonList[0].transform.LeanMove(middle.position, 1f);
            yield return new WaitForSeconds(1);
            positionIndex++;
        }
        else
        {
            balloonList[index - 1].SetActive(false);
            if(index == 5)
            {
                GameOver();
            }
            else
            {
                balloonList[index].transform.LeanMove(middle.position, 1f);

            }
            
            yield return new WaitForSeconds(1);
            positionIndex++;
        }
       
        
    }

    //timer
    #region timeer
    private void Update()
    {
        elpasedTimeCounter();
        averageCounter();
    }


    void elpasedTimeCounter()
    {
        if (countCummulative == true)
            currentCommulativeDuration += Time.deltaTime;


        if (countDurationOfAttmpts == true)
            durationOfSuccessFullAttempts += Time.deltaTime;
    }

    void averageCounter()
    {
        if (_audioSampler.pitchVal > 200)
        {
            averagePitch.Add(_audioSampler.pitchVal);
            averageLoudness.Add(_audioSampler.dbVal);
            countCummulative = true;
            countDurationOfAttmpts = true;
        }
        else
        {
            countCummulative = false;
            countDurationOfAttmpts = false;
        }



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

        gameOverUI._gameBase = gameBase;
        gameOverUI._grade = calculateGrade().ToString();
        gameOverUI._NumOfTrials = _topBar.GetTrailCont.ToString();
        gameOverUI._loundNessTarget = "N/a";

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
        if (_topBar.time <= 60)
        {
            grade = 10;
        }
        else
        {
            float sec = _topBar.time - 60;
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
