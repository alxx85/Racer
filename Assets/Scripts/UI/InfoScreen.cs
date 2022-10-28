using UnityEngine;
using DG.Tweening;

public class InfoScreen : MonoBehaviour
{
    [SerializeField] private Movement _car;
    [SerializeField] private float _showSpeedInSecond;
    [SerializeField] private float _waitingTime;

    private Vector3 _showPosition;
    private Vector3 _hidePosition;

    private const float DontShowPosition = 350f;

    private void Awake()
    {
        _showPosition = transform.position;
        _hidePosition = new Vector3(_showPosition.x, _showPosition.y - DontShowPosition, _showPosition.z);
        _car.OnEndRoad += OnShowInfo;
        transform.position = _hidePosition;
    }

    private void OnShowInfo()
    {
        ViewInfoField(_showPosition);
        ViewInfoField(_hidePosition, _waitingTime + _showSpeedInSecond);
    }

    private void ViewInfoField(Vector3 position, float delay = 0f)
    {
        transform.DOMove(position, _showSpeedInSecond).SetLoops(1).SetDelay(delay);
    }
}
