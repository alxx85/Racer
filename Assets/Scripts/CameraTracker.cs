using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _pickupBoostOffset;
    [SerializeField] private Movement _car;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private Vector3 _offset;
    private Vector3 _currentOffset;
    private CarBooster _booster;
    private bool _isInAir;

    private const float Factor = 2f;
    private const float CameraAirOffset = 3f;
    private const float Zero = 0f;

    private void Awake()
    {
        _offset = _virtualCamera.Follow.localPosition;
        _currentOffset = _offset;
        _booster = _player.GetComponent<CarBooster>();
        Debug.Log(_virtualCamera.Follow.localPosition);
    }

    private void OnEnable()
    {
        _booster.ChangedSpeed += OnChangedCarSpeed;
        _car.OnEndRoad += OnEndRoad;
    }

    private void OnDisable()
    {
        _booster.ChangedSpeed -= OnChangedCarSpeed;
        _car.OnEndRoad -= OnEndRoad;
        Properties.Instance.Finish.Finished -= OnFinished;
    }

    private void LateUpdate()
    {

        if (_isInAir)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _currentOffset, Properties.Instance.Speed * Time.deltaTime);
        }
    }

    private void OnEndRoad()
    {
        StopCoroutine(PickupBoost());
        _currentOffset = new Vector3(Zero, CameraAirOffset, -(_pickupBoostOffset * Factor));
        _isInAir = true;
        _virtualCamera.LookAt = null;

        Properties.Instance.Finish.Finished += OnFinished;
    }

    private void OnFinished()
    {
        _currentOffset = new Vector3(Zero, -Factor, CameraAirOffset * Factor);
    }

    private void OnChangedCarSpeed(float speed)
    {
        StartCoroutine(PickupBoost());
    }

    private IEnumerator PickupBoost()
    {
        yield return StartMove(_pickupBoostOffset);
        yield return Delay();
        yield return StartMove();
    }

    private IEnumerator Delay(float second = Factor)
    {
        yield return new WaitForSeconds(second);
    }

    private Coroutine StartMove(float stoppingDistance = Zero)
    {
        return StartCoroutine(MoveTo(stoppingDistance));
    }

    private IEnumerator MoveTo(float stoppingDistance)
    {
        Vector3 newOffset = new Vector3(Zero, _offset.y, -stoppingDistance);
        float speed = Factor * Time.deltaTime;

        while (Vector3.Distance(_virtualCamera.Follow.localPosition, newOffset) > Zero)
        {
            _virtualCamera.Follow.localPosition = Vector3.MoveTowards(_virtualCamera.Follow.localPosition, newOffset, speed);
            yield return null;
        }
    }
}
