using UnityEngine;
using UnityEngine.EventSystems;

namespace RTC.UI
{
	public class UIDrag : MonoBehaviour, IPointerDownHandler, IDragHandler
	{
		Vector2 cursorOffset;
		RectTransform canvasTransform;
		RectTransform thisTransform;

		void Start()
		{
			thisTransform = transform as RectTransform;
			canvasTransform = GetComponentInParent<Canvas>().transform as RectTransform;
		}

		public void OnDrag(PointerEventData eventData)
		{
			Vector2 cursorPosition;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, Input.mousePosition, eventData.pressEventCamera, out cursorPosition))
			{
				transform.localPosition = cursorPosition - cursorOffset;
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(thisTransform, eventData.position, eventData.pressEventCamera, out cursorOffset);
		}	
	}
}
