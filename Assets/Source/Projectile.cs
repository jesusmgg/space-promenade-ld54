using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _maxLifeTimeSeconds = 10;
    [SerializeField] ShipStance _targetShipStance;

    int _damage;
    float _speed;
    Vector3 _direction;
    float _lifeTimeLeft;
    bool _isSetup;

    SphereCollider _collider;
    Collider[] _collisionBuffer = new Collider[10];

    void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        Invoke(nameof(Destroy), _maxLifeTimeSeconds);
    }

    void Update()
    {
        if (_isSetup)
        {
            Vector3 translation = _direction * (_speed * Time.deltaTime);
            transform.Translate(translation);
            
            CheckCollision();
        }
    }

    /// <summary>
    /// Sets up the projectile and activates it.
    /// </summary>
    public void Setup(Vector3 targetPosition, ShipStance targetShipStance, int damage, float speed)
    {
        _direction = (targetPosition - transform.position).normalized;
        _targetShipStance = targetShipStance;
        _damage = damage;
        _speed = speed;
        _isSetup = true;
    }

    void CheckCollision()
    {
        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, _collider.radius, _collisionBuffer);
        for (var i = 0; i < colliderCount; i++)
        {
            Collider collider = _collisionBuffer[i];
            if (collider.CompareTag("Wall"))
            {
                Destroy();
                return;
            }
            
            var ship = collider.GetComponent<Ship>();
            if (ship != null && (_targetShipStance == ship.Stance ||
                                 (_targetShipStance == ShipStance.Player && ship.Stance == ShipStance.Ally)))
            {
                ship.HitPoints -= _damage;
                Destroy();
            }
        }
    }

    void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}