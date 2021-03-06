﻿using System.Collections.Generic;

namespace Clarifai.DTOs.Predictions
{
    public class Logo : IPrediction
    {
        public string TYPE => "logo";

        public Crop Crop { get; }

        public List<Concept> Concepts { get; }

        private Logo(Crop crop, List<Concept> concepts)
        {
            Crop = crop;
            Concepts = concepts;
        }

        public static Logo Deserialize(dynamic jsonObject)
        {
            var concepts = new List<Concept>();
            foreach (dynamic concept in jsonObject.data.concepts)
            {
                concepts.Add(Concept.Deserialize(concept));
            }
            return new Logo(DTOs.Crop.Deserialize(jsonObject.region_info.bounding_box), concepts);
        }

        public override bool Equals(object obj)
        {
            return obj is Logo logo &&
                   EqualityComparer<Crop>.Default.Equals(Crop, logo.Crop) &&
                   EqualityComparer<List<Concept>>.Default.Equals(Concepts, logo.Concepts);
        }

        public override int GetHashCode()
        {
            var hashCode = -1453176561;
            hashCode = hashCode * -1521134295 + EqualityComparer<Crop>.Default.GetHashCode(Crop);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Concept>>.Default.GetHashCode(Concepts);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[Logo: (crop: {Crop}, concepts: {Concepts})]";
        }
    }
}
