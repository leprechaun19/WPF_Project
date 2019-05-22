using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace HotelBooking.Classes
{
    class Validation : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            string patternLoginPassword = @"^[a-zA-Z0-9]+$";
            if (!Regex.IsMatch((string)value, patternLoginPassword, RegexOptions.IgnoreCase))
            {
                return new ValidationResult
                (false, "Спецсимволы и кириллица не допустимы!");
            }
            return ValidationResult.ValidResult;
        }
    }

    class PassportValidation : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            string patternPassport = @"^[a-zA-Z]{2}\d{7}$";
            if (!Regex.IsMatch((string)value, patternPassport, RegexOptions.IgnoreCase))
            {
                return new ValidationResult
                (false, "Неверный формат!");
            }
            return ValidationResult.ValidResult;
        }
    }

    class IDValidation : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            string patternPassport = @"^[0-9]{6,}";
            if (!Regex.IsMatch((string)value, patternPassport, RegexOptions.IgnoreCase))
            {
                return new ValidationResult
                (false, "Нужно не менее 6 цифр!");
            }
            return ValidationResult.ValidResult;
        }
    }

    class NameValidation : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            string patternPassport = @"^[\sа-яА-Я]+$";
            if (!Regex.IsMatch((string)value, patternPassport, RegexOptions.IgnoreCase))
            {
                return new ValidationResult
                (false, "Допустимы только буквы\n и пробельные символы!");
            }
            return ValidationResult.ValidResult;
        }
    }

    class DecimalValidation : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            string patternPassport = @"^[1-9]{1}[0-9]{1,3}$";
            if (!Regex.IsMatch((string)value, patternPassport, RegexOptions.IgnoreCase))
            {
                return new ValidationResult
                (false, "Допустимы только цифры.\nНе более 4 цифр!");
            }
            return ValidationResult.ValidResult;
        }
    }

    class AdultValidation : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            string patternPassport = @"^[1-4]$";
            if (!Regex.IsMatch((string)value, patternPassport, RegexOptions.IgnoreCase))
            {
                return new ValidationResult
                (false, "Допустима одна цифра(1-4)!");
            }
            return ValidationResult.ValidResult;
        }
    }

    class ChildrenValidation : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            string patternPassport = @"^[0-4]$";
            if (!Regex.IsMatch((string)value, patternPassport, RegexOptions.IgnoreCase))
            {
                return new ValidationResult
                (false, "Допустима одна цифра(0-4)!");
            }
            return ValidationResult.ValidResult;
        }
    }
    class PhoneValidation : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            string patternPassport = @"\d{3}\s\d{2}\s\d{3}\s\d{2}\s\d{2}";
            if (!Regex.IsMatch((string)value, patternPassport, RegexOptions.IgnoreCase))
            {
                return new ValidationResult
                (false, "Формат 375 44 567 23 91!");
            }
            return ValidationResult.ValidResult;
        }
    }
    class EmailValidation : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            string patternPassport = @"[A-Za-z]+[\.A-Za-z0-9_-]*[A-Za-z0-9]+@[A-Za-z]+\.[A-Za-z]";
            if (!Regex.IsMatch((string)value, patternPassport, RegexOptions.IgnoreCase))
            {
                return new ValidationResult
                (false, "Неверный формат!");
            }
            return ValidationResult.ValidResult;
        }
    }
    class DateValidation : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            string patternPassport = @"(^0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$";
            if (!Regex.IsMatch((string)value, patternPassport, RegexOptions.IgnoreCase))
            {
                return new ValidationResult
                (false, "Неверный формат даты!");
            }
            return ValidationResult.ValidResult;
        }
    }

}
