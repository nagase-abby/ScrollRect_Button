using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomScrollRect : ScrollRect
{
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        var lastObj = EventSystem.current.currentSelectedGameObject;

        base.OnInitializePotentialDrag(eventData);
        if (!checkValidEvent(eventData)) return;

        // 直前の選択オブジェクトを記憶
        _lastSelectedObj = null;
        if (lastObj != null)
        {
            _lastSelectedObj = lastObj;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (!checkValidEvent(eventData)) return;

        // 直前の選択オブジェクトがあればイベント通知
        if (_lastSelectedObj != null)
        {
            EventSystem.current.RaycastAll(eventData, _raycastResults);
            var raycast = getFirstRaycast(_raycastResults);
            _raycastResults.Clear();

            notifyClickEvent(raycast, eventData);
        }

        _lastSelectedObj = null;
    }

    protected static RaycastResult getFirstRaycast(IEnumerable<RaycastResult> results)
    {
        foreach (var result in results)
        {
            if (result.gameObject != null)
            {
                return result;
            }
        }
        return new RaycastResult();
    }

    protected void notifyClickEvent(RaycastResult raycast, PointerEventData original)
    {
        if (_lastSelectedObj == null) return;

        var target = raycast.gameObject;
        if (target == null) return;

        // イベントハンドラを取得
        var handler = ExecuteEvents.GetEventHandler<IEventSystemHandler>(target);
        if (handler == null) return;
        if (handler != _lastSelectedObj) return;

        // 通知用のイベントデータ生成
        var eventData = copyEventData(original);
        eventData.pointerCurrentRaycast = raycast;
        eventData.pointerPressRaycast = raycast;
        eventData.pointerPress = handler;
        eventData.rawPointerPress = target;
        eventData.eligibleForClick = true;

        // イベント実行
        ExecuteEvents.Execute(eventData.pointerPress, eventData, ExecuteEvents.pointerClickHandler);
    }

    protected bool checkValidEvent(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return false;
        }

        return IsActive();
    }

    protected PointerEventData copyEventData(PointerEventData original)
    {
        return new PointerEventData(EventSystem.current)
        {
            selectedObject = original.selectedObject,
            hovered = original.hovered,
            button = original.button,
            clickCount = original.clickCount,
            clickTime = original.clickTime,
            delta = original.delta,
            dragging = original.dragging,
            eligibleForClick = original.eligibleForClick,
            pointerCurrentRaycast = original.pointerCurrentRaycast,
            pointerDrag = original.pointerDrag,
            pointerEnter = original.pointerEnter,
            pointerId = original.pointerId,
            pointerPress = original.pointerPress,
            pointerPressRaycast = original.pointerPressRaycast,
            position = original.position,
            pressPosition = original.pressPosition,
            rawPointerPress = original.rawPointerPress,
            scrollDelta = original.scrollDelta,
            useDragThreshold = original.useDragThreshold,
        };
    }

    private static List<RaycastResult> _raycastResults = new List<RaycastResult>();

    private GameObject _lastSelectedObj = null;
}