using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] protected int _hitPoints = 2;
    [SerializeField] ShipStance _shipStance;

    protected Collider Collider;
    float _collisionRadius;

    Collider[] _wallCollisionBuffer = new Collider[100];
    
    protected ParticleManager ParticleManager;

    public ShipStance Stance
    {
        get => _shipStance;
        protected set => _shipStance = value;
    }

    public int HitPoints
    {
        get => _hitPoints;
        set
        {
            _hitPoints = value;
            if (_hitPoints <= 0)
            {
                Destroy();
            }
        }
    }

    protected virtual void Awake()
    {
        Collider = GetComponent<Collider>();
        ParticleManager = FindFirstObjectByType<ParticleManager>();
        _collisionRadius = Collider != null ? Collider.bounds.extents.z : 0f;
    }

    protected virtual void Update()
    {
        // Check wall collision
        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, _collisionRadius, _wallCollisionBuffer);

        for (var i = 0; i < colliderCount; i++)
        {
            var wall = _wallCollisionBuffer[i].GetComponent<Wall>();
            if (wall != null)
            {
                Destroy();
            }
        }
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}