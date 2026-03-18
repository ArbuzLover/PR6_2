using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace PR6_2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        private List<DataPoint> dataPoints;

        public Page3()
        {
            InitializeComponent();
            dataPoints = new List<DataPoint>();
        }

        /// <summary>
        /// Вычисляет значения функции и строит график
        /// </summary>
        public bool Calculate(double b, double x0, double xk, double dx)
        {
            if (!Page3Calculator.TryCalculateRange(b, x0, xk, dx, out List<double> xValues, out List<double> yValues))
            {
                MessageBox.Show("Некорректные параметры для вычисления");
                return false;
            }

            // Очищаем предыдущие результаты
            GraphCanvas.Children.Clear();
            dataPoints.Clear();
            Answer.Clear();

            // Заполняем точки данных
            for (int i = 0; i < xValues.Count; i++)
            {
                dataPoints.Add(new DataPoint(xValues[i], yValues[i]));
            }

            // Выводим результаты в текстовое поле
            DisplayResults(xValues, yValues, b);

            // Рисуем график
            if (dataPoints.Count > 0)
            {
                DrawGraph();
            }

            LastXValues = xValues;
            LastYValues = yValues;
            return true;
        }

        /// <summary>
        /// Отображает результаты вычислений
        /// </summary>
        private void DisplayResults(List<double> xValues, List<double> yValues, double b)
        {
            string resultText = "Результаты вычислений:\n";
            resultText += "═══════════════════════════════════════════════════════════════\n";
            resultText += $"Вариант 10: y = x·sin(√(x + b) - 0.0084), b = {b:F4}\n";
            resultText += "═══════════════════════════════════════════════════════════════\n";
            resultText += $"{"№",-4} {"X",-15} {"Y",-20}\n";
            resultText += "───────────────────────────────────────────────────────────────\n";

            for (int i = 0; i < xValues.Count; i++)
            {
                resultText += $"{i + 1,-4} {xValues[i],15:F6} {yValues[i],20:F6}\n";
            }

            resultText += "═══════════════════════════════════════════════════════════════\n";
            resultText += $"Всего точек: {xValues.Count}\n";

            Answer.Text = resultText;
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Вычислить»
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(X.Text.Replace('.', ','), out double x0) ||
                !double.TryParse(Xk.Text.Replace('.', ','), out double xk) ||
                !double.TryParse(dX.Text.Replace('.', ','), out double dx))
            {
                MessageBox.Show("Введите корректные числовые значения");
                return;
            }

            // Константа b согласно варианту
            double b = 3.4;

            Calculate(b, x0, xk, dx);
        }

        /// <summary>
        /// Обработчик нажатия кнопки «Очистить»
        /// </summary>
        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            X.Clear();
            Xk.Clear();
            dX.Clear();
            Answer.Clear();
            GraphCanvas.Children.Clear();
            dataPoints.Clear();
        }

        /// <summary>
        /// Проверка ввода чисел с плавающей точкой
        /// </summary>
        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
            Regex regex = new Regex(@"^-?\d*[.,]?\d*$");
            e.Handled = !regex.IsMatch(newText);
        }

        // Методы для рисования графика (DrawGraph, DrawGrid, DrawAxes, DrawFunctionLine, AddLabels, AddFormulaLabel)
        // остаются без изменений из исходного кода

        private void DrawGraph()
        {
            // ... (код из исходной Page3)
        }

        // Остальные методы рисования...

        /// <summary>
        /// Последние вычисленные значения X. Используется для тестирования.
        /// </summary>
        public List<double> LastXValues { get; private set; }

        /// <summary>
        /// Последние вычисленные значения Y. Используется для тестирования.
        /// </summary>
        public List<double> LastYValues { get; private set; }
    }

    // Класс для хранения точки данных
    public class DataPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public DataPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}