﻿using Smartstore.Core.Checkout.Orders;
using Smartstore.Core.Checkout.Payment;
using Smartstore.Core.Data;
using Smartstore.Core.Widgets;
using Smartstore.Engine.Modularity;
using Smartstore.PayPal.Client;
using Smartstore.PayPal.Components;

namespace Smartstore.PayPal.Providers
{
    [SystemName("Payments.PayPalSepa")]
    [FriendlyName("PayPal Sepa")]
    [Order(1)]
    public class PayPalSepaProvider : PayPalProviderBase
    {
        public PayPalSepaProvider(SmartDbContext db, PayPalHttpClient client, PayPalSettings settings, IPaymentService paymentService)
            : base(db, client, settings, paymentService)
        {
        }

        public override Widget GetPaymentInfoWidget()
            => new ComponentWidget(typeof(PayPalSepaViewComponent), true);
    }
}