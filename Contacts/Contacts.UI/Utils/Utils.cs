using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Contacts.UI.Utils
{
    internal static class Utils
    {
        internal static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}
