using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class IngredientsService : IIngredientsService
    {
        private readonly AvanaContext _context;
        public IngredientsService(AvanaContext avanaContext)
        {
            _context = avanaContext;
        }

        public bool DeleteIngredients(IngedientsEntity ingedientsEntity)
        {
            try
            {
                var objIngredient = _context.IngedientsEntities.FirstOrDefault(x => x.IngedientsEntityId == ingedientsEntity.IngedientsEntityId);
                {
                    if (objIngredient != null)
                    {
                        objIngredient.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }

        public IngedientsEntity GetIngredientById(IngedientsEntity ingedientsEntity)
        {
            try
            {
                IngedientsEntity ingredientEntityModel = _context.IngedientsEntities.Where(x => x.IngedientsEntityId == ingedientsEntity.IngedientsEntityId).FirstOrDefault();
                return ingredientEntityModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IngredientsModel GetIngredients()
        {
            try
            {
                List<IngedientsEntity> ingredientsEntities = _context.IngedientsEntities.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();

                IngredientsModel ingredientsModel = new IngredientsModel();
                ingredientsModel.Ingredients = ingredientsEntities;
                return ingredientsModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IngredientEntityModel SaveIngredients(IngredientEntityModel ingedientsEntity)
        {
            try
            {
                if (ingedientsEntity.IngedientsEntityId != 0)
                {
                    var objIngredients = _context.IngedientsEntities.Where(x => x.IngedientsEntityId == ingedientsEntity.IngedientsEntityId).FirstOrDefault();
                    objIngredients.Name = ingedientsEntity.Name;
                    objIngredients.Type = ingedientsEntity.Type;
                    objIngredients.Image = ingedientsEntity.Image;
                    objIngredients.ImageUrl = ingedientsEntity.Imageurl;
                    objIngredients.Description = ingedientsEntity.Description;
                    objIngredients.Challenges = ingedientsEntity.Challenges;
                }
                else
                {
                    _context.IngedientsEntities.Add(new IngedientsEntity()
                    {
                        IngedientsEntityId = ingedientsEntity.IngedientsEntityId,
                        Name = ingedientsEntity.Name,
                        Type = ingedientsEntity.Type,
                        Image = ingedientsEntity.Image,
                        ImageUrl = ingedientsEntity.Imageurl,
                        Description = ingedientsEntity.Description,
                        Challenges = ingedientsEntity.Challenges,
                        CreatedOn = DateTime.UtcNow,
                        IsActive = true
                    });
                }
                _context.SaveChanges();
                return ingedientsEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
