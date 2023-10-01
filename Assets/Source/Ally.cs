using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ally : Ship
{
    [SerializeField] Material _neutralMaterial;
    [SerializeField] Material _allyMaterial;

    [SerializeField] float _captureTime;

    [SerializeField] List<Weapon> _weapons;

    Vector3 _formationOffset;

    bool _isCapturing;

    Vector3 _targetPositionFluctuation;
    Vector3 _currentPositionFluctuation;

    Player _player;
    Transform _playerTransform;
    MeshRenderer _renderer;

    Vector3 TargetPosition => _playerTransform.position + _formationOffset;
    Quaternion TargetRotation => _playerTransform.rotation;

    protected override void Awake()
    {
        base.Awake();

        _renderer = GetComponent<MeshRenderer>();

        _player = FindFirstObjectByType<Player>();
        _playerTransform = _player.transform;
    }

    protected override void Update()
    {
        base.Update();

        if (Random.value > .9f)
        {
            _targetPositionFluctuation = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        }

        if (Stance == ShipStance.Ally && !_isCapturing)
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

    public void Capture()
    {
        if (Stance == ShipStance.Neutral)
        {
            _player.AllyList.Add(this);
            float offsetDistance = 2f + _player.AllyCount / 2f;
            _formationOffset = Util.GetRandomVector3(-offsetDistance, offsetDistance);

            Stance = ShipStance.Ally;
            _renderer.material = _allyMaterial;

            StartCoroutine(AnimateCapture());
        }
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

    public override void Destroy()
    {
        if (_player != null)
        {
            _player.AllyList.Remove(this);
        }

        base.Destroy();
    }
}