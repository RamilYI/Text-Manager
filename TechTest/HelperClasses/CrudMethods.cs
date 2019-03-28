using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using TechTest.Models;
using TechTest.ViewModels;
using TechTest.Views;

namespace TechTest.HelperClasses
{
    /// <summary>
    /// Класс, содержащий методы добавления, удаления, изменения и чтения объектов
    /// </summary>
    public static class CrudMethods
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        /// <param name="banRepeat">запрет изменения данных документа</param>
        public static void OpenDocumentWindow(ref bool banRepeat)
        {
            Presenter.SelectedObject = null;
            banRepeat = true;
            DocumentWindow window = new DocumentWindow();
            window.ShowDialog();
        }

        /// <summary>
        /// Открыть задачу
        /// </summary>
        /// <param name="banRepeat">запрет изменения данных документа</param>
        public static void OpenTaskWindow(ref bool banRepeat)
        {
            Presenter.SelectedObject = null;
            banRepeat = true;
            TaskWindow window = new TaskWindow();
            window.ShowDialog();
        }


        /// <summary>
        /// Добавить объект
        /// </summary>
        /// <param name="o">введённые значения объекта</param>
        /// <param name="addObject">метод добавления объекта</param>
        /// <param name="windownum">Номер окна-карточки объекта</param>
        public static void Add(object o, Action<int, object[], int> addObject, int windownum)
        {
            var values = (object[])o;

            if (Regex.IsMatch((string)values[1], @"\S"))
            {
                Presenter.CompositeCollection.Add(new Base());
                addObject(Presenter.CompositeCollection.Count - 1, values, Presenter.CompositeCollection.Count);
                Application.Current.Windows[windownum]?.Close();
            }
            else
            {
                MessageBox.Show("Введите наименование документа");
            }
        }

        /// <summary>
        /// Изменить объект
        /// </summary>
        /// <param name="o">введённые значения объекта</param>
        /// <param name="index">индекс выбранного объекта</param>
        /// <param name="addObject">метод добавления объекта</param>
        /// <param name="windownum">Номер окна-карточки объекта</param>
        public static void Update(object o, int index, Action<int, object[], int> addObject, int windownum)
        {
            var values = (object[])o;

            if (Regex.IsMatch((string)values[1], @"\S"))
            {
                addObject(index, values, Convert.ToInt32(values[0]));
                Application.Current.Windows[windownum]?.Close();
            }
            else
            {
                MessageBox.Show("Введите наименование документа");
            }
        }

        /// <summary>
        /// Создать документ
        /// </summary>
        /// <param name="index">индекс документа</param>
        /// <param name="values">массив данных документа</param>
        /// <param name="id">ID документа</param>
        public static void CreateDocument(int index, object[] values, int id)
        {
            var subscribed = (string)values[2] == "";

            Presenter.CompositeCollection[index] = new Document
            {
                ID = id,
                Name = (string)values[1],
                Uuid = subscribed ? "" : (string)values[2],
                Text = new TextRange(((FlowDocument)values[3]).ContentStart,
                    ((FlowDocument)values[3]).ContentEnd).Text
            };
        }

        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="index">идекс задачи</param>
        /// <param name="values">массив данных задачи</param>
        /// <param name="id">ID задачи</param>
        public static void CreateTask(int index, object[] values, int id)
        {
            Presenter.CompositeCollection[index] = new Task
            {
                ID = id,
                Name = (string)values[1],
                Text = new TextRange(((FlowDocument)values[3]).ContentStart,
                    ((FlowDocument)values[3]).ContentEnd).Text,
                TaskStat = Enum.Parse(typeof(TaskStat), (string)values[2]).ToString()
            };
        }

        /// <summary>
        /// Удалить объект
        /// </summary>
        public static void DeleteObject()
        {
            if (Presenter.SelectedObject != null)
            {
                Presenter.CompositeCollection.Remove(Presenter.SelectedObject);
                Presenter.SelectedObject = null;
            }
        }

        /// <summary>
        /// Открыть объект
        /// </summary>
        /// <param name="index">Индекс объекта</param>
        /// <param name="banRepeat">Запрет изменения данных документа</param>
        public static void OpenObject(ref int index, ref bool banRepeat)
        {
            if (Presenter.SelectedObject != null || index != -1)
            {
                if (Presenter.SelectedObject != null) index = Presenter.CompositeCollection.IndexOf(Presenter.SelectedObject);
                banRepeat = true;

                // объект является документом
                if (Presenter.CompositeCollection[index].GetType().GetProperty("Uuid") != null)
                {
                    // в случае наличия uuid запретим изменять данные
                    if (((Document)Presenter.CompositeCollection[index]).Uuid != "") banRepeat = false;
                    
                    do
                    {
                        var documentWindow = new DocumentWindow();
                        documentWindow.ShowDialog();

                        // при закрытии диалогового окна производим выход из метода
                        if (!documentWindow.DialogResult.GetValueOrDefault(true))
                        {
                            banRepeat = true;
                            return;
                        }
                    } while (true);
                }

                // объект является задачей
                do
                {
                    var taskWindow = new TaskWindow();
                    taskWindow.ShowDialog();

                    // при закрытии диалогового окна производим выход из метода
                    if (!taskWindow.DialogResult.GetValueOrDefault(true))
                    {
                        return;
                    }
                } while (true);
            }
        }

        /// <summary>
        /// Очистить таблицу
        /// </summary>
        public static void Clear()
        {
            var messageBoxText = "Вы уверены?";
            var messageBoxButton = MessageBoxButton.YesNo;
            var messageBox = MessageBox.Show(messageBoxText, "", messageBoxButton);

            if (messageBox == MessageBoxResult.Yes) Presenter.CompositeCollection.Clear();
        }

        /// <summary>
        /// Закрыть карточку объекта
        /// </summary>
        /// <param name="windownum">Номер окна-карточки объекта</param>
        public static void CloseCard(int windownum)
        {
            Application.Current.Windows[windownum]?.Close();
        }

        /// <summary>
        /// Завершить работы программы
        /// </summary>
        public static void CloseWindow()
        {
            Application.Current.Shutdown();
        }

    }
}