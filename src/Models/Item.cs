namespace todo.Models
{
    using Microsoft.Azure.Documents;
    using Newtonsoft.Json;

    public class Item
    {
        [JsonProperty(PropertyName = "deviceid")]
        public string DeviceID { get; set; }

        [JsonProperty(PropertyName = "OrderID")]
        public string OrderID { get; set; }

        [JsonProperty(PropertyName = "ProductID")]
        public string ProductID { get; set; }

        [JsonProperty(PropertyName = "Product_id")]
        public string Product { get; set; }

        [JsonProperty(PropertyName = "Product_weight")]
        public string Product_weight { get; set; }   


        [JsonProperty(PropertyName = "TagID")]
        public string TagID { get; set; }

        [JsonProperty(PropertyName = "GrossWeight")]
        public string Weight { get; set; }

        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }

        [JsonProperty(PropertyName = "time_stamp")]
        public string time_stamp { get; set; }


        [JsonProperty(PropertyName = "ProductWeight")]
        public string ProductWeight { get; set; }
        

    }
}