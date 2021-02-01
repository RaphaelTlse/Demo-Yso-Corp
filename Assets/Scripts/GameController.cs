using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField]
    private GameObject _spawner;
    [SerializeField]
    private GameObject _defaultBrick;
    [SerializeField]
    private GameObject _goldenBrick;
    [SerializeField]
    private GameObject _explosiveBrick;
    [SerializeField]
    private GameObject _miniDefaultBrick;
    [SerializeField]
    private GameObject _miniGoldenBrick;
    [SerializeField]
    private GameObject _miniExplosiveBrick;
    [SerializeField]
    private GameObject _spaceBrick;
    [SerializeField]
    private GameObject _ground;
    [SerializeField]
    private GameObject _cloud;
    [SerializeField]
    private GameObject _socle;
    [SerializeField]
    private GameObject _flyingSocle;
    [SerializeField]
    private PhysicMaterial _brickMaterial;
    [SerializeField]
    private Texture[] _brickTextures;

    public GameObject _previousSpawner;
    public GameObject _currentSpawner;
    public GameObject _nextSpawner;
    public GameObject _previousSocle;
    public GameObject _currentSocle;
    public GameObject _nextSocle;
    public GameObject[] _previousWall;
    public GameObject[] _currentWall;
    public GameObject[] _nextWall;
    public GameObject _previousGround;
    public GameObject _currentGround;
    public GameObject _nextGround;
    //private int money = 0;
    public int level = 0;
    public int maxLevel = 10;
    public int levelHeight = 50;
    public float wallDistance = 15;
    public int brickTotalNumber = 33;
    public int brickLineLength = 6;
    public int lineNumber = 6;
    public int destroyedBricks = 0;
    public float winPourcentage = 75;
    public bool frenzyMode = false;
    public float finalWallDistance = 100;
    public float finalWallHeight = 50;
    public int thrown = 0;
    
    public enum GameState
    {
        Ready,
        Observing,
        Pause
    }

    public GameState _currentState { get; private set; }

    public void SetCurrentState(GameState State)
    {
        _currentState = State;
        UpdateState();
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case GameState.Ready:
                break;
            case GameState.Observing:
                break;
            case GameState.Pause:
                break;
        }
    }

    private void Start()
    {
        _currentState = GameState.Ready;
    }

    public bool IsLevelOver()
    {
        int newLineLength = brickLineLength + (level + 1) / 2;
        int newLineNumber = lineNumber + ((level + 1) / 3 * 2);
        int total = ((2 * newLineLength) * (newLineNumber / 2));
        if ((float)destroyedBricks / total >= winPourcentage / 100)
            return (true);
        return (false);
    }

    public void InitializeGame()
    {
        level = 0;
        LoadLevel(0);
    }

    public void RemovePreviousLevel()
    {
        Destroy(_previousGround);
        Destroy(_previousSocle);
        Destroy(_previousSpawner);
        for (int i = 0; i < _previousWall.Length; i++)
            Destroy(_previousWall[i].gameObject);
    }

    public void Reset()
    {
        Destroy(_currentGround);
        Destroy(_currentSocle);
        Destroy(_currentSpawner);
        for (int i = 0; i < _currentWall.Length; i++)
            Destroy(_currentWall[i].gameObject);
        Destroy(_nextGround);
        Destroy(_nextSocle);
        Destroy(_nextSpawner);
        for (int i = 0; i < _nextWall.Length; i++)
            Destroy(_nextWall[i].gameObject);
        LoadLevel(0);
        CameraController.Instance.gameObject.transform.position = new Vector3(0, 3.5f, -7);
        CameraController.Instance.SetCurrentState(CameraController.CameraState.Fixed);
        SetCurrentState(GameState.Ready);
        CameraController.Instance.gameObject.transform.rotation = Quaternion.identity;
        CameraController.Instance._initPos = CameraController.Instance.gameObject.transform.position;
        CameraController.Instance._initRot = CameraController.Instance.gameObject.transform.rotation;
        thrown = 0;
        level = 0;
        SkyboxController.Instance.resetSkybox();
    }

    public void LoadLevel(int level)
    {
        if (level == 0)
        {
            _currentGround = Instantiate(_ground, new Vector3(0, -20, wallDistance / 2), Quaternion.identity);
            _nextGround = Instantiate(_cloud, new Vector3(_cloud.transform.position.x, levelHeight + _cloud.transform.position.y, wallDistance / 2 + _cloud.transform.position.z), Quaternion.identity);
            _nextGround.GetComponent<Renderer>().enabled = false;
            _currentSpawner = Instantiate(_spawner, new Vector3(0, 0, 0), Quaternion.identity);
            _nextSpawner = Instantiate(_spawner, new Vector3(0, levelHeight, 0), Quaternion.identity);
            _nextSpawner.GetComponent<Renderer>().enabled = false;
            _currentSocle = Instantiate(_socle, new Vector3(0, 0.1f, wallDistance), Quaternion.identity);
            _nextSocle = Instantiate(_flyingSocle, new Vector3(_flyingSocle.transform.position.x, levelHeight + 0.5f + _flyingSocle.transform.position.y, wallDistance + 4f), Quaternion.identity);
            _nextSocle.transform.localScale = new Vector3(_nextSocle.transform.localScale.x + 2f, _nextSocle.transform.localScale.y, _nextSocle.transform.localScale.z);
            _nextSocle.GetComponent<Renderer>().enabled = false;
            _currentWall = buildWallAt(level);
            _nextWall = buildWallAt(level + 1);
            for (int i = 0; i < _nextWall.Length; i++)
            {
                _nextWall[i].gameObject.GetComponent<Renderer>().enabled = false;
            }

        }
        else if (level < maxLevel)
        {
            _previousGround = _currentGround;
            _previousSocle = _currentSocle;
            _previousSpawner = _currentSpawner;
            _previousWall = _currentWall;
            _currentGround = _nextGround;
            _nextGround.GetComponent<Renderer>().enabled = true;
            _currentSocle = _nextSocle;
            _nextSocle.GetComponent<Renderer>().enabled = true;
            _currentSpawner = _nextSpawner;
            _currentWall = _nextWall;
            for (int i = 0; i < _nextWall.Length; i++)
            {
                if (_nextWall[i].gameObject != null)
                    _nextWall[i].gameObject.GetComponent<Renderer>().enabled = true;
            }
            if (level + 2 < maxLevel)
            {
                _nextGround = Instantiate(_cloud, new Vector3(_cloud.transform.position.x, (level + 1) * levelHeight + 0.1f + _cloud.transform.position.y, wallDistance / 2 + _cloud.transform.position.z), Quaternion.identity);
                _nextGround.GetComponent<Renderer>().enabled = false;
                _nextSocle = Instantiate(_flyingSocle, new Vector3(0, (level + 1) * levelHeight - 0.6f, wallDistance + (((level + 2) / 2) * 4)), Quaternion.identity);
                _nextSocle.transform.localScale = new Vector3(_nextSocle.transform.localScale.x + level + 2 - level % 2, _nextSocle.transform.localScale.y, _nextSocle.transform.localScale.z);
                _nextSocle.GetComponent<Renderer>().enabled = false;
                _nextWall = buildWallAt(level + 1);
                for (int i = 0; i < _nextWall.Length; i++)
                    _nextWall[i].gameObject.GetComponent<Renderer>().enabled = false;
            }
            else
                buildFinalWall();

            _nextSpawner = Instantiate(_spawner, new Vector3(0, (level + 1) * levelHeight, 0), Quaternion.identity);
            _nextSpawner.GetComponent<Renderer>().enabled = false;

        }
        _currentSpawner.GetComponent<SpawnerManager>().spawnObject();
    }

    public GameObject[] buildWallAt(int level)
    {

        int i = 0;
        int line = 0;
        int newLineLength = brickLineLength + (level + 1) / 2;
        int newLineNumber = lineNumber + ((level + 1) / 3 * 2);
        int total = ((2 * newLineLength + 1) * (newLineNumber / 2));
        GameObject[] wall = new GameObject[total];
        while (i < total)
        {
            for (int j = 0; j < newLineLength; j++)
            {
                GameObject tmp;
                int random;

                random = Random.Range(0, 100);
                if (random + level >= 98 || level == maxLevel - 1)
                    tmp = Instantiate(_goldenBrick, new Vector3(j * 2 - newLineLength + 1 + 0 % 2, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                else if (random < 2)
                    tmp = Instantiate(_explosiveBrick, new Vector3(j * 2 - newLineLength + 1, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                else
                {
                    tmp = Instantiate(_defaultBrick, new Vector3(j * 2 - newLineLength + 1, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                    tmp.GetComponent<Renderer>().material.mainTexture = _brickTextures[Random.Range(0, _brickTextures.Length)];
                }
                tmp.GetComponent<Collider>().material = _brickMaterial;
                tmp.GetComponent<Rigidbody>().mass += level * 5;
                wall[i] = tmp;
                i++;
            }
            line++;
            for (int j = 0; j < newLineLength + 1; j++)
            {
                GameObject tmp;
                int random;

                random = Random.Range(0, 100);
                if (random + level >= 98)
                {
                    if (j == 0)
                        tmp = Instantiate(_miniGoldenBrick, new Vector3(j * 2 - newLineLength + 0.5f, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                    else if (j == newLineLength)
                        tmp = Instantiate(_miniGoldenBrick, new Vector3(j * 2 - newLineLength - 0.5f, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                    else
                        tmp = Instantiate(_goldenBrick, new Vector3(j * 2 - newLineLength, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                }
                else if (random < 2)
                {
                    if (j == 0)
                        tmp = Instantiate(_miniExplosiveBrick, new Vector3(j * 2 - newLineLength + 0.5f, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                    else if (j == newLineLength)
                        tmp = Instantiate(_miniExplosiveBrick, new Vector3(j * 2 - newLineLength - 0.5f, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                    else
                        tmp = Instantiate(_explosiveBrick, new Vector3(j * 2 - newLineLength, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                }
                else
                {
                    if (j == 0)
                        tmp = Instantiate(_miniDefaultBrick, new Vector3(j * 2 - newLineLength + 0.5f, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                    else if (j == newLineLength)
                        tmp = Instantiate(_miniDefaultBrick, new Vector3(j * 2 - newLineLength - 0.5f, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                    else
                        tmp = Instantiate(_defaultBrick, new Vector3(j * 2 - newLineLength, level * levelHeight + line + 0.65f, wallDistance + ((level + 1) / 2 * 4)), Quaternion.identity) as GameObject;
                    tmp.GetComponent<Renderer>().material.mainTexture = _brickTextures[Random.Range(0, _brickTextures.Length)];
                }
                if (level == maxLevel - 1)
                    tmp.GetComponent<Rigidbody>().useGravity = false;
                tmp.GetComponent<Collider>().material = _brickMaterial;
                tmp.GetComponent<Rigidbody>().mass += level * 5;
                wall[i] = tmp;
                i++;
            }
            line++;
        }
        return (wall);
    }

    public void buildFinalWall()
    {
        GameObject tmp;

        finalWallHeight += (level + 1) * levelHeight;

        tmp = Instantiate(_spaceBrick, new Vector3(-10, 0 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-9, 0 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-8, 0 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-7, 0 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-6, 0 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-3, 0 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-10, -1 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-5, -1 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-10, -2 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-5, -2 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-3, -2 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(0, -2 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(1, -2 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(2, -2 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(5, -2 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(6, -2 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(7, -2 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(8, -2 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-10, -3 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-9, -3 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-8, -3 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-7, -3 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-6, -3 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-3, -3 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-1, -3 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(3, -3 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(5, -3 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(9, -3 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-10, -4 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-5, -4 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-3, -4 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-1, -4 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(0, -4 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(1, -4 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(2, -4 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(5, -4 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(9, -4 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-10, -5 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-5, -5 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-3, -5 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-1, -5 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(5, -5 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(9, -5 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-10, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-9, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-8, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-7, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-6, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-3, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(0, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(1, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(2, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(3, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(5, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(9, -6 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(7, -8 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(5, -9 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(6, -9 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(12, -9 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-9, -10 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(12, -10 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-6, -11 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-5, -11 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-4, -11 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-1, -11 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(2, -11 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(5, -11 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(6, -11 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(7, -11 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(12, -11 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-9, -12 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-7, -12 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-3, -12 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-1, -12 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(2, -12 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(4, -12 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(8, -12 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(12, -12 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-9, -13 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-7, -13 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-3, -13 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-1, -13 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(2, -13 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(4, -13 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(5, -13 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(6, -13 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(7, -13 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(12, -13 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-9, -14 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-7, -14 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-3, -14 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-1, -14 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(2, -14 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(4, -14 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(12, -14 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-12, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-9, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-6, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-5, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-4, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(0, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(1, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(2, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(5, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(6, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(7, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(8, -15 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-11, -16 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(-10, -16 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
        tmp = Instantiate(_spaceBrick, new Vector3(12, -16 + finalWallHeight, finalWallDistance), Quaternion.identity) as GameObject;
    }

    private void Update()
    {
        if (_currentState == GameState.Ready && IsLevelOver() == true && thrown > 0)
        {
            destroyedBricks = 0;
            thrown = 0;
            level++;
            LoadLevel(level);
            SetCurrentState(GameState.Pause);
            CameraController.Instance.SetCurrentState(CameraController.CameraState.Transition);
        }
    }
}
