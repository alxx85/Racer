using UnityEngine;

public class SliceTextViewer : MonoBehaviour
{
    [SerializeField] private SliceTextAnimation _sliceText;
    [SerializeField] private float _maxDistance;

    private Vector3 _currentPosition;

    public void Show()
    {
        float zPosition = Random.Range(-1f, 1f);
        float yPosition = 1 - Mathf.Abs(zPosition);
        _currentPosition = new Vector3(0, yPosition, zPosition);

        var text = Instantiate(_sliceText);
        text.transform.parent = transform;
        text.transform.localPosition = _currentPosition * _maxDistance;
        text.transform.localScale = Vector3.one;
    }
}
