using UnityEngine;
using DG.Tweening;

public class ObjectRotatement : MonoBehaviour
{
    [SerializeField] private float _timeRotate;
    [SerializeField] private Vector3 _angleRotate;

    private void Start()
    {
        transform.DOLocalRotate(_angleRotate, _timeRotate, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
