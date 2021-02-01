using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    Renderer _renderer;

    public void SlowFade()
    {
        _renderer = GetComponent<Renderer>();
        while (_renderer.material.color.a > 0)
        {
            Color oldCol = _renderer.material.color;
            Color newCol = new Color(oldCol.r, oldCol.g, oldCol.b, oldCol.a - 0.001f);
            _renderer.material.color = newCol;
        }
        Destroy(gameObject);
    }
}
