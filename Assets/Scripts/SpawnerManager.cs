using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
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
            Debug.Log("length :" + _normalObjects.Length);
            _currentObject = _normalObjects[Random.Range(0, _normalObjects.Length)];
            //objectHeight = _currentObject.GetComponent<Renderer>().bounds.size.y;
            tmp = Instantiate(_currentObject, new Vector3(0, transform.position.y + transform.localScale.y + (1.0f / 2), 0), Quaternion.identity);
            tmp.GetComponentInChildren<Renderer>().material = _materials[Random.Range(0, _materials.Length)];
        }
    }

    private void OnDestroy()
    {
        Destroy(tmp);
    }
}
