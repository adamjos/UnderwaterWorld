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
    private Color normalAmbientLightColor;

    public Camera fpsCamera;
    public GameObject underwaterPostProcessGO;
    public GameObject normalPostProcessGO;
    public GameObject normalDirectionalLight;

    private LightmapData[] lightmap_data;

    private Material skyboxMat;
    
    // Start is called before the first frame update
    void Start()
    {

        // Save reference to existing scene lightmap data.
        lightmap_data = LightmapSettings.lightmaps;

        normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        underwaterColor = new Color(0.1177f, 0.1529f, 0.1843f, 1); // Darker
        //underwaterColor = new Color(0.3804f, 0.6285f, 0.8490f, 1); // Bright blue
        normalAmbientLightColor = new Color(0.212f, 0.227f, 0.259f);

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
        // Disable lightmaps in scene by removing the lightmap data references
        LightmapSettings.lightmaps = new LightmapData[] { };

        normalDirectionalLight.SetActive(false);

        RenderSettings.fogColor = Color.black; //underwaterColor;
        RenderSettings.fogDensity = underwaterFogDensity;
        RenderSettings.skybox = null;
        RenderSettings.ambientLight = Color.black;

        fpsCamera.backgroundColor = Color.black; //underwaterColor;
        underwaterPostProcessGO.SetActive(true);
        normalPostProcessGO.SetActive(false);
    }

    void SetNormal ()
    {
        // Reenable lightmap data in scene.
        LightmapSettings.lightmaps = lightmap_data;

        normalDirectionalLight.SetActive(true);

        RenderSettings.fogColor = normalColor;
        RenderSettings.fogDensity = normalFogDensity;
        RenderSettings.ambientLight = normalAmbientLightColor;
        RenderSettings.skybox = skyboxMat;

        underwaterPostProcessGO.SetActive(false);
        normalPostProcessGO.SetActive(true);
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
