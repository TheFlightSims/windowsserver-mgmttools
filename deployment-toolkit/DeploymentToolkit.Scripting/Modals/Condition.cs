using System;

namespace DeploymentToolkit.Scripting.Modals
{
    public enum CompareType
    {
        String,
        Number
    }
    public enum Operator
    {
        Equal,
        NotEqual,
        Greater,
        GreaterEqual,
        Less,
        LessEqual
    }

    public class Condition
    {
        public CompareType CompareType { get; set; } = CompareType.String;
        public string FirstString { get; set; }
        public string SecondString { get; set; }
        public Operator Operator { get; set; }

        public bool IsTrue()
        {
            switch(Operator)
            {
                case Operator.Equal:
                case Operator.NotEqual:
                {
                    var result = false;

                    if(CompareType == CompareType.String)
                    {
                        if(string.IsNullOrEmpty(FirstString) && string.IsNullOrEmpty(SecondString))
                        {
                            result = true;
                        }
                        else
                        {
                            result = string.Compare(FirstString, SecondString, StringComparison.InvariantCulture) == 0;
                        }
                    }
                    else
                    {
                        if(!int.TryParse(FirstString, out var firstNumber) || !int.TryParse(SecondString, out var secondNumber))
                        {
                            return false;
                        }

                        result = firstNumber == secondNumber;
                    }

                    if(Operator == Operator.NotEqual)
                    {
                        return !result;
                    }

                    return result;
                }

                case Operator.Greater:
                case Operator.GreaterEqual:
                case Operator.Less:
                case Operator.LessEqual:
                {
                    if(!int.TryParse(FirstString, out var firstNumber) || !int.TryParse(SecondString, out var secondNumber))
                    {
                        return false;
                    }

                    switch(Operator)
                    {
                        case Operator.Greater:
                            return firstNumber > secondNumber;
                        case Operator.GreaterEqual:
                            return firstNumber >= secondNumber;
                        case Operator.Less:
                            return firstNumber < secondNumber;
                        case Operator.LessEqual:
                            return firstNumber <= secondNumber;
                    }

                    // This should never happen but the compiler wants it
                    return false;
                }

                default:
                {
                    // This should never happen
                    System.Diagnostics.Debug.WriteLine($"Invalid operator {Operator}");
                }
                return false;
            }
        }
    }
}
