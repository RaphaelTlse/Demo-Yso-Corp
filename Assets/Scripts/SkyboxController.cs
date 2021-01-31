using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : Singleton<SkyboxController>
{
    [SerializeField]
    public Material[] skyboxes;

    public float _blendSpeed = 10000000f;
    public int index = 0;

    private void Start()
    {
        Debug.Log(skyboxes.Length);
        Debug.Log("nom :" + skyboxes[0]);
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
        Debug.Log(index);
        RenderSettings.skybox = skyboxes[index];
        skyboxes[index].SetFloat("_Blend", 0);
    }
}
