using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyAvana.Models.Entities;
using MyAvanaApi.Models.Entities;
using System;

namespace MyAvana.DAL.Auth
{
    public class AvanaContext : IdentityDbContext<UserEntity, UserRoleEntity, Guid>
    {
        public AvanaContext(DbContextOptions<AvanaContext> options)
           : base(options)
        {
            // Database.EnsureCreated();
            //Database.SetCommandTimeout(9000);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GenericSetting>()
                .HasKey(c => new { c.SettingID, c.AdminAccountId, c.SettingName, c.SubSettingName });
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<UserEmails> UserEmails { get; set; }
        public virtual DbSet<UsersCode> Codes { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<GenericSetting> GenericSettings { get; set; }
        public virtual DbSet<PromoCode> PromoCodes { get; set; }
        public virtual DbSet<UserHistory> UserHistories { get; set; }
        public virtual DbSet<ProductEntity> ProductEntities { get; set; }
        public virtual DbSet<SubscriptionsEntity> SubscriptionsEntities { get; set; }
        public virtual DbSet<PaymentEntity> PaymentEntities { get; set; }
        public virtual DbSet<CodeEntity> CodeEntities { get; set; }
        public virtual DbSet<MediaLinkEntity> MediaLinkEntities { get; set; }
        public virtual DbSet<UsersTicketsEntity> UsersTicketsEntities { get; set; }
        public virtual DbSet<BlogArticle> BlogArticles { get; set; }
        public virtual DbSet<WebLogin> WebLogins { get; set; }
        public virtual DbSet<IngedientsEntity> IngedientsEntities { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Regimens> Regimens { get; set; }
        public virtual DbSet<RegimenSteps> RegimenSteps { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Questionaire> Questionaires { get; set; }
        public virtual DbSet<Elasticity> Elasticities { get; set; }
        public virtual DbSet<HairElasticity> HairElasticities { get; set; }
        public virtual DbSet<HairObservation> HairObservations { get; set; }
        public virtual DbSet<HairPorosity> HairPorosities { get; set; }
        public virtual DbSet<HairProfile> HairProfiles { get; set; }
        public virtual DbSet<HairStrands> HairStrands { get; set; }
        public virtual DbSet<Observation> Observations { get; set; }
        public virtual DbSet<Pororsity> Pororsities { get; set; }
        public virtual DbSet<RecommendedIngredients> RecommendedIngredients { get; set; }
        public virtual DbSet<RecommendedProducts> RecommendedProducts { get; set; }
        public virtual DbSet<RecommendedRegimens> RecommendedRegimens { get; set; }
        public virtual DbSet<RecommendedVideos> RecommendedVideos { get; set; }
        public virtual DbSet<Stylist> Stylists { get; set; }
        public virtual DbSet<StylistSpecialty> StylistSpecialties { get; set; }
        public virtual DbSet<StylishCommon> StylishCommons { get; set; }
        public virtual DbSet<HairStrandsImages> HairStrandsImages { get; set; }
        public virtual DbSet<Health> Healths { get; set; }
        public virtual DbSet<HairHealth> HairHealths { get; set; }
        public virtual DbSet<ObsElasticity> ObsElasticities { get; set; }
        public virtual DbSet<ObsChemicalProducts> ObsChemicalProducts { get; set; }
        public virtual DbSet<ObsPhysicalProducts> ObsPhysicalProducts { get; set; }
        public virtual DbSet<ObsDamage> ObsDamage { get; set; }
        public virtual DbSet<ObsBreakage> ObsBreakage { get; set; }
        public virtual DbSet<ObsSplitting> ObsSplitting { get; set; }
        public virtual DbSet<HairType> HairTypes { get; set; }
        public virtual DbSet<HairChallenges> HairChallenges { get; set; }
        public virtual DbSet<ProductIndicator> ProductIndicator { get; set; }
        public virtual DbSet<ProductTags> ProductTags { get; set; }
        public virtual DbSet<ProductClassification> ProductClassification { get; set; }
        public virtual DbSet<ProductCommon> ProductCommons { get; set; }
        public virtual DbSet<ProductTypeCategory> ProductTypeCategories { get; set; }
        public virtual DbSet<RecommendedProductsStylingRegimen> RecommendedProductsStyleRegimens { get; set; }
        public virtual DbSet<RecommendedStylist> RecommendedStylists { get; set; }
        public virtual DbSet<GroupPost> GroupPosts { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<LikePosts> LikePosts { get; set; }
        public virtual DbSet<VideoCategory> VideoCategories { get; set; }
        public virtual DbSet<CustomerHairGoals> CustomerHairGoals { get; set; }
        public virtual DbSet<AdditionalHairInfo> AdditionalHairInfo { get; set; }
        public virtual DbSet<CustomerHairChallenge> CustomerHairChallenge { get; set; }
        public virtual DbSet<DailyRoutineTracker> DailyRoutineTracker { get; set; }
        public virtual DbSet<Tools> Tools { get; set; }
        public virtual DbSet<TrackingDetails> TrackingDetails { get; set; }
        public virtual DbSet<HairStyles> HairStyles { get; set; }
        public virtual DbSet<DailyRoutineProducts> DailyRoutineProducts { get; set; }
        public virtual DbSet<DailyRoutineIngredients> DailyRoutineIngredients { get; set; }
        public virtual DbSet<DailyRoutineRegimens> DailyRoutineRegimens { get; set; }
        public virtual DbSet<DailyRoutineHairStyles> DailyRoutineHairStyles { get; set; }
        public virtual DbSet<RecommendedTools> RecommendedTools { get; set; }
        public virtual DbSet<RecommendedCategory> RecommendedCategories { get; set; }
        public virtual DbSet<RecommendedProductTypes> RecommendedProductTypes { get; set; }
        public virtual DbSet<CustomerMessage> CustomerMessage { get; set; }
        public virtual DbSet<PrePopulateTypes> PrePopulateTypes { get; set; }
        public virtual DbSet<LiveConsultationStatus> LiveConsultationStatus { get; set; }
        public virtual DbSet<LiveConsultationUserDetails> LiveConsultationUserDetails { get; set; }
        public virtual DbSet<LiveConsultationCustomer> LiveConsultationCustomer { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<ScheduleTime> ScheduleTimes { get; set; }
        public virtual DbSet<TimeZones> TimeZones { get; set; }
        public virtual DbSet<UserEntity> UserEntity { get; set; }
    }
}