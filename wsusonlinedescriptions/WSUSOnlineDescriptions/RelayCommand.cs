using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WSUSOnlineDescriptions
{
	public class RelayCommand : ICommand
	{
		bool canExecute;
		public event EventHandler CanExecuteChanged;
		public event EventHandler CommandExecuted;

		public RelayCommand(bool canExecute)
		{
			this.SetCanExecute(canExecute);
		}

		public void SetCanExecute(bool newValue)
		{
			this.canExecute = newValue;

			if (this.CanExecuteChanged != null)
				this.CanExecuteChanged(this, new EventArgs());
		}

		public bool CanExecute(object parameter)
		{
			return this.canExecute;
		}

		public void Execute(object parameter)
		{
			if (this.CommandExecuted != null)
				this.CommandExecuted(this, new EventArgs());
		}
	}
}