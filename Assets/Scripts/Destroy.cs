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
            GameController.Instance.destroyedBricks++;
            _renderer = collision.gameObject.GetComponent<Renderer>();
            StartCoroutine(Fade(collision.gameObject));
        }
    }
}
