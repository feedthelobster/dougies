using RTC.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RTC.UI
{
	public class AutocompleteContainer : MonoBehaviour
	{
		#region Variables
		[SerializeField]
		private int _maxSuggestionsCount;
		[SerializeField]
		private AutocompleteItem _autocompleteItem;
		[SerializeField]
		private InputBar _inputBar;
		CircularObjectPool _filterPool;
		private int _filterCount;
		#endregion

		#region Methods
		public bool IsFilterActive
		{
			get
			{
				return _filterCount > 0;
			}
		}

		public void Display(IEnumerable<CommandInfo> commands)
		{
			_filterPool.Clear();

			var result = commands.Take(_maxSuggestionsCount);			
			_filterCount = result.Count();

			if(_filterCount == 1)
			{
				if(result.First().Alias == _inputBar.Input)
				{
					return;
				}
			}

			foreach (var cmd in result)
			{
				var obj = _filterPool.GetObject();
				var filterItem = obj.GetComponent<AutocompleteItem>();
				filterItem.Name = cmd.Alias;
				obj.transform.SetParent(transform, false);
				obj.transform.SetAsLastSibling();
			}		
		}

		private void Start()
		{
			_filterPool = new CircularObjectPool(_maxSuggestionsCount, _autocompleteItem.gameObject);
		}

		private void Update()
		{
			if(IsFilterActive)
			{
				if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					var next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
					if (next != null)
					{
						next.Select();
					}
				}
				else if (Input.GetKeyDown(KeyCode.UpArrow))
				{
										
				}
				else if (Input.anyKeyDown)
				{
					_inputBar.Focus();
				}
			}
		}
		#endregion
	}
}
