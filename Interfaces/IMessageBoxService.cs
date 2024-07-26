using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Digident_Group3.Interfaces
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button);
    }
}
