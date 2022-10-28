using DG.Tweening;
using Dreamteck.Splines;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SplineFollower))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _sensitivityRotation;
    [SerializeField] private float _maxRoadLeftPosition;
    [SerializeField] private float _maxFlyingAngle;
    [SerializeField] private Transform _carBody;
    [SerializeField] private InputScreen _inputs;

    private SplineFollower _splineFollower;
    private float _moveUpSpeed = 5f;
    private bool _isFlying = false;
    private float _currentAngleRotation;
    private bool _isTouched;
    private CarBooster _booster;
    private float _pickupBoosts = 0f;

    private const float PhysicCoefficient = 2.0f;
    private const float AngleFinishRotate = 168f;
    private const float RotateSpeed = 1f;

    public event UnityAction OnEndRoad;

    private void Awake()
    {
        _splineFollower = GetComponent<SplineFollower>();
        _booster = Properties.Instance.CarBooster;
        _splineFollower.followSpeed = Properties.Instance.Speed;
    }

    private void OnEnable()
    {
        _inputs.ChangedPosition += OnChangeDirection;
        Properties.Instance.ChangedSpeed += OnChangedSpeed;
        _splineFollower.onEndReached += OnAirStart;
        Properties.Instance.Finished += OnFinished;
    }

    private void OnDisable()
    {
        _inputs.ChangedPosition -= OnChangeDirection;
        Properties.Instance.ChangedSpeed -= OnChangedSpeed;
        _splineFollower.onEndReached -= OnAirStart;
        Properties.Instance.Finished -= OnFinished;
        Properties.Instance.EnergyBoost.OnUseEnergy -= OnChangeDirection;
    }

    private void Update()
    {
        if (_isFlying)
        {
            MoveInAir();

            if (_isTouched)
            {
                MoveUp();
            }
        }
        else
        {
            ChangePosition();
        }
    }

    public void OnFinished()
    {
        _isFlying = false;
        //StartCoroutine(FinishRotate());
        FinishRotate();
    }

    private void OnAirStart(double index)
    {
        _splineFollower.enabled = false;
        _carBody.localPosition = Vector3.zero;
        _carBody.localRotation = Quaternion.identity;
        _isFlying = true;
        ChangeInput();
        OnEndRoad?.Invoke();
    }

    private void ChangeInput()
    {
        _inputs.ChangedPosition -= OnChangeDirection;
        Properties.Instance.EnergyBoost.OnUseEnergy += OnChangeDirection;
        Properties.Instance.EnergyBoost.SetActivate();
    }

    private void MoveUp()
    {
        if (transform.rotation.x * Mathf.Rad2Deg > -_maxFlyingAngle)
            transform.Rotate(Vector3.right, Physics.gravity.y * Time.deltaTime * (_moveUpSpeed * _pickupBoosts));
    }

    private void FlightPhysics()
    {
        Vector3 gravity = Physics.gravity * Time.deltaTime;
        transform.Translate(gravity * PhysicCoefficient);
        
        if (transform.rotation.x < _moveUpSpeed * -Mathf.Deg2Rad)
        {
            transform.Rotate(Vector3.left, gravity.y * PhysicCoefficient);
        }
    }

    private void OnChangedSpeed(float speed)
    {
        _splineFollower.followSpeed = Properties.Instance.Speed;
        _pickupBoosts++;
    }

    private void ChangePosition()
    {
        if (_currentAngleRotation == 0)
            return;

        float newXPosition;

        newXPosition = _currentAngleRotation * Properties.Instance.Speed * Time.deltaTime;
        newXPosition = Mathf.Clamp(_carBody.localPosition.x + newXPosition, -_maxRoadLeftPosition, _maxRoadLeftPosition);

        _carBody.localPosition = new Vector3(newXPosition, 0, 0);
    }

    private void OnChangeDirection(float direction)
    {
        if (_isFlying)
        {
            if (direction == 0)
            {
                _isTouched = false;
                _booster.MaxFireOff();
            }
            else
            {
                _isTouched = true;
                _booster.MaxFireOn();
            }
        }
        else
        {
            float angle;

            if (direction == 0)
            {
                angle = -_currentAngleRotation;
                _currentAngleRotation = 0;
            }
            else
            {
                float newAngle = (direction * _sensitivityRotation) / Mathf.Rad2Deg;
                angle = newAngle - _currentAngleRotation;
                _currentAngleRotation = newAngle;
            }

            _carBody.RotateAroundLocal(Vector3.up, angle);
            _carBody.RotateAroundLocal(Vector3.forward, angle / 2);
        }
    }

    private void MoveInAir()
    {
        FlightPhysics();
        transform.Translate(Vector3.forward * Properties.Instance.Speed * Time.deltaTime);
    }

    private void FinishRotate()
    {
        BoxCollider collider = _carBody.GetComponent<BoxCollider>();
        float carLenght = collider.bounds.size.z / 2;
        collider.enabled = false;

        Vector3 jumpHeight = new Vector3(_carBody.localPosition.x, carLenght, _carBody.localPosition.z);
        Vector3 rotationPosition = new Vector3(AngleFinishRotate, 0, 0);
        _carBody.DOLocalMove(jumpHeight, 0.3f).SetLoops(1);
        _carBody.DOLocalRotate(rotationPosition, RotateSpeed).SetLoops(1);
        _carBody.DOLocalMove(jumpHeight/2, 0.3f).SetLoops(1).SetDelay(0.4f);
    }
}
