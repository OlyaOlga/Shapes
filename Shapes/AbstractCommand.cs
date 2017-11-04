using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shapes
{
    public class AbstractCommand : ICommand
    {
        private readonly Action<object> execute;

        private readonly Predicate<object> canExecute;

        public AbstractCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public AbstractCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        [DebuggerStepThrough]
        public bool CanExecute(object parameters) => canExecute?.Invoke(parameters) ?? true;

        public void Execute(object parameters)
        {
            execute(parameters);
        }

    }
}

