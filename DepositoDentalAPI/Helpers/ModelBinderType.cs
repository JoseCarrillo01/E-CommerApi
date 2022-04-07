using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace DepositoDentalAPI.Helpers
{
    public class ModelBinderType<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nombrePropiedad = bindingContext.ModelName;

            var valueProveedor = bindingContext.ValueProvider.GetValue(nombrePropiedad);

            if(valueProveedor == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                var valorDeserealizado = JsonConvert.DeserializeObject<T>(valueProveedor.FirstValue);

                bindingContext.Result = ModelBindingResult.Success(valorDeserealizado);


            }
            catch (Exception)
            {

                bindingContext.ModelState.TryAddModelError(nombrePropiedad, "Valor invalido para el listado de Ids");
            }
            return Task.CompletedTask;
        }
    }
}
