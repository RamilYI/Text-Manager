using System;
using System.Windows.Input;

namespace TechTest.ViewModels 
{
    /// <summary>
    /// Класс, использующийся для реализации собственных команд
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Конструктор класса RelayCommand
        /// </summary>
        /// <param name="execute">Переданный метод</param>
        /// <param name="canExecute">указывает на выполнимость команды</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Проверить, может ли команда выполняться
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Вызывается в случае изменения условий, указывающих на
        /// выполнимость команды
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Выполнить переданный метод
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute(parameter ?? "<N/A>");
        }
    }
}