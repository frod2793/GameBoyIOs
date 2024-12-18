using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick_Pos_dragandDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
  [SerializeField]  float offsetY = 1000f;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasScaler canvasScaler;
    private Vector2 originalLocalPointerPosition;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasScaler = canvas.GetComponentInParent<CanvasScaler>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPointerPosition);

        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, eventData.pressEventCamera))
        {
            originalLocalPointerPosition = localPointerPosition;
        }

        Vector2 offset = localPointerPosition - originalLocalPointerPosition;
        Vector2 targetPosition = rectTransform.anchoredPosition + offset;

        Vector2 clampedPosition = ClampToCanvas(targetPosition);
        rectTransform.anchoredPosition = clampedPosition;

        originalLocalPointerPosition = localPointerPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 clampedPosition = ClampToCanvas(rectTransform.anchoredPosition);
        rectTransform.anchoredPosition = clampedPosition;
    }

    private Vector2 ClampToCanvas(Vector2 position)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        minX = -canvasRect.rect.width * canvasRect.pivot.x + rectTransform.rect.width * rectTransform.pivot.x;
        maxX = canvasRect.rect.width * (canvasRect.pivot.x) - rectTransform.rect.width * ( rectTransform.pivot.x);
       

        minY = -canvasRect.rect.height * canvasRect.pivot.y + rectTransform.rect.height * rectTransform.pivot.y + offsetY;
        maxY = canvasRect.rect.height * (canvasRect.pivot.y) - rectTransform.rect.height * (rectTransform.pivot.y) + offsetY;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }
    
    private void OnDrawGizmos()
    {
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(canvasRect.transform.position, canvasRect.rect.size);
        }
    }

}
