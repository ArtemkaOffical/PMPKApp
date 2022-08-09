using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMPK.Models
{
    [ComplexType]
    public class Passport : BaseType
    {
        public Passport(string type)
        {
            Type = type;
        }
        public override int Id { get; set; }
        public string Type { get; set; }
        public int? Series { get; set; }
        public int? INN { get; set; }
        public int? Number { get; set; }
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DateOfIssue { get; set; }
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DeliveryDate { get; set; }
        public string IssuedByWhom { get; set; }
    }
}
