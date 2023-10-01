using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] Ship _shipPrefab;
    [SerializeField] Transform _shipParent;
    [SerializeField] Collider _boundsCollider;
    
    [SerializeField] float _spawnPeriod = 5f;

    float _spawnTimer;

    void Start()
    {
        _spawnTimer = _spawnPeriod;
    }

    void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0f)
        {
            Vector3 position = Util.GetRandomPointInBounds(_boundsCollider);
            Instantiate(_shipPrefab, position, Quaternion.identity, _shipParent);
            _spawnTimer = _spawnPeriod;
        }
    }
}