using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] protected int _hitPoints = 2;
    [SerializeField] ShipStance _shipStance;

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

    void Destroy()
    {
        Destroy(gameObject);
    }
}