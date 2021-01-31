using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    Renderer _renderer;

    IEnumerator Fade(GameObject gameObject)
    {
        Color c;
        for (float ft = 1f; ft >= 0; ft -= 2f * Time.deltaTime)
        {
            if (gameObject != null)
            {
                c = gameObject.GetComponent<Renderer>().material.color;
                //Debug.Log("Fading, a = " + c.a);
                c.a = ft;
                gameObject.GetComponent<Renderer>().material.color = c;
            }
            yield return null;
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Brick")
        {
            //Debug.Log(collision.gameObject.name);
            GameController.Instance.destroyedBricks++;
            _renderer = collision.gameObject.GetComponent<Renderer>();
            //Debug.Log("Trying to fade, a = " + _renderer.material.color.a);
            StartCoroutine(Fade(collision.gameObject));

            //Debug.Log("Allo1");
            //
            //Debug.Log("Allo3");
            //Debug.Log("Add points");
        }
    }
}
