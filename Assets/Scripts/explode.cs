using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem explosionEffect;

    public float impactResistance = 5;
    public float power = 3000.0f;
    public float radius = 10.0f;
    private bool exploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        float impactSpeed = 0;

        if (collision.rigidbody != null)
            impactSpeed = Mathf.Abs(collision.rigidbody.velocity.x) + Mathf.Abs(collision.rigidbody.velocity.y) + Mathf.Abs(collision.rigidbody.velocity.z);
        

        if ((collision.gameObject.tag == "Throwable" || impactSpeed >= impactResistance) && exploded == false)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            exploded = true;
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                    gameObject.GetComponentInChildren<Light>().enabled = false;
                }
            }
            GameController.Instance.destroyedBricks++;
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }
}
