using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Markup;

namespace Wodsoft.UI.Input
{
    [ContentProperty("NameValue")]
    [TypeConverter(typeof(InputScopeNameConverter))]
    public class InputScopeName : IAddChild
    {
        #region Public Methods

        ///<summary>
        /// Default Constructor necesary for parser
        ///</summary>
        public InputScopeName()
        {
        }

        ///<summary>
        /// Constructor that takes name
        ///</summary>
        public InputScopeName(InputScopeNameValue nameValue)
        {
            _nameValue = nameValue;
        }


        #region implementation of IAddChild 
        ///<summary>
        /// Called to Add the object as a Child. For InputScopePhrase tag this is ignored
        ///</summary>
        ///<param name="value">
        /// Object to add as a child
        ///</param>
        public void AddChild(object value)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///  if text is present between InputScopePhrase tags, the text is added as a phrase name 
        /// </summary>
        ///<param name="name">
        /// Text string to add
        ///</param>
        public void AddText(string name)
        {
            // throw new System.NotImplementedException();
        }

        #endregion IAddChild

        #endregion Public Methods

        #region class public properties
        ///<summary>
        /// Name property - this is used when accessing the string that is set to InputScopePhrase
        ///</summary>
        public InputScopeNameValue NameValue
        {
            get { return _nameValue; }
            set
            {
                if (!IsValidInputScopeNameValue(value))
                {
                    throw new ArgumentException($"Invalid input scope name value \"{value}\".");
                }
                _nameValue = value;
            }
        }

        #endregion class public properties

        ///<summary>
        /// This validates the value for InputScopeName.
        ///</summary>
        private bool IsValidInputScopeNameValue(InputScopeNameValue name)
        {
            switch (name)
            {
                case InputScopeNameValue.Default: break; // = 0,  // IS_DEFAULT
                case InputScopeNameValue.Url: break; // = 1,  // IS_URL
                case InputScopeNameValue.FullFilePath: break; // = 2,  // IS_FILE_FULLFILEPATH
                case InputScopeNameValue.FileName: break; // = 3,  // IS_FILE_FILENAME
                case InputScopeNameValue.EmailUserName: break; // = 4,  // IS_EMAIL_USERNAME
                case InputScopeNameValue.EmailSmtpAddress: break; // = 5,  // IS_EMAIL_SMTPEMAILADDRESS
                case InputScopeNameValue.LogOnName: break; // = 6,  // IS_LOGINNAME
                case InputScopeNameValue.PersonalFullName: break; // = 7,  // IS_PERSONALNAME_FULLNAME
                case InputScopeNameValue.PersonalNamePrefix: break; // = 8,  // IS_PERSONALNAME_PREFIX
                case InputScopeNameValue.PersonalGivenName: break; // = 9,  // IS_PERSONALNAME_GIVENNAME
                case InputScopeNameValue.PersonalMiddleName: break; // = 10, // IS_PERSONALNAME_MIDDLENAME
                case InputScopeNameValue.PersonalSurname: break; // = 11, // IS_PERSONALNAME_SURNAME
                case InputScopeNameValue.PersonalNameSuffix: break; // = 12, // IS_PERSONALNAME_SUFFIX
                case InputScopeNameValue.PostalAddress: break; // = 13, // IS_ADDRESS_FULLPOSTALADDRESS
                case InputScopeNameValue.PostalCode: break; // = 14, // IS_ADDRESS_POSTALCODE
                case InputScopeNameValue.AddressStreet: break; // = 15, // IS_ADDRESS_STREET
                case InputScopeNameValue.AddressStateOrProvince: break; // = 16, // IS_ADDRESS_STATEORPROVINCE
                case InputScopeNameValue.AddressCity: break; // = 17, // IS_ADDRESS_CITY
                case InputScopeNameValue.AddressCountryName: break; // = 18, // IS_ADDRESS_COUNTRYNAME
                case InputScopeNameValue.AddressCountryShortName: break; // = 19, // IS_ADDRESS_COUNTRYSHORTNAME
                case InputScopeNameValue.CurrencyAmountAndSymbol: break; // = 20, // IS_CURRENCY_AMOUNTANDSYMBOL
                case InputScopeNameValue.CurrencyAmount: break; // = 21, // IS_CURRENCY_AMOUNT
                case InputScopeNameValue.Date: break; // = 22, // IS_DATE_FULLDATE
                case InputScopeNameValue.DateMonth: break; // = 23, // IS_DATE_MONTH
                case InputScopeNameValue.DateDay: break; // = 24, // IS_DATE_DAY
                case InputScopeNameValue.DateYear: break; // = 25, // IS_DATE_YEAR
                case InputScopeNameValue.DateMonthName: break; // = 26, // IS_DATE_MONTHNAME
                case InputScopeNameValue.DateDayName: break; // = 27, // IS_DATE_DAYNAME
                case InputScopeNameValue.Digits: break; // = 28, // IS_DIGITS
                case InputScopeNameValue.Number: break; // = 29, // IS_NUMBER
                case InputScopeNameValue.OneChar: break; // = 30, // IS_ONECHAR
                case InputScopeNameValue.Password: break; // = 31, // IS_PASSWORD
                case InputScopeNameValue.TelephoneNumber: break; // = 32, // IS_TELEPHONE_FULLTELEPHONENUMBER
                case InputScopeNameValue.TelephoneCountryCode: break; // = 33, // IS_TELEPHONE_COUNTRYCODE
                case InputScopeNameValue.TelephoneAreaCode: break; // = 34, // IS_TELEPHONE_AREACODE
                case InputScopeNameValue.TelephoneLocalNumber: break; // = 35, // IS_TELEPHONE_LOCALNUMBER
                case InputScopeNameValue.Time: break; // = 36, // IS_TIME_FULLTIME
                case InputScopeNameValue.TimeHour: break; // = 37, // IS_TIME_HOUR
                case InputScopeNameValue.TimeMinorSec: break; // = 38, // IS_TIME_MINORSEC
                case InputScopeNameValue.NumberFullWidth: break; // = 39, // IS_NUMBER_FULLWIDTH
                case InputScopeNameValue.AlphanumericHalfWidth: break; // = 40, // IS_ALPHANUMERIC_HALFWIDTH
                case InputScopeNameValue.AlphanumericFullWidth: break; // = 41, // IS_ALPHANUMERIC_FULLWIDTH
                case InputScopeNameValue.CurrencyChinese: break; // = 42, // IS_CURRENCY_CHINESE
                case InputScopeNameValue.Bopomofo: break; // = 43, // IS_BOPOMOFO
                case InputScopeNameValue.Hiragana: break; // = 44, // IS_HIRAGANA
                case InputScopeNameValue.KatakanaHalfWidth: break; // = 45, // IS_KATAKANA_HALFWIDTH
                case InputScopeNameValue.KatakanaFullWidth: break; // = 46, // IS_KATAKANA_FULLWIDTH
                case InputScopeNameValue.Hanja: break; // = 47, // IS_HANJA
                case InputScopeNameValue.PhraseList: break; // = -1, // IS_PHRASELIST
                case InputScopeNameValue.RegularExpression: break; // = -2, // IS_REGULAREXPRESSION
                case InputScopeNameValue.Srgs: break; // = -3, // IS_SRGS
                case InputScopeNameValue.Xml: break; // = -4, // IS_XML
                default:
                    return false;
            }

            return true;
        }

        private InputScopeNameValue _nameValue;
    }
}
