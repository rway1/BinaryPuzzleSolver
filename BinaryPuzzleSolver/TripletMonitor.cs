using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryPuzzleSolver
{
    class TripletMonitor
    {
        private int numberOfSolvedCells = 0;
        public bool Solved
        {
            get;
            private set;
        }

        CellParameter<char> m_first;
        CellParameter<char> m_second;
        CellParameter<char> m_third;

        public TripletMonitor(CellParameter<char> first, CellParameter<char> second, CellParameter<char> third)
        {
            m_first = first;
            m_first.CellValueChanged += CellValueChangedAction;

            m_second = second;
            m_second.CellValueChanged += CellValueChangedAction;

            m_third = third;
            m_third.CellValueChanged += CellValueChangedAction;
        }

        private void CellValueChangedAction(object sender, EventArgs e)
        {
            numberOfSolvedCells++;
            if (numberOfSolvedCells == 2)
            {
                if (m_first.Value == m_second.Value && m_first.Value != '-')
                {
                    m_third.Value = Solver.Opposite(m_first.Value);
                }
                else if (m_first.Value == m_third.Value && m_first.Value != '-')
                {
                    m_second.Value = Solver.Opposite(m_first.Value);
                }
                else if (m_second.Value == m_third.Value && m_third.Value != '-')
                {
                    m_first.Value = Solver.Opposite(m_third.Value);
                }
            }
        }

    }
}
