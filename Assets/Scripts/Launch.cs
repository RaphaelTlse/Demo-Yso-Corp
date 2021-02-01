using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    Vector3 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    Vector3 initPos;
    float touchTimeStart, touchTimeFinish, timeInterval, distance, throwForce; // to calculate swipe time to sontrol throw force in Z direction
    bool pressed = false;

    IEnumerator ExecuteAfterTime(float time)
    {
        //Debug.Log(distance);
        if (distance > 150)
        {
            gameObject.GetComponent<Animator>().runtimeAnimatorController = null;
            yield return new WaitForSeconds(time);
            GameController.Instance._currentSpawner.GetComponent<SpawnerManager>().spawnObject();
            CameraController.Instance.SetCurrentState(CameraController.CameraState.Fixed);
            GameController.Instance.SetCurrentState(GameController.GameState.Ready);
            Destroy(gameObject);
        }
        else
        {
            transform.position = initPos;
            rb.isKinematic = true;
        }
        CameraController.Instance.SetCurrentState(CameraController.CameraState.Fixed);
        GameController.Instance.SetCurrentState(GameController.GameState.Ready);
    }

    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("???");
        if (GameController.Instance._currentState == GameController.GameState.Ready && CameraController.Instance._currentState == CameraController.CameraState.Fixed)
        {
            //Debug.Log("ALORS ?");
            // if you touch the screen
            if (Input.GetButtonDown("Fire1"))
            {

                // getting touch position and marking time when you touch the screen
                throwForce = 0.0f;
                touchTimeStart = Time.time;
                startPos = Input.mousePosition;
                pressed = true;
            }

            // if you release your finger
            if (Input.GetButtonUp("Fire1") && pressed == true)
            {
                CameraController.Instance.SetCurrentState(CameraController.CameraState.Follow);
                GameController.Instance.SetCurrentState(GameController.GameState.Observing);
                if (AppController.Instance._currentState == AppController.AppState.MainMenu)
                    AppController.Instance.SetCurrentState(AppController.AppState.Game);
                //Debug.Log("ça lance");
                rb.isKinematic = false;
                //rb.AddForce(0, 1000, 15000);

                // marking time when you release it
                touchTimeFinish = Time.time;

                // calculate swipe time interval 
                timeInterval = touchTimeFinish - touchTimeStart;

                // getting release finger position
                endPos = Input.mousePosition;

                // calculating swipe direction in 2D space
                direction = startPos - endPos;

                distance = Vector3.Distance(startPos, endPos);

                if (timeInterval > 0)
                    throwForce = distance / timeInterval;
                else
                    throwForce = 0;
                //Debug.Log("Temps : " + timeInterval + ", Distance : " + distance);
                //Debug.Log("Direction : " + direction);
                //Debug.Log("ThrowForce : " + throwForce + ", time interval : " + timeInterval);
                // add force to balls rigidbody in 3D space depending on swipe time, direction and throw forces
                rb.isKinematic = false;
                //            Debug.Log("Throw Force xy : " + throwForceInXandY + ", Throw Force z : " + throwForceInZ + ", Time Interval : " + timeInterval);
                //rb.AddForce(0, 1500, 3000);
                rb.AddForce(0-direction.x * throwForce * rb.mass / 750, -direction.y * throwForce * rb.mass / 800, throwForce * rb.mass * 2f);

                StartCoroutine(ExecuteAfterTime(2.5f));
                pressed = false;
                // Destroy ball in 4 seconds
                /*if (distance > 150)
                {
                    GameController.Instance._currentSpawner.GetComponent<SpawnerManager>().spawnObject();
                    Destroy(gameObject, 3f);
                }*/
            }
        }
    }
}
