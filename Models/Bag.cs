using System;
using System.Text.Json.Serialization;

namespace OstaraDemo.Models {

    public class Bag {

        [JsonPropertyName("bag-number")]
        public string BagNumber { get; set; }
        [JsonPropertyName("product-date")]
        public DateTime ProductDate { get; set; }
        [JsonPropertyName("product-type")]
        public string ProductType { get; set; }
        [JsonPropertyName("product-weight-lbs")]
        public double ProductWeightLbs { get; set; }
        [JsonPropertyName("production-location")]
        public string ProductionLocation { get; set; }
        [JsonPropertyName("passed-inspection")]
        public string PasedInspection { get; set; }
        [JsonPropertyName("notes")]
        public string Notes { get; set; }
        [JsonPropertyName("sample-sent-date")]
        public DateTime SampleSentDate { get; set; }
        [JsonPropertyName("bag-pickup-requested")]
        public bool BagPickupRequested { get; set; }
        [JsonPropertyName("bag-pickup-date")]
        public DateTime BagPickupDate { get; set; }
        [JsonPropertyName("bag-status")]
        public string BagStatus { get; set; }
        [JsonPropertyName("bag-status-date")]
        public DateTime BagStatusDate { get; set; }
        [JsonPropertyName("qc-result")]
        public string QCResult { get; set; }
        [JsonPropertyName("qc-date")]
        public DateTime QCDate { get; set; }
        [JsonPropertyName("qc-notes")]
        public string QCNotes { get; set; }
    }
}