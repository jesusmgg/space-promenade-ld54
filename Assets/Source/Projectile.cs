using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _maxLifeTimeSeconds = 10;
    [SerializeField] ShipStance _targetShipStace;

    int _damage;
    float _speed;
    Vector3 _targetPosition;
    float _lifeTimeLeft;
    bool _isSetup;

    Collider _collider;

    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    void Start()
    {
        Invoke(nameof(Destroy), _maxLifeTimeSeconds);
    }

    void Update()
    {
        if (_isSetup)
        {
            float maxPosDelta = _speed * Time.deltaTime;
            Vector3 newPos = Vector3.MoveTowards(transform.position, _targetPosition, maxPosDelta);
            transform.position = newPos;

            if (Vector3.Distance(transform.position, _targetPosition) < float.Epsilon)
            {  
                Destroy();
            }
        }
    }

    /// <summary>
    /// Sets up the projectile and activates it.
    /// </summary>
    public void Setup(Vector3 targetPosition, ShipStance targetShipStance, int damage, float speed)
    {
        _targetPosition = targetPosition;
        _targetShipStace = targetShipStance;
        _damage = damage;
        _speed = speed;
        _isSetup = true;
    }

    void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}