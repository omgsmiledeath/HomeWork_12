
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace HomeWork_11
{
    class WordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;
            if (text == string.Empty) return new ValidationResult(false, "Заполните все поля");
            return new ValidationResult(true, "Succes");
        }


    }
}