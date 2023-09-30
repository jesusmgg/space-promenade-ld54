using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _acceleration = 1f;
    [SerializeField] float _deceleration = 1f;
    [SerializeField] float _topSpeed = 2f;

    [SerializeField] Collider _mouseCollider;

    [SerializeField] float _captureRadius = 10f;

    [SerializeField] float _minFov = 25f;
    [SerializeField] float _maxFov = 100f;
    [SerializeField] float _fovPerAlly = 2.5f;
    [SerializeField] float _fovSpeed = 1f;

    float _normalizedFov;

    float _speed;
    Vector3 _mousePos;

    int _allies;

    Camera _mainCamera;
    readonly Collider[] _captureBuffer = new Collider[10];

    public float Speed => _speed;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        UpdateTransform();
        UpdateCaptures();
    }

    void UpdateTransform()
    {
        // Get mouse position
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (_mouseCollider.Raycast(ray, out RaycastHit hitData, 1000))
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

    void UpdateCaptures()
    {
        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, _captureRadius, _captureBuffer);

        for (var i = 0; i < colliderCount; i++)
        {
            var ally = _captureBuffer[i].GetComponent<Ally>();
            if (ally != null)
            {
                if (ally.Capture())
                {
                    _allies += 1;
                }
            }
        }

        float currentNormalizedFov = (_fovPerAlly / _maxFov) * _allies;
        _normalizedFov = Mathf.MoveTowards(_normalizedFov, currentNormalizedFov, _fovSpeed * Time.deltaTime);
        _mainCamera.fieldOfView = Mathf.Lerp(_minFov, _maxFov, _normalizedFov);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _captureRadius);
    }
}