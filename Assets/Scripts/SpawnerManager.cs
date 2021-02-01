using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]
    private RuntimeAnimatorController[] _animationcontrollers;
    [SerializeField]
    private Material[] _materials;

    public GameObject[] _normalObjects;
    public GameObject[] _frenzyObjects;
    public GameObject _currentObject;
    public GameObject tmp;

    public float objectHeight = 0;

    public void spawnObject()
    {
        if (GameController.Instance.frenzyMode == false)
        {
            _currentObject = _normalObjects[Random.Range(0, _normalObjects.Length)];
            tmp = Instantiate(_currentObject, new Vector3(0, transform.position.y + transform.localScale.y + (1.0f / 2) - 1f, 1), Quaternion.identity);
            tmp.transform.rotation = Quaternion.Euler(0, 180, 0);
            tmp.GetComponentInChildren<Renderer>().material = _materials[Random.Range(0, _materials.Length)];
            tmp.GetComponentInChildren<Animator>().runtimeAnimatorController = _animationcontrollers[Random.Range(0, _animationcontrollers.Length)];
        }
    }

    private void OnDestroy()
    {
        Destroy(tmp);
    }
}
