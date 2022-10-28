using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [SerializeField] private float _pickupBoostOffset;
    [SerializeField] private Movement _car;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private Vector3 _offset;
    private Vector3 _currentOffset;
    private bool _isInAir;
    private float _pickupSpeed = 3f;

    private const float MoveDelay = 1f;
    private const float CameraAirOffset = 2.5f;
    private const float Zero = 0f;

    private void Awake()
    {
        _offset = _virtualCamera.Follow.localPosition;
        _currentOffset = _offset;
    }

    private void Start()
    {
        Properties.Instance.ChangedSpeed += OnChangedCarSpeed;
        _car.OnEndRoad += OnEndRoad;
    }

    private void OnDisable()
    {
        Properties.Instance.ChangedSpeed -= OnChangedCarSpeed;
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
        _currentOffset = new Vector3(Zero, _currentOffset.y, Zero);// -_pickupBoostOffset);
        _isInAir = true;
        _virtualCamera.LookAt = null;

        Properties.Instance.Finish.Finished += OnFinished;
    }

    private void OnFinished()
    {
        transform.SetParent(_car.transform);
        _currentOffset = new Vector3(Zero, Zero, CameraAirOffset);
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

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(MoveDelay);
    }

    private Coroutine StartMove(float stoppingDistance = Zero)
    {
        return StartCoroutine(MoveTo(stoppingDistance));
    }

    private IEnumerator MoveTo(float stoppingDistance)
    {
        Vector3 newOffset = new Vector3(Zero, _offset.y, -stoppingDistance);
        float speed = _pickupSpeed * Time.deltaTime;

        while (Vector3.Distance(_virtualCamera.Follow.localPosition, newOffset) > Zero)
        {
            _virtualCamera.Follow.localPosition = Vector3.MoveTowards(_virtualCamera.Follow.localPosition, newOffset, speed);
            yield return null;
        }
    }
}
