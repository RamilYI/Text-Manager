using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TechTest.HelperClasses;
using TechTest.Models;

namespace TechTest.ViewModels
{
    /// <summary>
    /// Класс, связывающий модели и представления
    /// </summary>
    public class Presenter : INotifyPropertyChanged
    {
        /// <summary>
        /// Коллекция, которая будет хранить объекты (документы и задачи)
        /// </summary>
        public static ObservableCollection<Base> CompositeCollection { get; set; } = new ObservableCollection<Base>();

        /// <summary>
        /// Номер окна-карточки
        /// </summary>
        private const int WINDOWNUM = 1;

        /// <summary>
        /// индекс выбранного объекта
        /// </summary>
        private static int _index = -1;

        /// <summary>
        /// выбранный объект
        /// </summary>
        public static Base SelectedObject { get; set; }

        /// <summary>
        /// ID объекта
        /// </summary>
        public int ObjectId
        {
            get
            {
                if (SelectedObject == null) return CompositeCollection?.Count + 1 ?? 1;
                return SelectedObject.ID;
            }
        }

        private string _uuid;

        /// <summary>
        /// Цифровая подпись документа
        /// </summary>
        public string Uuid
        {
            get
            {
                if (_uuid != null) return _uuid;
                _uuid = SelectedObject != null ? ((Document) SelectedObject).Uuid : "";
                return _uuid;
            }
            set
            {
                _uuid = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Состояние кнопки "изменить"
        /// </summary>
        public bool ChangeEvent => SelectedObject != null;

        /// <summary>
        /// Индекс константы перечисления InProcess
        /// </summary>
        private string TaskInProcess = "0";

        /// <summary>
        /// Индекс константы перечисления Complete
        /// </summary>
        private string TaskComplete = "1";

        /// <summary>
        /// Состояние задачи
        /// </summary>
        public string TaskEnum
        {
            get
            {
                if (SelectedObject != null)
                    return (((Task) SelectedObject).TaskStat == TaskStat.Complete.ToString())
                        ? TaskComplete
                        : TaskInProcess;
                return TaskInProcess;
            }
        }

        private static bool _banrepeat = true;

        /// <summary>
        /// Запрет изменения подписанного документа
        /// </summary>
        public bool BanRepeat
        {
            get => _banrepeat;
            set
            {
                _banrepeat = value;
                OnPropertyChanged();
            }
        }

        private string _nametext;

        /// <summary>
        /// Наименование объекта
        /// </summary>
        public string NameText
        {
            get
            {
                if (_nametext != null) return _nametext;
                _nametext = SelectedObject != null ? ((Document) SelectedObject).Name : "";
                return _nametext;
            }
            set
            {
                _nametext = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Конструктор VM, связывающий команды с их реализацией 
        /// </summary>
        public Presenter()
        {
            OpenDocument = new RelayCommand(o => CrudMethods.OpenDocumentWindow(ref _banrepeat));

            OpenTask = new RelayCommand(o => CrudMethods.OpenTaskWindow(ref _banrepeat));

            ExitCommand = new RelayCommand(o => CrudMethods.CloseWindow());

            AddDocument = new RelayCommand(o => CrudMethods.Add(o, CrudMethods.CreateDocument, WINDOWNUM));

            AddTask = new RelayCommand(o => CrudMethods.Add(o, CrudMethods.CreateTask, WINDOWNUM));

            DeleteCommand = new RelayCommand(o => CrudMethods.DeleteObject());

            SubscribeCommand = new RelayCommand(o => SubscribeDocument());

            ClearTable = new RelayCommand(o => CrudMethods.Clear());

            OpenCommand = new RelayCommand(o => CrudMethods.OpenObject(ref _index, ref _banrepeat));

            ChangeDocument = new RelayCommand(o => CrudMethods.Update(o, _index, CrudMethods.CreateDocument, WINDOWNUM));

            ChangeTask = new RelayCommand(o => CrudMethods.Update(o, _index, CrudMethods.CreateTask, WINDOWNUM));

            CloseChildWindow = new RelayCommand(o => CrudMethods.CloseCard(WINDOWNUM));
        }

        #region Свойства команд

        /// <summary>
        /// Закрытие диалогового окна
        /// </summary>
        public ICommand CloseChildWindow { get; set; }

        /// <summary>
        /// Команда открытия документа
        /// </summary>
        public ICommand OpenDocument { get; set; }

        /// <summary>
        /// Команда закрытия приложения
        /// </summary>
        public ICommand ExitCommand { get; set; }

        /// <summary>
        /// Команда добавления документа
        /// </summary>
        public ICommand AddDocument { get; set; }

        /// <summary>
        /// Команда открытия задачи
        /// </summary>
        public ICommand OpenTask { get; set; }

        /// <summary>
        /// Команда добавления задачи
        /// </summary>
        public ICommand AddTask { get; set; }

        /// <summary>
        /// Команда открытия созданного объекта
        /// </summary>
        public ICommand OpenCommand { get; set; }

        /// <summary>
        /// Команда удаления объекта
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// Команда установки цифровой подписи на документ
        /// </summary>
        public ICommand SubscribeCommand { get; set; }

        /// <summary>
        /// Команда очистки коллекции
        /// </summary>
        public ICommand ClearTable { get; set; }

        /// <summary>
        /// Команда редактирования задачи
        /// </summary>
        public ICommand ChangeTask { get; set; }

        /// <summary>
        /// Команда редактирования документа
        /// </summary>
        public ICommand ChangeDocument { get; set; }

        #endregion

        #region Реализация интерфейса INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// Установить цифровую подпись
        /// </summary>
        private void SubscribeDocument()
        {
            if (Regex.IsMatch(NameText, @"\S"))
            {
                Uuid = Guid.NewGuid().ToString();
                BanRepeat = false;
            }
            else
                MessageBox.Show("Введите наименование документа");
        }
    }
}