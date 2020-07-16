using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InformationManagementMVC.Models
{
    public class RegisterUser
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
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "EmailiD is not valid")]
        [BsonElement("EmailiD")]
        public string EmailiD { get; set; }
        [Required]
        [BsonElement("UserName")]
        public string UserName { get; set; }
        [Required]
        [BsonElement("PassWord")]
        public string PassWord { get; set; }
        [Compare("PassWord")]
        [BsonElement("ConfirmPassword")]
        public string ConfirmPassword { get; set; }
        [BsonElement("UserRole")]
        public string UserRole { get; set; }

    }
}