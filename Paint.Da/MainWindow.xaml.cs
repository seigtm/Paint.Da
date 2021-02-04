using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private readonly string AboutSoftware = "  Графический векторный редактор Paint.Da предназначен для создания простейших зарисовок." +
                "\n  Создано как проект лабораторной работы двумя студентами колледжа ПКГХ группы ИП-18-4: Барановым К.П. и Морозовым Н.Д. в 2021 году!" +
                "\n  Все права защищены законодательством РФ.";

        // Обработчик нажатия на кнопку "О программе".
        private void Button_About_Clicked(object sender, RoutedEventArgs e)
        {
            _ = MessageBox.Show(AboutSoftware, "О программе!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Обработчик изменения положения слайдера.
        private void MySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Меняем размер кисти в соответствии со значением слайдера.
            MyInkCanvas.DefaultDrawingAttributes.Width = MyInkCanvas.DefaultDrawingAttributes.Height = e.NewValue;
        }

        // Обработчик нажатия на кнопку "Очистка листа".
        private void Button_Erase_Clicked(object sender, RoutedEventArgs e) => ClearCanvas();

        // Очистка канваса и изображений на нём.
        private void ClearCanvas()
        {
            // Очищаем канвас.
            MyInkCanvas.Strokes.Clear();
            // Очищаем изображение на канвасе.
            InkCanvasImage.Source = null;
        }

        // Сохранение bitmap в png.
        private void SaveInkCanvasToJPEG()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Image (*.jpeg)|*.jpeg|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                FileStream fs = new FileStream(dialog.FileName, FileMode.Create);

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)MyInkCanvas.ActualWidth, (int)MyInkCanvas.ActualHeight, 96d, 96d, PixelFormats.Default);
                rtb.Render(MyInkCanvas);
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));

                encoder.Save(fs);
                fs.Close();

                _ = MessageBox.Show($"Расположение: {dialog.FileName}", "Успешно сохранено!", MessageBoxButton.OK, MessageBoxImage.Information);
                StatusBarText.Text = "Успешно сохранено!";
                return;
            }
            StatusBarText.Text = "Не удалось сохранить!";
            _ = MessageBox.Show("Произошла ошибка! Проверьте правильность пути.", "Не удалось сохранить!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Метод открытия JPEG файла изображения и добавления его на канвас.
        private void OpenJPEGToInkCanvas()
        {
            // Сначала очищаем канвас от мусора.
            ClearCanvas();
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Image (*.jpeg)|*.jpeg|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(dialog.FileName);
                bitmap.EndInit();

                InkCanvasImage.Source = bitmap;

                StatusBarText.Text = $"Открыт файл: {dialog.FileName}";
                return;
            }
            StatusBarText.Text = "Не удалось открыть файл!";
        }

        // Метод выхода из приложения.
        private void Exit()
        {
            MessageBoxResult MessageBoxExit =
                        MessageBox.Show("Вы уверены, что хотите выйти?",
                                        "Выход",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);
            if (MessageBoxExit == MessageBoxResult.Yes)
            {
                System.Environment.Exit(0);
            }
        }

        // Обработчик выбора пункта Меню.
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // В зависимости от выбранного пункта, выполняем определённые алгоритмы.
            switch (((MenuItem)sender).Header.ToString())
            {
                case "Открыть":
                    OpenJPEGToInkCanvas();
                    break;

                case "Сохранить":
                    SaveInkCanvasToJPEG();
                    break;

                case "Закрыть":
                    Exit();
                    break;
            }
        }

        private void ToolBarButton_Click(object sender, RoutedEventArgs e)
        {
            // В зависимости от выбранного пункта, выполняем определённые алгоритмы.
            switch (((Button)sender).Name)
            {
                case "OpenButton":
                    OpenJPEGToInkCanvas();
                    break;

                case "SaveButton":
                    SaveInkCanvasToJPEG();
                    break;

                case "AboutButton":
                    _ = MessageBox.Show(AboutSoftware, "О программе!", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case "ExitButton":
                    Exit();
                    break;
            }
        }
    }
}