using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Booster : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfxPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarBooster>(out CarBooster car))
        {
            GetBoost(car);
            VfxStart();
            Destroy(gameObject);
        }
    }

    public abstract void GetBoost(CarBooster car);

    private void VfxStart()
    {
        if (_vfxPickup == null)
            return;

        ParticleSystem vfx = Instantiate(_vfxPickup, transform.position, Quaternion.identity);
        vfx.Play();
    }
}
