using Microsoft.AspNetCore.Mvc;

public class PaginacionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(
        int paginaActual,
        int totalPaginas,
        int tamanoPagina,
        string actionName,
        string controllerName)
    {
        ViewBag.PaginaActual = paginaActual;
        ViewBag.TotalPaginas = totalPaginas;
        ViewBag.TamanoPagina = tamanoPagina;
        ViewBag.ActionName = actionName;
        ViewBag.ControllerName = controllerName;
        return View();
    }
}