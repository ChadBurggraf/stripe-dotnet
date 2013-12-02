using System;
using System.Collections.Generic;
using System.Linq;

namespace Stripe
{
	public class StripeObject : JsonObject
	{
        public StripeObject()
            : this(null)
        {
        }

        public StripeObject(IDictionary<string, object> model)
            : base(model)
        {
        }

		public bool IsError { get { return HasProperty("error"); } }
	}
}
