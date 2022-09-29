using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;

namespace MyAvana.CRM.Api.Contract
{
    public interface IIngredientsService
    {
        IngredientsModel GetIngredients();
        IngredientEntityModel SaveIngredients(IngredientEntityModel ingedientsEntity);
        bool DeleteIngredients(IngedientsEntity ingedientsEntity);
        IngedientsEntity GetIngredientById(IngedientsEntity ingedientsEntity);
    }
}
