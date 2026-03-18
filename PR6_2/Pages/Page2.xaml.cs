using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Вычисляет значение функции по формуле
        /// </summary>
        public bool Calculate(double fx, double m, double x)
        {
            if (!Page2Calculator.TryCalculate(fx, m, x, out double result))
            {
                MessageBox.Show("Некорректные входные данные (аргумент логарифма должен быть положительным)");
                return false;
            }

            Answer.Text = result.ToString("F6");
            LastResult = result;
            return true;
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Вычислить»
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем значение f(x) в зависимости от выбранной функции
            double fx;

            if (!double.TryParse(x.Text.Replace('.', ','), out double xValue))
            {
                MessageBox.Show("Введите корректное значение x");
                return;
            }

            if (ShFunctionRadio.IsChecked == true)
            {
                fx = Math.Sinh(xValue);
            }
            else if (SquareFunctionRadio.IsChecked == true)
            {
                fx = xValue * xValue;
            }
            else if (ExpFunctionRadio.IsChecked == true)
            {
                fx = Math.Exp(xValue);
            }
            else
            {
                MessageBox.Show("Выберите функцию f(x)");
                return;
            }

            if (!double.TryParse(m.Text.Replace('.', ','), out double mValue))
            {
                MessageBox.Show("Введите корректное значение m");
                return;
            }

            Calculate(fx, mValue, xValue);
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Очистить»
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            x.Clear();
            m.Clear();
            Answer.Clear();
        }

        /// <summary>
        /// Проверка ввода чисел с плавающей точкой
        /// </summary>
        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
            Regex regex = new Regex(@"^-?\d*\.?\d*$");
            e.Handled = !regex.IsMatch(newText);
        }

        /// <summary>
        /// Последний вычисленный результат. Используется для тестирования.
        /// </summary>
        public double LastResult { get; private set; }
    }
}