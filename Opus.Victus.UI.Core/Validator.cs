using System;
using System.Collections.Generic;
using System.Text;

namespace Opus.Victus.UI.Core
{
    public abstract class Validator : Attribute
    {
        public override string ToString()
        {
            return GetValidationString();
        }

        public abstract string GetValidationString();

        public string ErrorSymbol { get; set; }

        public string ErrorMessage { get; set; }

        public static Validator GetObjectBystring(string validatorstring, string errormessage, string errorSymbol = "*", params string[] param)
        {
            //var validatortype = "Acubec.Payments.Solutions.Utilities." + validatorstring;
            //var type = Type.GetType(validatortype);
            //if (type != null)
            //{
            //    var workFLowObject = Activator.CreateInstance(type,param);
            //    return (Validator)workFLowObject;
            //}
            //return null;


            switch (validatorstring.ToLower())
            {
                case "requiredvalidator":
                    return new RequiredValidator(errormessage, errorSymbol);
                case "regularexpressionvalidator":
                    return new RegularExpressionValidator(param[0], errormessage, errorSymbol);
                case "emailvalidator":
                    return new EmailValidator(errormessage, errorSymbol);
                case "phonevalidator":
                    return new PhoneValidator(param, errormessage, errorSymbol);
                case "cardnumbervalidator":
                    return new EmailValidator(errormessage, errorSymbol);
                case "cardexpirationvalidator":
                    return new CardExpirationValidator(errormessage, errorSymbol);
                default:
                    return null;
            }
        }
    }

    public class RequiredValidator : Validator
    {
        public RequiredValidator(string errorMessage, string errorSymbol = "*")
        {
            base.ErrorMessage = errorMessage;
            base.ErrorSymbol = errorSymbol;
        }

        public override string GetValidationString()
        {
            return $"Required:{base.ErrorSymbol}:{base.ErrorMessage}";
        }
    }

    public class RegularExpressionValidator : Validator
    {
        public RegularExpressionValidator(string regularExpression, string errorMessage, string errorSymbol = "*")
        {
            this.RegularExpression = regularExpression;
            base.ErrorMessage = errorMessage;
            base.ErrorSymbol = errorSymbol;
        }

        public string RegularExpression { get; set; }

        public override string GetValidationString()
        {
            return $"RegularExpression:{base.ErrorSymbol}:{base.ErrorMessage}:{RegularExpression}";
        }
    }

    public class EmailValidator : RegularExpressionValidator
    {
        public EmailValidator(string errorMessage, string errorSymbol = "*")
            : base(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", errorMessage, errorSymbol)
        {
        }
    }

    public class CompareValidator : Validator
    {
        public CompareValidator(string errorMessage, string errorSymbol = "*")
        {
            base.ErrorMessage = errorMessage;
            base.ErrorSymbol = errorSymbol;
        }

        public override string GetValidationString()
        {
            return $"CompareValidator:{base.ErrorSymbol}:{base.ErrorMessage}";
        }
    }

    public class Validators
    {
        int _maxLength;
        public string Length()
        {
            if (_maxLength > 0) return $"maxlength={_maxLength}";
            return string.Empty;
        }
        public void SetLength(int maxLength)
        {
            _maxLength = maxLength;
        }
        public List<Validator> Validations { get; set; } = new List<Validator>();
        public override string ToString()
        {
            string strValidations = string.Empty;
            foreach (var validation in Validations)
            {
                strValidations = $"{strValidations};{validation.ToString()}";
            }
            return strValidations;
        }
    }

    public class PhoneValidator : Validator
    {
        string[] regularExpressions;
        public PhoneValidator(string[] regularExpression, string errorMessage, string errorSymbol = "*")
        {
            regularExpressions = regularExpression;
            base.ErrorMessage = errorMessage;
            base.ErrorSymbol = errorSymbol;
        }

        public string[] RegularExpressions { get; set; }

        public override string GetValidationString()
        {
            string formattedRegEx = string.Empty;
            foreach (string regEx in regularExpressions)
            {
                formattedRegEx = formattedRegEx + ":" + regEx;
                //formattedRegEx.Append(":" + regEx);

            }
            return $"PhoneValidator:{base.ErrorSymbol}:{base.ErrorMessage}" + formattedRegEx;
        }
    }

    public class CardNumberValidator : Validator
    {
        public CardNumberValidator(string cardFormatExpression, string errorMessage, string errorSymbol = "*")
        {
            this.CardFormatExpression = cardFormatExpression;
            base.ErrorMessage = errorMessage;
            base.ErrorSymbol = errorSymbol;
        }

        public string CardFormatExpression { get; set; }

        public override string GetValidationString()
        {
            return $"CardNumberValidator:{base.ErrorSymbol}:{base.ErrorMessage}:{CardFormatExpression}";
        }
    }

    public class CardExpirationValidator : Validator
    {
        string expiryFormat = "(0[1-9]|1[0-2])[0-9]{2}";
        public CardExpirationValidator(string errorMessage, string errorSymbol = "*")
        {
            base.ErrorMessage = errorMessage;
            base.ErrorSymbol = errorSymbol;
        }

        public override string GetValidationString()
        {
            return $"CardExpirationValidator:{base.ErrorSymbol}:{base.ErrorMessage}:{expiryFormat}";
        }
    }
}
