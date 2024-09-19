using UnityEngine;
using UnityEngine.EventSystems;

public class TestButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    private bool isScrollButton = false;

    private bool isClick = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("down");
        isClick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isClick)
        {
            return;
        }

        if (!isScrollButton)
        {
            Debug.Log("up");
            isClick = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClick)
        {
            return;
        }

        if (isScrollButton)
        {
            Debug.Log("click");
            isClick = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isClick)
        {
            isClick = false;
            Debug.Log("exit");
        }
    }
}
