// Ruta sugerida: TechSolutions.API/Common/ApiError.cs
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TechSolutions.API.Common
{
    /// <summary>
    /// Modelo estándar de error para respuestas de la API.
    /// </summary>
    public class ApiError
    {
        public string Message { get; set; }
        public IDictionary<string, string[]>? Details { get; set; }

        public ApiError(string message, IDictionary<string, string[]>? details = null)
        {
            Message = message;
            Details = details;
        }

        /// <summary>
        /// Error simple con solo mensaje.
        /// </summary>
        public static ApiError FromMessage(string message) => new(message);

        /// <summary>
        /// Construye un error a partir de los errores de validación del ModelState.
        /// (Por si luego quieres usarlo manualmente en algún controlador.)
        /// </summary>
        public static ApiError FromModelState(ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(ms => ms.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return new ApiError("Se encontraron errores de validación.", errors);
        }
    }
}
