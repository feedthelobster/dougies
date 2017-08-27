using UnityEngine;
using UnityEngine.UI;
using RTC.Base;
using RTC.Core;
using System;
using System.Collections;
using RTC.Core.Extension;

namespace RTC.UI
{
	public class LogsContainer : MonoBehaviour
	{
		#region Variables
		private CircularObjectPool _logsPool;
		private AbstractLogger _logger;
		private string lastCommand;

		[SerializeField]
		private LogEntry _logEntry;
		[SerializeField]
		private Scrollbar _scrollbar;
		[SerializeField]
		private Color _logColor;
		[SerializeField]
		private Color _logAlternateColor;
		[SerializeField]
		private int _maxLogsCount;
		#endregion

		#region EventHandlers
		private void _logger_CollectionChanged(object sender, EventArgs e)
		{
			if (gameObject.activeInHierarchy)
			{
				DrawLogs();
			}
		}

		private void Instance_OnCommandRecieve(object sender, CommandReceivedEvent e)
		{
			lastCommand = e.Command;
		}

		private void Instance_OnCommandExecute(object sender, CommandEvent e)
		{
			if (!e.Command.Alias.StartsWith("RTC."))
			{
				RTConsole.Instance.Log(lastCommand);
			}
		}
		#endregion

		#region UnityCallbacks
		private void OnEnable()
		{
			RTConsole.Instance.CommandExecuted += Instance_OnCommandExecute;
			RTConsole.Instance.CommandReceived += Instance_OnCommandRecieve;

			DrawLogs();
		}

		private void OnDisable()
		{
			RTConsole.Instance.CommandExecuted -= Instance_OnCommandExecute;
			RTConsole.Instance.CommandReceived -= Instance_OnCommandRecieve;
		}

		private void Awake()
		{
			_logsPool = new CircularObjectPool(_maxLogsCount, _logEntry.gameObject);
			_logger = RTConsole.Instance.Logger;
			_logger.CollectionChanged += _logger_CollectionChanged;
		}

		private void OnDestroy()
		{
			RTConsole.Instance.Logger.CollectionChanged -= _logger_CollectionChanged;
		}
		#endregion

		#region Methods
		public int Count(string _logType)
		{
			return _logger.Count(_logType);
		}

		public void ClearFilter()
		{
			_logger.Filter((Log log) => { return true; });
		}

		public void Clear()
		{
			_logger.Clear();
			StartCoroutine(ResizeScrollbar_NextFrame());
		}

		public void FilterByType(string logType)
		{
			_logger.Filter((Log log) => { return log.Type == logType; });
		}

		public void ToggleStacked()
		{
			_logger.ToggleStacked();
		}

		private void DrawLogs()
		{
			_logsPool.Clear();

			int colorNumber = 0;
			var logs = _logger.Logs;
			foreach (var log in logs)
			{
				var obj = _logsPool.GetObject();
				var logEntry = obj.GetComponent<LogEntry>();
				logEntry.Create(log, _logger.GetCount(log));
				obj.transform.SetParent(transform, false);
				obj.transform.SetAsLastSibling();

				logEntry.Color = colorNumber++ % 2 == 0 ? _logColor : _logAlternateColor;
			}

			StartCoroutine(MoveScrollbar_NextFrame());
		}

		private IEnumerator ResizeScrollbar_NextFrame()
		{
			yield return null;
			_scrollbar.size = 1;
		}

		private IEnumerator MoveScrollbar_NextFrame()
		{
#warning "Updating all canvases can produce frame drops if there is much to draw. If you remove this, the scrollbar won't snap at the bottom when a new log is added."
			Canvas.ForceUpdateCanvases();
			yield return null;
			_scrollbar.value = _logger.IsStacked ? 1 : 0;
		}
		#endregion
	}
}
