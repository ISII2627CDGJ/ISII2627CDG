using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaForCreateDTO
    {
        public ReservaForCreateDTO(string nombreUsuario)
        {
            NombreUsuario = string.Empty;
            ApellidosUsuario = string.Empty;
            Dni = string.Empty;
            Email = string.Empty;
            Telefono = string.Empty;
            Pistas = new List<ReservaItemDTO>();
            DatosPago = string.Empty;
            NombreUsuario = nombreUsuario;
        }

        public ReservaForCreateDTO(
            string nombreUsuario,
            string apellidosUsuario,
            string dni,
            string email,
            string telefono,
            MetodoPago metodoPago,
            List<ReservaItemDTO> pistas,
            string datosPago,
            DateTime fechaReserva)
        {
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
            ApellidosUsuario = apellidosUsuario ?? throw new ArgumentNullException(nameof(apellidosUsuario));
            Dni = dni ?? throw new ArgumentNullException(nameof(dni));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Telefono = telefono ?? throw new ArgumentNullException(nameof(telefono));
            MetodoPago = metodoPago;
            Pistas = pistas ?? throw new ArgumentNullException(nameof(pistas));
            DatosPago = datosPago ?? throw new ArgumentNullException(nameof(datosPago));
            FechaReserva = fechaReserva;
        }

        [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Los apellidos del usuario son obligatorios.")]
        public string ApellidosUsuario { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string Telefono { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        public List<ReservaItemDTO> Pistas { get; set; }

        public string DatosPago { get; set; }

        [Required]
        public DateTime FechaReserva { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReservaForCreateDTO dTO &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   Dni == dTO.Dni &&
                   Email == dTO.Email &&
                   Telefono == dTO.Telefono &&
                   MetodoPago == dTO.MetodoPago &&
                   EqualityComparer<List<ReservaItemDTO>>.Default.Equals(Pistas, dTO.Pistas) &&
                   DatosPago == dTO.DatosPago &&
                   FechaReserva == dTO.FechaReserva;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(NombreUsuario);
            hash.Add(ApellidosUsuario);
            hash.Add(Dni);
            hash.Add(Email);
            hash.Add(Telefono);
            hash.Add(MetodoPago);
            hash.Add(Pistas);
            hash.Add(DatosPago);
            hash.Add(FechaReserva);
            return hash.ToHashCode();
        }
    }
}