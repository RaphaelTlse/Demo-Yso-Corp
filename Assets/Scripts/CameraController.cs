using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public enum CameraState
    {
        Fixed,
        Follow,
        Transition
    }

    public CameraState _currentState { get; private set; }
    public CameraState _previousState { get; private set; }

    public float _followMoveSpeed = 0.01f;
    public float _followRotateSpeed = 0.001f;
    public float _transitionMoveSpeed = 1f;
    public float _resetMoveSpeed = 1f;
    public float _cameraInitialHeight = 5f;
    public float _distanceMaxCameraWall = 10f;

    private Transform _target;
    private GameObject _targetGameObject;
    private float _smoothTransition;

    Vector3 _initPos;
    Quaternion _initRot;

    void Start()
    {
        _currentState = CameraState.Fixed;
        _previousState = CameraState.Fixed;
        _initPos = transform.position;
        _initRot = transform.rotation;
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case CameraState.Fixed:
                if (_previousState == CameraState.Transition)
                {
                    SkyboxController.Instance.nextSkybox();
                    GameController.Instance.RemovePreviousLevel();
                    GameController.Instance.destroyedBricks = 0;
                    StopCoroutine("Blend");
                }
                break;
            case CameraState.Follow:
                _targetGameObject = GameObject.FindGameObjectWithTag("Throwable");
                _target = _targetGameObject.transform;
                break;
            case CameraState.Transition:
                StartCoroutine(BlendSkybox());
                _initPos.y += GameController.Instance.levelHeight;
                _smoothTransition = _transitionMoveSpeed;
                break;
        }
    }

    public IEnumerator BlendSkybox()
    {
        int fixedIndex = SkyboxController.Instance.index;
        int levelHeight = GameController.Instance.levelHeight;
        float fixedNextLevelPosY = _initPos.y + levelHeight;

        /*for (float i = 0; i < 1; i += Time.deltaTime)
        {
            Debug.Log("compl�tion : " + (levelHeight - fixedNextLevelPosY + transform.position.y) / levelHeight);
            yield return null;
        }
        Debug.Log("mdr fini");*/
        while (SkyboxController.Instance.skyboxes[fixedIndex].GetFloat("_Blend") < 1)
        {
            SkyboxController.Instance.skyboxes[fixedIndex].SetFloat("_Blend", (levelHeight - fixedNextLevelPosY + transform.position.y) / levelHeight);
            Debug.Log(SkyboxController.Instance.skyboxes[fixedIndex].GetFloat("_Blend"));
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SetCurrentState(CameraState State)
    {
        _previousState = _currentState;
        _currentState = State;
        UpdateState();
    }

    void Update()
    {
        //Debug.Log("En ce moment �a : " + _currentState);
        //        Debug.Log("current state : " + _currentState);
        //        Debug.Log("previous state : " + _previousState);
        if (_currentState == CameraState.Follow)
        {

            Vector3 targetDirection = new Vector3(_target.position.x, GameController.Instance.levelHeight * GameController.Instance.level + _cameraInitialHeight, _target.position.z) - transform.position;
            //Debug.Log(targetDirection);
            if (transform.position.z + 6f < _target.position.z && transform.position.z + 6f + _distanceMaxCameraWall < (GameController.Instance.wallDistance + ((GameController.Instance.level + 1) / 2) * 3))
            {
                //Debug.Log("maxdist" + _distanceMaxCameraWall);
                //Debug.Log("posz" + transform.position.z);
                //Debug.Log("walldist" + GameController.Instance.wallDistance);
                //Debug.Log("Target name : " + _target.name);
                transform.position = Vector3.MoveTowards(transform.position, _target.position, _target.GetComponent<Rigidbody>().velocity.z * Time.fixedDeltaTime);
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _followRotateSpeed * (GameController.Instance.level + 1), 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            /*else if (transform.position.z + _cameraInitialHeight > _target.position.z)
            {
                transform.position = _initPos;
                transform.rotation = _initRot;
            }*/
        }
        else if (_currentState == CameraState.Transition)
        {
            //Debug.Log("_initPos.y - transform.position.y :" + _initPos.y + " - " + transform.position.y + ". Compar� � levelHeight / 2 : " + GameController.Instance.levelHeight / 2);
            if (_initPos.y - transform.position.y > GameController.Instance.levelHeight / 3f)
                _smoothTransition += 0.005f * Time.deltaTime;
            else if (_smoothTransition > 2 * _transitionMoveSpeed)
            {
                //Debug.Log("�a baisse, movspeed : " + _smoothTransition);
                _smoothTransition -= 0.01f * Time.deltaTime;
            }
            else
            {
                _smoothTransition = 2 * _transitionMoveSpeed;
            }
            transform.position = Vector3.MoveTowards(transform.position, _initPos, _smoothTransition * GameController.Instance.levelHeight);
            transform.rotation = Quaternion.Euler(5, 0, 0);
            if (transform.position.y >= _initPos.y)
            {
                transform.position = _initPos;
                GameController.Instance.SetCurrentState(GameController.GameState.Ready);
                SetCurrentState(CameraState.Fixed);
            }
        }
        else if (_currentState == CameraState.Fixed)
        {
            transform.position = Vector3.MoveTowards(transform.position, _initPos, _resetMoveSpeed * (transform.position.z - _initPos.z));
            transform.rotation = Quaternion.Euler(5, 0, 0);
        }
        //Debug.Log(_currentState);
    }
}
