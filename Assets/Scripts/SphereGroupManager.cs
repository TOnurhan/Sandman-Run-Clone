using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SphereGroupManager : MonoBehaviour
{
    [SerializeField] private SphereBehaviour[] _sphereArray;
    [SerializeField] private Transform[] _sphereTransformArray;
    [SerializeField] private SphereGroupManager[] _nextSphereGroup;
    private bool isEmpty = false;
    public event Action GroupBroken;
    public event Action GroupHealing;
    [SerializeField] private bool breakable = true;

    public bool IsEmpty() => isEmpty;

    private void Initialize()
    {
        for (int i = 0; i < _sphereArray.Length; i++)
        {
            var sphereBehaviour = _sphereArray[i];
            _sphereTransformArray[i].position = sphereBehaviour.transform.position;
            sphereBehaviour.SphereBroken += Break;
        }
    }

    private void Awake()
    {
        Initialize();
    }

    public void Break()
    {
        isEmpty = true;
        GroupBroken?.Invoke();
        for (int i = 0; i < _nextSphereGroup.Length; i++)
        {
            if (_nextSphereGroup[i] != null)
            {
                _nextSphereGroup[i].Break();
            }
            if (!breakable)
            {
                if (!_nextSphereGroup[i].IsEmpty())
                {
                    _nextSphereGroup[i].Break();
                    return;
                }
            }
        }

        for (int i = 0; i < _sphereArray.Length; i++)
        {
            var sphereBehaviour = _sphereArray[i];
            sphereBehaviour.GetRigidbody().isKinematic = false;
            sphereBehaviour.transform.SetParent(null);
            StartCoroutine(sphereBehaviour.SelfDisable(1f));
        }
    }

    public IEnumerator Regroup(Vector3 startPosition, float moveDuration, float waitTimeBetweenSpheres)
    {
        isEmpty = false;
        GroupHealing?.Invoke();

        for (int i = 0; i < _sphereArray.Length; i++)
        {
            var sphereBehaviour = _sphereArray[i];
            sphereBehaviour.transform.SetParent(transform);
            sphereBehaviour.gameObject.SetActive(true);
            sphereBehaviour.GetRigidbody().isKinematic = true;
            sphereBehaviour.transform.position = startPosition;
            yield return new WaitForSeconds(waitTimeBetweenSpheres);
            StartCoroutine(sphereBehaviour.MoveToPoint(_sphereTransformArray[i], moveDuration));
        }

        yield return null;
    }

    public void MoveSphereToJar(Transform parent, float jumpPower, float moveDuration)
    {
        for (int i = 0; i < _sphereArray.Length; i++)
        {
            _sphereArray[i].MoveSphereToJar(parent, jumpPower, moveDuration);
        }
    }

    public List<SphereBehaviour> GetSpheres()
    {
        List<SphereBehaviour> tempList = new List<SphereBehaviour>();
        for (int i = 0; i < _sphereArray.Length; i++)
        {
            if (_sphereArray[i].gameObject.activeInHierarchy)
            {
                tempList.Add(_sphereArray[i]);
            }
        }
        return tempList;
    }
}