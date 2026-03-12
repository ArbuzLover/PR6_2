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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calculate(X.Text, Y.Text, Z.Text);
        }

        public bool Calculate(string X, string Y, string Z)
        {
            if (X.Length > 0 & Y.Length > 0 & Z.Length > 0)
            {
                double x = Convert.ToDouble(X);
                double y = Convert.ToDouble(Y);
                double z = Convert.ToDouble(Z);



                Answer.Text = $"{Math.Pow(2, -x) * Math.Sqrt(x + Math.Pow(Math.Abs(y), 1.0 / 4.0) * Math.Pow(Math.Exp(x - 1) / Math.Sin(z), 1.0 / 3.0))}";
                return true;
            }
            else { MessageBox.Show("Введите все значения"); return false; }
           
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            X.Text = "";
            Y.Text = "";
            Z.Text = "";
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
    }
}
