using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    [SerializeField] Vector3 _formationOffset;

    [SerializeField] Material _neutralMaterial;
    [SerializeField] Material _allyMaterial;

    [SerializeField] float _captureTime;

    [SerializeField] List<Weapon> _weapons;

    ShipStance _stance = ShipStance.Neutral;

    bool _isCapturing;

    Vector3 _targetPositionFluctuation;
    Vector3 _currentPositionFluctuation;

    Player _player;
    Transform _playerTransform;
    MeshRenderer _renderer;

    Vector3 TargetPosition => _playerTransform.position + _formationOffset;
    Quaternion TargetRotation => _playerTransform.rotation;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();

        _player = FindFirstObjectByType<Player>();
        _playerTransform = _player.transform;
    }

    void Update()
    {
        if (Random.value > .99f)
        {
            _targetPositionFluctuation = new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        }

        if (_stance == ShipStance.Ally && !_isCapturing)
        {
            Transform tr = transform;

            _currentPositionFluctuation =
                Vector3.MoveTowards(_currentPositionFluctuation, _targetPositionFluctuation, Time.deltaTime * 0.1f);
            tr.position = TargetPosition + _currentPositionFluctuation;
            tr.rotation = TargetRotation;

            foreach (Weapon weapon in _weapons)
            {
                weapon.Shoot();
            }
        }
    }

    /// <returns>true if the ally was captured.</returns>
    public bool Capture()
    {
        if (_stance == ShipStance.Neutral)
        {
            _stance = ShipStance.Ally;
            _renderer.material = _allyMaterial;

            StartCoroutine(AnimateCapture());
            return true;
        }

        return false;
    }

    IEnumerator AnimateCapture()
    {
        _isCapturing = true;

        Transform tr = transform;
        Vector3 initialPosition = tr.position;
        Quaternion initialRotation = tr.rotation;

        var elapsed = 0f;
        var progress = 0f;
        while (progress <= 1f)
        {
            transform.position = Vector3.Lerp(initialPosition, TargetPosition, progress);
            transform.rotation = Quaternion.Lerp(initialRotation, TargetRotation, progress);
            elapsed += Time.deltaTime;
            progress = elapsed / _captureTime;
            yield return null;
        }

        _isCapturing = false;
    }
}