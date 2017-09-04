using RTC.UI;
using System.Linq;
using UnityEngine;
using RTC.Base;
using RTC.Core;
using RTC.Core.Extension;

namespace RTC
{
	public class ConsoleManager : MonoBehaviour
	{
		#region Initialization
		private static ConsoleManager _instance;
		public static ConsoleManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType(typeof(ConsoleManager)) as ConsoleManager;

					if (_instance == null)
					{
						GameObject obj = new GameObject("ConsoleManager");
						_instance = obj.AddComponent(typeof(ConsoleManager)) as ConsoleManager;
					}
				}
				
				return _instance;
			}
		}
		#endregion

		#region Variables
		
		[SerializeField]
		private LogTypeDesign[] _logTypes;
		[Header("Windows")]
		[SerializeField]
		private LogWindow _logWindow;
		[SerializeField]
		private ConsoleWindow _consoleWindow;

		[SerializeField]
		private KeyCode _toggleKey;

		#endregion

		#region Properties
		public LogWindow LogWindow
		{
			get
			{
				return _logWindow;
			}
		}

		public ConsoleWindow ConsoleWindow
		{
			get
			{
				return _consoleWindow;
			}
		}
		#endregion

		#region EventHandlers
		private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
		{
			Log log = new Log(LogTypes.Error, "Unity log callback", string.Format("Undefined log type: {0}!", type.ToString()));
			switch (type)
			{
				case LogType.Assert:
					log = new Log(LogTypes.Exception, condition, stackTrace);
					break;
				case LogType.Error:
					log = new Log(LogTypes.Error, condition, stackTrace);
					break;
				case LogType.Exception:
					log = new Log(LogTypes.Exception, condition, stackTrace);
					break;
				case LogType.Log:
					log = new Log(LogTypes.Log, condition, stackTrace);
					break;
				case LogType.Warning:
					log = new Log(LogTypes.Warning, condition, stackTrace);
					break;

			}
			RTConsole.Instance.Log(log);
		}
		#endregion

		#region Methods
		private void Update()
		{
			if (Input.GetKeyDown(_toggleKey))
			{
				ConsoleWindow.Toggle();
			}
		}

		private void OnEnable()
		{
			Application.logMessageReceived += Application_logMessageReceived;
		}

		private void OnDisable()
		{
			Application.logMessageReceived -= Application_logMessageReceived;
		}
		
		public LogTypeDesign GetLogTypeDesign(string log)
		{
			return _logTypes.FirstOrDefault(l => l.LogType == log);
		}
		#endregion
	}
}
