using RTC.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RTC.UI
{
	public class LogTypeFilter : MonoBehaviour, IPointerDownHandler
	{
		#region Variables
		[SerializeField]
		private string _logType;
		[SerializeField]
		private Text _count;
		[SerializeField]
		private LogsContainer _logsContainer;
		#endregion

		#region Methods
		private void Start()
		{
			_count.text = _logsContainer.Count(_logType).ToString();
			RTConsole.Instance.Logger.CollectionChanged += Logger_CollectionChanged;
		}

		private void Logger_CollectionChanged(object sender, System.EventArgs e)
		{
			_count.text = _logsContainer.Count(_logType).ToString();
		}

		private void OnDestroy()
		{
			RTConsole.Instance.Logger.CollectionChanged -= Logger_CollectionChanged;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_logsContainer.FilterByType(_logType);
		}
		#endregion
	}
}
