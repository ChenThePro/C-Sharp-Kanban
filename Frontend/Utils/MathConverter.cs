using System.Globalization;
using System.Windows.Data;

namespace Frontend.Utils
{
    public class MathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Binding.DoNothing;

            double numericValue;
            if (!double.TryParse(value.ToString(), out numericValue))
                return Binding.DoNothing;

            var expression = parameter.ToString()!.Replace("@VALUE", numericValue.ToString(CultureInfo.InvariantCulture));

            try
            {
                var result = new System.Data.DataTable().Compute(expression, "");
                return System.Convert.ChangeType(result, targetType);
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
