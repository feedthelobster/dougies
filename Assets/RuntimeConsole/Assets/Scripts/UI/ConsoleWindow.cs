using RTC.Base;
using UnityEngine;

namespace RTC.UI
{
	public class ConsoleWindow : MonoBehaviour
	{
		#region Variables
		[SerializeField]
		private LogsContainer _logsContainer;
		[SerializeField]
		private InputBar _inputBar;
		#endregion

		#region Properties
		public LogsContainer LogsContainer
		{
			get
			{
				return _logsContainer;
			}
		}

		public InputBar InputBar
		{
			get
			{
				return _inputBar;
			}
		}

		public bool IsOpen
		{
			get
			{
				return gameObject.activeSelf;
			}
		}
		#endregion

		#region Methods
		public void Close()
		{
			gameObject.SetActive(false);
		}

		public void Open()
		{
			gameObject.SetActive(true);
		}

		public void Toggle()
		{
			gameObject.SetActive(!gameObject.activeSelf);
		}
		#endregion
	}
}
