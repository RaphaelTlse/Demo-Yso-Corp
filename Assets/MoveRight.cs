using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : MonoBehaviour
{
    public float speed = 0.1f;
    public int resetvalue = 15;

    void Update()
    {
        if (transform.position.x > resetvalue)
            transform.position = new Vector3(-resetvalue, transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
    }
}
