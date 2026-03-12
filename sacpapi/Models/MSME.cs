using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sacpapi.Models
{
    public class MSME
    {
        [Key]
        public int Id { get; set; } = 0;
        public int koboEnterpriseId { get; set; } = 0;
        public string? formhub_uuid { get; set; } = null;
        public DateTime? today { get; set; } = null;
        public DateTime? start { get; set; } = null;
        public DateTime? end { get; set; } = null;
        public string? province { get; set; } = null;
        public string? district { get; set; } = null;
        public string? ward { get; set; } = null;
        public string? buscenter { get; set; } = null;
        public string? enumerator { get; set; } = null;
        public string? nameofbus { get; set; } = null;
        public string? tradingname { get; set; } = null;
        public string? regstatus { get; set; } = null;
        public string? namecontact { get; set; } = null;
        public string? address { get; set; } = null;
        public string? phone { get; set; } = null;
        public string? email { get; set; } = null;
        public string? legalstatus { get; set; } = null;
        public string? yearest { get; set; } = null;
        public int? staffno { get; set; } = 0;
        public int? femalesno { get; set; } = 0;
        public int? maleno { get; set; } = 0;
        public int? youthno { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? annualbudget { get; set; } = 0;
        public string? networks { get; set; } = null;
        public int? nobranches { get; set; } = 0;
        public string? location { get; set; } = null;
        public string? prductoff1 { get; set; } = null;
        public string? cusprod1 { get; set; } = null;
        public int? prod1 { get; set; } = 0;
        public int? serv1 { get; set; } = 0;
        public string? rawmat1 { get; set; } = null;
        public string? prductoff2 { get; set; } = null;
        public string? cusprod2 { get; set; } = null;
        public int? prod2 { get; set; } = 0;
        public int? serv2 { get; set; } = 0;
        public string? rawmat2 { get; set; } = null;
        public string? prductoff3 { get; set; } = null;
        public string? cusprod3 { get; set; } = null;
        public int? prod3 { get; set; } = 0;
        public int? serv { get; set; } = 0;
        public string? rawmat { get; set; } = null;
        public string? assetdes1 { get; set; } = null;
        public int? currentvalue1 { get; set; } = 0;
        public string? condition1 { get; set; } = null;
        public string? assetdes2 { get; set; } = null;
        public int? currentvalue2 { get; set; } = 0;
        public string? condition2 { get; set; } = null;
        public string? assetdes3 { get; set; } = null;
        public int? currentvalue3 { get; set; } = 0;
        public string? condition3 { get; set; } = null;
        public string? techcap { get; set; } = null;
        public string? valuechain1 { get; set; } = null;
        public string? locat1 { get; set; } = null;
        public int? funds1 { get; set; } = 0;
        public string? source1 { get; set; } = null;
        public int? periodacti1 { get; set; } = 0;
        public string? type1 { get; set; } = null;
        public string? valuechain2 { get; set; } = null;
        public string? locat2 { get; set; } = null;
        public int? funds2 { get; set; } = 0;
        public string? source2 { get; set; } = null;
        public int? periodacti2 { get; set; } = 0;
        public string? type2 { get; set; } = null;
        public string? valuechain3 { get; set; } = null;
        public string? locat3 { get; set; } = null;
        public int? funds3 { get; set; } = 0;
        public string? source3 { get; set; } = null;
        public int? periodacti3 { get; set; } = 0;
        public string? type3 { get; set; } = null;
        public string? modela { get; set; } = null;
        public string? genderenv { get; set; } = null;
        public string? sacpbene { get; set; } = null;
        public string? capacity { get; set; } = null;
        public string? plans { get; set; } = null;
        public string? procure { get; set; } = null;
        public string? fin { get; set; } = null;
        public string? org1 { get; set; } = null;
        public string? fon1 { get; set; } = null;
        public string? org2 { get; set; } = null;
        public string? fon2 { get; set; } = null;
        public string? org3 { get; set; } = null;
        public string? fon3 { get; set; } = null;
        public string? _version_ { get; set; } = null;
        public string? meta_instanceID { get; set; } = null;
        public string? _xform_id_string { get; set; } = null;
        public string? _uuid { get; set; } = null;
        public DateTime? _submission_time { get; set; } = null;
        public string? _submitted_by { get; set; } = null;
    }
}
