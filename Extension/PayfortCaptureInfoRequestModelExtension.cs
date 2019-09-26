﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Platform.Payment.PayfortModels;

namespace Platform.Payment.Extension
{
    public static class PayfortCaptureInfoRequestModelExtension
    {
        /// <summary>
        /// Generate SHA256 Signature
        /// </summary>
        /// <param name="request"></param>
        public static string GenerateCaptureSHA256Signature(this PayfortCaptureInfoRequestModel request)
        {
            try
            {
                //Align Parameters names in ascending order and then
                //convert to SHA256
                return ConvertToSHA256(GetCaptureSignatureAscending(request));
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
        public static string GetCaptureRequestParams(this PayfortCaptureInfoRequestModel request, string signature)
        {
            try
            {

                var parameters = GetCaptureParametersAsDictionary(request);
                parameters.Add("signature", signature);
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
        private static Dictionary<string, string> GetCaptureParametersAsDictionary(PayfortCaptureInfoRequestModel request)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("access_code", request.AccessCode);
            parameters.Add("amount", request.Amount);
            parameters.Add("command", request.Command);
            parameters.Add("currency", request.Currency);
            parameters.Add("fort_id", request.FortId);
            parameters.Add("language", request.Language);
            parameters.Add("merchant_identifier", request.MerchantIdentifier);

            return parameters;
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


        /// <summary>
        /// Generate SHA256 string
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string</returns>
        private static string GetCaptureSignatureAscending(PayfortCaptureInfoRequestModel request)
        {
            try
            {

                var parameters = GetCaptureParametersAsDictionary(request);

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




    }
}
