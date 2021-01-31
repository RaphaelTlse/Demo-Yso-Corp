using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * 3f * Time.deltaTime);
        //Debug.Log(transform.rotation.y);
        if (transform.rotation.y == 1)
        {
            transform.rotation = new Quaternion(transform.rotation.x, -1, transform.rotation.z, 0.0f);
        }
    }
}
