using System;
using System.Collections.Generic;
using System.Text;
using Platform.Payment.Enums;
using Platform.Payment.Extension;
using Platform.Payment.PayfortModels;
using Platform.Payment.PaymentProcessor;

namespace Platform.Payment.Payfort
{
    public class PayfortClient : IPayfortClient
    {
        /// <summary>
        /// Authorize
        /// </summary>
        public PaymentResponse Authorize(PayfortAuthorizationInfoRequest request)
        {
            //Declarations
            var errInfo = new PaymentResponse();
            //errInfo.UserId = request.UserId;
            //errInfo.BookingId = request.BookingId;

            if (request != null)
            {
                //1. Generate SHA256 Signature
                request.Signature = request.GenerateAuthorizationSHA256Signature();

                //2. Generate Json Request Parameter
                var jsonRequest = request.GetAuthorizationRequestParams();

                //3. Send Request
                return AuthorizeProcessor.Authorize(jsonRequest, request.Url, errInfo);
            }
            else
            {
                return ExceptionHandler.ExceptionHandler.GetPayfortExceptionResponseInfo(errInfo, PaymentCommandType.AUTHORIZATION);
            }
        }

        /// <summary>
        /// Purchase
        /// </summary>
        public PaymentResponse Purchase(PayfortAuthorizationInfoRequest request)
        {
            //Declarations
            var errInfo = new PaymentResponse();
            //errInfo.UserId = request.UserId;
            //errInfo.BookingId = request.BookingId;

            if (request != null)
            {
                //1. Generate SHA256 Signature
                request.Signature = request.GenerateAuthorizationSHA256Signature();

                //2. Generate Json Request Parameter
                var jsonRequest = request.GetAuthorizationRequestParams();

                //3. Send Request
                return PurchaseProcessor.Purchase(jsonRequest, request.Url, errInfo);
            }
            else
            {
                return ExceptionHandler.ExceptionHandler.GetPayfortExceptionResponseInfo(errInfo, PaymentCommandType.PURCHASE);
            }
        }

        /// <summary>
        /// Authorize
        /// </summary>
        public PaymentResponse Capture(PayfortCaptureInfoRequestModel request)
        {
            var errInfo = new PaymentResponse();
            errInfo.UserId = request.UserId;
            errInfo.BookingId = request.BookingId;

            if (request != null)
            {
                //1. Generate SHA256 Signature
                request.Signature = request.GenerateCaptureSHA256Signature();

                //2. Generate Json Request Parameter
                var jsonRequest = request.GetCaptureRequestParams(request.Signature);

                //3. Send Request
                return CaptureProcessor.Capture(jsonRequest, request.Url, errInfo);
            }
            else
            {
                return ExceptionHandler.ExceptionHandler.GetPayfortExceptionResponseInfo(errInfo, PaymentCommandType.CAPTURE);
            }
        }

        /// <summary>
        /// Authorize
        /// </summary>
        public PaymentResponse VoidAuthorize(PayfortAuthorizationInfoRequest request)
        {
            //Declarations
            var errInfo = new PaymentResponse();
            errInfo.UserId = request.UserId;
            errInfo.BookingId = request.BookingId;

            if (request != null)
            {
                //1. Generate SHA256 Signature
                request.Signature = request.GenerateVoidAuthorizationSHA256Signature();

                //2. Generate Json Request Parameter
                var jsonRequest = request.GetVoidAuthorizationRequestParams();

                //3. Send Request
                return AuthorizeProcessor.VoidAuthorize(jsonRequest, request.Url, errInfo);
            }
            else
            {
                return ExceptionHandler.ExceptionHandler.GetPayfortExceptionResponseInfo(errInfo, PaymentCommandType.VOID_AUTHORIZATION);
            }
        }

        /// <summary>
        /// Generates the invoice.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public PaymentResponse GenerateInvoice(PayfortInvoiceRequestModel request)
        {
            //Declarations
            var errInfo = new PaymentResponse();
            errInfo.BookingRefNumber = request.BookingReference;
            request.RequestExpiryDate = DateTimeOffset.UtcNow.AddDays(24);
            errInfo.RequestExpiryDate = request.RequestExpiryDate;
            if (request != null)
            {
                //1. Generate SHA256 Signature
                request.Signature = request.GenerateInvoiceSHA256Signature();

                //2. Generate Json Request Parameter
                var jsonRequest = request.GetInvoiceRequestParams();

                //3. Send Request
                return InvoiceProcessor.Invoice(jsonRequest, request.Url, errInfo);
            }
            else
            {
                return ExceptionHandler.ExceptionHandler.GetPayfortExceptionResponseInfo(errInfo, PaymentCommandType.AUTHORIZATION);
            }
        }

        /// <summary>
        /// Checks the invoice status.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public PayFortCheckInvoiceRequestModel CheckInvoiceStatus(PayFortCheckInvoiceRequestModel request)
        {
            //Declarations

            //1. Generate SHA256 Signature
            request.Signature = request.GenerateCheckInvoiceStatusSHA256Signature();

            //2. Generate Json Request Parameter
            var jsonRequest = request.GenerateCheckInvoiceStatusRequestParams();

            //3. Send Request
            return InvoiceProcessor.CheckStatus(jsonRequest, request);
        }

    }
}
