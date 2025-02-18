using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recordAudio : MonoBehaviour
{
    AudioClip myAudioClip;
    AudioSource audioSource;
    string audioClipPath;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioClipPath = Application.dataPath + "/";
    }
    public void startRecording()
    {
        myAudioClip = Microphone.Start(null, false, 10, 44100);
    }

    public void SaveRecording()
    {
        
        SavWav.Save("myfile", myAudioClip, audioClipPath);
    }

    public void PlayItBack()
    {
        StartCoroutine(StartAudio());
    }

    IEnumerator StartAudio()
    {
        audioSource.clip = myAudioClip;

        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        audioSource.Play();
    }
}
