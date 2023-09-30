using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] ShipStance _targetShipStance;

    [SerializeField] int _projectileDamage = 1;
    [SerializeField] float _projectileSpeed = 1f;
    [SerializeField] float _accuracy = 5f;
    [SerializeField] float _reloadTime = 1f;

    float _reloadTimer;

    public void Shoot(Vector3 targetPosition)
    {
        if (_reloadTimer >= _reloadTime)
        {
            Vector3 target = targetPosition +
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
    }
}