namespace Inmobiliaria_.Net_Core.Models
{
    public class PaginadoViewModel<T>
    {
        public List<T> Elementos { get; set; } = new();
        public int PaginaActual { get; set; }
        public int TamanoPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanoPagina);
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
        public int PaginaInicio => Math.Max(1, PaginaActual - 2);
        public int PaginaFin => Math.Min(TotalPaginas, PaginaActual + 2);
    }
}