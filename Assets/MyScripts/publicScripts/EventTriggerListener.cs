using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
//IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler 

public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger {
    public delegate void VoidDelegate(GameObject go);
    
    public VoidDelegate onClick;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    
    public VoidDelegate onDrag;
    public VoidDelegate onBeginDrag;
    public VoidDelegate onEndDrag;

    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
            listener = go.AddComponent<EventTriggerListener>();

        return listener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick(gameObject);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null)
            onEnter(gameObject);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null)
            onExit(gameObject);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
            onDrag(gameObject);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null)
            onBeginDrag(gameObject);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null)
            onEndDrag(gameObject);
    }

}
