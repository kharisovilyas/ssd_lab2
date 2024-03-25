using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using lab2.model;
using lab2.utils;
using Microsoft.Win32;

namespace lab2.viewmodel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _inputNumbers;
        private string _result;
        private string _resultSum;
        private bool _isSaveButtonVisible;
        private ObservableCollection<InputItemViewModel> _inputItems;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public string InputNumbers
        {
            get => _inputNumbers;
            set
            {
                _inputNumbers = value;
                FindMaxSumSequence();
                OnPropertyChanged(nameof(InputNumbers));
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public string ResultSum
        {
            get => _resultSum;
            set
            {
                _resultSum = value;
                OnPropertyChanged(nameof(ResultSum));
            }
        }

        public bool IsSaveButtonVisible
        {
            get => _isSaveButtonVisible;
            set {
                _isSaveButtonVisible = value;
                OnPropertyChanged(nameof(IsSaveButtonVisible));
            }
        }

        public ObservableCollection<InputItemViewModel> InputItems
        {
            get => _inputItems;
            set
            {
                _inputItems = value;
                OnPropertyChanged(nameof(InputItems));
            }
        }

        public ICommand FindMaxSumSequenceCommand { get; }
        public ICommand AddNewItemCommand { get; }
        public ICommand LoadDataFromFileCommand { get; }
        public ICommand SaveResultCommand {  get; }
        public ICommand ShowStartupInfoCommand { get; }

        public MainViewModel()
        {
            FindMaxSumSequenceCommand = new RelayCommand(FindMaxSumSequence);
            AddNewItemCommand = new RelayCommand(AddNewItem);
            InputItems = new ObservableCollection<InputItemViewModel>();
            LoadDataFromFileCommand = new RelayCommand(LoadDataFromFile);
            SaveResultCommand = new RelayCommand(SaveResult);
            ShowStartupInfoCommand = new RelayCommand(ShowStartupInfo);
            IsSaveButtonVisible = false;
        }

        private void SaveResult()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt"; // Устанавливаем фильтр для файлов

            if (saveFileDialog.ShowDialog() == true)
            {
                // Получение пути к выбранному файлу
                string filePath = saveFileDialog.FileName;

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine(Result);
                        writer.WriteLine(ResultSum);
                    }

                    MessageBox.Show("Результат сохранен успешно", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


    private void LoadDataFromFile()
        {
            // Диалог выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt"; // Устанавливаем фильтр для файлов

            if (openFileDialog.ShowDialog() == true)
            {
                // Получение пути к выбранному файлу
                string filePath = openFileDialog.FileName;

                if (Path.GetExtension(filePath) != ".txt")
                {
                    // Если выбран неподходящий файл, выводим предупреждение
                    MessageBox.Show("Допустимы только файлы с расширением .txt", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Завершаем метод
                }

                try
                {
                    DataParser parser = new DataParser();
                    MyData? data = parser.Parse(filePath);

                    if (data != null)
                    {
                        // Присваиваем значения из файла свойствам ViewModel
                        MaxSumSequenceFinder sequenceFinder = new MaxSumSequenceFinder(data.Numbers);
                        // Очищаем список InputItems перед добавлением новых элементов
                        InputItems.Clear();
                        int i = 0;
                        sequenceFinder.FindMaxSumSequence();
                        // Добавляем элементы из файла в список InputItems
                        foreach (var number in data.Numbers)
                        {
                            InputItems.Add(new InputItemViewModel(++i, number.ToString()));
                        }

                        // Выводим результат в виде текста
                        if (sequenceFinder.isResultEmpty)
                        {
                            Result = "Последовательность не введена\n Или не может быть найдена";
                            ResultSum = string.Empty;
                        }
                        else
                        {
                            Result = sequenceFinder.MaxSumSequence.Count > 10 ?
                                sequenceFinder.GetStartAndEndIndexes()
                                : $"Последовательность с максимальной суммой: {string.Join(", ", sequenceFinder.MaxSumSequence)}"; ;
                            ResultSum =  $"Максимальная сумма: {sequenceFinder.MaxSum}";
                            IsSaveButtonVisible = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка при чтении файла: неверный формат", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public static void ShowStartupInfo()
        {
            MessageBoxResult result = MessageBox.Show(
                "Добро пожаловать! Это информация при запуске программы." +
                "\nРабота №2. Алгоритмы и структуры данных\nСтудент выполняет задание обычной или повышенной сложности" +
                "\nЛабораторная работа предназначена для приобретения практического опыта в работе с наборами объектов в языке программирования С#.\n" +
                "\nЗадание 16 варианта: Напишите программу, находящую в массиве вещественных чисел последовательность, имеющую максимальную сумму.\n" +
                "Программа должна выводить начальный и конечный элемент.\n" +
                "\n\nХотите видеть эту информацию при запуске?\n" +
                "\n\nЕсли вы указали нет, то сможете найти инфомацию в разделе Информация о разработчике",
                "Программу разработал Харисов Ильяс Ренатович, 424 группа",
                MessageBoxButton.YesNo);

            // Если нажата кнопка "OK" или "Отмена", ничего не делаем
            if (result == MessageBoxResult.Yes || result == MessageBoxResult.Cancel)
            {
                SettingsManager.SaveShowStartupMessageSetting(true);
            }
            // Если нажата кнопка "Не показывать", сохраняем настройку
            else if (result == MessageBoxResult.No)
            {
                // Сохраняем настройку, чтобы больше не показывать всплывающее окно
                SettingsManager.SaveShowStartupMessageSetting(false);
            }
        }



        private void FindMaxSumSequence()
        {
            try
            {
                // Получаем введенные пользователем числа
                var numbers = InputItems.Select(item => double.Parse(item.Value)).ToList();

                // Создаем экземпляр класса для поиска последовательности с максимальной суммой
                var sequenceFinder = new MaxSumSequenceFinder(numbers);

                // Вызываем метод поиска
                sequenceFinder.FindMaxSumSequence();

                // Выводим результат в виде текста
                if (sequenceFinder.isResultEmpty)
                {
                    Result = $"Последовательность не введена\n Или не может быть найдена";
                    ResultSum = string.Empty;
                }
                else
                {
                    Result = sequenceFinder.GetStartAndEndIndexes();
                    ResultSum = $"Максимальная сумма: {sequenceFinder.MaxSum}";
                    IsSaveButtonVisible = true;
                }
            }
            catch (Exception ex)
            {
                // Обрабатываем возможные исключения и выводим сообщение об ошибке
                Result = "Неккоректный ввод";
            }
        }

        private void AddNewItem()
        {
            InputItems.Add(new InputItemViewModel(InputItems.Count + 1));
        }
    }

    public class InputItemViewModel : INotifyPropertyChanged
    {
        private int _index;
        private string _value;

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged();
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public InputItemViewModel(int index)
        {
            Index = index;
            Value = "";
        }



        public InputItemViewModel(int index, string value)
        {
            Index = index;
            Value = value;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
