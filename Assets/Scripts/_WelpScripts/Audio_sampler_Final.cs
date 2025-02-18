using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class Audio_sampler_Final : MonoBehaviour
{
    [Header("baseVals")]
    public float loudness;
    public float dbVal;
    public float pitchVal;

    private int bufferSize = 64;
    private float[] data;

    private const int QSamples = 64;
    public  float RefValue = 0.00005f;
    
    public float[] _spectrurmData;

    [Header("speed adjusters")]
    public float lowPassFactor = 0.98f; // Adjust this value to control the smoothing effect
    public float prevDBVal = 0f;


    [Header("audio mixer groups")]
    public AudioMixerGroup _audioMixerGroup;
    public AudioMixerGroup _playBackMixerGroup;

    [Header("saving audio clips")]
    public AudioSource _audioSource;
    public AudioSource _playBackAudioSource;

    public string audioClipPathToSave;
    public string fileName = "newClip";
    public List<AudioClip> tempAudioFiles = new List<AudioClip>();
    AudioClip myAudioClip;

    [Header("Time Vars")]
    public float secStart;
    public float secEnd;
    public List<float> secPauses = new List<float>();
    public List<float> secPlays = new List<float>();

    

    private void Awake()
    {
       
        _audioSource.loop = true;
        _audioSource.bypassEffects =
        _audioSource.bypassListenerEffects = false;

        
        // Start microphone
        _audioSource.clip = Microphone.Start(null, true, 300, 44100);

        audioClipPathToSave = Application.dataPath + "/";
        _audioSource.outputAudioMixerGroup = _audioMixerGroup;
        _audioSource.Play();

        
        _spectrurmData = new float[QSamples];
        data = new float[bufferSize];

    }

    void Update()
    {
        anaylyzeSound();
        //Debug.Log("RMS: " + rmsVal.ToString("F2"));
        //Debug.Log(dbVal.ToString("F1") + " dB");
        //Debug.Log(pitchVal.ToString("F0") + " Hz");
    }



    public float nextDbval;
    void anaylyzeSound()
    {

        _audioSource.GetOutputData(data, 0);
        float sum = 0;
        for (int i = 0; i < bufferSize; i++)
        {
            sum += Mathf.Abs(data[i]);

            
        }

        loudness = sum / bufferSize;
        loudness = lowPassFactor * prevDBVal + loudness * (1 - lowPassFactor);
        prevDBVal = loudness;
        dbVal = 20 * Mathf.Log10(loudness / RefValue);



        float[] fft = new float[bufferSize];
        _audioSource.GetSpectrumData(fft, 0, FFTWindow.Rectangular);
        float maxV = 0;
        int maxN = 0;
        for (int i = 0; i < bufferSize; i++)
        {
            if (fft[i] <= maxV || fft[i] <= 0.01)
                continue;

            maxV = fft[i];
            maxN = i;
        }
        pitchVal = maxN * 44100.0f / bufferSize;
    }

    //save recording
    public void saveRecording()
    {

        //setAudioClipPath(); 
        fileName = fileName + PermanentData.getGameDataCounter();
        if (secStart != 0 && secEnd != 0)
        {
            calculateAllTheClip();
        }


    }

    void calculateAllTheClip()
    {
        Debug.Log("Pause count " + secPauses.Count + " Sec plays count : " + secPlays.Count);


        if (secPauses.Count == 0)
        {
            Debug.Log("entered 0");
            AudioClip finalClip = snipAudioClip(_audioSource.clip, secStart, secEnd);
            SavWav.Save(fileName, finalClip, audioClipPathToSave);
            myAudioClip = finalClip;
            return;
        }
        if (secPauses.Count == secPlays.Count)
        {
            Debug.Log("entered simmallar");
            List<AudioClip> trimedClips = new List<AudioClip>();
            for (int i = 0; i < secPauses.Count; i++)
            {
                if (i == 0)
                {
                    if (secPauses[0] - secStart >= 1)
                        trimedClips.Add(snipAudioClip(_audioSource.clip, secStart, secPauses[0]));
                }
                    

                if (secPauses.Count != i + 1)
                {
                    if (secPauses[i + 1] - secPlays[i] >= 1)
                        trimedClips.Add(snipAudioClip(_audioSource.clip, secPlays[i], secPauses[i + 1]));
                }

                else
                {
                    if (secEnd - secPlays[i] >= 1)
                    trimedClips.Add(snipAudioClip(_audioSource.clip, secPlays[i], secEnd));
                }
                    

            }

            AudioClip finalClip = Combine(trimedClips.ToArray());

            SavWav.Save(fileName, finalClip, audioClipPathToSave);
            myAudioClip = finalClip;
            return;
        }
        else
        {
            Debug.Log("entered last");
            List<AudioClip> trimedClips = new List<AudioClip>();
            for (int i = 0; i < secPauses.Count; i++)
            {
                if (i == 0)
                {
                    if (secPauses[0] - secStart >= 1)
                        trimedClips.Add(snipAudioClip(_audioSource.clip, secStart, secPauses[0]));
                }

                if (secPauses.Count != i + 1)
                {
                    if (secPauses[i + 1] - secPlays[i] >= 1)
                        trimedClips.Add(snipAudioClip(_audioSource.clip, secPlays[i], secPauses[i + 1]));
                }
            }

            AudioClip finalClip = Combine(trimedClips.ToArray());
            SavWav.Save(fileName, finalClip, audioClipPathToSave);
            myAudioClip = finalClip;
            return;
        }
    }

    AudioClip snipAudioClip(AudioClip capturedClip, float startTime, float endTime)
    {
        

        Debug.Log(startTime + " start time and end time is : " + endTime);
        int startSample = (int)(startTime * capturedClip.frequency);

        int endSample = (int)(endTime * capturedClip.frequency);
        if (endSample > capturedClip.length * capturedClip.frequency)
            endSample = (int)capturedClip.length * capturedClip.frequency;

        int lengthSamples = endSample - startSample;

        // Create a new audio clip with the desired length
        AudioClip newClip = AudioClip.Create("SnippedClip", lengthSamples, capturedClip.channels, capturedClip.frequency, false);

        // Copy the desired portion of the original audio data to the new clip
        float[] originalData = new float[capturedClip.samples * capturedClip.channels];
        capturedClip.GetData(originalData, 0);

        float[] newData = new float[lengthSamples * capturedClip.channels];
        for (int i = 0; i < newData.Length; i++)
        {
            newData[i] = originalData[i + startSample * capturedClip.channels];
        }
        newClip.SetData(newData, 0);

        return newClip;
    }
    public void setAudioClipPath()
    {
        audioClipPathToSave = Application.dataPath;
        string newGameData = null;
        int pos = audioClipPathToSave.IndexOf(PermanentData.ALL_SIMULATIONS_PATH);
        if (pos >= 0)
        {
            // String after founder  
            newGameData = audioClipPathToSave.Remove(pos);


        }
        audioClipPathToSave = newGameData + PermanentData.ALL_MENUFILEPATH_FORSIM;
    }

    public AudioClip Combine(params AudioClip[] clips)
    {
        if (clips.Length < 2)
            return _audioSource.clip;
        int totalSamples = 0;
        int totalChannels = 0;
        int frequency = 0;

        foreach (AudioClip clip in clips)
        {
            
            totalSamples += clip.samples;
            totalChannels = Math.Max(totalChannels, clip.channels);
            frequency = clip.frequency;
        }

        AudioClip result = AudioClip.Create("Combined Clip", totalSamples, totalChannels, frequency, false);

        float[] combinedData = new float[totalSamples * totalChannels];

        // Copy the data from each clip into the combined array in the correct order
        int sampleOffset = 0;
        foreach (AudioClip clip in clips)
        {
            float[] data = new float[clip.samples * clip.channels];
            clip.GetData(data, 0);
            System.Array.Copy(data, 0, combinedData, sampleOffset, data.Length);
            sampleOffset += data.Length;
        }

        // Set the combined data back into the combined clip
        result.SetData(combinedData, 0);

        return result;
    }


    //play recording
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
        //Debug.Log("funtion called");
        //_audioSource.clip = myAudioClip;
        //_audioSource.Stop();
        _playBackAudioSource.outputAudioMixerGroup = _playBackMixerGroup;
        _playBackAudioSource.clip = myAudioClip;

        _playBackAudioSource.Play();

        yield return new WaitForSeconds(_audioSource.clip.length);

        _playBackAudioSource.Stop();
    }

    

    


    #region earlier analyzeSound

    //private void Awake()
    //{



    //    _samples = new float[QSamples];
    //    _spectrum = new float[QSamples];
    //    _fSample = AudioSettings.outputSampleRate;


    //}
    //private const float Threshold = 0.002f;

    //float[] _samples;
    //private float[] _spectrum;
    //private float _fSample;

    //int i;
    //float Sum = 0;
    //float maxV = 0;

    //void AnalyzeSound()
    //{
    //    _audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
    //    _audioSource.GetOutputData(_samples, 0); // fill array with samples

    //     Sum = 0;
    //     maxV = 0;
    //    var maxN = 0;
    //    for (i = 0; i < QSamples; i++)
    //    {
    //        Sum += _samples[i] * _samples[i]; // sum squared samples
    //        if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold))
    //            continue;
    //        //Debug.Log("entered the for");
    //        maxV = _spectrum[i];
    //        maxN = i; // maxN is the index of max
    //    }
    //    rmsVal = Mathf.Sqrt(Sum / QSamples); // rms = square root of average
    //    dbVal = 20 * Mathf.Log10(rmsVal / RefValue); // calculate dB
    //    if (dbVal < -160) dbVal = -160; // clamp it to -160dB min
    //                                    // get sound spectrum

    //    float freqN = maxN; // pass the index to a float variable
    //    if (maxN > 0 && maxN < QSamples - 1)
    //    { 
    //        var dL = _spectrum[maxN - 1] / _spectrum[maxN];
    //        var dR = _spectrum[maxN + 1] / _spectrum[maxN];
    //        freqN += 0.5f * (dR * dR - dL * dL);
    //    }
    //    pitchVal = freqN * /*AudioSettings.outputSampleRate(_fSample / 2) / QSamples; // convert index to frequency



    //}


    //audio clip combiner

    //public void colletctAudio()
    //{
    //    stopRecording = false;
    //    StartCoroutine(collectAudio_coroutine());
    //}
    //IEnumerator collectAudio_coroutine()
    //{
    //    if (!stopRecording)
    //        yield return new WaitForSeconds(5f);

    //    processAudioAndAddTotempList();
    //    if (!stopRecording)
    //        StartCoroutine(collectAudio_coroutine());

    //}


    //void processAudioAndAddTotempList()
    //{

    //    AudioClip sourceClip = _audioSource.clip;



    //    AudioClip savedClip = AudioClip.Create("Combined Clip", sourceClip.samples, sourceClip.channels, sourceClip.frequency, false);

    //    float[] audioData = new float[sourceClip.samples * sourceClip.channels];
    //    sourceClip.GetData(audioData, 0);

    //    ////Determine the number of samples in the last second of the audio clip
    //    //int samplesPerSecond = savedClip.frequency;
    //    //int numSamples = samplesPerSecond * savedClip.channels;

    //    //// Create a new array to hold the modified audio data
    //    //float[] modifiedData = new float[audioData.Length];

    //    //// Copy the last second of the audio data to the beginning of the new array
    //    //Array.Copy(audioData, audioData.Length - numSamples, modifiedData, 0, numSamples);

    //    //// Copy the rest of the audio data to the new array
    //    //Array.Copy(audioData, 0, modifiedData, numSamples, audioData.Length - numSamples);

    //    //// Create a new AudioClip with the modified data
    //    //AudioClip modifiedClip = AudioClip.Create("Modified Clip", modifiedData.Length, savedClip.channels, savedClip.frequency, false);
    //    //modifiedClip.SetData(modifiedData, 0);




    //    savedClip.SetData(audioData, 0);
    //    tempAudioFiles.Add(savedClip);
    //    Debug.Log("saved file");

    //    fileName = fileName + PermanentData.getGameDataCounter();
    //    //SavWav.Save(fileName, savedClip, audioClipPathToSave);



    //}

    #endregion
}
