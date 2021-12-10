using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace XmlyAlbumDownloader.ValidationRules
{
    /// <summary>
    /// ximalaya url验证
    /// </summary>
    public class AlbumUrlValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return ValidationResult.ValidResult;
            try
            {
                var r = ValidateUrl(value.ToString());

                return r ? ValidationResult.ValidResult : new ValidationResult(false, "地址格式不正确");
            }
            catch
            {
                return new ValidationResult(false, "地址格式不正确");
            }
        }

        private bool ValidateUrl(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                    return false;

                var uri = new Uri(url.TrimEnd('/'));

                if (uri.Authority.ToLower() != "www.ximalaya.com")
                    return false;

                if (uri.Segments.Count() != 3 && uri.Segments.Count() != 4)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
