using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using TMPro;

public class Wave : MonoBehaviour
{
    
    public List<float> soundWave = new();
    public bool isPlaying = false;

    public double exportLength;
    private double startTime;

    [SerializeField]
    private Play play;
    [SerializeField]
    private TextMeshProUGUI fileData;

    public Slider volumeSlider;
    public TMP_InputField frequencyField;
    public Toggle exportToggle;


    private const int wav_frequency = 44100;
    private const int sampling_frequency = 48000;
    private double increment;
    private double time;

    private double GetFrequency()
    {
        double frequency;
        if (!double.TryParse(frequencyField.text, out frequency) || frequency == 0)
        {
            return 0;
        }
        return Math.Abs(frequency);
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (soundWave.Count == 0) { return; }
        var frequency = GetFrequency();
        if (isPlaying)
        {
            increment = frequency * 2 * System.Math.PI / sampling_frequency;
            for (var i = 0; i < data.Length; i += channels)
            {
                time = time + increment;
                if (time > 2 * System.Math.PI)
                {
                    time = 0;
                }
                var value = soundWave[(int)(soundWave.Count * (time / (2 * System.Math.PI)))];
                data[i] = (float)value * 0.2f * volumeSlider.value;
                if (channels == 2)
                {
                    data[i + 1] = data[i];
                }
            }
        }
        
    }

    public void OnAudioRead(float[] data)
    {
        var time = 0d;

        if (soundWave.Count == 0) { return; }
        var frequency = GetFrequency();
        increment = frequency * 2 * System.Math.PI / wav_frequency;
        for (var i = 0; i < data.Length; i += 1)
        {
            time = time + increment;
            if (time > 2 * System.Math.PI)
            {
                time = time - 2 * System.Math.PI;
            }
            var value = soundWave[(int)(soundWave.Count * (time / (2 * System.Math.PI)))];
            data[i] = (float)value * 0.4f;
        }

    }

    public List<float> PointsToWave(List<Vector2> points)
    {
        var p = new StringBuilder();
        foreach (var item in points)
        {
            p.Append(item);
            p.Append("\n");
        }
        Debug.Log(p.ToString());

        var wave = new List<float>();
        for (int i = 150; i <= 929; i++)
        {
            
            var first = points.Where(point => point.x <= i).Last();
            var last = points.Where(point => point.x > i).First();
            var ratio = (last.y - first.y) / (last.x - first.x);
            if (i % 100 == 0)
            {
                Debug.Log($"first:{points.Where(point => point.x <= i).Last()},last:{points.Where(point => point.x >= i).First()},ratio:{(last.y - first.y) / (last.x - first.x)}");
            }
            wave.Add((first.y - 860 + ratio * (i - first.x)) / 460);
        }
        this.soundWave = wave;

        var s = new StringBuilder();
        foreach (var item in wave)
        {
            s.Append(item);
            s.Append("\n");
        }
        Debug.Log(s.ToString());

        return wave;
    }

    float Normalize(float value)
    {
        return (value - 900) / (1320 - 400) * 2;
    }

    public void Play()
    {
        isPlaying = true;
        play.Image.sprite = play.pauseSprite;
        startTime = Time.realtimeSinceStartup;
    }

    public void Stop()
    {
        isPlaying = false;
        play.Image.sprite = play.playSprite;
        exportLength = Time.realtimeSinceStartup - startTime;
        if (exportToggle.isOn && exportLength > 0) 
        {
            var fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            CreateAudioClip.CreateClip();
            SavWav.Save(fileName,CreateAudioClip.clip);
            fileData.text = $"The file has been exported as \"{Application.persistentDataPath.ToString() + "/" + fileName + ".wav"}\".";
            Debug.Log($"ファイルは{Application.persistentDataPath.ToString()}に書き出されました。");
        }
    }
}
