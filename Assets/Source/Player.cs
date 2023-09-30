using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _acceleration = 1f;
    [SerializeField] float _deceleration = 1f;
    [SerializeField] float _topSpeed = 2f;
    
    [SerializeField] Collider _mouseCollider;

    [SerializeField] float _speed;
    
    Vector3 _mousePos;
    
    Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        // Get mouse position
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if(_mouseCollider.Raycast(ray, out RaycastHit hitData, 1000))
        {
            _mousePos = hitData.point;
        }
        
        // Move ship
        Vector3 flatMousePos = _mousePos;
        flatMousePos.y = 0f;
        Vector3 direction = (flatMousePos - transform.position).normalized;
        
        _speed += (_acceleration * Time.deltaTime * Input.GetAxis("Fire2"));
        _speed -= _deceleration * Time.deltaTime;
        _speed = Mathf.Clamp(_speed, 0f, _topSpeed);
        
        transform.Translate(direction * _speed, Space.World);
        
        // Rotate ship
        transform.LookAt(flatMousePos, Vector3.up);
    }
}