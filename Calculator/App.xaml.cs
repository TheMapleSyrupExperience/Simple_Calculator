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
            if (char.IsNumber(userInput.Peek()) | userInput.Peek() == '.') // I've added in a decimal case, but I haven't implemented these as I'm not sure how to go about it
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
        Stack<double> operands = new Stack<double>();

        while (parsedExpression.Count() != 0)
        {

            switch (char.IsNumber(parsedExpression.Peek()))
            {

                case true:
                    
                    string currentInteger = "";
                    Stack<char> reversingNumber = new Stack<char>();
                    do
                    {
                            reversingNumber.Push(parsedExpression.Dequeue());  
                    }
                    while (char.IsNumber(parsedExpression.Peek()) | parsedExpression.Peek() == '.');

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
                   
                    operands.Push(Convert.ToDouble(currentInteger));

                    break;

                case false:

                    double[] operationOperands = new double[2];
                    operationOperands[0] = operands.Pop();
                    operationOperands[1] = operands.Pop();
                    double currentAnswer = 0;

                    switch (parsedExpression.Dequeue())
                    {
                        case '^':
                            double exponentResponse = operationOperands[0];

                            for (int i = 1; i <= operationOperands[1]; i++)
                            {
                                exponentResponse =+ exponentResponse * operationOperands[0];
                            }
                            operands.Push(exponentResponse);

                            currentAnswer = exponentResponse;

                            break;

                        case '*':
                            currentAnswer = operationOperands[1] * operationOperands[0];
                            operands.Push(currentAnswer);
                            currentAnswer = 0;
                            break;

                        case '/':
                            currentAnswer = operationOperands[1] / operationOperands[0];
                            operands.Push(currentAnswer);
                            currentAnswer = 0;

                            break;

                        case '+':
                            currentAnswer = operationOperands[1] + operationOperands[0];
                            operands.Push(currentAnswer);
                            currentAnswer = 0;
                            
                            break;

                        case '-':
                            currentAnswer = operationOperands[1] - operationOperands[0];
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
}   
      