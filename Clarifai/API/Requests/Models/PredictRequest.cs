﻿using System;
using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Clarifai.Exceptions;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Request for running a prediction on a model.
    /// </summary>
    /// <typeparam name="T">the model type</typeparam>
    public class PredictRequest<T> : ClarifaiRequest<ClarifaiOutput<T>>
        where T : IPrediction
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url
        {
            get
            {
                if (_modelVersionID == null)
                {
                    return $"/v2/models/{_modelID}/outputs";
                }
                else
                {
                    return $"/v2/models/{_modelID}/versions/{_modelVersionID}/outputs";
                }
            }
        }

        private readonly string _modelID;
        private readonly IClarifaiInput _input;
        private readonly string _modelVersionID;
        private readonly string _language;
        private readonly decimal? _minValue;
        private readonly int? _maxConcepts;
        private readonly IEnumerable<Concept> _selectConcepts;
        private readonly int? _sampleMs;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="input">the Clarifai input</param>
        /// <param name="modelVersionID">the model version ID - leave null for latest</param>
        /// <param name="language">the language</param>
        /// <param name="minValue">
        /// only predictions with a value greater than or equal to to minValue will be returned
        /// </param>
        /// <param name="maxConcepts">
        /// the maximum maxConcepts number of predictions that will be returned
        /// </param>
        /// <param name="selectConcepts">only selectConcepts will be returned</param>
        /// <param name="sampleMs">video frame prediction every [sampleMs] milliseconds</param>
        public PredictRequest(IClarifaiHttpClient httpClient, string modelID,
            IClarifaiInput input, string modelVersionID = null,
            string language = null, decimal? minValue =  null, int? maxConcepts = null,
            IEnumerable<Concept> selectConcepts = null, int? sampleMs = null)
            : base(httpClient)
        {
            _modelID = modelID;
            _input = input;
            _modelVersionID = modelVersionID;
            _language = language;
            _minValue = minValue;
            _maxConcepts = maxConcepts;
            _selectConcepts = selectConcepts;
            _sampleMs = sampleMs;
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            JObject body = new JObject(
                new JProperty("inputs", new JArray(_input.Serialize())));

            if (_language != null || _minValue != null || _maxConcepts != null ||
                _selectConcepts != null || _sampleMs != null)
            {
                var outputConfig = new JObject();
                if (_language != null)
                {
                    outputConfig.Add("language", _language);
                }
                if (_minValue != null)
                {
                    outputConfig.Add("min_value", _minValue);
                }
                if (_maxConcepts != null)
                {
                    outputConfig.Add("max_concepts", _maxConcepts);
                }
                if (_sampleMs != null)
                {
                    outputConfig.Add("sample_ms", _sampleMs);
                }

                if (_selectConcepts != null)
                {
                    outputConfig.Add("select_concepts",
                        new JArray(_selectConcepts.Select(c => c.Serialize())));
                }

                body.Add(new JProperty("model", new JObject(
                    new JProperty("output_info", new JObject(
                        new JProperty("output_config", outputConfig))))));
            }
            return body;
        }

        /// <inheritdoc />
        protected override ClarifaiOutput<T> Unmarshaller(dynamic jsonObject)
        {
            if (jsonObject.outputs != null && jsonObject.outputs.Count == 1)
            {
                var jsonOutput = jsonObject.outputs[0];
                return ClarifaiOutput<T>.Deserialize(HttpClient, jsonOutput);
            }
            throw new ClarifaiException("The response does not contain exactly one output.");
        }
    }
}
