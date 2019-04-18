using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SinewaveExample : MonoBehaviour {
    [Range(0, 36)]  //Creates a slider in the inspector
    public int frequency1;

    [Range(0, 36)]  //Creates a slider in the inspector
    public int frequency2;

    [Range(0, 8)] public int octaves;

    public float sampleRate = 44100;
    public float waveLengthInSeconds = 2.0f;

    public float noise;

    AudioSource audioSource;
    int timeIndex = 0;

    void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.playOnAwake = true;
        audioSource.spatialBlend = 1; //force 3D sound
        audioSource.Play(); //avoids audiosource from starting to play automatically
    }

    void OnAudioFilterRead(float[] data, int channels) {
        for (int i = 0; i < data.Length; i += channels) {
            data[i] = CreateSine(timeIndex, Mathf.Pow(1.059463f, frequency1) * 261.6f /2, sampleRate);

            if (channels == 2)
                data[i + 1] = CreateSine(timeIndex, Mathf.Pow(1.059463f, frequency2) * 261.6f /2, sampleRate);

            timeIndex++;

            //if timeIndex gets too big, reset it to 0
            if (timeIndex >= (sampleRate * waveLengthInSeconds)) {
                timeIndex = 0;
            }
        }
    }

    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        float total = 0;
        
        for (int i = 0; i < octaves; i++)
        {
            
            total += Mathf.Sin( Mathf.Pow(2,i+1) * Mathf.PI * timeIndex * frequency / sampleRate ) / Mathf.Pow(i,i) + noise; 
        }

        return total;
    }
}

