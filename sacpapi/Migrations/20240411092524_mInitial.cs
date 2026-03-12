using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sacpapi.Migrations
{
    /// <inheritdoc />
    public partial class mInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameInShort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutputId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivitiesIndicators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    IndicatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitiesIndicators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivitiesSubComponent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubComponentId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitiesSubComponent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityAttendants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KoboBeneficiaryId = table.Column<int>(type: "int", nullable: false),
                    FormhubUuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enumerator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseholdHead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseholdHeadSex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaInstanceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XFormIdString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAttendants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "APGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    district = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nameapg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    point = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    namevil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    yearest = table.Column<int>(type: "int", nullable: false),
                    nointeger = table.Column<int>(type: "int", nullable: false),
                    females = table.Column<int>(type: "int", nullable: false),
                    males = table.Column<int>(type: "int", nullable: false),
                    youth = table.Column<int>(type: "int", nullable: false),
                    youth_001 = table.Column<int>(type: "int", nullable: false),
                    subsector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valuechain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    enumerator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    chair = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age = table.Column<int>(type: "int", nullable: false),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_001 = table.Column<int>(type: "int", nullable: false),
                    category_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_002 = table.Column<int>(type: "int", nullable: false),
                    category_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ward_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_003 = table.Column<int>(type: "int", nullable: false),
                    category_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    treas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_004 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_004 = table.Column<int>(type: "int", nullable: false),
                    category_004 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_005 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_005 = table.Column<int>(type: "int", nullable: false),
                    category_005 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_006 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_006 = table.Column<int>(type: "int", nullable: false),
                    category_006 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_007 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_007 = table.Column<int>(type: "int", nullable: false),
                    category_007 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_008 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_008 = table.Column<int>(type: "int", nullable: false),
                    category_008 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_004 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_009 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_009 = table.Column<int>(type: "int", nullable: false),
                    category_009 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_005 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_010 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_010 = table.Column<int>(type: "int", nullable: false),
                    category_010 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_006 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_011 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_011 = table.Column<int>(type: "int", nullable: false),
                    category_011 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_007 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_012 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_012 = table.Column<int>(type: "int", nullable: false),
                    category_012 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_008 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_013 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_013 = table.Column<int>(type: "int", nullable: false),
                    category_013 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_009 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_014 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_014 = table.Column<int>(type: "int", nullable: false),
                    category_014 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_010 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_015 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_015 = table.Column<int>(type: "int", nullable: false),
                    category_015 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_011 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_016 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_016 = table.Column<int>(type: "int", nullable: false),
                    category_016 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_012 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_017 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_017 = table.Column<int>(type: "int", nullable: false),
                    category_017 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_013 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_018 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_018 = table.Column<int>(type: "int", nullable: false),
                    category_018 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_014 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_019 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_019 = table.Column<int>(type: "int", nullable: false),
                    category_019 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_015 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_020 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_020 = table.Column<int>(type: "int", nullable: false),
                    category_020 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_016 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_021 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_021 = table.Column<int>(type: "int", nullable: false),
                    category_021 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_017 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_022 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_022 = table.Column<int>(type: "int", nullable: false),
                    category_022 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_018 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_023 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_023 = table.Column<int>(type: "int", nullable: false),
                    category_023 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_019 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_024 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_024 = table.Column<int>(type: "int", nullable: false),
                    category_024 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_020 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_025 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_025 = table.Column<int>(type: "int", nullable: false),
                    category_025 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_021 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_026 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_026 = table.Column<int>(type: "int", nullable: false),
                    category_026 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_022 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_027 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_027 = table.Column<int>(type: "int", nullable: false),
                    category_027 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_023 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_028 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_028 = table.Column<int>(type: "int", nullable: false),
                    category_028 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_024 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_029 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_029 = table.Column<int>(type: "int", nullable: false),
                    category_029 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_025 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_030 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_030 = table.Column<int>(type: "int", nullable: false),
                    category_030 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_026 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_031 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_031 = table.Column<int>(type: "int", nullable: false),
                    category_031 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_027 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_032 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_032 = table.Column<int>(type: "int", nullable: false),
                    category_032 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_028 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_033 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_033 = table.Column<int>(type: "int", nullable: false),
                    category_033 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_029 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_034 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_034 = table.Column<int>(type: "int", nullable: false),
                    category_034 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _id = table.Column<int>(type: "int", nullable: false),
                    _uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _submission_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _submitted_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    __version__ = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Beneficiaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KoboBeneficiaryId = table.Column<int>(type: "int", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    FarmerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseholdHeadSex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationshipToHead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtensionOfficerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumHouseholdMembers = table.Column<int>(type: "int", nullable: true),
                    NumFemalesUnder5 = table.Column<int>(type: "int", nullable: true),
                    NumMalesUnder5 = table.Column<int>(type: "int", nullable: true),
                    NumFemales5To18 = table.Column<int>(type: "int", nullable: true),
                    NumMales5To18 = table.Column<int>(type: "int", nullable: true),
                    NumFemales18To35 = table.Column<int>(type: "int", nullable: true),
                    NumMales18To35 = table.Column<int>(type: "int", nullable: true),
                    NumFemalesOver35 = table.Column<int>(type: "int", nullable: true),
                    NumMalesOver35 = table.Column<int>(type: "int", nullable: true),
                    NumHouseholdLabourers = table.Column<int>(type: "int", nullable: true),
                    Disabled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaterSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalArableLandHa = table.Column<double>(type: "float", nullable: true),
                    TotalProductionArea = table.Column<double>(type: "float", nullable: true),
                    CropsGrown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainCropGrown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalHarvestMainCrop = table.Column<double>(type: "float", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CSATechnologiesUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToIrrigation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoodInsecurityPastYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthsFoodInsecurity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberOfAgricultureGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembershipPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanContributeToProject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnsCattle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CattleNo = table.Column<int>(type: "int", nullable: true),
                    OwnsDonkeys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonkeysNo = table.Column<int>(type: "int", nullable: true),
                    OwnsGoats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoatsNo = table.Column<int>(type: "int", nullable: true),
                    OwnsSheep = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SheepNo = table.Column<int>(type: "int", nullable: true),
                    OwnsPoultry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoultryNo = table.Column<int>(type: "int", nullable: true),
                    MainHouseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToFinance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemittancesAmount = table.Column<int>(type: "int", nullable: true),
                    FoodCropSalesAmount = table.Column<int>(type: "int", nullable: true),
                    CashCropSalesAmount = table.Column<int>(type: "int", nullable: true),
                    VegetableSalesAmount = table.Column<int>(type: "int", nullable: true),
                    CasualLabourIncome = table.Column<int>(type: "int", nullable: true),
                    LivestockSalesIncome = table.Column<int>(type: "int", nullable: true),
                    LivestockProductsIncome = table.Column<int>(type: "int", nullable: true),
                    SkilledTradeIncome = table.Column<int>(type: "int", nullable: true),
                    OwnBusinessIncome = table.Column<int>(type: "int", nullable: true),
                    PensionIncome = table.Column<int>(type: "int", nullable: true),
                    FormalSalariesIncome = table.Column<int>(type: "int", nullable: true),
                    FishingIncome = table.Column<int>(type: "int", nullable: true),
                    SmallScaleMiningIncome = table.Column<int>(type: "int", nullable: true),
                    GovSocialTransfersIncome = table.Column<int>(type: "int", nullable: true),
                    NonStateSocialTransfersIncome = table.Column<int>(type: "int", nullable: true),
                    CrossBorderTradeIncome = table.Column<int>(type: "int", nullable: true),
                    RentalsIncome = table.Column<int>(type: "int", nullable: true),
                    OtherIncomeSources = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherIncomeAmount = table.Column<int>(type: "int", nullable: true),
                    WillingToParticipateInTrainings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvgCultivatedLandArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumHouseholdLaborersForAgri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainCropAvgProduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvgMonthlyIncomePerHousehold = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FarmerCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeneficialTrainings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormHubUuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Today = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaInstanceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XFormIdString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClusterWards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClusterId = table.Column<int>(type: "int", nullable: false),
                    WardCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusterWards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommandLevelPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommandId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandLevelPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoveredClusters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ClusterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoveredClusters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoveredDistricts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoveredDistricts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoveredProvinces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoveredProvinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoveredWards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    WardCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoveredWards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentsObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    ObjectType = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentsObjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EOIBeneficiaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KoboBeneficiaryId = table.Column<int>(type: "int", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    FarmerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseholdHeadSex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationshipToHead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtensionOfficerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumHouseholdMembers = table.Column<int>(type: "int", nullable: true),
                    NumFemalesUnder5 = table.Column<int>(type: "int", nullable: true),
                    NumMalesUnder5 = table.Column<int>(type: "int", nullable: true),
                    NumFemales5To18 = table.Column<int>(type: "int", nullable: true),
                    NumMales5To18 = table.Column<int>(type: "int", nullable: true),
                    NumFemales18To35 = table.Column<int>(type: "int", nullable: true),
                    NumMales18To35 = table.Column<int>(type: "int", nullable: true),
                    NumFemalesOver35 = table.Column<int>(type: "int", nullable: true),
                    NumMalesOver35 = table.Column<int>(type: "int", nullable: true),
                    NumHouseholdLabourers = table.Column<int>(type: "int", nullable: true),
                    Disabled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaterSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalArableLandHa = table.Column<double>(type: "float", nullable: true),
                    TotalProductionArea = table.Column<double>(type: "float", nullable: true),
                    CropsGrown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainCropGrown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalHarvestMainCrop = table.Column<double>(type: "float", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CSATechnologiesUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToIrrigation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoodInsecurityPastYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthsFoodInsecurity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberOfAgricultureGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembershipPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanContributeToProject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnsCattle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CattleNo = table.Column<int>(type: "int", nullable: true),
                    OwnsDonkeys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonkeysNo = table.Column<int>(type: "int", nullable: true),
                    OwnsGoats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoatsNo = table.Column<int>(type: "int", nullable: true),
                    OwnsSheep = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SheepNo = table.Column<int>(type: "int", nullable: true),
                    OwnsPoultry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoultryNo = table.Column<int>(type: "int", nullable: true),
                    MainHouseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToFinance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemittancesAmount = table.Column<int>(type: "int", nullable: true),
                    FoodCropSalesAmount = table.Column<int>(type: "int", nullable: true),
                    CashCropSalesAmount = table.Column<int>(type: "int", nullable: true),
                    VegetableSalesAmount = table.Column<int>(type: "int", nullable: true),
                    CasualLabourIncome = table.Column<int>(type: "int", nullable: true),
                    LivestockSalesIncome = table.Column<int>(type: "int", nullable: true),
                    LivestockProductsIncome = table.Column<int>(type: "int", nullable: true),
                    SkilledTradeIncome = table.Column<int>(type: "int", nullable: true),
                    OwnBusinessIncome = table.Column<int>(type: "int", nullable: true),
                    PensionIncome = table.Column<int>(type: "int", nullable: true),
                    FormalSalariesIncome = table.Column<int>(type: "int", nullable: true),
                    FishingIncome = table.Column<int>(type: "int", nullable: true),
                    SmallScaleMiningIncome = table.Column<int>(type: "int", nullable: true),
                    GovSocialTransfersIncome = table.Column<int>(type: "int", nullable: true),
                    NonStateSocialTransfersIncome = table.Column<int>(type: "int", nullable: true),
                    CrossBorderTradeIncome = table.Column<int>(type: "int", nullable: true),
                    RentalsIncome = table.Column<int>(type: "int", nullable: true),
                    OtherIncomeSources = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherIncomeAmount = table.Column<int>(type: "int", nullable: true),
                    WillingToParticipateInTrainings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvgCultivatedLandArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumHouseholdLaborersForAgri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainCropAvgProduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvgMonthlyIncomePerHousehold = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FarmerCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeneficialTrainings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormHubUuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Today = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaInstanceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XFormIdString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EOIBeneficiaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facilitators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilitatorId = table.Column<int>(type: "int", nullable: false),
                    GeneralActivityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilitators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldRegister",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KoboBeneficiaryId = table.Column<int>(type: "int", nullable: false),
                    FormhubUuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enumerator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseholdHead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseholdHeadSex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaInstanceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XFormIdString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldRegister", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilePermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorOrganization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplySecurity = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionedBy = table.Column<int>(type: "int", nullable: true),
                    ToBeActionedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ObjectTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FocusGroupDiscussions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    today = table.Column<DateTime>(type: "datetime2", nullable: false),
                    point = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    enumerator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    district = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    village = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_of_the_Village_2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_of_the_Village_3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_of_the_Village_4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_of_the_Village_5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_of_the_Village_6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_of_the_Village_7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_of_the_Village_8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_of_the_Village_9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_of_the_Village_10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    noparticipants = table.Column<int>(type: "int", nullable: false),
                    youth = table.Column<int>(type: "int", nullable: false),
                    youth_001 = table.Column<int>(type: "int", nullable: false),
                    females = table.Column<int>(type: "int", nullable: false),
                    males = table.Column<int>(type: "int", nullable: false),
                    disability = table.Column<int>(type: "int", nullable: false),
                    intro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallirr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size_of_irrigation_scheme_in_village_1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallirr_009 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallirr_008 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallirr_007 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallirr_006 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallirr_005 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallirr_004 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallirr_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallirr_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallirr_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numberirr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nameirr1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nameirr2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nameirr3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sizeirr1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sizeirr2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sizeirr3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_piped_surface_open_channel_flow__pumped_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_overhead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_localised = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_na = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    otherirr1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_001_piped_surface_open_channel_flow__pumped_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_001_overhead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_001_localised = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_001_na = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_001_other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    otherirr2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_002_piped_surface_open_channel_flow__pumped_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_002_overhead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_002_localised = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_002_na = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    systemused_002_other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    otherirr3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    challengs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    challengs_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    challengs_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_government = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_accessinpu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_mktaccess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_irrinfra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_wateravail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_n_a = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_otherchale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_001_government = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_001_accessinpu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_001_mktaccess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_001_irrinfra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_001_wateravail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_001_n_a = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_001_otherchale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_002_government = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_002_accessinpu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_002_mktaccess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_002_irrinfra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_002_wateravail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_002_n_a = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatchale_002_otherchale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    feeder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    road = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    road_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    road_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    points = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    domuse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    domuse_taps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    domuse_borehole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    domuse_wells = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    domuse_river = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    domuse_otherdomeuse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    otherdomuse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rainfall = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reliability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sani = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ablution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ablution_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ablution_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ablution_001_007 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ablution_001_006 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ablution_001_005 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ablution_001_004 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ablution_001_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ablution_001_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ablution_001_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    otherablution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    env = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_007 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_007_gullies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_007_minepit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_007_otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environ_009 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_gullies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_minepit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environ_008 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_003_gullies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_003_minepit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_003_otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environ_007 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_009 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_009_gullies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_009_minepit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_009_otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environ_006 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_002_gullies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_002_minepit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_002_otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environ_005 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_008 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_008_gullies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_008_minepit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_008_otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environ_004 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_006 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_006_gullies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_006_minepit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_006_otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environ_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_005 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_005_gullies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_005_minepit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_005_otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environ_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_004 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_004_gullies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_004_minepit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_004_otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    environ_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_001_gullies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_001_minepit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issues_001_otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    otherissues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ngo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    conl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    This_note_can_be_read_out_loud = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Youth_group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    redressing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rankneeds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suggestedneeds_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Female_group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    redressing_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rankneeds_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suggestedneeds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Male_Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    redressing_001_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rankneeds_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    suggestedneeds_001_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    road_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _id = table.Column<int>(type: "int", nullable: false),
                    _uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _submission_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _submitted_by = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FocusGroupDiscussions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    SubComponentId = table.Column<int>(type: "int", nullable: false),
                    InterventionId = table.Column<int>(type: "int", nullable: false),
                    ActivityTypeId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    ActiveStatusId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    Site = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaterpointId = table.Column<int>(type: "int", nullable: false),
                    IrrigationSchemeId = table.Column<int>(type: "int", nullable: false),
                    LeadFacilitatorId = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuesComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralActivityParticipants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralActivityId = table.Column<int>(type: "int", nullable: false),
                    ParticipantTypeId = table.Column<int>(type: "int", nullable: false),
                    ParticipantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralActivityParticipants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    BeneficiaryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    district = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nameapg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    point = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    namevil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    yearest = table.Column<int>(type: "int", nullable: false),
                    nointeger = table.Column<int>(type: "int", nullable: false),
                    females = table.Column<int>(type: "int", nullable: false),
                    males = table.Column<int>(type: "int", nullable: false),
                    youth = table.Column<int>(type: "int", nullable: false),
                    youth_001 = table.Column<int>(type: "int", nullable: false),
                    subsector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valuechain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    enumerator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    chair = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age = table.Column<int>(type: "int", nullable: false),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_001 = table.Column<int>(type: "int", nullable: false),
                    category_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_002 = table.Column<int>(type: "int", nullable: false),
                    category_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ward_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_003 = table.Column<int>(type: "int", nullable: false),
                    category_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    treas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_004 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_004 = table.Column<int>(type: "int", nullable: false),
                    category_004 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_005 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_005 = table.Column<int>(type: "int", nullable: false),
                    category_005 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_001 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_006 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_006 = table.Column<int>(type: "int", nullable: false),
                    category_006 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_002 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_007 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_007 = table.Column<int>(type: "int", nullable: false),
                    category_007 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_003 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_008 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_008 = table.Column<int>(type: "int", nullable: false),
                    category_008 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_004 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_009 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_009 = table.Column<int>(type: "int", nullable: false),
                    category_009 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_005 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_010 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_010 = table.Column<int>(type: "int", nullable: false),
                    category_010 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_006 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_011 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_011 = table.Column<int>(type: "int", nullable: false),
                    category_011 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_007 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_012 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_012 = table.Column<int>(type: "int", nullable: false),
                    category_012 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_008 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_013 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_013 = table.Column<int>(type: "int", nullable: false),
                    category_013 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_009 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_014 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_014 = table.Column<int>(type: "int", nullable: false),
                    category_014 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_010 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_015 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_015 = table.Column<int>(type: "int", nullable: false),
                    category_015 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_011 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_016 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_016 = table.Column<int>(type: "int", nullable: false),
                    category_016 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_012 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_017 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_017 = table.Column<int>(type: "int", nullable: false),
                    category_017 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_013 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_018 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_018 = table.Column<int>(type: "int", nullable: false),
                    category_018 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_014 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_019 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_019 = table.Column<int>(type: "int", nullable: false),
                    category_019 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_015 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_020 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_020 = table.Column<int>(type: "int", nullable: false),
                    category_020 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_016 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_021 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_021 = table.Column<int>(type: "int", nullable: false),
                    category_021 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_017 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_022 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_022 = table.Column<int>(type: "int", nullable: false),
                    category_022 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_018 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_023 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_023 = table.Column<int>(type: "int", nullable: false),
                    category_023 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_019 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_024 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_024 = table.Column<int>(type: "int", nullable: false),
                    category_024 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_020 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_025 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_025 = table.Column<int>(type: "int", nullable: false),
                    category_025 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_021 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_026 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_026 = table.Column<int>(type: "int", nullable: false),
                    category_026 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_022 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_027 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_027 = table.Column<int>(type: "int", nullable: false),
                    category_027 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_023 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_028 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_028 = table.Column<int>(type: "int", nullable: false),
                    category_028 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_024 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_029 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_029 = table.Column<int>(type: "int", nullable: false),
                    category_029 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_025 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_030 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_030 = table.Column<int>(type: "int", nullable: false),
                    category_030 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_026 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_031 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_031 = table.Column<int>(type: "int", nullable: false),
                    category_031 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_027 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_032 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_032 = table.Column<int>(type: "int", nullable: false),
                    category_032 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_028 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_033 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_033 = table.Column<int>(type: "int", nullable: false),
                    category_033 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    group_029 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex_034 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_034 = table.Column<int>(type: "int", nullable: false),
                    category_034 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _id = table.Column<int>(type: "int", nullable: false),
                    _uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _submission_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _submitted_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    __version__ = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Indicators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndicatorTypeId = table.Column<int>(type: "int", nullable: false),
                    IndicatorCategoryId = table.Column<int>(type: "int", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false),
                    OutcomeId = table.Column<int>(type: "int", nullable: false),
                    OutputId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Definition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitOfMeasurementId = table.Column<int>(type: "int", nullable: false),
                    BaselineValue = table.Column<int>(type: "int", nullable: false),
                    DataSourceId = table.Column<int>(type: "int", nullable: false),
                    DataCollectionMethodId = table.Column<int>(type: "int", nullable: false),
                    ToolId = table.Column<int>(type: "int", nullable: false),
                    DataCollectionFrequencyId = table.Column<int>(type: "int", nullable: false),
                    ResponsibleParty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgramTargetValue = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndicatorTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndicatorId = table.Column<int>(type: "int", nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinancialYear = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<int>(type: "int", nullable: false),
                    Achievement = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicatorTargets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IrrigationSchemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgroEcologicalRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchemeManagementModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateEstablished = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalDevelopedAreaToDate = table.Column<double>(type: "float", nullable: true),
                    AreaUnderIrrigation = table.Column<double>(type: "float", nullable: true),
                    PotentialAreaOfScheme = table.Column<double>(type: "float", nullable: true),
                    IrrigationSchemeStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    NumberOfIndividuals = table.Column<int>(type: "int", nullable: true),
                    Women = table.Column<int>(type: "int", nullable: true),
                    Men = table.Column<int>(type: "int", nullable: true),
                    Youth = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IrrigationSchemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "luClusters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_luClusters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "luDisaggregationLabels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitOfMeasurement = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_luDisaggregationLabels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "luDistricts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_luDistricts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "luFunctionalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_luFunctionalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "luProvinces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_luProvinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "luUnitsOfMeasurement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_luUnitsOfMeasurement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MainMenuPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainMenuPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowMenu = table.Column<bool>(type: "bit", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MSMERegister",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    koboEnterpriseId = table.Column<int>(type: "int", nullable: false),
                    formhub_uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    today = table.Column<DateTime>(type: "datetime2", nullable: true),
                    start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    district = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    buscenter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    enumerator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nameofbus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tradingname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    namecontact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    legalstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    yearest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    staffno = table.Column<int>(type: "int", nullable: true),
                    femalesno = table.Column<int>(type: "int", nullable: true),
                    maleno = table.Column<int>(type: "int", nullable: true),
                    youthno = table.Column<int>(type: "int", nullable: true),
                    annualbudget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    networks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nobranches = table.Column<int>(type: "int", nullable: true),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prductoff1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cusprod1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prod1 = table.Column<int>(type: "int", nullable: true),
                    serv1 = table.Column<int>(type: "int", nullable: true),
                    rawmat1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prductoff2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cusprod2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prod2 = table.Column<int>(type: "int", nullable: true),
                    serv2 = table.Column<int>(type: "int", nullable: true),
                    rawmat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prductoff3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cusprod3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prod3 = table.Column<int>(type: "int", nullable: true),
                    serv = table.Column<int>(type: "int", nullable: true),
                    rawmat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    assetdes1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentvalue1 = table.Column<int>(type: "int", nullable: true),
                    condition1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    assetdes2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentvalue2 = table.Column<int>(type: "int", nullable: true),
                    condition2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    assetdes3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentvalue3 = table.Column<int>(type: "int", nullable: true),
                    condition3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    techcap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valuechain1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    locat1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    funds1 = table.Column<int>(type: "int", nullable: true),
                    source1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    periodacti1 = table.Column<int>(type: "int", nullable: true),
                    type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valuechain2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    locat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    funds2 = table.Column<int>(type: "int", nullable: true),
                    source2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    periodacti2 = table.Column<int>(type: "int", nullable: true),
                    type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valuechain3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    locat3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    funds3 = table.Column<int>(type: "int", nullable: true),
                    source3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    periodacti3 = table.Column<int>(type: "int", nullable: true),
                    type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modela = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    genderenv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sacpbene = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    capacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    plans = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    procure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    org1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fon1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    org2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fon2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    org3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fon3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _version_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    meta_instanceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _xform_id_string = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _submission_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    _submitted_by = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSMERegister", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MSMEs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    koboEnterpriseId = table.Column<int>(type: "int", nullable: false),
                    formhub_uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    today = table.Column<DateTime>(type: "datetime2", nullable: true),
                    start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end = table.Column<DateTime>(type: "datetime2", nullable: true),
                    province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    district = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    buscenter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    enumerator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nameofbus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tradingname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    namecontact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    legalstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    yearest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    staffno = table.Column<int>(type: "int", nullable: true),
                    femalesno = table.Column<int>(type: "int", nullable: true),
                    maleno = table.Column<int>(type: "int", nullable: true),
                    youthno = table.Column<int>(type: "int", nullable: true),
                    annualbudget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    networks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nobranches = table.Column<int>(type: "int", nullable: true),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prductoff1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cusprod1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prod1 = table.Column<int>(type: "int", nullable: true),
                    serv1 = table.Column<int>(type: "int", nullable: true),
                    rawmat1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prductoff2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cusprod2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prod2 = table.Column<int>(type: "int", nullable: true),
                    serv2 = table.Column<int>(type: "int", nullable: true),
                    rawmat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prductoff3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cusprod3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prod3 = table.Column<int>(type: "int", nullable: true),
                    serv = table.Column<int>(type: "int", nullable: true),
                    rawmat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    assetdes1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentvalue1 = table.Column<int>(type: "int", nullable: true),
                    condition1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    assetdes2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentvalue2 = table.Column<int>(type: "int", nullable: true),
                    condition2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    assetdes3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currentvalue3 = table.Column<int>(type: "int", nullable: true),
                    condition3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    techcap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valuechain1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    locat1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    funds1 = table.Column<int>(type: "int", nullable: true),
                    source1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    periodacti1 = table.Column<int>(type: "int", nullable: true),
                    type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valuechain2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    locat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    funds2 = table.Column<int>(type: "int", nullable: true),
                    source2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    periodacti2 = table.Column<int>(type: "int", nullable: true),
                    type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valuechain3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    locat3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    funds3 = table.Column<int>(type: "int", nullable: true),
                    source3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    periodacti3 = table.Column<int>(type: "int", nullable: true),
                    type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modela = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    genderenv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sacpbene = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    capacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    plans = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    procure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    org1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fon1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    org2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fon2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    org3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fon3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _version_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    meta_instanceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _xform_id_string = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _uuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _submission_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    _submitted_by = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSMEs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Objectives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objectives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNo = table.Column<int>(type: "int", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outcomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outcomes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutcomeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outputs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMenu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowMenu = table.Column<bool>(type: "bit", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Acronym = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActiveStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalGoalStatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EvaluationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalProjectBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BeneficiaryDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StakeholderDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetedNoOfDirectBeneficiaries = table.Column<int>(type: "int", nullable: false),
                    TargetedNoOfGroups = table.Column<int>(type: "int", nullable: false),
                    TargetedNoOfMsmes = table.Column<int>(type: "int", nullable: false),
                    Men = table.Column<int>(type: "int", nullable: false),
                    Women = table.Column<int>(type: "int", nullable: false),
                    Youth = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaffMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalIdNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubMenuPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubMenuPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffMemberId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailedPasswordAttemptCount = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsLockedOut = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastActivityDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLockoutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPasswordChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordQuestionAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordExpires = table.Column<bool>(type: "bit", nullable: true),
                    PasswordExpirationDays = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserUserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaterPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    NumberOfHouseholds = table.Column<int>(type: "int", nullable: true),
                    NumberOfIndividuals = table.Column<int>(type: "int", nullable: true),
                    Men = table.Column<int>(type: "int", nullable: true),
                    Women = table.Column<int>(type: "int", nullable: true),
                    Youth = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterPoints", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "ActivitiesIndicators");

            migrationBuilder.DropTable(
                name: "ActivitiesSubComponent");

            migrationBuilder.DropTable(
                name: "ActivityAttendants");

            migrationBuilder.DropTable(
                name: "APGroups");

            migrationBuilder.DropTable(
                name: "AuditTrail");

            migrationBuilder.DropTable(
                name: "Beneficiaries");

            migrationBuilder.DropTable(
                name: "ClusterWards");

            migrationBuilder.DropTable(
                name: "CommandLevelPermissions");

            migrationBuilder.DropTable(
                name: "CoveredClusters");

            migrationBuilder.DropTable(
                name: "CoveredDistricts");

            migrationBuilder.DropTable(
                name: "CoveredProvinces");

            migrationBuilder.DropTable(
                name: "CoveredWards");

            migrationBuilder.DropTable(
                name: "DocumentsObjects");

            migrationBuilder.DropTable(
                name: "EOIBeneficiaries");

            migrationBuilder.DropTable(
                name: "Facilitators");

            migrationBuilder.DropTable(
                name: "FieldRegister");

            migrationBuilder.DropTable(
                name: "FilePermissions");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "FocusGroupDiscussions");

            migrationBuilder.DropTable(
                name: "GeneralActivities");

            migrationBuilder.DropTable(
                name: "GeneralActivityParticipants");

            migrationBuilder.DropTable(
                name: "GroupMembers");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Indicators");

            migrationBuilder.DropTable(
                name: "IndicatorTargets");

            migrationBuilder.DropTable(
                name: "IrrigationSchemes");

            migrationBuilder.DropTable(
                name: "luClusters");

            migrationBuilder.DropTable(
                name: "luDisaggregationLabels");

            migrationBuilder.DropTable(
                name: "luDistricts");

            migrationBuilder.DropTable(
                name: "luFunctionalities");

            migrationBuilder.DropTable(
                name: "luProvinces");

            migrationBuilder.DropTable(
                name: "luUnitsOfMeasurement");

            migrationBuilder.DropTable(
                name: "MainMenuPermissions");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "MSMERegister");

            migrationBuilder.DropTable(
                name: "MSMEs");

            migrationBuilder.DropTable(
                name: "Objectives");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.DropTable(
                name: "Outcomes");

            migrationBuilder.DropTable(
                name: "Outputs");

            migrationBuilder.DropTable(
                name: "ProjectMenu");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectSites");

            migrationBuilder.DropTable(
                name: "StaffMembers");

            migrationBuilder.DropTable(
                name: "SubMenuPermissions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserUserGroups");

            migrationBuilder.DropTable(
                name: "WaterPoints");
        }
    }
}
