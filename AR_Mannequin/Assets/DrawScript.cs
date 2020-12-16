using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawScript : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float waveAmplitude;
    public float lenght;
    public float speed;
    public GameObject speedNumber;
    private int ratioSpeed;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3[] positions = new Vector3[3] { new Vector3(0, 0, 0), new Vector3(-1, 1, 0), new Vector3(1, 1, 0) };
    }

    private void Update() {
        try
        {
           ratioSpeed = int.Parse(speedNumber.GetComponent<Text>().text);
        }
        catch { }
        speed = 2 * ((float)ratioSpeed / 86);
        if (ratioSpeed < 0.5) { DrawTravellingSineWave(new Vector3(0, 0, 0), 0, lenght, speed); }
        else
        {
            DrawTravellingSineWave(new Vector3(0, 0, 0), waveAmplitude, lenght, speed);
        }
    }

    void DrawTriangle(Vector3[] vertexPositions, float startWidth, float endWidth)
    {
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.loop = true;
        lineRenderer.positionCount = 3;
        lineRenderer.SetPositions(vertexPositions);
    }
    void DrawTravellingSineWave(Vector3 startPoint, float amplitude, float wavelength, float waveSpeed){

    float x = 0f;
    float y;
    float k = 2 * Mathf.PI / wavelength;
    float w = k * waveSpeed;
    lineRenderer.positionCount = 200;
    for (int i = 0; i < lineRenderer.positionCount; i++){
        x += i * 0.001f;
        y = amplitude * Mathf.Sin(k * x + w * Time.time);
        lineRenderer.SetPosition(i, new Vector3(x, y, 0) + startPoint);
    }
}
}
