using Microsoft.EntityFrameworkCore;
using sacpapi.Models;

namespace sacpapi.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }


        public DbSet<AWPActivity> AWPActivities { get; set; }
        public DbSet<FieldRegisterBeneficiary> FieldRegister { get; set; }
        public DbSet<ActivityAttendant> ActivityAttendants { get; set; }
        public DbSet<MSMERegisterEnterprise> MSMERegister { get; set; }
        public DbSet<EOIBeneficiary> EOIBeneficiaries { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<vwBeneficiaries> vwBeneficiaries { get; set; }
        public DbSet<MSME> MSMEs { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<APGroup> APGroups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<Outcome> Outcomes { get; set; }
        public DbSet<Output> Outputs { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<GeneralActivity> GeneralActivities { get; set; }
        public DbSet<GeneralActivityParticipant> GeneralActivityParticipants { get; set; }
        public DbSet<Facilitator> Facilitators { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<MenuItem> Menu { get; set; }
        public DbSet<ProjectMenuItem> ProjectMenu { get; set; }
        public DbSet<StaffMember> StaffMembers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserUserGroup> UserUserGroups { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<FocusGroupDiscussion> FocusGroupDiscussions { get; set; }
        public DbSet<ProjectSite> ProjectSites { get; set; }
        public DbSet<District> luDistricts { get; set; }
        public DbSet<CoveredProvince> CoveredProvinces { get; set; }
        public DbSet<CoveredDistrict> CoveredDistricts { get; set; }
        public DbSet<CoveredWard> CoveredWards { get; set; }
        public DbSet<CoveredCluster> CoveredClusters { get; set; }
        public DbSet<IndicatorTarget> IndicatorTargets { get; set; }
        public DbSet<SubMenuPermission> SubMenuPermissions { get; set; }
        public DbSet<MainMenuPermission> MainMenuPermissions { get; set; }
        public DbSet<CommandLevelPermission> CommandLevelPermissions { get; set; }
        public DbSet<ActivityIndicator> ActivitiesIndicators { get; set; }
        public DbSet<Functionality> luFunctionalities { get; set; }
        public DbSet<IrrigationScheme> IrrigationSchemes { get; set; }
        public DbSet<WaterPoint> WaterPoints { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<FinancialRecord> FinancialRecords { get; set; }
        public DbSet<DocumentObject> DocumentsObjects { get; set; }
        public DbSet<ActivitySubComponent> ActivitiesSubComponent { get; set; }
        public DbSet<ClusterWard> ClusterWards { get; set; }
        public DbSet<Cluster> luClusters { get; set; }
        public DbSet<Province> luProvinces { get; set; }
        public DbSet<FilePermission> FilePermissions { get; set; }
        public DbSet<DisaggregationLabel> luDisaggregationLabels { get; set; }
        public DbSet<UnitOfMeasurement> luUnitsOfMeasurement { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<AuditTrail> AuditTrail { get; set; }

        public DbSet<MSEInfo> MSEInfos { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<ParticipantTemplateUploads> ParticipantTemplateUploads { get; set; }
        public DbSet<StakeParticipant> tblStakeParticipants { get; set; }

        public DbSet<VBUModel> VBU { get; set; }
        public DbSet<ActivityMapping> ActivityMappings { get; set; }
        public DbSet<SubActivity> SubActivities { get; set; }
        public DbSet<BeneficiaryV3> Beneficiaryv3 { get; set; }
         public DbSet<vwBenefeciariesUnion> vwBenefeciariesUnion { get; set; }
        public DbSet<StakeholderParticipants> StakeholderParticipants { get; set; }

        public DbSet<WaterUser> WaterUsers { get; set; }
        public DbSet<RoadUser> RoadUsers { get; set; }
        public DbSet<SchoolBusinessUnit> SchoolBusinessUnits { get; set; }
        public DbSet<EmploymentRecord> EmploymentRecords { get; set; }
        public DbSet<IrrigationSchemesDatabase> IrrigationScheme { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StakeParticipant>()
           .ToTable("tblStakeParticipants");

            modelBuilder.Entity<Participants>(entity =>
            {
                // 1. Map to the correct table name seen in your screenshot
                entity.ToTable("tblParticipants");

                // 2. Tell EF Core about the trigger to stop the "OUTPUT clause" error
                entity.ToTable(tb => tb.HasTrigger("trg_CleanParticipantIDs"));
            });

            modelBuilder.Entity<AWPActivity>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.SubComponentId)
                      .IsRequired();

                entity.Property(e => e.Description)
                      .IsRequired();
            });
            modelBuilder.Entity<IrrigationSchemesDatabase>(entity =>
            {
                entity.ToTable("tblrrigationSchemesDatabase");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Province)
                      .HasMaxLength(100);

                entity.Property(e => e.District)
                      .HasMaxLength(100);

                entity.Property(e => e.Ward)
                      .HasMaxLength(50);

                entity.Property(e => e.IrrigationScheme)
                      .HasMaxLength(150);

                entity.Property(e => e.FirstName)
                      .HasMaxLength(100);

                entity.Property(e => e.Surname)
                      .HasMaxLength(100);

                entity.Property(e => e.HouseholdIdentifier)
                      .HasMaxLength(100);

                entity.Property(e => e.GenderOfRegisteredPerson)
                      .HasMaxLength(10);

                entity.Property(e => e.YearOfBirth);

                entity.Property(e => e.YouthStatus)
                      .HasMaxLength(50);

                entity.Property(e => e.MaritalStatus)
                      .HasMaxLength(50);

                entity.Property(e => e.ContactNumber)
                      .HasMaxLength(20);

                entity.Property(e => e.GenderOfHouseholdHead)
                      .HasMaxLength(10);

                entity.Property(e => e.PwD)
                      .HasMaxLength(20);

                entity.Property(e => e.FamilySize);

                entity.Property(e => e.Male);

                entity.Property(e => e.Female);

                entity.Property(e => e.ImcPosition)
                      .HasMaxLength(100);

                entity.Property(e => e.LeadershipPosition)
                      .HasMaxLength(100);

                entity.Property(e => e.NameOfChairperson)
                      .HasMaxLength(100);

                entity.Property(e => e.ContactNumberOfChairperson)
                      .HasMaxLength(20);

                // ✅ Unique index on HouseholdIdentifier
                entity.HasIndex(e => e.HouseholdIdentifier)
                      .IsUnique();
            });
            modelBuilder.Entity<RoadUser>()
.HasIndex(r => r.NationalIdNumber)
.IsUnique();

            modelBuilder.Entity<SchoolBusinessUnit>()
                .HasIndex(s => new { s.NameOfSchoolBusinessUnit, s.District })
                .IsUnique();

            modelBuilder.Entity<EmploymentRecord>()
       .HasIndex(e => new { e.Province, e.District, e.Ward, e.Gender, e.Yob, e.PwDStatus })
       .IsUnique();


            modelBuilder.Entity<WaterUser>()
    .HasIndex(w => w.HouseholdIdentifier)
    .IsUnique();

            modelBuilder.Entity<StakeholderParticipants>(entity =>
            {
                entity.ToTable("StakeholderParticipants");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.NameOfParticipant)
                    .HasMaxLength(255)
                    .IsRequired(false);

                entity.Property(e => e.Sex)
                    .HasMaxLength(10)
                    .IsRequired(false);

                entity.Property(e => e.Organisation)
                    .HasMaxLength(255)
                    .IsRequired(false);

                entity.Property(e => e.Position)
                    .HasMaxLength(255)
                    .IsRequired(false);

                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(50)
                    .IsRequired(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(255)
                    .IsRequired(false);


                entity.Property(e => e.UploadedBy)
                    .HasMaxLength(255)
                    .IsRequired(false);

                entity.Property(e => e.UploadedDate)
                    .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<ActivityMapping>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ComponentId)
                      .IsRequired();

                entity.Property(e => e.SubComponentId)
                      .IsRequired();

                entity.Property(e => e.ActivityId)
                      .IsRequired();

                entity.Property(e => e.SubActivityId)
                      .IsRequired();
            });




            modelBuilder.Entity<SubActivity>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.SubActivityId)
                      .IsRequired(false); // nullable in SQL

                entity.Property(e => e.Description)
                      .IsRequired();

                entity.Property(e => e.ActivityId)
                      .IsRequired();
            });


    

            modelBuilder.Entity<VBUModel>()

                .HasKey(v => v.Id); // 
         

            modelBuilder.Entity<vwBenefeciariesUnion>().HasNoKey().ToView("vwBenefeciariesUnion");
            // Configure the vwBeneficiaries view as a keyless entity
            modelBuilder.Entity<vwBeneficiaries>()
                .HasNoKey()
                .ToView("vwBeneficiaries"); // This should match the view name in your database

// This should match the view name in your database


        }
    }
}
