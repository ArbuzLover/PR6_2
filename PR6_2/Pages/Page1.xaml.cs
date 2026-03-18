using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace PR6_2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вычисляет значение функции по формуле
        /// </summary>
        public bool Calculate(double x, double y, double z)
        {
            if (!Page1Calculator.TryCalculate(x, y, z, out double result))
            {
                MessageBox.Show("Некорректные входные данные (деление на ноль или отрицательное подкоренное выражение)");
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
            if (string.IsNullOrWhiteSpace(X.Text) ||
                string.IsNullOrWhiteSpace(Y.Text) ||
                string.IsNullOrWhiteSpace(Z.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (double.TryParse(X.Text.Replace('.', ','), out double x) &&
                double.TryParse(Y.Text.Replace('.', ','), out double y) &&
                double.TryParse(Z.Text.Replace('.', ','), out double z))
            {
                Calculate(x, y, z);
            }
            else
            {
                MessageBox.Show("Введены некорректные числа");
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Очистить»
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            X.Clear();
            Y.Clear();
            Z.Clear();
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