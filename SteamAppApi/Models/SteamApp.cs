using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace SteamAppApi.Models
{
    [Serializable]
    [DataContract]
    public class SteamApp
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int AppId { get; set; }
        [DataMember]
        public List<string> Tags { get; set; }
        [DataMember]
        public string Developer { get; set; }
        [DataMember]
        public string Publisher { get; set; }
        [DataMember]
        public string ReleaseDate { get; set; }
        [DataMember]
        public bool SinglePlayer { get; set; }
        [DataMember]
        public bool MultiPlayer { get; set; }
        [DataMember]
        public bool Controller { get; set; }
        [DataMember]
        public bool TradingCards { get; set; }
    }
}