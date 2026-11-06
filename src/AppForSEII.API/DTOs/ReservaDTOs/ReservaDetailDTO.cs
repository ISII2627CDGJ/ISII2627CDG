using System;
using System.Collections.Generic;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaDetailDTO : ReservaForCreateDTO
    {
        public ReservaDetailDTO() : base(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, default, new List<ReservaItemDTO>(), string.Empty, default)
        {
        }

        public ReservaDetailDTO(
            int id,
            string nombreUsuario,
            string apellidosUsuario,
            string dni,
            string email,
            string telefono,
            MetodoPago metodoPago,
            double precioTotal,
            List<ReservaItemDTO> pistas,
            string datosPago,
            DateTime fechaReserva)
            : base(nombreUsuario, apellidosUsuario, dni, email, telefono, metodoPago, pistas, datosPago, fechaReserva)
        {
            Id = id;
            PrecioTotal = precioTotal;
        }

        public int Id { get; set; }
        public double PrecioTotal { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReservaDetailDTO dTO &&
                   base.Equals(obj) &&
                   Id == dTO.Id &&
                   PrecioTotal == dTO.PrecioTotal;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Id);
            hash.Add(PrecioTotal);
            return hash.ToHashCode();
        }
    }
}