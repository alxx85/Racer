using UnityEngine;

public class SliceTextViewer : MonoBehaviour
{
    [SerializeField] private SliceTextAnimation _sliceText;
    [SerializeField] private float _maxDistance;

    private Vector3 _currentPosition;
    private float _factor = 1.5f;

    public void Show()
    {
        float zPosition = Random.Range(-_factor, _factor);
        float yPosition = (_maxDistance - Mathf.Abs(zPosition)) / _factor;
        _currentPosition = new Vector3(0, yPosition, zPosition);

        var text = Instantiate(_sliceText);
        text.transform.parent = transform;
        text.transform.localPosition = _currentPosition * _maxDistance;
        text.transform.localScale = Vector3.one;
    }
}
