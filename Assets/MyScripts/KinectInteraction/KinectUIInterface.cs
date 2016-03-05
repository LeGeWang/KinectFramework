using UnityEngine;
using System.Collections;

namespace UnityEngine.EventSystems
{
    public interface IKinectSelectHandler:IEventSystemHandler
    {
        void OnKinectSelect(PointerEventData eventData);
    }

    public interface IKinectDragMoveHandler:IEventSystemHandler
    {
        void OnKinectDrag(PointerEventData eventData);
    }

    public interface IKinectDropHandler:IEventSystemHandler
    {
        void OnKinectDrop(PointerEventData eventData);
    }

    public static class KinectModuleEvent
    {
        private static void Execute(IKinectDragMoveHandler handler, BaseEventData eventData)
        {
            handler.OnKinectDrag(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
        }

        public static ExecuteEvents.EventFunction<IKinectDragMoveHandler> kinectMoveHandler
        {
            get { return Execute; }
        }
    }
}