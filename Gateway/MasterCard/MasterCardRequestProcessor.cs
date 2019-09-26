using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Platform.Payment.Contracts;
using Platform.Payment.Models.Request;
using Platform.Payment.Models.Response;

namespace Platform.Payment.Gateway.MasterCard
{
    public class MasterCardRequestProcessor : IPaymentProcessor
    {
        private ILogger<MasterCardRequestProcessor> _masterCardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterCardRequestProcessor"/> class.
        /// </summary>
        /// <param name="masterCardService">The master card service.</param>
        /// <exception cref="ArgumentNullException">masterCardService</exception>
        public MasterCardRequestProcessor(ILogger<MasterCardRequestProcessor> masterCardService)
        {
            _masterCardService = masterCardService ?? throw new ArgumentNullException(nameof(masterCardService));
        }

        /// <summary>
        /// Authorizes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<AuthorizeResponseModel> Authorize(AuthorizeRequestModel request)
        {
            try
            {
                return new AuthorizeResponseModel();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Captures the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<CaptureResponseModel> Capture(CaptureRequestModel request)
        {
            try
            {
                return new CaptureResponseModel();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Invoices the pay.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<InvoicePayResponseModel> InvoicePay(InvoicePayRequestModel request)
        {
            try
            {
                return new InvoicePayResponseModel();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Voids the authorize.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<VoidAuthorizeResponseModel> VoidAuthorize(VoidAuthorizeRequestModel request)
        {
            try
            {
                return new VoidAuthorizeResponseModel();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
