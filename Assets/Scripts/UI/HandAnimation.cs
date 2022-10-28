using UnityEngine;
using DG.Tweening;

public class HandAnimation : MonoBehaviour
{
    [SerializeField] private float _minSize;
    [SerializeField] private float _scalingTime;
    private void Start()
    {
        transform.DOScale(_minSize, _scalingTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
