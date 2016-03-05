using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace UnityEngine.EventSystems
{
    [AddComponentMenu("Event/Kinect Input Module")]
    public class KinectPointerInputModule : PointerInputModule
    {
        public const int kKinectLeftHandId = -1;
        public const int kKinectRightHandId = -2;

        public const float enterPressedTime = 0.8f;
        public const float durationClickTime = 0.7f;

        private GameObject eligibleGoForClick = null;

        private float enterTime = float.MaxValue;
        private float pressedTime = float.MaxValue;


        protected KinectPointerInputModule()
        { }

        protected virtual PointerEventData GetKinectPointerEventData()
        {
            PointerEventData eventData;
            var created = GetPointerData(kKinectLeftHandId, out eventData, true);

            eventData.Reset();

            if (created)
                eventData.position = Input.mousePosition;

            Vector2 pos = Input.mousePosition;
            eventData.delta = pos - eventData.position;
            eventData.position = pos;
            eventData.scrollDelta = Vector2.zero;
            eventSystem.RaycastAll(eventData, m_RaycastResultCache);
            var raycast = FindFirstRaycast(m_RaycastResultCache);
            eventData.pointerCurrentRaycast = raycast;
            m_RaycastResultCache.Clear();

            return eventData;
        }

        public override void Process()
        {
            var pointerData = GetKinectPointerEventData();

            var pressed = Input.GetMouseButtonDown(0);
            var released = Input.GetMouseButtonUp(0);

            GetKinectStata(pointerData, out pressed, out released);

            ProcessTouchPress(pointerData, pressed, released);

            if (!released)
            {
                ProcessMove(pointerData);
                ProcessDrag(pointerData);

                ProcessKinectDrag(pointerData);
            }
            else
                RemovePointerData(pointerData);

            //if (pointerData.selectedObject != null)
            //    Debug.Log(pointerData.selectedObject.name);
        }

        private void ProcessKinectDrag(PointerEventData pointerEvent)
        {
            var selectedGo = pointerEvent.selectedObject;

            if (selectedGo != null)
            {
                ExecuteEvents.Execute(selectedGo, pointerEvent, KinectModuleEvent.kinectMoveHandler);
            }      
        }

        private void GetKinectStata(PointerEventData pointerEvent, out bool pressed, out bool released)
        {
            var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

            // initial the bool value
            pressed = false;
            released = false;

            //var pressedGo = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);
            var pressedGo = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

            if (pointerEvent.selectedObject != null && pointerEvent.selectedObject == pressedGo)
                return;

            if(pressedGo == null)
            {
                eligibleGoForClick = null;
                enterTime = float.MaxValue;
                pressedTime = float.MaxValue;
                return;
            }

            if(eligibleGoForClick == null && pressedGo!=null)
            {
                eligibleGoForClick = pressedGo;
                enterTime = Time.unscaledTime;
//                Debug.Log("++++++Enter " + eligibleGoForClick.name);
                return;
            }

            if (eligibleGoForClick != null && eligibleGoForClick == pressedGo)
            {
                // kinect pointer down
                if (Time.unscaledTime - enterTime > enterPressedTime)
                {
                    enterTime = float.MaxValue;
                    pressedTime = Time.unscaledTime;
                    pressed = true;
//                    Debug.Log("++++++Down");
                    return;
                }
                else if (Time.unscaledTime - pressedTime > durationClickTime)  // kinect pointer up
                {
                    released = true;
//                    Debug.Log("+++++Click");
                }
                else
                    return;
            }

            eligibleGoForClick = null;
            enterTime = float.MaxValue;
            pressedTime = float.MaxValue;

        }

        private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
        {
            var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

            // pointer down notification
            if(pressed)
            {
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

                DeselectIfSelectionChanged(currentOverGo, pointerEvent);

                if(pointerEvent.pointerEnter!=currentOverGo)
                {
                    // send a pointer enter to the touched element if it isnt the one to select...
                    HandlePointerExitAndEnter(pointerEvent, currentOverGo);
                    pointerEvent.pointerEnter = currentOverGo;
                }

                // search for the control that will receive the press
                // if we cant find a press handler set the press
                // handler to be that would receive a click
                var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);


                // didnt find a press handler... search for a click handler
                if (newPressed == null)
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                float time = Time.unscaledTime;

                if(newPressed == pointerEvent.lastPress)
                {
                    var diffTime = time - pointerEvent.clickTime;
                    if (diffTime < 0.3f)
                        ++pointerEvent.clickCount;
                    else
                        pointerEvent.clickCount = 1;

                    pointerEvent.clickTime = time;
                }
                else
                {
                    pointerEvent.clickCount = 1;
                }
                
                pointerEvent.pointerPress = newPressed;
                pointerEvent.rawPointerPress = currentOverGo;

                pointerEvent.clickTime = time;
            }

            // PointerUp notification
            if(released)
            {
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                // see if we mouse up on the same element that we clicked on...
                var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                // pointerclick and drop events
                if(pointerEvent.pointerPress==pointerUpHandler&&pointerEvent.eligibleForClick)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);

                    // Extended By: xiaoxiao.yang
                    if (pointerEvent.selectedObject == null || pointerEvent.selectedObject != pointerEvent.pointerPress)
                        pointerEvent.selectedObject = pointerEvent.pointerPress;
                }

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;
                pointerEvent.dragging = false;
                pointerEvent.pointerDrag = null;

                // send exit events as we need to simulate this on touch up on touch device
                ExecuteEvents.ExecuteHierarchy(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerExitHandler);
                pointerEvent.pointerEnter = null;
            }
        }
    }
}


