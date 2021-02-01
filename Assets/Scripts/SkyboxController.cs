using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : Singleton<SkyboxController>
{
    [SerializeField]
    public Material[] skyboxes;

    public int index = 0;

    private void Start()
    {
        resetSkybox();
    }

    public void resetSkybox()
    {
        index = 0;
        RenderSettings.skybox = skyboxes[0];
        skyboxes[0].SetFloat("_Blend", 0);
    }

    public void nextSkybox()
    {
        index++;
        RenderSettings.skybox = skyboxes[index];
        skyboxes[index].SetFloat("_Blend", 0);
    }
}
