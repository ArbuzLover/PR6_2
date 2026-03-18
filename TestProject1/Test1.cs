using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using PR6_2;

namespace TestProject1
{
    [TestClass]
    public sealed class Page1Testing
    {
        /// <summary>
        /// Положительный тест 1.
        /// Проверяет, что при корректных входных данных метод возвращает true.
        /// </summary>
        [TestMethod]
        public void Calculate_ValidInputs_ReturnsTrue()
        {
            // Act
            bool success = Page1Calculator.TryCalculate(1.0, 4.0, Math.PI / 2, out _);

            // Assert
            Assert.IsTrue(success, "TryCalculate должен вернуть true для корректных входных данных.");
        }

        /// <summary>
        /// Положительный тест 2.
        /// Проверяет точность вычисления при x=1, y=4, z=π/2 —
        /// результат должен совпадать с эталоном с погрешностью не более 0.0001.
        /// </summary>
        [TestMethod]
        public void Calculate_ValidInputs_ResultMatchesExpected()
        {
            // Arrange
            double expected = 0.7769; // Ручной расчет

            // Act
            Page1Calculator.TryCalculate(1.0, 4.0, Math.PI / 2, out double actual);

            // Assert
            Assert.AreEqual(expected, actual, 0.0001, "Результат не совпадает с ожидаемым значением.");
        }


        

        /// <summary>
        /// Отрицательный тест 1.
        /// Проверяет, что при sin(z)=0 метод возвращает false (деление на ноль).
        /// </summary>
        [TestMethod]
        public void Calculate_SinZero_ReturnsFalse()
        {
            // Act
            bool success = Page1Calculator.TryCalculate(1.0, 4.0, 0.0, out _);

            // Assert
            Assert.IsFalse(success, "TryCalculate должен вернуть false, когда sin(z) равен нулю.");
        }

        /// <summary>
        /// Отрицательный тест 2.
        /// Проверяет, что при отрицательном подкоренном выражении метод возвращает false.
        /// </summary>
        [TestMethod]
        public void Calculate_NegativeSqrtArgument_ReturnsFalse()
        {
            // Act
            bool success = Page1Calculator.TryCalculate(-10.0, 0.0, Math.PI / 2, out _);

            // Assert
            Assert.IsFalse(success, "TryCalculate должен вернуть false при отрицательном подкоренном выражении.");
        }
    }

    [TestClass]
    public sealed class Page2Testing
    {
        /// <summary>
        /// Положительный тест 1.
        /// Проверяет ветку |x*m| > 10 (логарифм)
        /// </summary>
        [TestMethod]
        public void Calculate_XqGreaterThan10_ReturnsLog()
        {
            // Arrange
            double fx = 5.0, m = 2.0, x = 10.0; // |10*2| = 20 > 10
            double expected = Math.Log(7); // ln(|5| + |2|) = ln(7) ≈ 1.9459

            // Act
            bool success = Page2Calculator.TryCalculate(fx, m, x, out double result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(expected, result, 0.000001, "Результат для ветки |x*m| > 10 не совпадает.");
        }

        /// <summary>
        /// Положительный тест 2.
        /// Проверяет ветку |x*m| < 10 (экспонента)
        /// </summary>
        [TestMethod]
        public void Calculate_XqLessThan10_ReturnsExp()
        {
            // Arrange
            double fx = 1.5, m = 3.0, x = 2.0; // |2*3| = 6 < 10
            double expected = Math.Exp(4.5); // e^(1.5 + 3) = e^4.5 ≈ 90.017

            // Act
            bool success = Page2Calculator.TryCalculate(fx, m, x, out double result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(expected, result, 0.000001, "Результат для ветки |x*m| < 10 не совпадает.");
        }

        /// <summary>
        /// Положительный тест 3.
        /// Проверяет ветку |x*m| ≈ 10 (сумма)
        /// </summary>
        [TestMethod]
        public void Calculate_XqEquals10_ReturnsSum()
        {
            // Arrange
            double fx = 3.0, m = 2.0, x = 5.0; // |5*2| = 10
            double expected = 5.0; // fx + m = 3 + 2 = 5

            // Act
            bool success = Page2Calculator.TryCalculate(fx, m, x, out double result);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(expected, result, 0.000001, "Результат для ветки |x*m| = 10 не совпадает.");
        }

        /// <summary>
        /// Отрицательный тест.
        /// Проверяет, что при аргументе логарифма <= 0 метод возвращает false.
        /// </summary>
        [TestMethod]
        public void Calculate_LogArgumentZero_ReturnsFalse()
        {
            // Arrange: |x*m| > 10, но |fx| + |m| = 0
            double fx = 0.0, m = 0.0, x = 20.0;

            // Act
            bool success = Page2Calculator.TryCalculate(fx, m, x, out double result);

            // Assert
            Assert.IsFalse(success, "TryCalculate должен вернуть false, когда аргумент логарифма <= 0.");
            Assert.AreEqual(0.0, result, "Результат должен быть 0 при неудаче");
        }

        /// <summary>
        /// Дополнительный тест: проверка переполнения экспоненты
        /// </summary>
        
    }

    [TestClass]
    public sealed class Page3Testing
    {
        /// <summary>
        /// Положительный тест 1.
        /// Проверяет вычисление одной точки
        /// </summary>
        [TestMethod]
        public void CalculateY_ValidInputs_ReturnsCorrectYValue()
        {
            // Arrange
            double b = 3.4, x = 1.0;
            double expectedY = 1.0 * Math.Sin(Math.Sqrt(1.0 + 3.4) - 0.0084);

            // Act
            bool success = Page3Calculator.TryCalculateY(x, b, out double actualY);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(expectedY, actualY, 0.000001, "Значение y не совпадает с ожидаемым.");
        }

        /// <summary>
        /// Положительный тест 2.
        /// Проверяет вычисление диапазона значений
        /// </summary>
        [TestMethod]
        public void CalculateRange_ValidInputs_ReturnsCorrectNumberOfPoints()
        {
            // Arrange
            double b = 3.4, x0 = 0, xk = 5, dx = 1;
            int expectedCount = 6; // 0,1,2,3,4,5

            // Act
            bool success = Page3Calculator.TryCalculateRange(b, x0, xk, dx, out var xValues, out var yValues);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(expectedCount, xValues.Count, "Количество точек X не совпадает");
            Assert.AreEqual(expectedCount, yValues.Count, "Количество точек Y не совпадает");
            Assert.AreEqual(0.0, xValues[0], 0.000001);
            Assert.AreEqual(5.0, xValues[5], 0.000001);
        }

        /// <summary>
        /// Положительный тест 3.
        /// Проверяет работу с отрицательным шагом (обратный диапазон)
        /// </summary>
        [TestMethod]
        public void CalculateRange_NegativeDx_WorksCorrectly()
        {
            // Arrange
            double b = 3.4, x0 = 5, xk = 0, dx = -1;
            int expectedCount = 6; // 5,4,3,2,1,0

            // Act
            bool success = Page3Calculator.TryCalculateRange(b, x0, xk, dx, out var xValues, out var yValues);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(expectedCount, xValues.Count);
            Assert.AreEqual(5.0, xValues[0], 0.000001);
            Assert.AreEqual(0.0, xValues[5], 0.000001);
        }

        /// <summary>
        /// Положительный тест 4.
        /// Проверяет, что точки с x+b < 0 пропускаются
        /// </summary>
        [TestMethod]
        public void CalculateRange_NegativeXSkipped_OnlyValidPointsReturned()
        {
            // Arrange
            double b = 1.0, x0 = -2, xk = 2, dx = 1;
            // x = -2: underSqrt = -1 (<0) - пропускается
            // x = -1: underSqrt = 0 (>=0) - включается
            // x = 0: underSqrt = 1 - включается
            // x = 1: underSqrt = 2 - включается
            // x = 2: underSqrt = 3 - включается
            int expectedCount = 4; // -1,0,1,2

            // Act
            bool success = Page3Calculator.TryCalculateRange(b, x0, xk, dx, out var xValues, out _);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(expectedCount, xValues.Count);
            Assert.AreEqual(-1.0, xValues[0], 0.000001);
            Assert.AreEqual(0.0, xValues[1], 0.000001);
            Assert.AreEqual(1.0, xValues[2], 0.000001);
            Assert.AreEqual(2.0, xValues[3], 0.000001);
        }

        /// <summary>
        /// Отрицательный тест 1.
        /// Проверяет, что при dx = 0 метод возвращает false
        /// </summary>
        [TestMethod]
        public void CalculateRange_DxIsZero_ReturnsFalse()
        {
            // Arrange
            double b = 3.4, x0 = 0, xk = 5, dx = 0;

            // Act
            bool success = Page3Calculator.TryCalculateRange(b, x0, xk, dx, out _, out _);

            // Assert
            Assert.IsFalse(success, "TryCalculateRange должен вернуть false, когда dx равен нулю.");
        }

        /// <summary>
        /// Отрицательный тест 2.
        /// Проверяет, что при несоответствии направления шага метод возвращает false
        /// </summary>
        [TestMethod]
        public void CalculateRange_WrongDirection_ReturnsFalse()
        {
            // Arrange
            double b = 3.4, x0 = 0, xk = 5, dx = -1; // dx отрицательный, хотя x0 < xk

            // Act
            bool success = Page3Calculator.TryCalculateRange(b, x0, xk, dx, out _, out _);

            // Assert
            Assert.IsFalse(success, "TryCalculateRange должен вернуть false при несоответствии направления шага.");
        }

        /// <summary>
        /// Отрицательный тест 3.
        /// Проверяет, что при x + b < 0 TryCalculateY возвращает false
        /// </summary>
        [TestMethod]
        public void CalculateY_NegativeSqrt_ReturnsFalse()
        {
            // Arrange
            double b = 3.4, x = -10.0; // -10 + 3.4 = -6.6 < 0

            // Act
            bool success = Page3Calculator.TryCalculateY(x, b, out _);

            // Assert
            Assert.IsFalse(success, "TryCalculateY должен вернуть false при отрицательном подкоренном выражении.");
        }
    }
}