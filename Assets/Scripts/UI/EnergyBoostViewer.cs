using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class EnergyBoostViewer : MonoBehaviour
{
    [SerializeField] private InputScreen _input;
    [SerializeField] private Slider _energyBar;
    [SerializeField] private float _maxEnergy;

    private float _energyDownRate;
    private float _currentEnergy;
    private bool _changedEnergy;
    private bool _isActive;
    private bool _isTouched;
    private Vector3 _startPosition;

    public event UnityAction<float> OnUseEnergy;

    private const float Use = 1f;
    private const float DontUse = 0f;
    private const float DontShowPosition = 200f;

    private void Awake()
    {
        _input.ChangedPosition += OnTouch;
    }

    private void OnDisable()
    {
        _input.ChangedPosition -= OnTouch;
        Properties.Instance.ChangedSpeed -= OnChangedEnergyRate;
    }

    private void Start()
    {
        _startPosition = transform.position;
        transform.position = new Vector3(_startPosition.x - DontShowPosition, _startPosition.y, _startPosition.z);

        Properties.Instance.ChangedSpeed += OnChangedEnergyRate;
        _currentEnergy = _maxEnergy;
        _energyDownRate = Properties.Instance.Speed;
        _energyBar.maxValue = _maxEnergy;
        _energyBar.value = _maxEnergy;
    }

    private void Update()
    {
        if (_isTouched)
            UseEnergy();

        if (_changedEnergy)
            ChangeEnergy();
    }

    public void SetActivate() => Activate();

    private void OnTouch(float direction)
    {
        if (_isActive == false || Time.timeScale == 0)
            return;

        if (direction == 0 & _currentEnergy > 0)
        {
            _isTouched = false;
            OnUseEnergy?.Invoke(DontUse);
        }
        else
        {
            _isTouched = true;
            OnUseEnergy?.Invoke(Use);
        }
    }

    private void ChangeEnergy()
    {
        if (_energyBar.value > _currentEnergy)
            _energyBar.value = Mathf.Lerp(_energyBar.value, _currentEnergy, _energyDownRate);
        else
            _changedEnergy = false;
    }

    private void OnChangedEnergyRate(float degreaceRate)
    {
        _energyDownRate -= 1 / Properties.Instance.Speed;
    }

    private void UseEnergy()
    {
        _currentEnergy -= 1 / Properties.Instance.Speed;
        _changedEnergy = true;

        if (_currentEnergy <= 0)
        {
            _isTouched = false;
            OnUseEnergy?.Invoke(DontUse);
        }
    }

    private void Activate()
    {
        transform.DOMove(_startPosition, Use).SetLoops(1);
        _isActive = true;
    }
}
