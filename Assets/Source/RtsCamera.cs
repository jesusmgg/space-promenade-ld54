using UnityEngine;

public class RtsCamera : MonoBehaviour
{
    [SerializeField] Transform _followTarget;

    Vector3 _offset;

    void Start()
    {
        RecalculateOffset();
    }

    void Update()
    {
        if (_followTarget != null)
        {
            transform.position = _followTarget.transform.position + _offset;
        }
    }

    void RecalculateOffset()
    {
        _offset = transform.position - _followTarget.transform.position;
    }
}