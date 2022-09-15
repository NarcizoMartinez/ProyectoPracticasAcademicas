using System.Globalization;
using System.Text;

namespace SistemaPracticasAcademicas.Areas.Admin.Helpers
{
    public class RemoveDiacritics
    {
        /// <summary>
        /// Método para quitar acentos de las letras.
        /// </summary>
        public string QuitarAcentos(string _texto)
        {
            var normalizedString = _texto.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().ToUpper().Normalize(NormalizationForm.FormC);
        }
    }
}
