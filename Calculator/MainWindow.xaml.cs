using System.Windows;

namespace Calculator;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
       
    }
    
    Stack<char> completeExpression = new Stack<char>();
    Queue<char> currentInput = new Queue<char>();
    Boolean answerFlag = false;
    Boolean signFlag = false;
    double lastAnswer = 0;
    private void one_Click(object sender, RoutedEventArgs e)
    {

        Add_Operand('1');

    }

    private void four_Click(object sender, RoutedEventArgs e)
    {

        Add_Operand('4');

    }

    private void seven_Click(object sender, RoutedEventArgs e)
    {

        Add_Operand('7');


    }

    private void openParenthetical_Click(object sender, RoutedEventArgs e)
    {

    }

    private void two_Click(object sender, RoutedEventArgs e)
    {

        Add_Operand('2');

    }

    private void five_Click(object sender, RoutedEventArgs e)
    {

        Add_Operand('5');

    }

    private void eight_Click(object sender, RoutedEventArgs e)
    {
        Add_Operand('8');
    }

    private void zero_Click(object sender, RoutedEventArgs e)
    {
        Add_Operand('0');
    }

    private void Exponentiate_Click(object sender, RoutedEventArgs e)
    {
        Add_Operator('^');
    }

    private void three_Click(object sender, RoutedEventArgs e)
    {
        Add_Operand('3');

    }

    private void six_Click(object sender, RoutedEventArgs e)
    {
        Add_Operand('6');
    }

    private void nine_Click(object sender, RoutedEventArgs e)
    {
        Add_Operand('9');
    }

    private void decimal_Click(object sender, RoutedEventArgs e)
    {
        Add_Operand('.');
    }

    private void multiply_Click(object sender, RoutedEventArgs e)
    {
        Add_Operator('*');
    }

    private void divide_Click(object sender, RoutedEventArgs e)
    {
        Add_Operator('/');
    }

    private void subtract_Click(object sender, RoutedEventArgs e)
    {
        Add_Operator('-');
    }

    private void add_Click(object sender, RoutedEventArgs e)
    {
        Add_Operator('+');
    }

    private void equals_Click(object sender, RoutedEventArgs e)
    {
        if (!answerFlag)
        {
            while (currentInput.Count != 0)
            {
                completeExpression.Push(currentInput.Dequeue());
            }
            try
            {
                
                lastAnswer = App.input_parsing(completeExpression);
                
                if (App.Answer_Formatter(lastAnswer))
                {
                    calculatorInput.Text = lastAnswer.ToString();
                }
                else
                {
                    calculatorInput.Text = lastAnswer.ToString("e2");
                }
                                 
            }
            catch (InvalidOperationException)
            {
                calculatorInput.Text = "Syntax Error";
            }
            catch (App.InvalidDivisionException)
            {
                calculatorInput.Text = "Division by 0";
            }
            
            answerFlag = true;
        }

    }

    private void delete_Click(object sender, RoutedEventArgs e)
    {
       if (!answerFlag)
        {
            currentInput.Clear();
            completeExpression.Clear();
            calculatorInput.Text = "";
        }
        else
        {
            answerFlag = false;
            calculatorInput.Text = "";
        }
    }

    private void invertSign_Click(object sender, RoutedEventArgs e)
    {
        Add_Sign_Inversion('~');
    }

    private void Add_Operand(char pressedButton)
    {
        if (!answerFlag)
        {
            currentInput.Enqueue(pressedButton);
            calculatorInput.Text += pressedButton;
        }
        return;
    }

    private void Add_Operator(char pressedButton)
    {
    if (!answerFlag)
        {
            while (currentInput.Count != 0)
            {
                completeExpression.Push(currentInput.Dequeue());
            }
            completeExpression.Push(pressedButton);
            calculatorInput.Text += pressedButton;
        }
       
    }
    private void Add_Sign_Inversion(char invert)
    {
        if (!answerFlag)
        {
            currentInput.Enqueue(invert);
        }
        if (!signFlag)
        {
            signFlag = !signFlag;
            calculatorInput.Text = calculatorInput.Text.Insert(0, "-");
        }
        else
        {
            signFlag = !signFlag;
            calculatorInput.Text = calculatorInput.Text.TrimStart('-');
        }
        return;
    }
}