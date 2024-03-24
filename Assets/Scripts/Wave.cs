using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<float> PointsToWave(List<Vector2> points)
    {
        var wave = new List<float>();
        for (int i = 150; i <= 930; i++)
        {
            var first = points.Where(point => point.x <= i).Last();
            var last = points.Where(point => point.x >= i).First();
            var ratio = (last.y - first.y) / (last.x - first.x);
            wave.Add(first.y + ratio * (i - first.x));
        }
        return wave;
    }

    float Normalize(float value)
    {
        return (value - 900) / (1320 - 400) * 2;
    }
}
