using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] ShipStance _targetShipStance;

    [SerializeField] int _projectileDamage = 1;
    [SerializeField] float _projectileSpeed = 1f;
    [SerializeField] float _range = 40f;
    [SerializeField] float _accuracy = 5f;
    [SerializeField] float _reloadTime = 1f;

    float _targetAcquireTimer;
    float _reloadTimer;

    Vector3 _targetPosition;
    bool _hasTarget;
    Collider[] _targetColliderBuffer = new Collider[100];

    void Start()
    {
        ResetTargetAcquireTimer();
    }

    public void Shoot()
    {
        if (_hasTarget && _reloadTimer >= _reloadTime)
        {
            Vector3 target = _targetPosition +
                             new Vector3(Random.Range(-_accuracy, _accuracy), 0f, Random.Range(-_accuracy, _accuracy));

            Projectile projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity, null);
            projectile.Setup(target, _targetShipStance, _projectileDamage, _projectileSpeed);
            _reloadTimer = 0f;
        }
    }

    void Update()
    {
        if (_reloadTimer < _reloadTime)
        {
            _reloadTimer += Time.deltaTime + Random.Range(0f, Time.deltaTime);
        }

        UpdateTarget();
    }

    void UpdateTarget()
    {
        if (_targetAcquireTimer > 0f)
        {
            _targetAcquireTimer -= Time.deltaTime;
            return;
        }

        _hasTarget = false;
        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, _range, _targetColliderBuffer);
        for (var i = 0; i < colliderCount; i++)
        {
            var ship = _targetColliderBuffer[i].GetComponent<Ship>();
            if (ship != null && (_targetShipStance == ship.Stance ||
                                 (_targetShipStance == ShipStance.Player && ship.Stance == ShipStance.Ally)))
            {
                _targetPosition = ship.transform.position;
                _hasTarget = true;
            }
        }

        ResetTargetAcquireTimer();
    }

    void ResetTargetAcquireTimer()
    {
        _targetAcquireTimer = Random.Range(1f, 2f);
    }

    void OnDrawGizmos()
    {
        Color color = _hasTarget ? Color.red : Color.magenta;
        color.a = .1f;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}