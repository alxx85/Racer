using UnityEngine;
using DG.Tweening;

public class SliceTextAnimation : MonoBehaviour
{
    [SerializeField] private float _timeRotate;
    [SerializeField] private Vector3 _angleRotate;
    [Range(0, 5)]
    [SerializeField] private float _timeToRemove;

    void Start()
    {
        RectTransform[] texts = GetComponentsInChildren<RectTransform>();

        foreach (var text in texts)
        {
            text.DOLocalRotate(_angleRotate, _timeRotate).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }

        if (_timeToRemove == 0)
            return;

        Destroy(gameObject, _timeToRemove);
    }
}
