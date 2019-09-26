using System;
using System.Collections.Generic;
using System.Text;
using Platform.Payment.Contracts;
using Platform.Payment.Contracts.Payfort;
using Platform.Payment.Enums;
using Platform.Payment.Models.Configuration;
using Platform.Payment.Models.Request;
using Platform.Payment.PayfortModels;

namespace Platform.Payment.Gateway.Payfort
{
    public class PayfortRequestParser : IPayfortRequestParser
    {
        private IGatewaySettingRepository _gatewaySettingRepository;

        public PayfortConfigurationModel PayfortConfigurationModel = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PayfortRequestParser"/> class.
        /// </summary>
        /// <param name="gatewaySettingRepository">The gateway setting repository.</param>
        /// <exception cref="ArgumentNullException">gatewaySettingRepository</exception>
        public PayfortRequestParser(IGatewaySettingRepository gatewaySettingRepository)
        {
            _gatewaySettingRepository = gatewaySettingRepository ?? throw new ArgumentNullException(nameof(gatewaySettingRepository));
            PayfortConfigurationModel = _gatewaySettingRepository.GetPayfortConfiguration();
        }

        /// <summary>
        /// Converts to authorize request model.
        /// </summary>
        /// <param name="authorizeRequestModel">The authorize request model.</param>
        /// <returns></returns>
        public PayfortAuthorizationInfoRequest ConvertToAuthorizeRequestModel(AuthorizeRequestModel authorizeRequestModel)
        {
            try
            {
                return new PayfortAuthorizationInfoRequest()
                {
                    RequestPhrase = PayfortConfigurationModel.RequestPhrase,
                    AccessCode = PayfortConfigurationModel.AccessCode,
                    Amount = authorizeRequestModel.Amount.ToString(),
                    Command = PaymentCommandType.AUTHORIZATION.ToString(),
                    Currency = authorizeRequestModel.Currency,
                    CustomerEmail = authorizeRequestModel.CustomerEmail,
                    CustomerIp = authorizeRequestModel.IPAddress,
                    CustomerName = authorizeRequestModel.CustomerName,
                    Language = authorizeRequestModel.Language,
                    MerchantIdentifier = PayfortConfigurationModel.MerchantIdentifier,
                    MerchantReference = PayfortConfigurationModel.MerchantReference,
                    RememberMe = "YES",
                    ReturnUrl = PayfortConfigurationModel.ReturnUrl,
                    TokenName = authorizeRequestModel.Token,
                    MerchantExtra = authorizeRequestModel.BookingNumber,
                    Url = PayfortConfigurationModel.URL
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Converts to authorize request model.
        /// </summary>
        /// <param name="invoicePayRequestModel">The invoice pay request model.</param>
        /// <returns></returns>
        public PayfortInvoiceRequestModel ConvertToInvoiceRequestModel(InvoicePayRequestModel invoicePayRequestModel)
        {
            try
            {
                return new PayfortInvoiceRequestModel()
                {
                    RequestPhrase = PayfortConfigurationModel.RequestPhrase,
                    AccessCode = PayfortConfigurationModel.AccessCode,
                    Amount = invoicePayRequestModel.Amount.ToString(),
                    Currency = invoicePayRequestModel.Currency,
                    CustomerEmail = invoicePayRequestModel.CustomerEmail,
                    CustomerName = invoicePayRequestModel.CustomerName,
                    CustomerPhoneNumber = invoicePayRequestModel.CustomerContact,
                    MerchantIdentifier = PayfortConfigurationModel.MerchantIdentifier,
                    BookingReference = invoicePayRequestModel.BookingReferenceNumber,
                    OrderDescription = invoicePayRequestModel.OrderDescription,
                    CreditCardType = invoicePayRequestModel.CardType,
                    RequestExpiryDate = invoicePayRequestModel.ExpiryDate,
                    ReturnUrl = PayfortConfigurationModel.ReturnUrl,
                    DeviceFingerPrint = invoicePayRequestModel.DeviceFingerPrint,
                    Url = PayfortConfigurationModel.URL
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Converts to capture request model.
        /// </summary>
        /// <param name="captureRequestModel">The capture request model.</param>
        /// <returns></returns>
        public PayfortCaptureInfoRequestModel ConvertToCaptureRequestModel(CaptureRequestModel captureRequestModel)
        {
            try
            {
                return new PayfortCaptureInfoRequestModel()
                {
                    RequestPhrase = PayfortConfigurationModel.RequestPhrase,
                    AccessCode = PayfortConfigurationModel.AccessCode,
                    Amount = captureRequestModel.Amount.ToString(),
                    Command = PaymentCommandType.CAPTURE.ToString(),
                    Currency = captureRequestModel.Currency,
                    FortId = captureRequestModel.GatewayIdentifier,
                    Language = "en",
                    MerchantIdentifier = PayfortConfigurationModel.MerchantIdentifier,
                    Url = PayfortConfigurationModel.URL
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Converts to void authorize request model.
        /// </summary>
        /// <param name="authorizeRequestModel">The authorize request model.</param>
        /// <returns></returns>
        public PayfortAuthorizationInfoRequest ConvertToVoidAuthorizeRequestModel(VoidAuthorizeRequestModel authorizeRequestModel)
        {
            try
            {
                return new PayfortAuthorizationInfoRequest()
                {
                    RequestPhrase = PayfortConfigurationModel.RequestPhrase,
                    AccessCode = PayfortConfigurationModel.AccessCode,
                    Command = PaymentCommandType.VOID_AUTHORIZATION.ToString(),
                    FortId = "",
                    Language = "en",
                    MerchantIdentifier = PayfortConfigurationModel.MerchantIdentifier
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
