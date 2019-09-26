using System;
using System.Collections.Generic;
using System.Text;
using Platform.Payment.Enums;

namespace Platform.Payment.Models.Request
{
    public class VoidAuthorizeRequestModel
    {
        public PaymentEngine Engine { get; set; }
    }
}
