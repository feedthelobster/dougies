using System;

namespace RTC
{
	public sealed class CircularBuffer<T>
	{
		#region Variables
		private T[] _buffer;
		private int _front;
		private int _position = -1;
		private bool _full;
		#endregion

		#region Methods
		public CircularBuffer(int capacity)
		{
			if(capacity <= 0)
			{
				throw new InvalidOperationException("Capacity cannot be less or equal to zero.");
			}
			
			_buffer = new T[capacity];
		}

		public void Add(T item)
		{
			_buffer[_front++] = item;

			if (_front == _buffer.Length)
			{
				_front = 0;
				_full = true;
			}
		}

		public T GetNext()
		{
			++_position;
			if(_position == _buffer.Length || _position == _front)
			{
				_position = 0;
			}
			return _buffer[_position];
		}

		public T GetPrevious()
		{
			--_position;
			if(_position <= -1)
			{ 
				_position = _full ? _position = _buffer.Length - 1 : _position = _front;
			}
			return _buffer[_position];
		}
		#endregion
	}
}
