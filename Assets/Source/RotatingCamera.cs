using UnityEngine;

public class RotatingCamera : MonoBehaviour
{
    [SerializeField] Vector3 _axis;
    void Update()
    {
        transform.Rotate(_axis, Time.deltaTime);
    }
}