using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour
{

    public float waterHeight;

    public float underwaterFogDensity;
    public float normalFogDensity;

    private bool isUnderwater;
    private bool isCameraUnderwater;
    private Color normalColor;
    private Color underwaterColor;

    public Camera fpsCamera;

    private Material skyboxMat;
    
    // Start is called before the first frame update
    void Start()
    {
        normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        underwaterColor = new Color(0.1177f, 0.1529f, 0.1843f, 1); // Darker

        //underwaterColor = new Color(0.3804f, 0.6285f, 0.8490f, 1); // Bright blue

        skyboxMat = RenderSettings.skybox;

    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.y < waterHeight) != isUnderwater)
        {
            isUnderwater = (transform.position.y < waterHeight);
        }

        if ((fpsCamera.transform.position.y < waterHeight) != isCameraUnderwater)
        {
            isCameraUnderwater = (fpsCamera.transform.position.y < waterHeight);
        }

        if (isCameraUnderwater)
        {
            SetUnderwater();
        }

        if (!isCameraUnderwater)
        {
            SetNormal();
        }

    }

    void SetUnderwater ()
    {
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = underwaterFogDensity;

        RenderSettings.skybox = null;

        fpsCamera.backgroundColor = underwaterColor;
        

    }

    void SetNormal ()
    {
        RenderSettings.fogColor = normalColor;
        RenderSettings.fogDensity = normalFogDensity;

        RenderSettings.skybox = skyboxMat;
    }

    public bool IsUnderwater ()
    {
        return isUnderwater;
    }

    public float GetWaterHeight ()
    {
        return waterHeight;
    }

}
