using AwsZapasAdrianJacek.Models;
using Microsoft.EntityFrameworkCore;

namespace AwsZapasAdrianJacek.Data;

public class DataContext : DbContext
{

    public DataContext( DbContextOptions<DataContext> options ) : base(options) { }

    public DbSet<Zapatilla> Zapatillas { get; set; }
    
}