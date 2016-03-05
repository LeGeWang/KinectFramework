using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IKinectDragMoveHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler{

    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;

    public void OnKinectDrag(PointerEventData eventData)
    {
        Debug.Log("On Kinect Move");
        this.transform.position = eventData.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("On Pointer Click");
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("On Select");
        parentToReturnTo = this.transform.parent;
        placeholderParent = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("On Deselect");
        this.transform.SetParent(placeholderParent);
        parentToReturnTo = null;
        placeholderParent = null;

        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    
    void Update()
    {
//        Debug.Log("Update");
    }

}