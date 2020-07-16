using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformationManagementMVC.Models
{
    public class CallList
    {
        [BsonId]
        public Double _id { get; set; }
        public Double Id
        {
            get { return Double.Parse(_id.ToString()); }
            set { _id = Double.Parse(value.ToString()); }
        }
        [Required]
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Gender")]
        public string Gender { get; set; }
        [Phone]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only numeric values are allowed")]
        [BsonElement("OffNo")]
        public string OffNo { get; set; }
        [Required]
        [Phone]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only numeric values are allowed")]
        [BsonElement("MobNo")]
        public string MobNo { get; set; }
        [Required]
        [Phone]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only numeric values are allowed")]
        [BsonElement("HomeTelNo")]
        public string HomeTelNo { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "EmailiD is not valid")]
        [BsonElement("EmailID")]
        public string EmailID { get; set; }
        [BsonElement("CurAdd")]
        public string CurAdd { get; set; }
        [BsonElement("OffAdd")]
        public string OffAdd { get; set; }
        [BsonElement("UserID")]
        public double UserID { get; set; }
        [BsonElement("accessToken")]
        public string accessToken { get; set; }

    }
}