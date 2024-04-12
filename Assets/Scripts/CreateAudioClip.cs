using UnityEngine;
using System.Collections;

public class CreateAudioClip 
{
    
    static public int samplerate = 44100;

    static public AudioClip clip;

    static private Wave wave;

    public CreateAudioClip()
    {
        wave = Camera.main.GetComponent<Wave>();
    }

    public static void CreateClip()
    {
        wave = Camera.main.GetComponent<Wave>(); 
        clip = AudioClip.Create("clip", (int)(samplerate * wave.exportLength), 1, samplerate, false, wave.OnAudioRead);
    }
}