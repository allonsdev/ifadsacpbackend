using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sacpapi.Models
{
    [Table("tblStakeParticipants")]
    public class StakeParticipant
    {
        [Key]
        public int Id { get; set; }

        // GTID comes from the system
        [Column("Gtid")]
        public Guid Gtid { get; set; }

        public int? UserId { get; set; }

        [Column("Name of Participant")]
        public string? NameOfParticipant { get; set; }

        [Column("Sex (M/F)")]
        public string? Sex { get; set; }

        [Column("Organisation")]
        public string? Organisation { get; set; }

        [Column("Position")]
        public string? Position { get; set; }

        [Column("Contact Number")]
        public string? ContactNumber { get; set; }

        [Column("Email Address")]
        public string? EmailAddress { get; set; }

        [Column("Signature of Participant")]
        public string? SignatureOfParticipant { get; set; }
    }
}
