using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paint.Da
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // При инициализации главной формы:
            MyComboBox.SelectedIndex = 0;           // Устанавливаем ComboBox с выбором цвета в позицию 0.
            MyRadioButtonPainting.IsChecked = true; // Устанавливаем RadioButton рисования в позицию checked.
            MySlider.Value = 10;                    // Размер кисти = 10.
            MySlider.Minimum = 1;                   // Минимальный размер кисти = 1.
        }

        // Обработчик выбора цвета в ComboBox.
        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)MyComboBox.SelectedItem;
            // В зависимости от выбранного item в ComboBox, устанавливаем необходимый цвет кисти.
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

        // Обработчик выбора RadioButton с доступными действиями над канвасом.
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton selectedRadioButton = (RadioButton)sender;
            // В зависимости от выбранного RadioButton, изменяем режим редактирования канваса.
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
                    MyInkCanvas.EditingMode = (clearingMode == MessageBoxResult.Yes)
                                               ? InkCanvasEditingMode.EraseByPoint
                                               : InkCanvasEditingMode.EraseByStroke;
                    break;
            }
        }

        // Обработчик нажатия на кнопку "О программе".
        private void Button_About_Clicked(object sender, RoutedEventArgs e)
        {
            _ = MessageBox.Show("  Графический растровый редактор Paint.Da предназначен для создания простейших зарисовок." +
                "\n  Создано как проект лабораторной работы двумя студентами колледжа ПКГХ группы ИП-18-4: Барановым К.П. и Морозовым Н.Д. в 2021 году!" +
                "\n  Все права защищены законодательством РФ.", "О программе!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Обработчик изменения положения слайдера.
        private void MySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Меняем размер кисти в соответствии со значением слайдера.
            MyInkCanvas.DefaultDrawingAttributes.Width = MyInkCanvas.DefaultDrawingAttributes.Height = e.NewValue;
        }

        // Обработчик нажатия на кнопку "Очистка листа".
        private void Button_Erase_Clicked(object sender, RoutedEventArgs e)
        {
            // Очищаем канвас.
            MyInkCanvas.Strokes.Clear();
        }
    }
}