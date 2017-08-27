using RTC.Core;
using RTC.Core.Extension;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RTC.UI
{
	public class InputBar : MonoBehaviour
	{
		#region Variables
		[SerializeField]
		private InputField _inputField;
		[SerializeField]
		private Button _executeButton;
		[SerializeField]
		private AutocompleteContainer _suggestionsContainer;
		[SerializeField]
		private KeyCode _submitKey;
		[SerializeField]
		private int _commandHistoryCount;
		private CircularBuffer<string> _executedCommands;
		private CommandFilter _commandFilter;
		#endregion

		#region Properties
		public InputField InputField
		{
			get
			{
				return _inputField;
			}
		}

		public string Input
		{
			get
			{
				return _inputField.text;
			}
			set
			{
				_inputField.text = value ?? string.Empty;
				Focus();
			}
		}
		#endregion

		#region Methods
		public void Execute(string command)
		{
			if (!command.IsNullOrWhiteSpace())
			{
				RTConsole.Instance.Execute(command.TrimEnd());
				_executedCommands.Add(string.Format("{0} ", command));			

				Input = string.Empty;
			}
		}

		public void Focus()
		{
			_inputField.ActivateInputField();
			if (this.isActiveAndEnabled)
			{
				StartCoroutine(MoveTextEnd_NextFrame());
			}
		}

		private IEnumerator MoveTextEnd_NextFrame()
		{
			yield return 0;		
			_inputField.MoveTextEnd(false);		
		}

		private void OnEnable()
		{
			Focus();
		}

		private void Start()
		{
			_commandFilter = new CommandFilter();
			_executedCommands = new CircularBuffer<string>(_commandHistoryCount);

			_inputField.onEndEdit.AddListener((string cmd) =>
			{
				if (UnityEngine.Input.GetKeyDown(_submitKey))
				{
					Execute(cmd);
				}
			});

#if UNITY_5_3_OR_NEWER
			_inputField.onValueChanged.AddListener((string prefix) =>
			{
				_suggestionsContainer.Display(_commandFilter.FilterByName(prefix));
			});
#else
			_inputField.onValueChange.AddListener((string prefix) =>
			{
				_suggestionsContainer.Display(_commandFilter.FilterByName(prefix));
			});
#endif

			_executeButton.onClick.AddListener(() => 
			{
				Execute(_inputField.text);
			});
		}

		public void Update()
		{
			if (!_suggestionsContainer.IsFilterActive && _inputField.isFocused)
			{
				if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
				{
					Input = _executedCommands.GetNext();
				}
				else if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
				{
					Input = _executedCommands.GetPrevious();
				}
			}
		}
		#endregion
	}
}
