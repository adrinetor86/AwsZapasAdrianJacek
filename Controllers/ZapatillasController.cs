using AwsZapasAdrianJacek.Models;
using AwsZapasAdrianJacek.Repositories;
using AwsZapasAdrianJacek.Services;
using Microsoft.AspNetCore.Mvc;

namespace AwsZapasAdrianJacek.Controllers;

public class ZapatillasController : Controller
{
    private RepositoryZapatillas _repo;
    private ServiceStorageS3 _serviceS3;
    public ZapatillasController(RepositoryZapatillas repo,ServiceStorageS3 serviceS3)
    {
        _repo = repo;
        _serviceS3 = serviceS3;
    }
    
    public async Task<IActionResult> Index()
    {
        List<Zapatilla> zapatillas = await _repo.GetZapatillasAsync();
        return View(zapatillas);
    }
    
    
    // GET: Zapatillas/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Zapatillas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Zapatilla zapatilla, IFormFile archivoImagen)
    {
        if (archivoImagen != null && archivoImagen.Length > 0)
        {
            // 1. Obtenemos el nombre del archivo (ej. "mis-tenis.jpg")
            string fileName = archivoImagen.FileName;

            // 2. Abrimos el flujo de lectura del archivo subido
            using (var stream = archivoImagen.OpenReadStream())
            {
                // 3. Subimos la imagen a S3 usando tu servicio existente
                // Este método internamente usará PutObjectAsync
                await _serviceS3.UploadFileAsync(fileName, stream);
            }

            // 4. Asignamos SOLO el nombre del archivo a la propiedad de la base de datos
            // Ya que tu método GetZapatillasAsync() se encarga de concatenar la URL completa del bucket.
            zapatilla.Imagen = fileName;
        }
        else
        {
            // Si no suben imagen, puedes asignar un valor por defecto o manejar el error
            zapatilla.Imagen = "default.png"; 
        }

        // 5. Guardamos el objeto Zapatilla en MySQL mediante tu repositorio
        // (Asegúrate de tener este método implementado en tu repositorio)
        await _repo.InsertZapatillaAsync(zapatilla);

        return RedirectToAction(nameof(Index));
    }
}