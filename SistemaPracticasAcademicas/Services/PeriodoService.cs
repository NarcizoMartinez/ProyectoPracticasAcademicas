using System;

namespace SistemaPracticasAcademicas.Services
{
    public class PeriodoService
    {
        public string GetPeriodo()
        {
            DateTime hoy = DateTime.Today;
            int mes = hoy.Month;
            string año = hoy.Year.ToString();
            string periodo;
            if (mes >= 1 && mes <= 6)
            {
                periodo = $"Febrero-Junio {año}";
            }
            else
            {
                periodo = $"Agosto-Diciembre {año}";
            }
            return periodo;
        }
    }
}
