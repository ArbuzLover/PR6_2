using System;
using System.Collections.Generic;

namespace PR6_2
{
    /// <summary>
    /// Калькулятор для первой страницы
    /// </summary>
    public static class Page1Calculator
    {
        /// <summary>
        /// Вычисляет значение функции u = 2^(-x) * sqrt(x + |y|^(1/4) * (e^(x-1)/sin(z))^(1/3))
        /// </summary>
        public static bool TryCalculate(double x, double y, double z, out double result)
        {
            result = 0;

            // Проверка деления на ноль
            if (Math.Abs(Math.Sin(z)) < 1e-10)
                return false;

            double term1 = Math.Pow(2, -x);

            // Вычисление (e^(x-1)/sin(z))^(1/3) с использованием кубического корня
            double fraction = Math.Exp(x - 1) / Math.Sin(z);

            // ИСПРАВЛЕНО: Правильное вычисление кубического корня для отрицательных чисел
            double cubeRoot;
            if (fraction >= 0)
                cubeRoot = Math.Pow(fraction, 1.0 / 3.0);
            else
                cubeRoot = -Math.Pow(-fraction, 1.0 / 3.0); // Для отрицательных чисел

            // Подкоренное выражение в sqrt
            double sqrtArgument = x + Math.Pow(Math.Abs(y), 1.0 / 4.0) * cubeRoot;

            // ИСПРАВЛЕНО: Добавлена проверка на NaN и Infinity
            if (sqrtArgument < 0 || double.IsNaN(sqrtArgument) || double.IsInfinity(sqrtArgument))
                return false;

            result = term1 * Math.Sqrt(sqrtArgument);

            // ИСПРАВЛЕНО: Проверка результата
            if (double.IsNaN(result) || double.IsInfinity(result))
                return false;

            return true;
        }
    }
}

/// <summary>
/// Калькулятор для второй страницы
/// </summary>
public static class Page2Calculator
{
    /// <summary>
    /// Вычисляет значение функции в зависимости от условия |x*m| > 10, <10, =10
    /// </summary>
    public static bool TryCalculate(double fx, double m, double x, out double result)
    {
        result = 0;
        const double epsilon = 1e-10;
        double xq = Math.Abs(x * m);
        double argument = Math.Abs(fx) + Math.Abs(m);
        if (xq > 10 + epsilon)
        {
            

            // ИСПРАВЛЕНО: Строгая проверка аргумента логарифма
            if (argument <= epsilon) 
             return false;   // argument должен быть строго больше 0
                

            result = Math.Log(argument);

            // ИСПРАВЛЕНО: Дополнительная проверка результата
            if (double.IsNaN(result) || double.IsInfinity(result))
                return false;

            return true;
        }

        if (xq < 10 - epsilon)
        {
            if (argument <= epsilon)
                return false;

            result = Math.Exp(fx + m);

            // ИСПРАВЛЕНО: Проверка на переполнение
            if (double.IsInfinity(result))
                return false;

            return true;
        }

        // |xq| ≈ 10 (с учетом погрешности)
        result = fx + m;
        return true;
    }
}

/// <summary>
/// Калькулятор для третьей страницы
/// </summary>
public static class Page3Calculator
    {
        /// <summary>
        /// Вычисляет значение функции y = x * sin(√(x + b) - 0.0084)
        /// </summary>
        public static bool TryCalculateY(double x, double b, out double result)
        {
            result = 0;
            double underSqrt = x + b;

            if (underSqrt < 0)
                return false;

            double sqrtValue = Math.Sqrt(underSqrt);
            double argument = sqrtValue - 0.0084;
            result = x * Math.Sin(argument);
            return true;
        }

        /// <summary>
        /// Вычисляет массив значений функции в диапазоне [x0, xk] с шагом dx
        /// </summary>
        public static bool TryCalculateRange(double b, double x0, double xk, double dx,
            out List<double> xValues, out List<double> yValues)
        {
            xValues = new List<double>();
            yValues = new List<double>();

            if (dx == 0)
                return false;

            // Проверка направления обхода
            if (x0 > xk && dx > 0)
                return false;
            if (x0 < xk && dx < 0)
                return false;

            double x = x0;
            int count = 0;
            const int maxIterations = 1000;

            while (((dx > 0 && x <= xk + 1e-10) || (dx < 0 && x >= xk - 1e-10)) && count < maxIterations)
            {
                if (TryCalculateY(x, b, out double y))
                {
                    xValues.Add(x);
                    yValues.Add(y);
                }

                x += dx;
                count++;
            }

            return xValues.Count > 0;
        }
    }