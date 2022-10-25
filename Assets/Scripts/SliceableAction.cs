using UnityEngine;

[RequireComponent(typeof(Sliceable))]
public class SliceableAction : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfxAction;
    [SerializeField, HideInInspector] private bool _isActive = false;

    private Sliceable _sliceable;

    private void Awake()
    {
        _sliceable = GetComponent<Sliceable>();
        _sliceable.Sliced += OnSliced;
    }

    private void Start()
    {
        if (_isActive == true)
        {
            SliceableMovement.Instance.AddSlicedObject(this);
        }
            
        _isActive = !_isActive;
    }

    private void OnSliced(Vector3 position)
    {
        if (_vfxAction == null)
            return;

        Vector3 particlePosition = position;
        particlePosition.x = transform.position.x;
        particlePosition.y += 0.01f;

        ParticleSystem vfx = Instantiate(_vfxAction, particlePosition, Quaternion.identity);
        vfx.Play();
    }
}
