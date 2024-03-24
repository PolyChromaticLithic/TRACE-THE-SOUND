using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pen : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem penParticle;
    private float latestX;
    ParticleSystem.MinMaxGradient white;
    ParticleSystem.MinMaxGradient red;

    [SerializeField] 
    private ParticleSystemRenderer penRenderer;

    [SerializeField]
    private Material whiteMaterial;

    [SerializeField]
    private Material redMaterial;

    [SerializeField]
    private Wave wave;

    void White()
    {
        penRenderer.material = whiteMaterial;
        penRenderer.trailMaterial = whiteMaterial;
    }

    void Red()
    {
        penRenderer.material = redMaterial;
        penRenderer.trailMaterial = redMaterial;
    }


    private List<Vector2> points;
    // Start is called before the first frame update
    void Start()
    {
        points = new();
    }

    private bool isDrawing = false;

    public void Down()
    {
        isDrawing = true;
        penParticle.Clear();
        latestX = 150;
        White();
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(150, 860, 0));
        penParticle.Emit(1);
        wave.isPlaying = false;
    }

    public void Up()
    {
        isDrawing = false;
        if (isEndPoint)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(930, 860, 0));
            penParticle.Emit(1);
            wave.PointsToWave(points);
            wave.isPlaying = true;
        }
        else
        {
            Red();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var clampedPositionX = Mathf.Clamp(Input.mousePosition.x, 151, 1080 - 151);
        var clampedPositionY = Mathf.Clamp(Input.mousePosition.y, 401, 1920 - 601);
        
        if (isDrawing)
        {
            if (clampedPositionX > latestX)
            {
                penParticle.Emit(1);
                points.Add(new Vector2(clampedPositionX, clampedPositionY));
                latestX = clampedPositionX;
                transform.position = Camera.main.ScreenToWorldPoint(new Vector3(clampedPositionX, clampedPositionY, 10));
            }
        }
        
        

    }

    private bool isEndPoint = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DropPoint")
        {
            isEndPoint = true;
            Debug.Log("End Point");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "DropPoint")
        {
            isEndPoint = false;
        }
    }
}
