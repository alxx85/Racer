using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputScreen : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private float _touchPosition;

    public event UnityAction<float> ChangedPosition;

    private const int Half = 2;

    public void OnDrag(PointerEventData eventData)
    {
        _touchPosition = eventData.position.x;
        ChangedPosition?.Invoke(TouchDirection(_touchPosition));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchPosition = eventData.pressPosition.x;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ChangedPosition?.Invoke(0);
    }

    private float TouchDirection(float position)
    {
        float halfScreen = Screen.width / Half;
     
        return (position - halfScreen)/halfScreen;
    }
}
