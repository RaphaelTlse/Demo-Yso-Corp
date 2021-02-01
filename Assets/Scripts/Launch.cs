using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launch : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    public int maxThrow = 5;

    Vector3 startPos, endPos, direction;
    Vector3 initPos;
    float touchTimeStart, touchTimeFinish, timeInterval, distance, throwForce;
    bool pressed = false;

    IEnumerator ExecuteAfterTime(float time)
    {
        if (distance > 150)
        {
            GameController.Instance.thrown++;
            gameObject.GetComponent<Animator>().runtimeAnimatorController = null;
            if (GameController.Instance.thrown == 5)
            {
                yield return new WaitForSeconds(time);
                if (GameController.Instance.IsLevelOver() == false)
                    GameController.Instance.Reset();
            }
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
    
    void Update()
    {
        if (GameController.Instance._currentState == GameController.GameState.Ready && CameraController.Instance._currentState == CameraController.CameraState.Fixed)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                throwForce = 0.0f;
                touchTimeStart = Time.time;
                startPos = Input.mousePosition;
                pressed = true;
            }
            
            if (Input.GetButtonUp("Fire1") && pressed == true)
            {
                CameraController.Instance.SetCurrentState(CameraController.CameraState.Follow);
                GameController.Instance.SetCurrentState(GameController.GameState.Observing);
                if (AppController.Instance._currentState == AppController.AppState.MainMenu)
                    AppController.Instance.SetCurrentState(AppController.AppState.Game);
                rb.isKinematic = false;
                touchTimeFinish = Time.time;
                
                timeInterval = touchTimeFinish - touchTimeStart;
                
                endPos = Input.mousePosition;
                
                direction = startPos - endPos;

                distance = Vector3.Distance(startPos, endPos);

                if (timeInterval > 0)
                    throwForce = distance / timeInterval;
                else
                    throwForce = 0;
                rb.isKinematic = false;
                rb.AddForce(0-direction.x * throwForce * rb.mass / 350, -direction.y * throwForce * rb.mass / 650, throwForce * rb.mass * 3f);

                StartCoroutine(ExecuteAfterTime(2.5f));
                pressed = false;
            }
        }
    }
}
