using System;
using System.Windows.Forms;

namespace Calculator_Form
{
    public partial class Calculator : Form
    {
        Calculation calc = new Calculation();
        string formula;
        public Calculator()
        {
            InitializeComponent();
            this.ActiveControl = calcDisplay;
        }
        private void updateTextBox()
        {
            calcDisplay.Text = formula;
        }
        private void AddToFormula(string txt)
        {
              calcDisplay.Text = calcDisplay.Text + txt;
        }
        private void EnterKeyMethod(object sender, KeyEventArgs e)
        {        
                if(e.KeyValue == 13)
                {
                    RunCalculation();
                }
        }
        #region Button Controls
        private void btn0_Click(object sender, EventArgs e)
        {
            AddToFormula("0");
        }
        private void btn1_Click(object sender, EventArgs e)
        {
            AddToFormula("1");
        }
        private void btn2_Click(object sender, EventArgs e)
        {
            AddToFormula("2");
        }
        private void btn3_Click(object sender, EventArgs e)
        {
            AddToFormula("3");
        }
        private void btn4_Click(object sender, EventArgs e)
        {
            AddToFormula("4");
        }
        private void btn5_Click(object sender, EventArgs e)
        {
            AddToFormula("5");
        }
        private void btn6_Click(object sender, EventArgs e)
        {
            AddToFormula("6");
        }
        private void btn7_Click(object sender, EventArgs e)
        {
            AddToFormula("7");
        }
        private void btn8_Click(object sender, EventArgs e)
        {
            AddToFormula("8");
        }
        private void btn9_Click(object sender, EventArgs e)
        {
            AddToFormula("9");
        }
        private void btnDecimal_Click(object sender, EventArgs e)
        {
            AddToFormula(".");
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddToFormula("+");
        }
        private void btnSubtract_Click(object sender, EventArgs e)
        {
            AddToFormula("-");
        }
        private void btnMultiply_Click(object sender, EventArgs e)
        {
            AddToFormula("*");
        }
        private void btnDivide_Click(object sender, EventArgs e)
        {
            AddToFormula("/");
        }
        private void btnSquare_Click(object sender, EventArgs e)
        {
            AddToFormula("^2");
        }
        private void btnLeftPar_Click(object sender, EventArgs e)
        {
            AddToFormula("(");
        }
        private void btnRightPar_Click(object sender, EventArgs e)
        {
            AddToFormula(")");
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            formula = "";
            updateTextBox();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            formula = calcDisplay.Text;
            if (formula.Length != 0) {
                formula = formula.Remove(formula.Length - 1);
            }
            updateTextBox();
        }
        private void btnEquals_Click(object sender, EventArgs e)
        {
            RunCalculation();
        }
        private void btnExponent_Click(object sender, EventArgs e)
        {
            AddToFormula("^");
        }
        #endregion
        private void RunCalculation()
        {
            formula = calc.RunCalculations(calcDisplay.Text);
            resultTextBox.Text = formula;
            this.ActiveControl = calcDisplay;
            calcDisplay.SelectionStart = calcDisplay.Text.Length;
        }
    }
}