﻿using Smartstore.Core.Checkout.Payment;
using Smartstore.Core.Identity;
using Smartstore.Core.Stores;

namespace Smartstore.PayPal.Services
{
    public class PayPalHelper : ICookiePublisher
    {
        private readonly IStoreContext _storeContext;
        private readonly IPaymentService _paymentService;
        private readonly Localizer T;

        public PayPalHelper(IStoreContext storeContext, IPaymentService paymentService, Localizer localizer)
        {
            _storeContext = storeContext;
            _paymentService = paymentService;
            T = localizer;
        }

        public Task<bool> IsPayPalStandardActiveAsync()
            => _paymentService.IsPaymentMethodActiveAsync("Payments.PayPalStandard", null, _storeContext.CurrentStore.Id);

        public Task<bool> IsPayUponInvoiceActiveAsync()
            => _paymentService.IsPaymentMethodActiveAsync("Payments.PayPalPayUponInvoice", null, _storeContext.CurrentStore.Id);

        public Task<bool> IsCreditCardActiveAsync()
            => _paymentService.IsPaymentMethodActiveAsync("Payments.PayPalCreditCard", null, _storeContext.CurrentStore.Id);

        public Task<bool> IsPayLaterActiveAsync()
            => _paymentService.IsPaymentMethodActiveAsync("Payments.PayPalPayLater", null, _storeContext.CurrentStore.Id);

        public Task<bool> IsSepaActiveAsync()
            => _paymentService.IsPaymentMethodActiveAsync("Payments.PayPalSepa", null, _storeContext.CurrentStore.Id);

        public Task<bool> IsGiropayActiveAsync()
            => _paymentService.IsPaymentMethodActiveAsync("Payments.PayPalGiropay", null, _storeContext.CurrentStore.Id);

        public Task<bool> IsSofortActiveAsync()
            => _paymentService.IsPaymentMethodActiveAsync("Payments.PayPalSofort", null, _storeContext.CurrentStore.Id);

        public async Task<bool> IsAnyMethodActiveAsync(params string[] providerSystemNames)
        {
            Guard.NotEmpty(providerSystemNames);

            var activePaymentMethods = await _paymentService.LoadActivePaymentMethodsAsync(null, _storeContext.CurrentStore.Id);
            return activePaymentMethods.Any(x => providerSystemNames.Contains(x.Metadata.SystemName));
        }

        // TODO: (mh) (core) Add the others Bancontact, Blik, Eps,  Ideal, MercadoPago, P24, Venmo

        public async Task<IEnumerable<CookieInfo>> GetCookieInfosAsync()
        {
            // INFO: APMs don't need cookies as everything on page is handled via API requests.
            // The pages to which the customer is redirected when using APMs must handle cookie consent themsleves.
            if (await IsAnyMethodActiveAsync(
                "Payments.PayPalStandard",
                "Payments.PayPalPayUponInvoice",
                "Payments.PayPalCreditCard",
                "Payments.PayPalPayLater",
                "Payments.PayPalSepa"))
            {
                var cookieInfo = new CookieInfo
                {
                    Name = T("Plugins.FriendlyName.Smartstore.PayPal"),
                    Description = T("Plugins.Smartstore.PayPal.CookieInfo"),
                    CookieType = CookieType.Required
                };

                return new List<CookieInfo> { cookieInfo }.AsEnumerable();
            }

            return null;
        }
    }
}
