using UnityEngine;
using UnityEngine.UI;

namespace RTC.UI
{
	public class LogWindow : MonoBehaviour
	{
		#region Variables
		[SerializeField]
		private Image _icon;
		[SerializeField]
		private Text _title;
		[SerializeField]
		private Text _message;
		[SerializeField]
		private Text _stackTrace;
		[SerializeField]
		private Scrollbar _scrollbar;
		#endregion

		#region Methods
#if UNITY_5_3_OR_NEWER
		private void Start()
		{
			var scrollrect = GetComponentInChildren<ScrollRect>();
			scrollrect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
			scrollrect.viewport = scrollrect.transform.GetChild(0) as RectTransform;
		}
#endif

		public void Display(LogEntry logEntry)
		{
			_icon.sprite = logEntry.Icon;
			_title.text = logEntry.Title;
			_message.text = logEntry.Message;
			_stackTrace.text = logEntry.StackTrace;

			_scrollbar.value = 1;
			gameObject.SetActive(true);
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}
#endregion;
	}
}
