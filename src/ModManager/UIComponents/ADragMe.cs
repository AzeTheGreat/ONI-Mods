using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ModManager
{
    // DragMe instantiates a new object as a child of the canvas when dragging.
    // When using virtual scroll, this child is set by the layout group, and thus fights with the dragging.
    // This class enables this behavior to be fixed, and for additional custom behavior to be implemented.
    public class ADragMe : MonoBehaviour, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private GameObject dragObj;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (GetComponentInParent<Canvas>() is not Canvas canvas)
                return;

            dragObj = Instantiate(gameObject, canvas.transform, true);
            // The drag object should always ignore layouts, since it should be set only by the dragging.
            dragObj.AddOrGet<LayoutElement>().ignoreLayout = true;
            if (dragObj.GetComponent<GraphicRaycaster>() is GraphicRaycaster rc)
                rc.enabled = false;

            SetDraggedPosition(eventData);
            AExecuteEvents.ExecuteHierarchy<IDragHandler>(gameObject, x => x.OnBeginDrag(eventData));
        }

        public void OnDrag(PointerEventData eventData)
        { 
            SetDraggedPosition(eventData);
            AExecuteEvents.ExecuteHierarchy<IDragHandler>(gameObject, x => x.OnDrag(eventData));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            AExecuteEvents.ExecuteHierarchy<IDragHandler>(gameObject, x => x.OnEndDrag(eventData));
            if (dragObj != null)
                Destroy(dragObj);
        }

        private void SetDraggedPosition(PointerEventData eventData)
        {
            if (dragObj == null)
                return;

            var rt = dragObj.GetComponent<RectTransform>();
            rt.position = new(rt.position.x, eventData.position.y);
        }

        // A custom target is needed, rather than just reusing the drag handlers, to prevent passing the drag up the chain.
        // That causes weirdness like the panel using drag scrolling while trying to drag an entry.
        // Interface can't just inherit from drag handlers, because then listeners of this target, will also respond to the parent targets.
        public interface IDragHandler : IEventSystemHandler
        {
            void OnBeginDrag(PointerEventData data);
            void OnDrag(PointerEventData data);
            void OnEndDrag(PointerEventData data);
        }
    }
}
