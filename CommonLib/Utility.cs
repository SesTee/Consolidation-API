using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace CommonLib
{
    public static class Utility
    {
        [DebuggerStepThrough]
        public static IConfigurationRoot AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            return configurationBuilder.Build();
        }

        public static string RemoveInvalidXmlChars(string xmlDocument)
        {
            string text = RemoveAllNamespaces(xmlDocument);

            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }

        private static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName)
                {
                    Value = xmlDocument.Value
                };

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
        internal static bool ValidateSolID(string solID)
        {
            if (string.IsNullOrEmpty(solID))
                return false;

            if (solID.Length == 3)
                return true;
            else
                return false;
        }

        public static bool ValidateAccountNo(string accNo)
        {

            if (string.IsNullOrEmpty(accNo))
            {
                return false;
            }

            var validAccNosLength = AppConfiguration().GetSection("Validations").GetSection("ValidAccountNoLength").Value;
            if (string.IsNullOrEmpty(validAccNosLength))
            {
                return false;
            }
            else
            {
                var lengths = validAccNosLength.Split(',');
                var validLength = false;
                for(var i =0; i < lengths.Length; i++)
                {
                    if(accNo.Trim().Length == int.Parse(lengths[i]))
                    {
                        validLength = true;
                        break;
                    }
                }


                if (!validLength)
                {
                    return false;
                }
            }


            if (accNo.Length == 10)
            {
                //Accepts only 10 digits, no more no less. 
                Regex pattern = new Regex(@"(?<!\d)\d{10}(?!\d)");

                return pattern.IsMatch(accNo) ? true : false;
            }

            return true;
        }

        public static bool ValidateCIF(string cif)
        {
                //Accepts only 1 capital letter and 9 digits, no more no less.
                Regex pattern = new Regex(@"[A-Z]{1}(?<!\d)\d{9}(?!\d)");

                return pattern.IsMatch(cif) ? true : false;
        }

        public static bool ValidateBVN(string bvn)
        {
            //Accepts only 11 digits, no more no less. 
            Regex pattern = new Regex(@"(?<!\d)\d{11}(?!\d)");

            return pattern.IsMatch(bvn) ? true : false;
        }


        public static bool ValidateImage(string imagestring)
        {
            try
            {
                if (string.IsNullOrEmpty(imagestring))
                {
                    return false;
                }

                byte[] imgByte = Convert.FromBase64String(imagestring);

                using (MemoryStream ms = new MemoryStream(imgByte))
                {
                    using (Image img = Image.FromStream(ms))
                    {
                        if (img.RawFormat.Equals(ImageFormat.Bmp) ||
                            img.RawFormat.Equals(ImageFormat.Gif) ||
                            img.RawFormat.Equals(ImageFormat.Jpeg) ||
                            img.RawFormat.Equals(ImageFormat.Png))
                        {
                            return true;
                        }
                    }
                }


            }            
            catch (Exception)
            {              
            }

            return false;
        }


        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        public static string MaskPan(string pan)
        {
            return (!pan.Contains("*")) ? (pan.Substring(0, 6).PadRight(pan.Length - 4, '*') + pan.Substring(pan.Length - 4, 4)) : pan;
        }

        public static string EncodePanID(string pan)//TODO: Lanre Implement properly
        {
            string CoreVariable = AppConfiguration().GetSection("Keys").GetSection("pass").Value;

            return EncryptionClass.Encrypt(pan, CoreVariable);
        }

        public static string DecodePan(string pan)//TODO: Lanre Implement properly
        {
            string CoreVariable = AppConfiguration().GetSection("Keys").GetSection("pass").Value;

            return EncryptionClass.Decrypt(pan, CoreVariable);
        }

    }
}
