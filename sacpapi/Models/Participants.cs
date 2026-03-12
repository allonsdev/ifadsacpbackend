using System.ComponentModel.DataAnnotations.Schema;

[Table("tblParticipants")]
public class Participants
{

    public int Id { get; set; }

    [Column("Gtid")]
    public int? Gtid { get; set; }

    [Column("HhNationalId")]
    public string? HhNationalId { get; set; }

    [Column("FirstName")]
    public string? FirstName { get; set; }

    [Column("Surname")]
    public string? Surname { get; set; }

    [Column("Gender")]
    public string? Gender { get; set; }

    [Column("YearOfBirth")]
    public int? YearOfBirth { get; set; }

    [Column("GroupType")]
    public string? GroupType { get; set; }

    [Column("CategoryName")]
    public string? CategoryName { get; set; }

    [Column("ParticipantNationalId")]
    public string? ParticipantNationalId { get; set; }

    [Column("PwdTin")]
    public string? PwdTin { get; set; }

    [Column("HhGender")]
    public string? HhGender { get; set; }

    [Column("ContactNumber")]
    public string? ContactNumber { get; set; }

    [Column("Createdby")]
    public string? Createdby { get; set; }
}

