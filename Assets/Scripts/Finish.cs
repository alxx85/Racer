using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Finish : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _vfxFinished;

    private WaitForSeconds _delay = new WaitForSeconds(1f);

    public event UnityAction Finished;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarBooster>(out CarBooster car))
        {
            Finished?.Invoke();
            StartCoroutine(StartVFX());
        }
    }

    private IEnumerator StartVFX()
    {
        Properties.Instance.Finishes(_delay);
        yield return _delay;

        foreach (var vfx in _vfxFinished)
        {
            vfx.Play();
        }
    }
}
