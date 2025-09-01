using System;
using System.Globalization;
using System.Windows.Data;
using MiniSocialApp.ViewModels;

namespace MiniSocialApp.Converters
{
    public class CommentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2) return null;

            var content = values[0] as string ?? "";
            var post = values[1] as PostDisplay;

            if (post == null) return null;

            return Tuple.Create(post, content);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
