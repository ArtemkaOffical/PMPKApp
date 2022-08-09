using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMPK.Models
{
    public class Parent : BaseType
    {
        public override int Id { get; set; }
        public string FullName { get; set; }
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DateOfBirth { get; set; }
        public string Phone { get; set; }
        public int ChildrenId { get; set; }
        public Children Children { get; set; }
        public Passport Passport { get; set; }


    }
}
