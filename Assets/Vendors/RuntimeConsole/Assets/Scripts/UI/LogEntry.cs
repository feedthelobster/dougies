using RTC.Base;
using RTC.Core;
using RTC.Core.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace RTC.UI
{
	public class LogEntry : MonoBehaviour
	{
		#region Variables
		[SerializeField]
		private Image _icon;
		[SerializeField]
		private Text _title;
		[SerializeField]
		private Text _count;
		[SerializeField]
		private LogExpand _expandButton;
		[SerializeField]
		private Image _background;
		
		private bool _canExpand;
		#endregion

		#region Properties
		public Color Color
		{
			set
			{
				_background.color = value;
			}
		}

		public Sprite Icon
		{
			get
			{
				return _icon.sprite;
			}
		}

		public string Title
		{
			get
			{
				return _title.text;
			}
			set
			{
				_title.text = value;
			}
		}

		public string StackTrace { get; set; }
		public string Message { get; set; }
		#endregion

		#region Methods
		public void Create(Log log, int count)
		{
			Title = log.Title;
			StackTrace = log.StackTrace;
			Message = log.Message;

			_canExpand = !(log.Message.IsNullOrWhiteSpace() && log.StackTrace.IsNullOrWhiteSpace());

			var logDesign = ConsoleManager.Instance.GetLogTypeDesign(log.Type) ?? new LogTypeDesign();
			_title.color = logDesign.TitleColor;
			_icon.sprite = logDesign.Icon;			

			ShowLogsCount(count);
		}

		public void Expand()
		{
			ConsoleManager.Instance.LogWindow.Display(this);
		}

		private void ShowLogsCount(int count)
		{
			_expandButton.SetActive(count <= 1 && _canExpand);
			_count.gameObject.SetActive(count > 1);
			_count.text = count.ToString();
		}
		#endregion
	}
}
