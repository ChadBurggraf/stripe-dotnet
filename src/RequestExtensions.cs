using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Extensions;
using RestSharp.Validation;

namespace Stripe
{
    public static class RequestExtensions
    {
        public static void WithMetadata(this RestRequest request, IDictionary<string, object> metadata, string metadataKey = null)
        {
            if(request == null)
            {
                throw new ArgumentNullException("request", "request cannot be null.");
            }

            if (metadata != null)
            {
                metadataKey = metadataKey ?? "metadata";

                foreach (string key in metadata.Keys)
                {
                    object value = metadata[key];

                    if (value != null)
                    {
                        string requestMetadataKey = string.Concat(metadataKey, "[", key, "]");
                        request.AddParameter(requestMetadataKey, metadata[key]);
                    }
                }
            }
        }
    }
}
