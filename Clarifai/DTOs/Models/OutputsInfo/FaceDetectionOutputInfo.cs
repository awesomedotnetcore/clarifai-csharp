﻿using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Models.OutputsInfo
{
    /// <summary>
    /// Certain information regarding the FaceDetection model.
    /// </summary>
    public class FaceDetectionOutputInfo : IOutputInfo
    {
        /// <inheritdoc />
        public string Type { get; }

        /// <inheritdoc />
        public string TypeExt { get; }

        /// <inheritdoc />
        public string Message { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="typeExt">the type ext</param>
        /// <param name="message">the message</param>
        private FaceDetectionOutputInfo(string type, string typeExt, string message)
        {
            Type = type;
            TypeExt = typeExt;
            Message = message;
        }

        public JObject Serialize()
        {
            return new JObject();
        }

        public static FaceDetectionOutputInfo Deserialize(dynamic jsonObject)
        {
            return new FaceDetectionOutputInfo(
                (string) jsonObject.type,
                (string) jsonObject.type_ext,
                (string) jsonObject.message
            );
        }

        public override string ToString()
        {
            return "[FaceDetectionOutputInfo]";
        }
    }
}
