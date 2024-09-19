// reference: https://hexadrive.jp/hexablog/program/15948/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomScrollRect : ScrollRect
{
    private List<RaycastResult> _raycastResults = new List<RaycastResult>();

    private GameObject _lastSelectedObj = null;

    private Vector2 _startPoint = Vector2.zero;

    [Header("ボタンタップ時しつつ、スクロールした時にボタンクリックが反応する閾値")]
    [SerializeField, Range(0.0f, 1000.0f)]
    private float _dragThrehold = 0;

    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        var lastObj = EventSystem.current.currentSelectedGameObject;

        base.OnInitializePotentialDrag(eventData);
        if (!CheckValidEvent(eventData)) return;

        // 直前の選択オブジェクトを記憶
        _lastSelectedObj = null;
        if (lastObj != null)
        {
            _lastSelectedObj = lastObj;
            _startPoint = eventData.position;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (!CheckValidEvent(eventData)) return;

        // 直前の選択オブジェクトがあればイベント通知
        if (_lastSelectedObj != null)
        {
            EventSystem.current.RaycastAll(eventData, _raycastResults);
            var raycast = GetFirstRaycast(_raycastResults);
            _raycastResults.Clear();

            if (Vector2.Distance(_startPoint, eventData.position) < _dragThrehold)
            {
                NotifyClickEvent(raycast, eventData);
            }
        }

        _lastSelectedObj = null;
    }

    private bool CheckValidEvent(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return false;
        }

        return IsActive();
    }

    private RaycastResult GetFirstRaycast(IEnumerable<RaycastResult> results)
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

    private void NotifyClickEvent(RaycastResult raycast, PointerEventData original)
    {
        if (_lastSelectedObj == null) return;

        var target = raycast.gameObject;
        if (target == null) return;

        // イベントハンドラを取得
        var handler = ExecuteEvents.GetEventHandler<IEventSystemHandler>(target);
        if (handler == null) return;
        if (handler != _lastSelectedObj) return;

        // 通知用のイベントデータ生成
        var eventData = CopyEventData(original);
        eventData.pointerCurrentRaycast = raycast;
        eventData.pointerPressRaycast = raycast;
        eventData.pointerPress = handler;
        eventData.rawPointerPress = target;
        eventData.eligibleForClick = true;

        // イベント実行
        if (ExecuteEvents.Execute(eventData.pointerPress, eventData, ExecuteEvents.pointerClickHandler))
        {
            // イベント発火する場合はスクロールを止める
            velocity = Vector2.zero;
        }
    }

    private PointerEventData CopyEventData(PointerEventData original)
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
}