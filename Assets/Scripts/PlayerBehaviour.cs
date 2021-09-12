using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private AnimatorManager _animator;
    [SerializeField] private List<BodyPartManager> _bodyPartManagerList;
    [SerializeField] private float _assembleDuration;
    [SerializeField] private int _bodyPartHealCount;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _waitTimeBetweenSpheres;
    [SerializeField] private float sphereScale;
    public event Action GameFinished;
    public event Action GameOver;
    public event Action<Vector3> ShapeChanged;

    private BodyPartManager[] emptyBodyParts;

    public void Initialize()
    {
        emptyBodyParts = new BodyPartManager[_bodyPartHealCount];

        for (int i = 0; i < _bodyPartManagerList.Count; i++)
        {
            _bodyPartManagerList[i].GroupBroken += BodyPartBroken;
            _bodyPartManagerList[i].GroupHealing += BodyPartHealed;
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void BodyPartBroken(bool isCore)
    {
        if (_animator != null)
        {
            _animator.Break(transform);
        }

        if (isCore)
        {
            GameOver?.Invoke();
        }
    }

    private void BodyPartHealed()
    {
        if (_animator != null)
        {
            _animator.Heal(transform);
        }
    }

    public void StartRunAnimation()
    {
        if (_animator != null)
        {
            _animator.StartRunning();
        }
    }

    private void Assemble(Vector3 startPosition,float moveDuration)
    {
        emptyBodyParts = DetectedEmptyBodyParts();

        for (int i = 0; i < emptyBodyParts.Length; i++)
        {
            if(emptyBodyParts[i] != null)
            {
                emptyBodyParts[i].RegroupSphereGroups(startPosition, moveDuration, _waitTimeBetweenSpheres);
            }
        }
    }

    private BodyPartManager[] DetectedEmptyBodyParts()
    {
        BodyPartManager[] emptyBodyPartArray = new BodyPartManager[_bodyPartHealCount];

        int bodyPartIndex = 0;
        for (int i = 0; i < _bodyPartManagerList.Count; i++)
        {
            if (bodyPartIndex == _bodyPartHealCount) 
            {
                return emptyBodyPartArray;
            }

            if (_bodyPartManagerList[i].BodyPartIsAllEmpty())
            {
                emptyBodyPartArray.SetValue(_bodyPartManagerList[i], bodyPartIndex);
                bodyPartIndex++;
            }
        }
        return emptyBodyPartArray;
    }

    public List<SphereBehaviour> GetAllSpheres()
    {
        List<SphereBehaviour> tempList = new List<SphereBehaviour>();
        for (int i = 0; i < _bodyPartManagerList.Count; i++)
        {
            tempList.AddRange(_bodyPartManagerList[i].GetSpheres());
        }
        return tempList;
    }

    public void MoveSpheresToJar(SphereBehaviour sphere, Transform parent)
    {
        sphere.MoveSphereToJar(parent, _jumpPower, _moveDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            Assemble(other.transform.position, _assembleDuration);
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Finish"))
        {
            GameFinished?.Invoke();
            if (_animator != null)
            {
                _animator.FinishAnimation();
            }
        }

        if (other.CompareTag("ShapeChanger"))
        {
            ShapeChanged?.Invoke(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Breaker"))
        {
            if(_animator != null)
            {
                _animator.ChangeTransform(transform);
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _bodyPartManagerList.Count; i++)
        {
            _bodyPartManagerList[i].GroupBroken -= BodyPartBroken;
            _bodyPartManagerList[i].GroupHealing -= BodyPartHealed;
        }
    }
}
