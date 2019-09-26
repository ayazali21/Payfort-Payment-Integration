using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Platform.Payment.Enums;
using Platform.Payment.PayfortModels;

namespace Platform.Payment.Extension
{
    public static class PayfortInvoiceRequestModelExtension
    {
        /// <summary>
        /// Generate SHA256 Signature
        /// </summary>
        /// <param name="request"></param>
        public static string GenerateInvoiceSHA256Signature(this PayfortInvoiceRequestModel request)
        {
            try
            {
                //Align Parameters names in ascending order and then
                //convert to SHA256
                return ConvertToSHA256(GetInvoiceSignatureAscending(request));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Generate SHA256 string
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string</returns>
        private static string GetInvoiceSignatureAscending(PayfortInvoiceRequestModel request)
        {
            try
            {
               
                var parameters = GetInvoiceParametersAsDictionary(request);

                if (null == parameters && parameters.Count == 0)
                    return string.Empty;

                System.Text.StringBuilder builder = new System.Text.StringBuilder();

                builder.Append(request.RequestPhrase);
                foreach (var key in parameters.Keys.OrderBy((t => t)))
                {
                    var value = parameters[key];
                    builder.Append($"{key}={value}");
                }
                builder.Append(request.RequestPhrase);

                var signature = builder.ToString();
                return signature;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static Dictionary<string, string> GetInvoiceParametersAsDictionary(PayfortInvoiceRequestModel request)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("access_code", request.AccessCode);
            parameters.Add("amount", Convert.ToInt64(Convert.ToDecimal(request.Amount) * 100) + string.Empty);
            parameters.Add("currency", request.Currency);
            parameters.Add("customer_email", request.CustomerEmail);
            parameters.Add("customer_name", request.CustomerName);
            parameters.Add("customer_phone", request.CustomerPhoneNumber);
            parameters.Add("language", request.Language);
            parameters.Add("merchant_reference", request.MerchantReference);
            parameters.Add("merchant_identifier", request.MerchantIdentifier);
            parameters.Add("notification_type", PaymentNotificationsType.NONE.ToString());
            //parameters.Add("order_description", request.OrderDescription);
            parameters.Add("payment_option", string.IsNullOrEmpty(request.CreditCardType) ? "VISA" : request.CreditCardType);
            parameters.Add("request_expiry_date", request.RequestExpiryDate.ToString("yyyy-MM-ddTHH:mm:ss") + "+00:00");
            //parameters.Add("return_url", request.ReturnUrl);
            parameters.Add("service_command", "PAYMENT_LINK");

            return parameters;
        }

        /// <summary>
        /// Get Authorization Request Params
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string</returns>
        public static string GetInvoiceRequestParams(this PayfortInvoiceRequestModel request)
        {
            try
            {
                Dictionary<string, string> parameters = GetInvoiceParametersAsDictionary(request);
                parameters.Add("signature", request.Signature);
                if (null == parameters && parameters.Count == 0)
                    return string.Empty;

                System.Text.StringBuilder builder = new System.Text.StringBuilder();

                builder.Append("{");
                foreach (var key in parameters.Keys.OrderBy((t => t)))
                {
                    var value = parameters[key];
                    builder.Append($"\"{key}\":\"{value}\",");
                }
                if (builder.ToString().EndsWith(","))
                    builder.Remove(builder.Length - 1, 1);
                builder.Append("}");

                var requestAsString = builder.ToString();
                return requestAsString;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts string to SHA 256 string 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        private static string ConvertToSHA256(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var hashstring = new SHA256Managed();
            var hash = hashstring.ComputeHash(bytes);
            var hashString = string.Empty;
            foreach (var x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

    }
}
