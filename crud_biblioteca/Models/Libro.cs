using System;
using System.Collections.Generic;

namespace crud_biblioteca.Models;

public partial class Libro
{
    public int IdLibros { get; set; }

    public string Titulo { get; set; } = null!;

    public string Autor { get; set; } = null!;

    public string Isbn { get; set; } = null!;

    public int AñoPublicacion { get; set; }

    public bool? Disponibilidad { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
