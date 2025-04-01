using System.Configuration;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Media;

namespace Calculator;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    static public double input_parsing(Stack<char> userInput)
    {
        Stack<char> intermediateStack = new Stack<char>();
        Queue<char> parsedInput = new Queue<char>();
        Dictionary<char, int> keyValuePairs = new Dictionary<char, int>();

        keyValuePairs.Add('^', 1);
        keyValuePairs.Add('*', 2);
        keyValuePairs.Add('/', 3);
        keyValuePairs.Add('+', 4);
        keyValuePairs.Add('-', 5);

        while (userInput.Count() != 0) //first we perform the shunting yard algorithm with the help of our key-value dictionary
       {
            if (char.IsNumber(userInput.Peek()) | userInput.Peek() == '.' | userInput.Peek() == '~') // tilde denotes a sign change
            {
                parsedInput.Enqueue(userInput.Pop());
            }
            else
            {
                parsedInput.Enqueue('\0'); // queuing a null character to denote when my number has ended

                if (intermediateStack.Count() == 0) // don't quite understand what associativity of an operator is yet, I will cross that bridge when I get there
                {
                    intermediateStack.Push(userInput.Pop());
                }
                else if (keyValuePairs[intermediateStack.Peek()] < keyValuePairs[userInput.Peek()]) // gotta do this in this order because peeking when null will cause an exception
                {
                    while (intermediateStack.Count() != 0)
                    {
                        parsedInput.Enqueue(intermediateStack.Pop());
                    }   
                    intermediateStack.Push(userInput.Pop());
                }
                else
                {
                    intermediateStack.Push(userInput.Pop());
                }

            }
        }
        while (intermediateStack.Count() != 0)
        {
            parsedInput.Enqueue(intermediateStack.Pop());
        }
        return Calc_Logic(parsedInput);
    }

    static private double Calc_Logic(Queue<char> parsedExpression)
    {
        
        double finalAnswer = 0;
        double convertedStackNumber = 0;
        Stack<double> operands = new Stack<double>();

        while (parsedExpression.Count() != 0)
        {
            string currentInteger = "";
            Stack<char> reversingNumber = new Stack<char>();
            int signCount = 0;

            if(char.IsNumber(parsedExpression.Peek()) | parsedExpression.Peek() == '~')
            {
                do
                {
                    if (parsedExpression.Peek() != '~')
                    {
                        reversingNumber.Push(parsedExpression.Dequeue());
                    }
                    else
                    {
                        parsedExpression.Dequeue();
                        signCount++;
                    }
                }
                while (char.IsNumber(parsedExpression.Peek()) | parsedExpression.Peek() == '.'| parsedExpression.Peek() == '~');
                if (parsedExpression.Peek() == '\0')
                {
                    parsedExpression.Dequeue();
                }

                while (reversingNumber.Count() != 0)
                {
                    if (reversingNumber.Peek() != '.')
                    {
                        currentInteger += reversingNumber.Pop() - '0';
                    }
                    else
                    {
                        currentInteger += reversingNumber.Pop();
                    }

                }
                convertedStackNumber = Convert.ToDouble(currentInteger);

                if (int.IsOddInteger(signCount))
                {
                    convertedStackNumber = -convertedStackNumber;
                }
                operands.Push(convertedStackNumber);

            }
            
            else
            {
                    double[] operationOperands = new double[2];
                    operationOperands[0] = operands.Pop();
                    operationOperands[1] = operands.Pop();
                    double currentAnswer = 0;

                    switch (parsedExpression.Dequeue())
                    {
                        case '^':
                            double exponentResponse = operationOperands[0];

                            for (int i = 1; i < operationOperands[1]; i++)
                            {
                                exponentResponse = +exponentResponse * operationOperands[0];
                            }
                            operands.Push(exponentResponse);

                            currentAnswer = exponentResponse;

                            break;

                        case '*':
                            currentAnswer = operationOperands[0] * operationOperands[1];
                            operands.Push(currentAnswer);
                            currentAnswer = 0;
                            break;

                        case '/':
                            if (operationOperands[1] == 0)
                            {
                                throw new InvalidDivisionException("Cannot Devide by zero");
                            }
                            currentAnswer = operationOperands[0] / operationOperands[1];
                            operands.Push(currentAnswer);
                            currentAnswer = 0;

                            break;

                        case '+':
                            currentAnswer = operationOperands[0] + operationOperands[1];
                            operands.Push(currentAnswer);
                            currentAnswer = 0;

                            break;

                        case '-':
                            currentAnswer = operationOperands[0] - operationOperands[1];
                            operands.Push(currentAnswer);
                            currentAnswer = 0;
                            break;

                    }

                    break;

                }
        }
        finalAnswer = operands.Pop();
        return finalAnswer;
    }

    static public bool Answer_Formatter(double calculationAnswer)
    {

        string evaluativeAnswer = calculationAnswer.ToString();

        if (evaluativeAnswer.Length < 11)
        {
            return true;
        }
        else
        {
            return false;
        }
            

    }
    public class InvalidDivisionException : Exception
    {
        public InvalidDivisionException() : base() { }
        public InvalidDivisionException(string message) : base("Cannot devide by zero") { }
        public InvalidDivisionException(string message, Exception inner) : base("Cannot Devide by Zero", null) { }
    }

}   
      