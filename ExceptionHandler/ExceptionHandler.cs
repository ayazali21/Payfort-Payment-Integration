using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Platform.Payment.Enums;
using Platform.Payment.PayfortModels;

namespace Platform.Payment.ExceptionHandler
{
    public static class ExceptionHandler
    {
        /// <summary>
        /// Payfort Error Info
        /// </summary>
        /// <param name="errInfo"></param>
        /// <returns>PayfortErrorInfo</returns>
        public static PaymentResponse GetPayfortExceptionResponseInfo(PaymentResponse errInfo, PaymentCommandType CommandType)
        {
            if (CommandType == PaymentCommandType.AUTHORIZATION)
                errInfo.ErrorId = (int)CheckOutBookingError.AuthorizationException;
            else if (CommandType == PaymentCommandType.VOID_AUTHORIZATION)
                errInfo.ErrorId = (int)CheckOutBookingError.AuthorizationException;
            else if (CommandType == PaymentCommandType.PURCHASE)
                errInfo.ErrorId = (int)CheckOutBookingError.PurchaseException;
            else
                errInfo.ErrorId = (int)CheckOutBookingError.CaptureException;
            errInfo.ErrorTypeId = (int)ErrorType.Payment;
            errInfo.ErrorTypeDescription = GetEnumDescription((CheckOutBookingError)errInfo.ErrorId);

            return errInfo;
        }

        private static string GetEnumDescription(Enum en)
        {
            var type = en.GetType();

            var memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
    }
}
