using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _target = default;
    [SerializeField] private Vector3 _offset = default;
    [SerializeField] private float _smoothDampValue = 0.125f;
    private Vector3 _smoothDampVelocity;
    private bool _isFollowing;

    public void IsFollowing(bool isFollowing) => _isFollowing = isFollowing;
    public void SetTarget(Transform newTarget) => _target = newTarget;

    void LateUpdate()
    {
        //if (_isFollowing)
        //{
            Vector3 desiredPosition = _target.position + _offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _smoothDampVelocity, _smoothDampValue);
            transform.position = smoothedPosition;
        //}
    }
}
