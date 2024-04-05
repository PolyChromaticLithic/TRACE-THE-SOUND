using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [SerializeField]
    private Wave wave;

    [SerializeField] private Image Image;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite pauseSprite;

    [SerializeField] private Pen pen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!wave.isPlaying)
        {
            Image.sprite = playSprite;
        }
    }

    public void OnClick()
    {
        if (!pen.isConnected) return;
        if (wave.isPlaying)
        {
            wave.isPlaying = false;
            Image.sprite = playSprite;

        }
        else
        {
            wave.isPlaying = true;
            Image.sprite = pauseSprite;
        }
    }
}
