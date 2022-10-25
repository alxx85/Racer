using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class RezultScreen : MonoBehaviour
{
    [SerializeField] private float _useTime;
    [SerializeField] private Button _continueButton;

    private Vector3 _startPosition;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(Continue);
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(Continue);
    }

    void Start()
    {
        _startPosition = transform.position;
        transform.position = new Vector3(-Screen.width, _startPosition.y, _startPosition.z);
    }

    public void Show()
    {
        transform.DOMove(_startPosition, _useTime).SetLoops(1);
    }

    private void Continue()
    {
        SceneManager.LoadScene(0);
    }
}
