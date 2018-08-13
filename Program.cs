/*
Expression Evaluation from Coding Challenges
by Silvio Duka

Last modified date: 2018-03-08

Consider an arithmetic expression: A+B 
This is called an infix notation, when the operator (+) is between the operands (A and B). 
We, humans, know how to interpret the infix notation, as that's what we are taught in schools. But this form is not very suitable for the computer to read and evaluate the expressions, because each operator has a precedence, which shows the order in which the expression needs to be evaluated. 

For example, A+B*C. We know that the multiplication need to be done first, because it has higher precedence, than the + operator. Parentheses also make a difference, for example: (A+B)*C. Now the addition needs to be done first, as parentheses dictate the precedence. 

Now, we need an algorithm to "understand" the expression and calculate the result. This can be achieved using two Stacks - one for the operators, and another for the operands. 

Assuming that each operator is a single character and we use only binary operators, the algorithm is: 
Until the end of the expression is reached, get one character and perform only one of the steps (a) through (f): 
(a) If it's an operand, push it onto the operand stack. 
(b) If it's an operator, and the operator stack is empty then push it onto the operator stack. 
(c) If it's an operator and the operator stack is not empty, and the character's precedence is greater than the precedence of the stack top operator, then push the character onto the operator stack. 
(d) If it's "(", then push it onto operator stack. 
(e) If it's ")", then, until "(" is encountered, do the following: 
  1) pop operand stack once (value1) 
  2) pop operator stack once (operator) 
  3) pop operand stack again (value2) 
  4) compute value1 operator value2 
  5) push the value obtained in operand stack. 
  6) pop operator stack to ignore the "(" 
When there are no more input characters, keep processing the steps in (e), until the operator stack becomes empty. The value left in the operand stack is the final result of the expression. 

Let’s consider the expression (5+3)*6. 
1) "(" is pushed (=>) onto the operator stack 
2) 5 => operand stack 
3) + => operator stack 
4) 3 => operand stack 
5) ")" is encountered. The top operator and operands are popped from the stack, resulting in 5+3 being evaluated and pushed onto the operand stack. Now we have 8 on the operator stack 
6) * => operator stack 
7) 6 => operand stack 
8) The top operator and operands are popped from the stack, resulting in 8*6 being evaluated, and resulting in 48. This is the final value, as the operator stack is empty. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEvaluation
{
    class Program
    {
        static string expression = "(2.2*3+2)*5-(3*4)/4"; // Insert an (correct) expression to evaluate, for example: (600-500+2)/2

        static Stack<char> stOperator = new Stack<char>();
        static Stack<double> stOperand = new Stack<double>();

        static void Main(string[] args)
        {
            expression = expression.Replace(" ", String.Empty).Replace(",", ".");

            EvaluateExpression(expression);

            Console.WriteLine($"Input:  {expression}");
            Console.WriteLine($"Output: {((stOperand.Count==1)?(stOperand.Pop()).ToString().Replace(",", "."):"ERROR!")}");
        }

        static void EvaluateExpression(string e)
        {
            string number = String.Empty;

            for (int i = 0; i < e.Length; i++)
            {
                char c = e[i];

                if (c == '+' || c == '-' || c == '*' || c == '/' || c == '(' || c == ')')
                {
                    if (number != String.Empty) { stOperand.Push(Parse(number)); number = String.Empty; }

                    if (c == ')')
                    {
                        while (stOperator.Count > 0 && stOperator.Peek() != '(')
                        {
                            EvaluateNext();
                        }

                        stOperator.Pop();

                        continue;
                    }

                    if (stOperator.Count > 0)
                    {
                        if ((stOperator.Peek() == '*' || stOperator.Peek() == '/') && c != '(') EvaluateNext();
                    }

                    stOperator.Push(c);

                    continue;
                }

                number += c.ToString();
            }

            if (number != String.Empty) { stOperand.Push(Parse(number)); number = String.Empty; }

            while (stOperator.Count > 0)
            {
                EvaluateNext();
            }
        }

        static void EvaluateNext()
        {
            char o = stOperator.Pop();

            double b = stOperand.Pop();
            double a = stOperand.Pop();

            if (stOperator.Count > 0)
            {
                if (stOperator.Peek() == '-' && o != '*' && o != '/') b = -b;
            }            

            switch (o)
            {
                case '+':
                    stOperand.Push(a + b);
                    break;
                case '-':
                    stOperand.Push(a - b);
                    break;
                case '*':
                    stOperand.Push(a * b);
                    break;
                case '/':
                    stOperand.Push(a / b);
                    break;
            }
        }

        static double Parse(string number)
        {
            return (Double.Parse(number, System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}