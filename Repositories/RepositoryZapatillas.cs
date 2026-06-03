using AwsZapasAdrianJacek.Models;
using AwsZapasAdrianJacek.Data;
using Microsoft.EntityFrameworkCore;

namespace AwsZapasAdrianJacek.Repositories;

public class RepositoryZapatillas
{
    private DataContext _context;
    private readonly string _bucketUrl;

    public RepositoryZapatillas(DataContext context,IConfiguration configuration)
    {
        _context = context;
        _bucketUrl = configuration.GetValue<string>("AWS:BucketUrl");
    }



    public async Task<List<Zapatilla>> GetZapatillasAsync()
    {
        // 1. Sacamos todas las zapatillas de la base de datos MySQL
        List<Zapatilla> zapatillas = await _context.Zapatillas.ToListAsync();

        // 2. Modificamos la propiedad Imagen para que contenga la URL completa de S3
        foreach (Zapatilla zapa in zapatillas)
        {
            // Ejemplo: https://bucket-zapas-jacek.s3.amazonaws.com/ + airmag.jpg
            zapa.Imagen = _bucketUrl + zapa.Imagen;
        }

        return zapatillas;
    }
    
    private async Task<int> GetMaxIdZapatillaAsync() 
    { 
        return await _context.Zapatillas 
            .MaxAsync(x => x.IdProducto) + 1; 
    } 
 
    public async Task CreateZapatillaAsync 
        (string nombre,string descripcion, string imagen) 
    { 
        Zapatilla p = new Zapatilla(); 
        p.IdProducto = await GetMaxIdZapatillaAsync(); 
        p.Nombre = nombre; 
        p.Descripcion = descripcion; 
        p.Imagen = imagen; 
        await _context.Zapatillas.AddAsync(p); 
        await _context.SaveChangesAsync(); 
    }
    
    public async Task InsertZapatillaAsync(Zapatilla zapatilla)
    {
        // Calculamos el siguiente ID disponible si tu columna IDPRODUCTO no es AUTO_INCREMENT
        if (zapatilla.IdProducto == 0)
        {
            int maxId = await _context.Zapatillas.MaxAsync(z => (int?)z.IdProducto) ?? 0;
            zapatilla.IdProducto = maxId + 1;
        }

        _context.Zapatillas.Add(zapatilla);
        await _context.SaveChangesAsync();
    }
}