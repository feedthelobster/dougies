using RTC.Base;
using UnityEngine;
using UnityEngine.UI;

namespace RTC.UI
{
	[RequireComponent(typeof(Button))]
	public class AutocompleteItem : MonoBehaviour
	{
		[SerializeField]
		private Text _name;

		public string Name
		{
			get
			{
				return _name.text;
			}
			set
			{
				_name.text = value;
			}
		}

		private void Start()
		{
			GetComponent<Button>().onClick.AddListener(() => 
			{
				ConsoleManager.Instance.ConsoleWindow.InputBar.Input = Name;
			});
		}
	}
}
