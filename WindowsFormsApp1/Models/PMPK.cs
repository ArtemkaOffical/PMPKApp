using System;
using System.Text.Json.Serialization;

namespace PMPK.Models
{
    public class PMPK : BaseType
    {
        public override int Id { get; set; }
        [JsonPropertyName("Адрес")]
        public string Address { get; set; }
        public string CommissionWithdrawal { get; set; }
        public string WhereStuding { get; set; }
        public byte OVZ { get; set; }
        public byte Control { get; set; }
        public byte Invalid { get; set; }
        public byte MSE { get; set; }
        public byte GIA9 { get; set; }
        public byte GIA11 { get; set; }
        public byte NonRuss { get; set; }
        public byte Programm { get; set; }
        public string AProgram { get; set; }
        public string Logopedist { get; set; }
        public string Psychologist { get; set; }
        public string Psychiatrist { get; set; }
        public string Defectologist { get; set; }
        public string Pedagog { get; set; }
        public string OwnerPMPK { get; set; }
        public byte FirstPriem { get; set; }
        public byte Direction { get; set; }
        public string FormStudy { get; set; }
        public string Organization { get; set; }
        public string Class { get; set; }
        public string LocationOfEventPMPK { get; set; }
        public DateTime? DateOfPMPK { get; set; }
        public DateTime? DateOfNextPMPK { get; set; }
        public string SentByToPMPK { get; set; }
        public int NumberOfProtocol { get; set; }
        public int ChildrenId { get; set; }
        public Children Children { get; set; }
    }
}
