using Microsoft.AspNetCore.Http;
using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class IngredientEntityModel
    {
        public int IngedientsEntityId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public IFormFile File { get; set; }
        public string Image { get; set; }
        public string Imageurl { get; set; }
        public string Description { get; set; }
        public string Challenges { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
