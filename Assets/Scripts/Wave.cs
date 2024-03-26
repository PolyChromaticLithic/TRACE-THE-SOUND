using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using System.Text;

public class Wave : MonoBehaviour
{
    public List<float> wave = new();
    public bool isPlaying = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public double frequency = 110;
    private double sampling_frequency = 48000;
    private double increment;
    private double time;

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (wave.Count == 0) { return; }
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
                var value = wave[(int)(wave.Count * (time / (2 * System.Math.PI)))];
                data[i] = (float)value * 0.05f;
                if (channels == 2)
                {
                    data[i + 1] = data[i];
                }
            }
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
        this.wave = wave;

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
}
