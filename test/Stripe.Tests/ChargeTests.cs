using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Stripe.Tests
{
	public class ChargeTests
	{
		private StripeClient _client;

		private dynamic _customer;
		private CreditCard _card;

		public ChargeTests()
		{
			_card = new CreditCard {
				Number = "4242 4242 4242 4242",
				ExpMonth = DateTime.Now.Month,
				ExpYear = DateTime.Now.AddYears(1).Year
			};

			_client = new StripeClient(Constants.ApiKey);
			_customer = _client.CreateCustomer(_card);
		}
        [Fact]
        public void CreateCharge_Using_Token()
        {
            dynamic token = _client.CreateCardToken(_card);
            dynamic response = _client.CreateChargeWithToken(100M,token.Id);
            Assert.NotNull(response);
            Assert.False(response.IsError);
            Assert.True(response.Paid);
        }
		[Fact]
		public void CreateCharge_Card_Test()
		{
			dynamic response = _client.CreateCharge(100M, "usd", _card);

			Assert.NotNull(response);
			Assert.False(response.IsError);
			Assert.True(response.Paid);
		}

		[Fact]
		public void CreateCharge_Customer_Test()
		{
			dynamic response = _client.CreateCharge(100M, "usd", _customer.Id);

			Assert.NotNull(response);
			Assert.False(response.IsError);
			Assert.True(response.Paid);
		}

        [Fact]
        public void CreateCharge_Customer_Test_With_Metadata()
        {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata.Add("guid", Guid.NewGuid());
            metadata.Add("date", DateTime.UtcNow);
            metadata.Add("string", "stripe");
            metadata.Add("double", 2.0);
            metadata.Add("bool", false);

            dynamic response = _client.CreateCharge(100M, "usd", _customer.Id, null, metadata);

            Assert.NotNull(response);
            Assert.False(response.IsError);
            Assert.True(response.Paid);

            JsonObject meta = response.Metadata as JsonObject;
            Assert.NotNull(meta);
            Assert.Equal(5, meta.Count());
        }

		[Fact]
		public void RetrieveCharge_Test()
		{
			dynamic charge = _client.CreateCharge(100M, "usd", _customer.Id);
			dynamic response = _client.RetrieveCharge(charge.Id);

			Assert.NotNull(response);
			Assert.False(response.IsError);
			Assert.Equal(charge.Id, response.Id);
		}

		[Fact]
		public void RefundCharge_Test()
		{
			dynamic charge = _client.CreateCharge(100M, "usd", _customer.Id);
			dynamic response = _client.RefundCharge(charge.Id);

			Assert.NotNull(response);
			Assert.False(response.IsError);
			Assert.Equal(charge.Id, response.Id);
			Assert.True(response.Refunded);
		}

		[Fact]
		public void ListCharges_Test()
		{
			StripeArray response = _client.ListCharges();

			Assert.NotNull(response);
			Assert.False(response.IsError);
			Assert.True(response.Any());
		}

        [Fact]
        public void ListCharges_TestDateRange()
        {
            var begin = new DateTimeOffset(new DateTime(2013, 1, 1));
            var end =  new DateTimeOffset(new DateTime(2014, 1, 1));
            StripeArray response = _client.ListCharges(periodBegin: begin, periodEnd: end);
            Assert.NotNull(response);
            Assert.False(response.IsError);
            Assert.True(response.Any());

            foreach (var obj in response)
            {
                var created = long.Parse(obj["created"].ToString()).ToDateTime();
                Assert.True(created >= begin);
                Assert.True(created < end);
            }
        }
	}
}
