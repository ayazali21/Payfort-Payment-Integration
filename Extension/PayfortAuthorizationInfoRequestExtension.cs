using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Platform.Payment.Enums;
using Platform.Payment.PayfortModels;

namespace Platform.Payment.Extension
{
    public  static class PayfortAuthorizationInfoRequestExtension
    {
        /// <summary>
        /// Generate SHA256 Signature
        /// </summary>
        /// <param name="request"></param>
        public static string GenerateAuthorizationSHA256Signature(this PayfortAuthorizationInfoRequest request)
        {
            try
            {
                //Align Parameters names in ascending order and then
                //convert to SHA256
                return ConvertToSHA256(GetAuthorizationSignatureAscending(request));
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Get Authorization Request Params
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string</returns>
        public static string GetAuthorizationRequestParams(this PayfortAuthorizationInfoRequest request)
        {
            try
            {
                Dictionary<string, string> parameters = GetAuthorizationRequestParamsAsDictionary(request);
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
        private static Dictionary<string, string> GetAuthorizationRequestParamsAsDictionary(PayfortAuthorizationInfoRequest request)
        {
            Dictionary<string, string> parameters = GetAuthorizationParametersAsDictionary(request);
            parameters.Add("signature", request.Signature);
            return parameters;
        }

        /// <summary>
        /// Generate SHA256 Signature
        /// </summary>
        /// <param name="request"></param>
        public static string GenerateVoidAuthorizationSHA256Signature(this PayfortAuthorizationInfoRequest request)
        {
            try
            {
                //Align Parameters names in ascending order and then
                //convert to SHA256
                return ConvertToSHA256(GetVoidAuthorizationSignatureAscending(request));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get Authorization Request Params
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string</returns>
        public static string GetVoidAuthorizationRequestParams(this PayfortAuthorizationInfoRequest request)
        {
            try
            {
                return string.Format("{{\"command\":\"{0}\",\"access_code\":\"{1}\",\"merchant_identifier\":\"{2}\",\"language\":\"{3}\",\"fort_id\":\"{4}\",\"signature\":\"{5}\"}}",
                                    PaymentCommandType.VOID_AUTHORIZATION.ToString(),
                                   request.AccessCode,
                                  request.MerchantIdentifier,
                                 "en",
                                request.FortId,
                               request.Signature);
            }
            catch
            {
                return string.Empty;
            }
        }

        #region private Methods
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

        /// <summary>
        /// Generate SHA256 string
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string</returns>
        private static string GetAuthorizationSignatureAscending(PayfortAuthorizationInfoRequest request)
        {
            try
            {
                var parameters = GetAuthorizationParametersAsDictionary(request);

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

        private static Dictionary<string, string> GetAuthorizationParametersAsDictionary(PayfortAuthorizationInfoRequest request)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("access_code", request.AccessCode);
            parameters.Add("amount", request.Amount);
            parameters.Add("command", request.Command);
            parameters.Add("currency", request.Currency);
            parameters.Add("customer_email", request.CustomerEmail);
            parameters.Add("customer_ip", request.CustomerIp);
            parameters.Add("customer_name", request.CustomerName);
            parameters.Add("device_fingerprint", request.DeviceFingerPrint);
            parameters.Add("language", request.Language);
            parameters.Add("merchant_identifier", request.MerchantIdentifier);
            parameters.Add("merchant_reference", request.MerchantReference);
            parameters.Add("remember_me", request.RememberMe);
            parameters.Add("return_url", request.ReturnUrl);
            parameters.Add("token_name", request.TokenName);

            if (!string.IsNullOrEmpty(request.check_3ds) && request.check_3ds == "NO")
                parameters.Add("check_3ds", request.check_3ds);

            if (!string.IsNullOrWhiteSpace(request.CardSecurityCode))
                parameters.Add("card_security_code", request.CardSecurityCode);
            return parameters;
        }

        /// <summary>
        /// Generate SHA256 string
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string</returns>
        private static string GetVoidAuthorizationSignatureAscending(PayfortAuthorizationInfoRequest request)
        {
            try
            {
                return string.Format("{0}access_code={1}command={2}fort_id={3}language={4}merchant_identifier={5}{0}",
                                  request.RequestPhrase,
                                 request.AccessCode,
                                PaymentCommandType.VOID_AUTHORIZATION.ToString(),
                               request.FortId,
                              "en",
                             request.MerchantIdentifier);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
