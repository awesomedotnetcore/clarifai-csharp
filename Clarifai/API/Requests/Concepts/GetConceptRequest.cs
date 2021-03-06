﻿using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Concepts
{
    /// <summary>
    /// A request for getting concept.
    /// </summary>
    public class GetConceptRequest : ClarifaiRequest<Concept>
    {
        private readonly string _conceptID;

        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/concepts/" + _conceptID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="conceptID">the concept ID</param>
        public GetConceptRequest(IClarifaiHttpClient httpClient, string conceptID)
            : base(httpClient)
        {
            _conceptID = conceptID;
        }

        /// <inheritdoc />
        protected override Concept Unmarshaller(dynamic jsonObject)
        {
            return Concept.Deserialize(jsonObject.concept);
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject();
        }
    }
}
