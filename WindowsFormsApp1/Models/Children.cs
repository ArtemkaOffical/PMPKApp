using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PMPK.Models
{
    public class Children : BaseType
    {
        public Children()
        {
            PMPKs = new List<PMPK>();
            Parents = new List<Parent>();
            Documents = new List<Document>();
        }
        [JsonPropertyName("ФИО")]
        public string FullName { get; set; }
        [JsonPropertyName("Дата рождения")]
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateOfBirth { get; set; }
        [JsonPropertyName("Возраст")]
        public int Age { get; set; }
        [JsonPropertyName("Дата снятия с учёта")]
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? dateOfDeRegistration { get; set; }
        [JsonPropertyName("Пол")]
        public char? Sex { get; set; }
        [JsonPropertyName("Родители")]
        public List<Parent> Parents { get; set; }
        [JsonPropertyName("Сирота")]
        public byte Sirota { get; set; }
        [JsonPropertyName("Многодетные")]
        public byte Mnogodet { get; set; }
        [JsonPropertyName("Малоимущие")]
        public byte MalIm { get; set; }
        public List<Document> Documents { get; set; }
        public List<PMPK> PMPKs { get; set; }
        public override int Id { get; set; }
    }
}
