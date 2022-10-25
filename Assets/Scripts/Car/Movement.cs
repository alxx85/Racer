using Dreamteck.Splines;
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
    private float _startSpeed;
    private bool _isFlying = false;
    private float _currentAngleRotation;
    private bool _isTouched;
    private CarBooster _booster;
    private float _currentWingSize;

    private const float PhysicCoefficient = 2.0f;

    public event UnityAction OnEndRoad;

    private void Awake()
    {
        _splineFollower = GetComponent<SplineFollower>();
        _booster = Properties.Instance.CarBooster;
        _splineFollower.followSpeed = Properties.Instance.Speed;
        _startSpeed = Properties.Instance.Speed;
    }

    private void OnEnable()
    {
        _inputs.ChangedPosition += OnChangeDirection;
        _booster.ChangedSpeed += OnChangedSpeed;
        _splineFollower.onEndReached += OnAirStart;
    }

    private void OnDisable()
    {
        _inputs.ChangedPosition -= OnChangeDirection;
        Properties.Instance.EnergyBoost.OnUseEnergy -= OnChangeDirection;
        _booster.ChangedSpeed -= OnChangedSpeed;
        _splineFollower.onEndReached -= OnAirStart;
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

    public void StopMove()
    {
        _isFlying = false;
    }

    private void OnAirStart(double index)
    {
        _splineFollower.enabled = false;
        _carBody.localPosition = Vector3.zero;
        _carBody.localRotation = Quaternion.identity;
        _currentWingSize = _booster.WingSize;
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
            transform.Rotate(Vector3.right, Physics.gravity.y * Time.deltaTime * (Properties.Instance.Speed - _startSpeed));
    }

    private void FlightPhysics()
    {
        Vector3 gravity = Physics.gravity * Time.deltaTime;
        transform.Translate(gravity);
        
        if (transform.rotation.x < _currentWingSize * -Mathf.Deg2Rad)
        {
            transform.Rotate(Vector3.left, gravity.y * PhysicCoefficient);
        }
    }

    private void OnChangedSpeed(float speed)
    {
        Properties.Instance.AddSpeed(speed);
        _splineFollower.followSpeed = Properties.Instance.Speed;
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
}
