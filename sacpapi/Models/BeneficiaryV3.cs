using System;
using System.ComponentModel.DataAnnotations;
public class BeneficiaryV3
{
    [Key]
    public int ID { get; set; }
    public string? HouseholdIdentifierNumber { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Village { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Sex { get; set; }
    public int? Age { get; set; } // Nullable int
    public DateTime? DateOfBirth { get; set; } // Nullable DateTime
    public string? SexOfHousehold { get; set; }
    public string? ContactNumber { get; set; }
    public string? DisabilityStatus { get; set; }
    public string? YouthStatus { get; set; }
    public string? LandSize { get; set; }
    public string? ValueChain { get; set; }
    public string? APGGroup { get; set; }
    public string? Chairperson { get; set; }

    public string? Status { get; set; }

    // File Details
    public string? FileName { get; set; } // Original file name
    public string? FileType { get; set; } // File type (e.g. "application/pdf")
    public long? FileSize { get; set; } // Nullable long for file size

    // This stores the uploaded file as a byte array
    public byte[]? FileData { get; set; } // Nullable byte array

    // Nullable timestamp for when the record was created
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
}
