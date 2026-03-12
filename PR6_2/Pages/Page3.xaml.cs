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
    public partial class Page3 : Page
    {
        private List<DataPoint> dataPoints;
        private double minY, maxY;
        private double xMin, xMax;

        public Page3()
        {
            InitializeComponent();
            dataPoints = new List<DataPoint>();

            // Устанавливаем значения по умолчанию согласно варианту 10
            X.Text = "";
            Xk.Text = "";
            dX.Text = "";
        }

        // Проверка ввода чисел с плавающей точкой
        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

            // Регулярное выражение для проверки double числа
            Regex regex = new Regex(@"^-?\d*[.,]?\d*$");
            e.Handled = !regex.IsMatch(newText);
        }

        // Обработка нажатия кнопки "Вычислить"
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Парсинг входных параметров
                if (!TryParseDouble(X.Text, out double x0))
                {
                    ShowError("Ошибка ввода", "Введите корректное значение X");
                    return;
                }

                if (!TryParseDouble(Xk.Text, out double xk))
                {
                    ShowError("Ошибка ввода", "Введите корректное значение Xk");
                    return;
                }

                if (!TryParseDouble(dX.Text, out double dx))
                {
                    ShowError("Ошибка ввода", "Введите корректное значение dX");
                    return;
                }

                // Константа b согласно варианту
                double b = 3.4;

                // Проверка шага
                if (dx == 0)
                {
                    ShowError("Ошибка", "Шаг dX не может быть равен 0");
                    return;
                }

                // Проверка направления обхода
                if (x0 > xk && dx > 0)
                {
                    ShowError("Ошибка", "При X > Xk шаг dX должен быть отрицательным");
                    return;
                }
                if (x0 < xk && dx < 0)
                {
                    ShowError("Ошибка", "При X < Xk шаг dX должен быть положительным");
                    return;
                }

                // Вычисление функции
                CalculateFunction(x0, xk, dx, b);

                // Обновление информации о диапазоне
                RangeInfo.Text = $"Диапазон: X от {xMin:F2} до {xMax:F2}, Y от {minY:F2} до {maxY:F2} | Всего точек: {dataPoints.Count} | b = {b}";
            }
            catch (Exception ex)
            {
                ShowError("Ошибка вычисления", ex.Message);
            }
        }

        // Обработка нажатия кнопки "Очистить"
        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            // Очистка полей ввода
            X.Text = "";
            Xk.Text = "";
            dX.Text = "";

            // Очистка результатов
            Answer.Clear();

            // Очистка графика
            GraphCanvas.Children.Clear();
            dataPoints.Clear();

            // Сброс информации
            RangeInfo.Text = "Диапазон: ";
        }

        // Вспомогательный метод для парсинга double с учетом разделителей
        private bool TryParseDouble(string text, out double result)
        {
            return double.TryParse(text.Replace('.', ','), out result);
        }

        /// <summary>
        /// Вычисление функции y = x * sin(√(x + b) - 0.0084)
        /// </summary>
        private double CalculateY(double x, double b)
        {
            // Проверка области определения: подкоренное выражение должно быть >= 0
            double underSqrt = x + b;

            if (underSqrt < 0)
            {
                throw new ArgumentException($"Значение x + b = {underSqrt:F4} отрицательно. Функция не определена.");
            }

            double sqrtValue = Math.Sqrt(underSqrt);
            double argument = sqrtValue - 0.0084;

            return x * Math.Sin(argument);
        }

        // Основной метод вычисления функции и построения графика
        private void CalculateFunction(double x0, double xk, double dx, double b)
        {
            dataPoints.Clear();
            GraphCanvas.Children.Clear();

            xMin = Math.Min(x0, xk);
            xMax = Math.Max(x0, xk);

            string resultText = "Результаты вычислений:\n";
            resultText += "═══════════════════════════════════════════════════════════════\n";
            resultText += $"Вариант 10: y = x·sin(√(x + b) - 0.0084)\n";
            resultText += $"X = {x0:F4}\tXk = {xk:F4}\tdX = {dx:F4}\tb = {b:F4}\n";
            resultText += "═══════════════════════════════════════════════════════════════\n";
            resultText += $"{"№",-4} {"X",-15} {"y = x·sin(√(x+b) - 0.0084)",-35}\n";
            resultText += "───────────────────────────────────────────────────────────────\n";

            int count = 0;
            double x = x0;
            minY = double.MaxValue;
            maxY = double.MinValue;

            List<string> errors = new List<string>();

            while ((dx > 0 && x <= xk + 1e-10) || (dx < 0 && x >= xk - 1e-10))
            {
                try
                {
                    // Вычисление функции
                    double y = CalculateY(x, b);

                    // Сохраняем точку
                    dataPoints.Add(new DataPoint(x, y));

                    // Обновляем min и max Y
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;

                    // Добавляем в текстовый результат
                    resultText += $"{count + 1,-4} {x,15:F6} {y,35:F6}\n";
                }
                catch (ArgumentException ex)
                {
                    errors.Add($"x = {x:F6}: {ex.Message}");
                    resultText += $"{count + 1,-4} {x,15:F6} {"[ОШИБКА: вне области определения]",-35}\n";
                }

                x += dx;
                count++;

                // Защита от бесконечного цикла
                if (count > 1000)
                {
                    resultText += "───────────────────────────────────────────────────────────────\n";
                    resultText += "⚠ Достигнуто максимальное количество итераций (1000)\n";
                    break;
                }
            }

            resultText += "═══════════════════════════════════════════════════════════════\n";
            resultText += $"Всего вычислено значений: {count}\n";

            if (dataPoints.Count > 0)
            {
                resultText += $"Минимальное Y: {minY:F6}\tМаксимальное Y: {maxY:F6}\n";
            }

            if (errors.Count > 0)
            {
                resultText += "\n⚠ Обнаружены ошибки:\n";
                foreach (string error in errors)
                {
                    resultText += $"  • {error}\n";
                }
            }

            Answer.Text = resultText;

            // Рисуем график только если есть точки
            if (dataPoints.Count > 0)
            {
                DrawGraph();
            }
            else
            {
                RangeInfo.Text = "Нет точек для отображения графика";
            }
        }

        // Метод для рисования графика на Canvas
        private void DrawGraph()
        {
            if (dataPoints.Count == 0) return;

            // Отступы от краев Canvas
            const int margin = 40;
            double canvasWidth = GraphCanvas.Width - 2 * margin;
            double canvasHeight = GraphCanvas.Height - 2 * margin;

            // Добавляем небольшой отступ по Y для лучшей визуализации
            double yRange = maxY - minY;
            if (yRange == 0) yRange = 1;

            // Добавляем 10% отступа сверху и снизу
            double yMinWithPadding = minY - yRange * 0.1;
            double yMaxWithPadding = maxY + yRange * 0.1;

            // Рисование сетки
            DrawGrid(margin, canvasWidth, canvasHeight, yMinWithPadding, yMaxWithPadding);

            // Рисование осей координат
            DrawAxes(margin, canvasWidth, canvasHeight);

            // Рисование графика
            DrawFunctionLine(margin, canvasWidth, canvasHeight, yMinWithPadding, yMaxWithPadding);

            // Добавление подписей
            AddLabels(margin, canvasWidth, canvasHeight);

            // Добавление заголовка с формулой
            AddFormulaLabel();
        }

        // Рисование сетки
        private void DrawGrid(int margin, double canvasWidth, double canvasHeight, double yMin, double yMax)
        {
            const int gridLines = 8;
            Brush gridBrush = new SolidColorBrush(Color.FromRgb(220, 220, 220));

            // Вертикальные линии сетки
            for (int i = 0; i <= gridLines; i++)
            {
                double x = margin + (i * canvasWidth / gridLines);

                Line line = new Line
                {
                    X1 = x,
                    Y1 = margin,
                    X2 = x,
                    Y2 = GraphCanvas.Height - margin,
                    Stroke = gridBrush,
                    StrokeThickness = 0.5
                };
                GraphCanvas.Children.Add(line);

                // Подписи значений X
                if (i % 2 == 0)
                {
                    double xValue = xMin + (i * (xMax - xMin) / gridLines);
                    TextBlock label = new TextBlock
                    {
                        Text = xValue.ToString("F2"),
                        FontSize = 9,
                        Foreground = Brushes.Gray
                    };
                    Canvas.SetLeft(label, x - 15);
                    Canvas.SetTop(label, GraphCanvas.Height - margin + 5);
                    GraphCanvas.Children.Add(label);
                }
            }

            // Горизонтальные линии сетки
            for (int i = 0; i <= gridLines; i++)
            {
                double y = margin + (i * canvasHeight / gridLines);

                Line line = new Line
                {
                    X1 = margin,
                    Y1 = y,
                    X2 = GraphCanvas.Width - margin,
                    Y2 = y,
                    Stroke = gridBrush,
                    StrokeThickness = 0.5
                };
                GraphCanvas.Children.Add(line);

                // Подписи значений Y
                if (i % 2 == 0)
                {
                    double yValue = yMax - (i * (yMax - yMin) / gridLines);
                    TextBlock label = new TextBlock
                    {
                        Text = yValue.ToString("F2"),
                        FontSize = 9,
                        Foreground = Brushes.Gray
                    };
                    Canvas.SetLeft(label, 5);
                    Canvas.SetTop(label, y - 8);
                    GraphCanvas.Children.Add(label);
                }
            }
        }

        // Рисование осей координат
        private void DrawAxes(int margin, double canvasWidth, double canvasHeight)
        {
            // Ось X
            Line xAxis = new Line
            {
                X1 = margin,
                Y1 = GraphCanvas.Height - margin,
                X2 = GraphCanvas.Width - margin,
                Y2 = GraphCanvas.Height - margin,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            GraphCanvas.Children.Add(xAxis);

            // Ось Y
            Line yAxis = new Line
            {
                X1 = margin,
                Y1 = margin,
                X2 = margin,
                Y2 = GraphCanvas.Height - margin,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            GraphCanvas.Children.Add(yAxis);

            // Стрелки на осях
            Polygon xArrow = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(GraphCanvas.Width - margin + 5, GraphCanvas.Height - margin - 5),
                    new Point(GraphCanvas.Width - margin + 5, GraphCanvas.Height - margin + 5),
                    new Point(GraphCanvas.Width - margin + 15, GraphCanvas.Height - margin)
                },
                Fill = Brushes.Black
            };
            GraphCanvas.Children.Add(xArrow);

            Polygon yArrow = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(margin - 5, margin - 5),
                    new Point(margin + 5, margin - 5),
                    new Point(margin, margin - 15)
                },
                Fill = Brushes.Black
            };
            GraphCanvas.Children.Add(yArrow);
        }

        // Рисование линии функции
        private void DrawFunctionLine(int margin, double canvasWidth, double canvasHeight, double yMin, double yMax)
        {
            if (dataPoints.Count < 2) return;

            PointCollection points = new PointCollection();

            foreach (var point in dataPoints)
            {
                double canvasX = margin + ((point.X - xMin) / (xMax - xMin)) * canvasWidth;
                double canvasY = GraphCanvas.Height - margin - ((point.Y - yMin) / (yMax - yMin)) * canvasHeight;

                points.Add(new Point(canvasX, canvasY));
            }

            Polyline polyline = new Polyline
            {
                Points = points,
                Stroke = new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                StrokeThickness = 2
            };
            GraphCanvas.Children.Add(polyline);

            // Добавляем точки маркеры (для наглядности)
            foreach (var point in dataPoints)
            {
                double canvasX = margin + ((point.X - xMin) / (xMax - xMin)) * canvasWidth;
                double canvasY = GraphCanvas.Height - margin - ((point.Y - yMin) / (yMax - yMin)) * canvasHeight;

                Ellipse ellipse = new Ellipse
                {
                    Width = 3,
                    Height = 3,
                    Fill = Brushes.Red,
                    Stroke = Brushes.White,
                    StrokeThickness = 0.5
                };

                Canvas.SetLeft(ellipse, canvasX - 1.5);
                Canvas.SetTop(ellipse, canvasY - 1.5);
                GraphCanvas.Children.Add(ellipse);
            }
        }

        // Добавление подписей осей
        private void AddLabels(int margin, double canvasWidth, double canvasHeight)
        {
            // Подпись оси X
            TextBlock xLabel = new TextBlock
            {
                Text = "X",
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            Canvas.SetLeft(xLabel, GraphCanvas.Width - margin + 5);
            Canvas.SetTop(xLabel, GraphCanvas.Height - margin - 20);
            GraphCanvas.Children.Add(xLabel);

            // Подпись оси Y
            TextBlock yLabel = new TextBlock
            {
                Text = "Y",
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };
            Canvas.SetLeft(yLabel, margin - 20);
            Canvas.SetTop(yLabel, margin - 20);
            GraphCanvas.Children.Add(yLabel);
        }

        // Добавление заголовка с формулой
        private void AddFormulaLabel()
        {
            TextBlock formulaLabel = new TextBlock
            {
                Text = "y = x·sin(√(x+3.4) - 0.0084)",
                FontSize = 10,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                Background = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255))
            };
            Canvas.SetLeft(formulaLabel, 10);
            Canvas.SetTop(formulaLabel, 5);
            GraphCanvas.Children.Add(formulaLabel);
        }

        // Метод для отображения ошибок
        private void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
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