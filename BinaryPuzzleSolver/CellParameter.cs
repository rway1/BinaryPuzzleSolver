using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryPuzzleSolver
{
    class CellParameter<T> where T : struct
    {
        public EventHandler CellValueChanged;

        private T _value;

        public T Value
        {
            get { return _value; }
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    CellValueChanged?.Invoke(this, null);

                }
            }
        }
    }
}
