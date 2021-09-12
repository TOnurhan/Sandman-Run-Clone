using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartManager : MonoBehaviour
{
    [SerializeField] private List<SphereGroupManager> _sphereGroupList;
    public event Action<bool> GroupBroken;
    public event Action GroupHealing;
    [SerializeField] private bool coreBodyPart = false;

    private void Broken() => GroupBroken?.Invoke(coreBodyPart);
    private void Healing() => GroupHealing?.Invoke();

    private void Awake()
    {
        for (int i = 0; i < _sphereGroupList.Count; i++)
        {
            _sphereGroupList[i].GroupBroken += Broken;
            _sphereGroupList[i].GroupHealing += Healing;
        }
    }

    public void RegroupSphereGroups(Vector3 startPosition, float moveDuration, float waitTimeBetweenSpheres)
    {
        StartCoroutine(RegroupSphereGroupsCoroutine(startPosition, moveDuration, waitTimeBetweenSpheres));
    }

    private IEnumerator RegroupSphereGroupsCoroutine(Vector3 startPosition, float moveDuration, float waitTimeBetweenSpheres)
    {
        for (int i = 0; i < _sphereGroupList.Count; i++)
        {
            if (_sphereGroupList[i].IsEmpty())
            {
                StartCoroutine(_sphereGroupList[i].Regroup(startPosition, moveDuration, waitTimeBetweenSpheres));
            }
            yield return new WaitForSeconds(waitTimeBetweenSpheres);
        }
        GroupHealing?.Invoke();
    }

    public int GetCount()
    {
        int totalCount = 0;
        for (int i = 0; i < _sphereGroupList.Count; i++)
        {
            if (!_sphereGroupList[i].IsEmpty())
            {
                totalCount++;
            }
        }
        return totalCount;
    }

    public bool BodyPartIsAllEmpty()
    {
        for (int i = 0; i < _sphereGroupList.Count; i++)
        {
            if (!_sphereGroupList[i].IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    public void MoveSpheresToJar(Transform parent, float jumpPower, float moveDuration)
    {
        for (int i = 0; i < _sphereGroupList.Count; i++)
        {
            _sphereGroupList[i].MoveSphereToJar(parent, jumpPower, moveDuration);
        }
    }

    public List<SphereBehaviour> GetSpheres()
    {
        List<SphereBehaviour> tempList = new List<SphereBehaviour>();
        for (int i = 0; i < _sphereGroupList.Count; i++)
        {
            tempList.AddRange(_sphereGroupList[i].GetSpheres());
        }
        return tempList;
    }
}
