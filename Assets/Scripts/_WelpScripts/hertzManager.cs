using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class hertzManager : MonoBehaviour
{
    public string gName = "HERTZmeter";
    public float rmsVal;
    public float dbVal;
    public float pitchVal;

    private const int QSamples = 1024;
    public  float RefValue = 0.1f;
    private const float Threshold = 0.02f;

    private float[] _samples;
    private float[] _spectrum;
    private float _fSample;

    public AudioMixerGroup _audioMixerGroup;
    public AudioMixerGroup _playBackMixerGroup;


    public float[] _freqBand = new float[8];

    AudioSource _audioSource;
    string audioClipPathToSave;
    string fileName = "newClip";

    [Header("Bars")]
    public Image Hz200_650;
    public Image Hz700_2800;
    public Image Hz3000_6500;
    public Image Hz90_600;
    public Image Db45_90;

    [Header("buttons")]
    public Button Button200_650;
    public Button Button700_2800;
    public Button Button3000_6500;
    public Button Button90_600;
    public Button Buttondb45_90;
    bool bool200_650 = true;
    bool bool700_2800 = true;
    bool bool3000_6500 = true;
    bool bool90_600 = true;
    bool booldb45_90 = true;

    private void Start()
    {

        // Create audio source
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.bypassEffects =
        audioSource.bypassListenerEffects = false;
        // Start microphone
        audioSource.clip = Microphone.Start(null, true, 10, AudioSettings.outputSampleRate);
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.Play();

        _audioSource = GetComponent<AudioSource>();
        audioClipPathToSave = Application.dataPath + "/";


        _samples = new float[QSamples];
        _spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;
    }




    void Update()
    {
        AnalyzeSound();
    }




    void AnalyzeSound()
    {
        //22050/512   43hz per sample
        /*
         * 
         * 0. 2 = 86hz
         * 1. 4 = 87-258
         * 2. 8 = 159 - 602
         * 3. 16 = 602  - 1290
         * 4. 32  = 1291-2666
         * 5. 64 =  2667 - 5418
         * 6. 128 = 5419 - 10922
         * 7. 256 = 10923- 21930
         * 510
         * 
         */

        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
        MakeFrequencyBand();
        CalculateLoudness();


        set200_650();
        set700_2800();
        set3000_6500();
        set90_600();
        setDb45_90();

    }


    void MakeFrequencyBand()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }

            average /= count;
            _freqBand[i] = average * 10;
        }

    }
    void CalculateLoudness()
    {
        GetComponent<AudioSource>().GetOutputData(_samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples
        }
        rmsVal = Mathf.Sqrt(sum / QSamples); // rms = square root of average
        dbVal = 20 * Mathf.Log10(rmsVal / RefValue); // calculate dB
        if (dbVal < -160) dbVal = -160; // clamp it to -160dB min
                                        // get sound spectrum
    }

    void set200_650()
    {
        if (!bool200_650)
            return;

        Hz200_650.fillAmount = _freqBand[2];
    }

    void set700_2800()
    {
        if (!bool700_2800)
            return;

        float val = 0;
        if (_freqBand[3] < 1)
        {
            val = _freqBand[3] / 3;
        }
        else
        {
            val =  0.25f + 2 *(_freqBand[4]/3) ;
        }

       
        Hz700_2800.fillAmount = val;
    }

    void set3000_6500()
    {
        if (!bool3000_6500)
            return;

        Hz3000_6500.fillAmount = _freqBand[5];
    }

    void set90_600()
    {
        if (!bool90_600)
            return;

        float val = 0;
        if (_freqBand[1] < 0.2)
        {
            val = _freqBand[1];
        }
        else
        {
            val = 0.2f + 2 * (_freqBand[2] / 3);
        }


        Hz90_600.fillAmount = val;
    }

    void setDb45_90()
    {
        if (!booldb45_90)
            return;

        float val = convertBetweenTwoScales(dbVal, 45, 90, 0, 1);

        Db45_90.fillAmount = val;

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


    public void Toggle200_650()
    {
        bool200_650 = !bool200_650;

        if (bool200_650)
            Button200_650.image.color = Color.green;
        else
            Button200_650.image.color = Color.red;
    }
    public void Toggle700_2800()
    {
        bool700_2800 = !bool700_2800;

        if (bool700_2800)
            Button700_2800.image.color = Color.green;
        else
            Button700_2800.image.color = Color.red;
    }
    public void Toggle3000_6500()
    {
        bool3000_6500 = !bool3000_6500;

        if (bool3000_6500)
            Button3000_6500.image.color = Color.green;
        else
            Button3000_6500.image.color = Color.red;
    }
    public void Toggle90_600()
    {
        bool90_600 = !bool90_600;

        if (bool90_600)
            Button90_600.image.color = Color.green;
        else
            Button90_600.image.color = Color.red;
    }
    public void ToggleDb45_90()
    {
        booldb45_90 = !booldb45_90;

        if (booldb45_90)
            Buttondb45_90.image.color = Color.green;
        else
            Buttondb45_90.image.color = Color.red;
    }



    public void saveRecording()
    {
        SavWav.Save(fileName, _audioSource.clip, audioClipPathToSave);
        myAudioClip = _audioSource.clip;
    }

    AudioClip myAudioClip;
    public void PlayItBack()
    {
        StartCoroutine(loadAduio());

    }

    IEnumerator loadAduio()
    {
        WWW request = getAudioFromFile(audioClipPathToSave, fileName);
        yield return request;

        myAudioClip = request.GetAudioClip();
        myAudioClip.name = fileName;
        StartCoroutine(StartAudio());
    }

    WWW getAudioFromFile(string path, string fileName)
    {
        string audioToLoad = string.Format(path + "{0}" + ".wav", fileName);
        Debug.Log(audioToLoad);
        WWW request = new WWW(audioToLoad);
        return request;
    }

    IEnumerator StartAudio()
    {
        Debug.Log("funtion called");
        //_audioSource.clip = myAudioClip;
        _audioSource.Stop();
        _audioSource.outputAudioMixerGroup = _playBackMixerGroup;
        _audioSource.clip = myAudioClip;

        _audioSource.Play();

        yield return new WaitForSeconds(_audioSource.clip.length);

        _audioSource.Stop();
    }
}
