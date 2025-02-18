using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;
using System.Text;
using UnityEngine.UI;
using TMPro;

public class trainManager : MonoBehaviour
{
    public static trainManager instance;
    public string gName;
    public string gameBase;

    public List<string> testList = new List<string>();

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    [Header("KeyWords")]
    public List<string> keywords = new List<string>();
    public TextMeshPro[] keywordText = new TextMeshPro[2];
    public List<InputField> keywordField = new List<InputField>();

    [Header("GamePrefabsAndObjects")]
    public GameObject gas;
    public GameObject coal;
    public List<Transform> gasTracks = new List<Transform>();
    public List<Transform> coalTracks = new List<Transform>();
    

    [Header("Game Variables")]
    public int postionIndex = 0;


    public float timeToDrop;
    public float targetFreq = 150;
    public float targetLoudness = 1;
    public List<float> timestamps;


    [Header("OtherScripts")]
    public Audio_sampler_Final _audioSampler;
    public resultScreen gameOverUI;
    public topBar _topBar;
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
    public int numOfGoodAttmpts;
    public float totaleNumOfAttmpts;


    public void Awake()
    {
        instance = this;


    }

    private void Start()
    {
        //gameOverUI._gameLog.setGameDataPath();
        //gameOverUI._gameLog.dataPath.text = gameOverUI._gameLog.gameLogPath;
        

    }

    public void StartGame()
    {
        keywords.Add(keywordField[0].text);
        keywords.Add(keywordField[1].text);

        keywordText[0].text = keywords[0];
        keywordText[1].text = keywords[1];

        testList = keywords.Distinct().ToList();

        for (int i = 0; i < testList.Count; i++)
        {
            actions.Add(testList[i], moveAhead);
        }


        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        _topBar.gameStarted = true;
        // _blinkWord.switchText(0);
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

    void moveAhead()
    {



        if (recgWord == 0)
        {
            moveCoal();
        }
        if (recgWord == 1)
        {
            moveGas();
        }
        timestamps.Add(_topBar.time);

        StartCoroutine(waitFroEnd());
    }
    IEnumerator waitFroEnd()
    {
        yield return new WaitForSeconds(0.3f);
        if (GasPostionIndex >= gasTracks.Count && coalPostionIndex >= coalTracks.Count)
            GameOver();
    }

    public int GasPostionIndex;
    void moveGas()
    {
       
        if(GasPostionIndex < gasTracks.Count)
        {
            Debug.Log("moving") ;
            gas.transform.LeanMove(gasTracks[GasPostionIndex].position, 0.3f);

        }
        

        GasPostionIndex++;
    }

    public int coalPostionIndex;
    void moveCoal()
    {
        if (coalPostionIndex < coalTracks.Count)
            coal.transform.LeanMove(coalTracks[coalPostionIndex].position, 0.3f);

        coalPostionIndex++;
    }


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
