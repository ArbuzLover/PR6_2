using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PR6_2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double fx;
            if (ShFunctionRadio.IsChecked == true)
            {
                fx = Math.Sinh(Convert.ToDouble(x.Text));
            }
            else if (SquareFunctionRadio.IsChecked == true)
            {
                fx = Convert.ToDouble(x.Text) * Convert.ToDouble(x.Text);
            }
            else // ExpFunctionRadio
            {
                fx = Math.Exp(Convert.ToDouble(x.Text));
            }

            // Вычисление основной функции согласно варианту 8
            double result = CalculateMainFunction(fx, Convert.ToDouble(m.Text), Convert.ToDouble(x.Text));

            Answer.Text = $"{result}";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            x.Text = "";
            m.Text = "";
            Answer.Text = "";
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

            // Регулярное выражение для проверки double числа
            // Разрешает: цифры, один минус в начале, одну точку
            Regex regex = new Regex(@"^-?\d*\.?\d*$");
            e.Handled = !regex.IsMatch(newText);
        }


        private double CalculateMainFunction(double fx, double m, double x)
        {
            double xq = Math.Abs(x * m);

            // Константа для сравнения с плавающей точкой
            const double epsilon = 1e-10;

            // Проверяем условие |xq| > 10
            if (xq > 10 + epsilon)
            {
                // ln(|f(x)| + |q|)
                // Проверяем, что аргумент логарифма положительный
                double argument = Math.Abs(fx) + Math.Abs(m);
                if (argument <= 0)
                {
                    throw new ArgumentException("Аргумент логарифма должен быть положительным");
                }
                return Math.Log(argument);
            }

            // Проверяем условие |xq| < 10
            if (xq < 10 - epsilon)
            {
                // e^(f(x) + q)
                return Math.Exp(fx + m);
            }

            // Условие |xq| = 10 (с учетом погрешности)
            if (Math.Abs(xq - 10) <= epsilon)
            {
                // f(x) + q
                return fx + m;
            }

            // Если ни одно условие не выполнилось (маловероятно)
            throw new InvalidOperationException("Не удалось определить условие для вычисления функции");
        }
    }
}
