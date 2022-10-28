using UnityEngine;

public class CarBooster : MonoBehaviour
{
    [SerializeField] private Transform _wings;
    [SerializeField] private Transform[] _rockets;

    private ParticleSystem[] _fireRockets;
    private int _currentAddingRockets = 0;

    public float WingSize => _wings.localScale.x;

    private const float MaxFire = 0.12f;
    private const float MinFire = 0.08f;
    
    public void AddWing(float size)
    {
        Vector3 newScale = new Vector3(_wings.localScale.x + size, _wings.localScale.y + size, _wings.localScale.z + size);
        _wings.localScale = newScale;
    }

    public void AddRocket(float speed)
    {
        _rockets[_currentAddingRockets].gameObject.SetActive(true);
        _fireRockets = GetComponentsInChildren<ParticleSystem>();

        if (_currentAddingRockets <= _rockets.Length)
            _currentAddingRockets++;

        Properties.Instance.AddSpeed(speed);
    }

    public void MaxFireOn()
    {
        foreach (var fireRocket in _fireRockets)
        {
            fireRocket.startLifetime = MaxFire;
        }
    }

    public void MaxFireOff()
    {
        foreach (var fireRocket in _fireRockets)
        {
            fireRocket.startLifetime = MinFire;
        }
    }
}
