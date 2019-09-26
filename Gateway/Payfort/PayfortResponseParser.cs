using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Platform.Payment.Contracts.Payfort;
using Platform.Payment.Enums;
using Platform.Payment.Models.Response;
using Platform.Payment.PayfortModels;
using Platform.Payfort.Payment.Extension;

namespace Platform.Payment.Gateway.Payfort
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Payment.Domain.Contracts.Payfort.IPayfortResponseParser" />
    public class PayfortResponseParser : IPayfortResponseParser
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<PayfortResponseParser> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PayfortResponseParser"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        public PayfortResponseParser(ILogger<PayfortResponseParser> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Maps the authorization response.
        /// </summary>
        /// <param name="paymentResponse">The payment response.</param>
        /// <returns></returns>
        public AuthorizeResponseModel MapAuthorizationResponse(PaymentResponse paymentResponse)
        {
            try
            {
                return new AuthorizeResponseModel()
                {
                    IsSuccess = paymentResponse.PayfortResponse.status.Equals("20", StringComparison.OrdinalIgnoreCase),
                    Is3DSecure = !paymentResponse.PayfortResponse.Threeds_url.IsNullOrEmpty(),
                    Amount = paymentResponse.PayfortResponse.amount.ToDecimal(),
                    Currency = paymentResponse.PayfortResponse.currency,
                    EngineType = (int)PaymentEngine.PAYFORT,
                    Message = paymentResponse.PayfortResponse.response_message,
                    URL3DSecure = paymentResponse.PayfortResponse.Threeds_url,
                    FortId = paymentResponse.FortId
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Maps the capture response.
        /// </summary>
        /// <param name="paymentResponse">The payment response.</param>
        /// <returns></returns>
        public CaptureResponseModel MapCaptureResponse(PaymentResponse paymentResponse)
        {
            try
            {
                return new CaptureResponseModel()
                {
                    IsSuccess = paymentResponse.IsValid,
                    Amount = paymentResponse.PayfortResponse.amount.ToDecimal(),
                    BookingRefNumber = paymentResponse.BookingRefNumber,
                    BookingId = paymentResponse.BookingId,
                    Currency = paymentResponse.PayfortResponse.currency,
                    EngineType = (int)PaymentEngine.PAYFORT,
                    Message = paymentResponse.PayfortResponse.response_message,
                    ErrorId = paymentResponse.ErrorId,
                    ErrorTypeDescription = paymentResponse.ErrorTypeDescription,
                    ErrorTypeId = paymentResponse.ErrorTypeId
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Maps the invoice pay response.
        /// </summary>
        /// <param name="paymentResponse">The payment response.</param>
        /// <returns></returns>
        public InvoicePayResponseModel MapInvoicePayResponse(PaymentResponse paymentResponse)
        {
            try
            {

                //     var payfortResponse = JsonConvert.DeserializeObject<PayfortCheckStatus>(pfResponse);
                var InvoicePayResponseModel = new InvoicePayResponseModel();

                InvoicePayResponseModel.IsValid = paymentResponse.IsValid;
                InvoicePayResponseModel.ResponseCode = paymentResponse.ResponseCode;
                InvoicePayResponseModel.PaymentLink = paymentResponse.PaymentLink;
                InvoicePayResponseModel.PaymentLinkId = paymentResponse.PaymentLinkId;
                InvoicePayResponseModel.ResponseCode = paymentResponse.ResponseCode;
                return InvoicePayResponseModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Maps the void authorization response.
        /// </summary>
        /// <param name="paymentResponse">The payment response.</param>
        /// <returns></returns>
        public VoidAuthorizeResponseModel MapVoidAuthorizationResponse(PaymentResponse paymentResponse)
        {
            try
            {
                var voidAuthorizeResponseModel = new VoidAuthorizeResponseModel();
                voidAuthorizeResponseModel.IsValid = paymentResponse.IsValid;

                return voidAuthorizeResponseModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
