using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ModManager
{
    // DragMe instantiates a new object as a child of the canvas when dragging.
    // When using virtual scroll, this child is set by the layout group, and thus fights with the dragging.
    // This class enables this behavior to be fixed, and for additional custom behavior to be implemented.
    public class ADragMe : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
    {
        public IDragListener Listener { get; set; }

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
            Listener.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        { 
            SetDraggedPosition(eventData);
            Listener.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Listener.OnEndDrag(eventData);
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

        public interface IDragListener : IBeginDragHandler, IDragHandler, IEndDragHandler { }
    }
}
