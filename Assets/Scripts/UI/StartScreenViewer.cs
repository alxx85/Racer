using UnityEngine;
using UnityEngine.EventSystems;

public class StartScreenViewer : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RaycastTarget _input;

    private const int StartTime = 1;
    private const int StopTime = 0;

    private void Awake()
    {
        Time.timeScale = StopTime;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            StartGame();
    }

    private void StartGame()
    {
        _input.enabled = true;
        Time.timeScale = StartTime;
        Destroy(gameObject);
    }
}
