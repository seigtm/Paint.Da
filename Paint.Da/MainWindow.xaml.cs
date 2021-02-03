using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Paint.Da
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyComboBox.SelectedIndex = 0;
            MyRadioButtonPainting.IsChecked = true;
            MySlider.Value = 10;
            MySlider.Minimum = 1;
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)MyComboBox.SelectedItem;

            switch (selectedComboBoxItem.Content.ToString())
            {
                case "Black":
                    MyInkCanvas.DefaultDrawingAttributes.Color = Colors.Black;
                    break;

                case "Green":
                    MyInkCanvas.DefaultDrawingAttributes.Color = Colors.Green;
                    break;

                case "Blue":
                    MyInkCanvas.DefaultDrawingAttributes.Color = Colors.Blue;
                    break;

                case "Red":
                    MyInkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
                    break;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton selectedRadioButton = (RadioButton)sender;
            switch (selectedRadioButton.Content.ToString())
            {
                case "Рисование":
                    MyInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    break;

                case "Редактирование":
                    MyInkCanvas.EditingMode = InkCanvasEditingMode.Select;
                    break;

                case "Удаление":
                    MessageBoxResult clearingMode = 
                        MessageBox.Show("Удаление ластиком (если нет - фигуры целиком)?",
                                        "Выбор режима удаления",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Exclamation);
                    
                    if (clearingMode == MessageBoxResult.Yes)
                    {
                        MyInkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                        break;
                    }
                    MyInkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    break;
            }
        }

        private void Button_About_Clicked(object sender, RoutedEventArgs e)
        {
            _ = MessageBox.Show("  Графический растровый редактор Paint.Da предназначен для создания простейших зарисовок." +
                "\n  Создано как проект лабораторной работы двумя студентами колледжа ПКГХ группы ИП-18-4: Барановым К.П. и Морозовым Н.Д. в 2021 году!" +
                "\n  Все права защищены законодательством РФ.", "О программе!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MyInkCanvas.DefaultDrawingAttributes.Width = MyInkCanvas.DefaultDrawingAttributes.Height = e.NewValue;
        }

        private void Button_Erase_Clicked(object sender, RoutedEventArgs e)
        {
            MyInkCanvas.Strokes.Clear();
        }
    }
}

/*
    Рисование      InkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
    Редактирование InkCanvas1.EditingMode = InkCanvasEditingMode.Select;
    Удаление       InkCanvas1.EditingMode = InkCanvasEditingMode.EraseByStroke;
    Удаление       InkCanvas1.EditingMode = InkCanvasEditingMode.EraseByPoint;
    Удаление       InkCanvas1.Strokes.Clear();
*/
