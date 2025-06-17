using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Frontend.Utils
{
    public class MembersToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<string> members)
                return "Members: " + string.Join(", ", members);
            return "Members: None";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}