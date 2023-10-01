using UnityEngine;
using UnityEngine.Serialization;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] Ship _shipPrefab;
    [SerializeField] Transform _shipParent;
    [FormerlySerializedAs("_boundsCollider")] [SerializeField] Collider _spawnCollider;
    [SerializeField] Collider _targetAreaCollider;
    
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
            Vector3 spawnPosition = Util.GetRandomPointInBounds(_spawnCollider);
            Vector3 localScale = _spawnCollider.transform.localScale;
            spawnPosition -= _spawnCollider.bounds.center / 4f;
            spawnPosition.x /= localScale.x;
            spawnPosition.z /= localScale.z;
            spawnPosition = _spawnCollider.transform.TransformPoint(spawnPosition);
            spawnPosition.y = 0f;
            
            Ship ship = Instantiate(_shipPrefab, spawnPosition, Quaternion.identity, _shipParent);
            if (ship is Enemy enemy)
            {
                enemy.TargetAreaCollider = _targetAreaCollider;
            }

            _spawnTimer = _spawnPeriod;
        }
    }
}