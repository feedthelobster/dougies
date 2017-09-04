using UnityEngine;
using UnityEngine.EventSystems;

namespace RTC.UI
{
	public class LogExpand : MonoBehaviour, IPointerDownHandler
	{
		[SerializeField]
		private LogEntry _log;

		public void OnPointerDown(PointerEventData eventData)
		{
			_log.Expand();
		}

		public void SetActive(bool value)
		{
			gameObject.SetActive(value);
		}
	}
}
