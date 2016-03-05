using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Droppable : MonoBehaviour, ISelectHandler, IDeselectHandler,IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler{

    public void OnSelect(BaseEventData baseEventData)
    {
        
    }

    public void OnDeselect(BaseEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var outstring = eventData.pointerEnter != null ? ("OnPointerEnter + " + eventData.pointerEnter.name) : "OnPointerEnter";
                Debug.Log(outstring);

        if (eventData.selectedObject == null)
            return;

        Draggable d = eventData.selectedObject.GetComponent<Draggable>();
        if (d != null)
        {
            d.placeholderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var outstring = eventData.pointerEnter != null ? "OnPointerExit + " + eventData.pointerEnter.name : "OnPointerEnter";
                Debug.Log(outstring);

        if (eventData.selectedObject == null)
            return;

        Draggable d = eventData.selectedObject.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform)
        {
            d.placeholderParent = d.parentToReturnTo;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

}
