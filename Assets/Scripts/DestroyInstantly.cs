using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInstantly : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Brick")
        {
            GameController.Instance.destroyedBricks++;
            Destroy(collision.gameObject);
        }
    }
}
