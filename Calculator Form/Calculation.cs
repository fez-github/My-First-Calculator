using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator_Form
{
    public class Calculation
    {
        public Calculation()
        {
        }
        /// <summary>
        /// Take an equation as a string, and run it through calculations via the order of operations.
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public string RunCalculations(string formula)
        {
            //Regex match used to detect if number is placed next to parentheses.  Cannot properly calculate with this.
            if(Regex.IsMatch(formula, "([0-9]\\(|\\)[0-9])"))
            {
                return "SYNTAX ERROR";
            }
            while (formula.Contains('(') || formula.Contains(')'))
            {
                formula = SearchParentheses(formula);
            }
            while (formula.Contains('^'))
            {
                formula = SearchValue(formula, '^', '^');
            }
            while (formula.Contains('*') || formula.Contains('/'))
            {
                formula = SearchValue(formula, '*', '/');
            }
            //Regex is used to take values in scientific notation into account, which uses "E+."
                //Not accounting for this could create an infinite while loop.
            //Substring is used to account for possible negative values at the start.
            while (formula.Substring(1).Contains('-')  || Regex.IsMatch(formula.Substring(1), "(\\)|[0-9])\\+"))
            {
                formula = SearchValue(formula, '+', '-');
            }
            return formula;
        }
        /// <summary>
        /// Search for any parentheses in formula and call method to calculate what's within them.  Returns updated formula.
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
        private string SearchParentheses(string equation)
        {
            int leftParIndex = -1, rightParIndex = -1;
            for (int i = 0; i <= equation.Length - 1; i++)
            {//Search through equation and note of the innermost instance of left/right parentheses.
                if (equation[i] == '(')
                {
                    leftParIndex = i;
                }
                else if (equation[i] == ')')
                {
                    rightParIndex = i;
                }
                if (rightParIndex > -1 && leftParIndex == -1)
                {
                    //Syntax Error.  Right parentheses before left.
                        return "SYNTAX ERROR";
                }
                if (rightParIndex > -1 && leftParIndex > -1)
                {//Innermost parentheses are found.  Pass their indexes through w/ equation to operate on that subsection.
                    equation = ParenthesesNewString(equation, leftParIndex, rightParIndex);
                    leftParIndex = -1;
                    rightParIndex = -1;
                    i = -1;
                }
            }
            return equation;
        }
        /// <summary>
        /// Takes a string with parentheses and converts it into a new string.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="leftPar"></param>
        /// <param name="rightPar"></param>
        /// <returns></returns>
        string ParenthesesNewString(string equation, int leftPar, int rightPar)
        {
            string parenEquation = equation.Substring(leftPar + 1, (rightPar - leftPar - 1));
            string newString = SearchValue(SearchValue(SearchValue(parenEquation, '^', '^'), '*', '/'), '+', '-');
            return equation.Remove(leftPar, rightPar - leftPar + 1).Insert(leftPar, newString);
        }

        /// <summary>
        /// Iterate through formula and perform equation on predetermined segments.
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="operator1"></param>
        /// <param name="operator2"></param>
        /// <returns></returns>
        private string SearchValue(string equation, char operator1, char operator2)
        {
            char sign;
            string leftVal = "", rightVal = "";
            int j, k;
            //Loop through string until sign is found.
            for (int i = 0; i <= equation.Length - 1; i++)
            {   //Check for negative values at the start.  This must not be operated on.
                if (i == 0)
                {
                    if (equation[i] == '-')
                    {
                        i++;
                    }
                }
                else if(i > 0)
                {
                    if(equation[i] == '+' && equation[i-1] == 'E')
                    {
                        i++;
                    }
                }
                if (equation[i] == operator1 | equation[i] == operator2)
                {
                    sign = equation[i];
                    //Loop forward starting with sign's position to get righthand numbers.
                    for (k = i + 1; k <= equation.Length - 1; k++)
                    {   //Check for values beyond a subtraction sign, and determine whether it's negative or subtraction.
                        if (char.IsNumber(equation[k]) || equation[k] == '-' || equation[k] == '.')
                        {
                            if(k - i >= 2 && equation[k] == '-')
                            {
                                k--;
                                break;
                            }
                            rightVal = rightVal + equation[k];
                        }
                        else
                        {
                            k--;
                            break;
                        }
                    }
                    //Loop backward starting with sign's position to get lefthand numbers.
                    for (j = i - 1; j >= 0; j--)
                    {
                        if (char.IsNumber(equation[j]) || equation[j] == '-' || equation[j] == '.')
                        //Check for number before minus sign, and determine if it's meant to be a negative or subtraction.
                        {   if(equation[j] == '-' && j!= 0)
                            {
                                if(char.IsNumber(equation[j - 1]))
                                {
                                    j++;
                                    break;
                                }
                            }
                            leftVal = equation[j] + leftVal;
                            if(i - j >= 2 && equation[j] == '-')
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    //Adjust values if needed.
                    if(k > equation.Length - 1)
                    {
                        k = equation.Length - 1;
                    }
                    if(j < 0)
                    {
                        j = 0;
                    }
                    try
                   {
                        switch (sign)
                        {
                            case '^':
                                //      return equation.Replace(partEquation, Convert.ToString(Math.Pow(Convert.ToDouble(leftVal), Convert.ToDouble(rightVal))));
                                return equation.Remove(j, (k - j + 1)).Insert(j, Convert.ToString(Math.Pow(Convert.ToDouble(leftVal), Convert.ToDouble(rightVal))));
                            case '*':
                                // return equation.Replace(partEquation, Convert.ToString(Convert.ToDouble(leftVal) * Convert.ToDouble(rightVal)));
                                return equation.Remove(j, (k - j + 1)).Insert(j, Convert.ToString(Convert.ToDouble(leftVal) * Convert.ToDouble(rightVal)));
                            case '/':
                              //  return equation.Replace(partEquation, Convert.ToString(Convert.ToDouble(leftVal) / Convert.ToDouble(rightVal)));
                                return equation.Remove(j, (k - j + 1)).Insert(j, Convert.ToString(Convert.ToDouble(leftVal) / Convert.ToDouble(rightVal)));
                            case '+':
                               // return equation.Replace(partEquation, Convert.ToString(Convert.ToDouble(leftVal) + Convert.ToDouble(rightVal)));
                                return equation.Remove(j, (k - j + 1)).Insert(j, Convert.ToString(Convert.ToDouble(leftVal) + Convert.ToDouble(rightVal)));
                            case '-':
                              //  return equation.Replace(partEquation, Convert.ToString(Convert.ToDouble(leftVal) - Convert.ToDouble(rightVal)));
                                return equation.Remove(j, (k - j + 1)).Insert(j, Convert.ToString(Convert.ToDouble(leftVal) - Convert.ToDouble(rightVal)));
                            default:
                                i = -1;
                                return equation;
                        }
                    }
                    catch (FormatException)
                    {
                        return "SYNTAX ERROR";
                    }
                    catch(OverflowException)
                    {
                        return "OVERFLOW ERROR";
                    }
                }
            }
            return equation;
        }
    }
}