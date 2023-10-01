using UnityEngine;
using UnityEngine.Serialization;

public class ShipSpawner : Ship
{
    [SerializeField] Ship _shipPrefab;
    [SerializeField] Transform _shipParent;

    [FormerlySerializedAs("_boundsCollider")] [SerializeField]
    Collider _spawnCollider;

    [SerializeField] Collider _targetAreaCollider;

    [SerializeField] float _spawnPeriod = 5f;

    float _spawnTimer;

    Player _player;

    protected override void Awake()
    {
        _player = FindFirstObjectByType<Player>();
        ParticleManager = FindFirstObjectByType<ParticleManager>();
        AudioManager = FindFirstObjectByType<AudioManager>();
    }

    void Start()
    {
        _spawnTimer = _spawnPeriod;
    }

    protected override void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0f)
        {
            if (_player != null)
            {
                Vector3 spawnPosition = Util.GetRandomPointInBounds(_spawnCollider);
                Vector3 localScale = _spawnCollider.transform.localScale;
                if (_spawnCollider is BoxCollider)
                {
                    spawnPosition -= _spawnCollider.bounds.center / 4f;
                }
                spawnPosition.x /= localScale.x;
                spawnPosition.z /= localScale.z;
                spawnPosition = _spawnCollider.transform.TransformPoint(spawnPosition);
                spawnPosition.y = 0f;

                Ship ship = Instantiate(_shipPrefab, spawnPosition, Quaternion.identity, _shipParent);
                if (ship is Enemy enemy)
                {
                    enemy.TargetAreaCollider = _targetAreaCollider;
                }
            }

            _spawnTimer = _spawnPeriod;
        }
    }

    public override void Destroy()
    {
        if (_player != null)
        {
            _player.TargetEnemySpawners.Remove(this);
        }
        ParticleManager.EmitExplosionLarge(transform.position);
        base.Destroy();
    }
}