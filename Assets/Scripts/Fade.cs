using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    Renderer _renderer;

    public void SlowFade()
    {
        //Debug.Log("Allo2");
        _renderer = GetComponent<Renderer>();
        //Debug.Log("Trying to fade, a = " + _renderer.material.color.a);
        while (_renderer.material.color.a > 0)
        {
            //Debug.Log("Fading, a = " + _renderer.material.color.a);
            Color oldCol = _renderer.material.color;
            Color newCol = new Color(oldCol.r, oldCol.g, oldCol.b, oldCol.a - 0.001f);
            _renderer.material.color = newCol;
        }
        Destroy(gameObject);
    }
}
