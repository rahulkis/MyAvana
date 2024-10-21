using Microsoft.AspNetCore.Http;
using MyavanaAdminModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyavanaAdminApiClient
{
    public partial class ApiClient
    {
        public async Task<List<IngredientsModel>> GetIngredients()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Ingredients/GetIngredients"));
            var response = await GetAsyncData<IngredientsModel>(requestUrl);
            List<IngredientsModel> ingredients = JsonConvert.DeserializeObject<List<IngredientsModel>>(Convert.ToString(response.value.ingredients));
            return ingredients;
        }
        public async Task<Message<IngredientsModel>> GetIngredientById(IngredientsModel ingredients)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Ingredients/GetIngredientById"));
            var result = await PostAsync<IngredientsModel>(requestUrl, ingredients);
            return result;
        }
        public async Task<Message<IngredientsModel>> DeleteIngredient(IngredientsModel ingredients)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Ingredients/DeleteIngredients"));
            var result = await PostAsync<IngredientsModel>(requestUrl, ingredients);
            return result;
        }
        public async Task<Message<IngredientEntityModel>> SaveIngredient(IngredientEntityModel ingredientEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Ingredients/SaveIngredients"));
            var result = await PostAsync<IngredientEntityModel>(requestUrl, ingredientEntity);
            return result;
        }
    }
}
