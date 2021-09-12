using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private PlayerBehaviour _playerBehaviour;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Transform _finalPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject _newShape;
    [SerializeField] private InputSystem _inputSystem;
    [SerializeField] private CameraBehaviour _camera;
    [SerializeField] private float _timeToFinishLevel;
    [SerializeField] private int _jarPositionIndex;
    [SerializeField] private List<Transform> _jarSpherePositionList;

    private void Initialize()
    {
        _playerBehaviour.GameFinished += CollectPoints;
        _playerBehaviour.GameOver += GameOver;
        _playerBehaviour.ShapeChanged += SpawnNewPlayer;
        _playerMovement.Initialize();
    }

    private void Awake()
    {
        Initialize();
    }

    private void CollectPoints()
    {
        StartCoroutine(CollectPointsCoroutine());
    }

    private IEnumerator CollectPointsCoroutine()
    {
        DOTween.SetTweensCapacity(200, 125);
        List<SphereBehaviour> tempList = _playerBehaviour.GetAllSpheres();
        _playerMovement.Stop();

        var movePoint = new Vector3(_finalPoint.position.x, _playerBehaviour.transform.position.y, _finalPoint.position.z);
        Tween myTween = _playerBehaviour.transform.DOMove(movePoint, 1f);
        yield return myTween.WaitForCompletion();

        for (int i = 0; i < tempList.Count; i++)
        {
            _playerBehaviour.MoveSpheresToJar(tempList[i], _jarSpherePositionList[_jarPositionIndex]);
            _jarPositionIndex++;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(_timeToFinishLevel);
        LevelCompleted();
    }

    private void LevelCompleted()
    {
        _UIManager.OpenLevelCompletedMenu();
    }

    private void GameOver()
    {
        _playerMovement.Stop();
        _UIManager.OpenRetryLevelMenu();
    }

    public void SpawnNewPlayer(Vector3 pos)
    {
        Destroy(_playerBehaviour.gameObject);
        _playerMovement.Stop();

        var spawnPosition = new Vector3(pos.x, _newShape.transform.position.y, pos.z);
        GameObject newObject = Instantiate(_newShape, spawnPosition, Quaternion.identity);
        InitiliazeNewPlayer(newObject);

    }

    private void InitiliazeNewPlayer(GameObject newObject)
    {
        _camera.SetTarget(newObject.transform);

        newObject.TryGetComponent(out PlayerBehaviour playerBehaviour);
        _playerBehaviour = playerBehaviour;
        _playerBehaviour.Initialize();
        _playerBehaviour.GameFinished += CollectPoints;
        _playerBehaviour.GameOver += GameOver;

        newObject.TryGetComponent(out PlayerMovement playerMovement);
        _playerMovement = playerMovement;
        _playerMovement.SetInputSystem(_inputSystem);
        _playerMovement.Initialize();
    }
}
