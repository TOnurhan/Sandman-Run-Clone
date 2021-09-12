using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SphereBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    public event Action SphereBroken;

    public Rigidbody GetRigidbody() => _rigidBody;

    public IEnumerator SelfDisable(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    }

    public void MoveSphereToJar(Transform parent, float jumpPower, float moveDuration)
    {
        transform.SetParent(parent);
        transform.DOLocalJump(Vector3.zero, jumpPower, 1, moveDuration);
    }

    public IEnumerator MoveToPoint(Transform target, float moveDuration)
    {
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, target.position, time / moveDuration);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Breaker")) 
        {
            SphereBroken?.Invoke();
        }
    }
}
