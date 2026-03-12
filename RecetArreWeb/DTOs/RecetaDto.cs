using System.ComponentModel.DataAnnotations;

namespace RecetArreWeb.DTOs
{
    public class RecetaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = default!;
        public string? Descripcion { get; set; }
        public string Instrucciones { get; set; } = default!;
        public int TiempoPreparacionMinutos { get; set; }
        public int TiempoCoccionMinutos { get; set; }
        public int Porciones { get; set; }
        public bool EstaPublicado { get; set; }
        public DateTime CreadoUtc { get; set; }
        public DateTime ModificadoUtc { get; set; }
    }

    public class RecetaCreacionDto
    {
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Titulo { get; set; } = default!;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(15000)]
        public string Instrucciones { get; set; } = default!;

        [Range(0, 24 * 60)]
        public int TiempoPreparacionMinutos { get; set; }

        [Range(0, 24 * 60)]
        public int TiempoCoccionMinutos { get; set; }

        [Range(1, 100)]
        public int Porciones { get; set; } = 1;

        public bool EstaPublicado { get; set; } = true;
    }

    public class RecetaModificacionDto
    {
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Titulo { get; set; } = default!;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(15000)]
        public string Instrucciones { get; set; } = default!;

        [Range(0, 24 * 60)]
        public int TiempoPreparacionMinutos { get; set; }

        [Range(0, 24 * 60)]
        public int TiempoCoccionMinutos { get; set; }

        [Range(1, 100)]
        public int Porciones { get; set; } = 1;

        public bool EstaPublicado { get; set; } = true;
    }
}
