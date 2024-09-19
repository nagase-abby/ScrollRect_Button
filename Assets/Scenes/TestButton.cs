using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestButton : MonoBehaviour
{
    private EventTrigger trigger = null;

    // private void Awake()
    // {
    //     TryGetComponent(out trigger);
    //     SetOnPointerUp();
    // }

    // public void SetOnPointerUp()
    // {
    //     EventTrigger.Entry entry = new EventTrigger.Entry();
    //     entry.eventID = EventTriggerType.PointerUp;
    //     entry.callback.AddListener((eventDate) => { Log(); });
    //     trigger.triggers.Add(entry);
    // }

    // private void Log()
    // {
    //     Debug.Log("aaaa");
    // }
}
