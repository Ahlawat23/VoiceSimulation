using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class testLoudness : MonoBehaviour
{
    // Reference to the audio source component
    public AudioSource audioSource;

    // Reference to the bird's Rigidbody2D component
    public Rigidbody2D birdRigidbody;

    // Strength of the upward force applied to the bird
    public float flapForce = 10f;

    // Threshold for detecting a loud voice in decibels
    public float loudnessThresholdDB = -30f;

    // Reference level for converting loudness to decibels
    public float referenceLevel = 0.1f;

    // Flag to check if the game has ended
    private bool gameOver = false;

    // Buffer for the audio data
    private float[] buffer = new float[2048];

    public float lowPassFilterTimeConstant = 0.2f;

    // Index of the current sample in the buffer
    private int bufferIndex = 0;

    public float loudnessDB;

    public float filteredLoudnessDB = 0f;

    void Start()
    {
        // Start recording audio from the default microphone
        audioSource.clip = Microphone.Start(null, true, 10, 44100);
        audioSource.Play();
    }

    void Update()
    {
        // Check if the game has ended
        if (gameOver)
        {
            return;
        }

        // Get the current position of the audio data
        int position = Microphone.GetPosition(null);

        // Check if there is any new audio data available
        if (position > 0)
        {
            // Get the audio data from the current position to the end of the clip
            int samplesToRead = Mathf.Min(1024, position);
            audioSource.clip.GetData(buffer, bufferIndex);

            // Calculate the average loudness of the audio data
            float loudness = 0f;
            for (int i = 0; i < samplesToRead; i++)
            {
                loudness += Mathf.Abs(buffer[bufferIndex + i]);
            }
            loudness /= samplesToRead;

            // Convert the loudness to decibels
             loudnessDB = 20f * Mathf.Log10(loudness / referenceLevel);

            filteredLoudnessDB = Mathf.Lerp(filteredLoudnessDB, loudnessDB, Time.deltaTime / lowPassFilterTimeConstant);

            if (filteredLoudnessDB > loudnessThresholdDB)
            {
                birdRigidbody.velocity = Vector2.zero;
                birdRigidbody.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);


            }

            bufferIndex = (bufferIndex + samplesToRead) % buffer.Length;

        }
    }

    
}
