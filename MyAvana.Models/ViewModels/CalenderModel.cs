using Microsoft.AspNetCore.Http;
using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class HairCareParts
    {
        public string Image { get; set; }
        public List<RoutineProducts> routineProducts { get; set; }
        public List<RoutineProducts> selectedProducts { get; set; }
        public List<RoutineIngredients> routineIngredients { get; set; }
        public List<RoutineIngredients> selectedIngredients { get; set; }
        public List<RoutineRegimens> routineRegimens { get; set; }
        public List<RoutineRegimens> selectedRegimens { get; set; }
        public List<RoutineProducts> selectedNewProducts { get; set; }
        public List<RoutineIngredients> selectedNewIngredients { get; set; }
        public List<RoutineRegimens> selectedNewRegimens { get; set; }
        public List<HairStyles> hairStyles { get; set; }
        public List<RoutineHairStyles> selectedHairStyles { get; set; }
        public List<RoutineHairStyles> selectedNewHairStyles { get; set; }
    }

    public class RoutineHairStyles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
    public class DailyRoutineContent
    {
        public DailyRoutineTracker dailyRoutineTracker { get; set; }
        public List<BlogArticle> blogArticleModel { get; set; }

        public List<DateTime> trackTimes { get; set; }
    }
    public class RoutineProducts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
    }
    public class RoutineIngredients
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
    public class RoutineRegimens
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public string Description { get; set; }
    }

    public class UserRoutineHairCare
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime TrackDate { get; set; }
        public int RoutineTrackerId { get; set; }
        public bool IsProduct { get; set; }
        public bool IsIngredient { get; set; }
        public bool IsRegimen { get; set; }
        public bool IsHairStyle { get; set; }
    }

    public class UserRoutineHairCareModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public DateTime TrackDate { get; set; }
        public int RoutineTrackerId { get; set; }
        public bool IsProduct { get; set; }
        public bool IsIngredient { get; set; }
        public bool IsRegimen { get; set; }
    }


}

