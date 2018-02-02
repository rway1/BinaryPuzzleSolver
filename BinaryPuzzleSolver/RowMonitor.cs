using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryPuzzleSolver
{
    class RowMonitor
    {
        private int OneCount = 0;
        private int ZeroCount = 0;
        private List<CellParameter<char>> CellParameters;
        public RowMonitor(List<CellParameter<char>> cellParameters)
        {
            CellParameters = cellParameters;
            foreach (var cellParameter in CellParameters)
            {
                cellParameter.CellValueChanged += CellValueChangedAction;
            }
        }

        private void CellValueChangedAction(object sender, EventArgs e)
        {
            var changedCell = sender as CellParameter<char>;
            if (changedCell.Value == '1') OneCount++;
            else ZeroCount++;

            //Fill Last Cell
            if (OneCount + ZeroCount == CellParameters.Count -1)
            {
                char ValueToFillLast;
                if (OneCount < ZeroCount) ValueToFillLast = '1';
                else ValueToFillLast = '0';

                foreach (var cellParameter in CellParameters)
                {
                    if (cellParameter.Value == '-') cellParameter.Value = ValueToFillLast;
                }
            }

            //Fill remaining if other kind is max
            if (OneCount == CellParameters.Count/2 || ZeroCount == CellParameters.Count/2)
            {
                char ValueToUse;
                if (OneCount == CellParameters.Count / 2) ValueToUse = '0';
                else ValueToUse = '1';

                foreach (var cellParameter in CellParameters)
                {
                    if (cellParameter.Value == '-') cellParameter.Value = ValueToUse;
                }
            }
        }
    }
}
