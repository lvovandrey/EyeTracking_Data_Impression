using System;
using System.ComponentModel;
using System.Windows.Input;

namespace DataImpression.AbstractMVVM
{
    /// <summary>
    /// Хелпер-класс для реализации INPC интерфейса. От него нужно наследовать всякие VM-ки (в общем все, что нуждается в INPC)
    /// </summary>
    public class INPCBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Класс реализующий паттерн Команда.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        /// <summary>
        /// Производит некое невалидное ("force") обновление условий возможности выполнения команды. 
        /// из msdn - вызывает обновление "conditions that might change the ability of a command to execute"
        /// </summary>
        public void InvalidateRequerySuggested() 
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}