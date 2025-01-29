using System;
using System.Collections.Generic;

namespace crud_biblioteca.Models;

public partial class Prestamo
{
    public int IdPrestamos { get; set; }

    public int IdUsuarios { get; set; }

    public int IdLibros { get; set; }

    public DateTime? FechaPrestamo { get; set; }

    public DateTime? FechaDevolucion { get; set; }

    public string? Estado { get; set; }

    public virtual Libro IdLibrosNavigation { get; set; } = null!;

    public virtual Usuario IdUsuariosNavigation { get; set; } = null!;
}
