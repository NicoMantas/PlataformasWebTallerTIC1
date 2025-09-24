using AutoMapper;
using Taller_TIC1_Backend.Models;
using Taller_TIC1_Backend.Models.DTOs;

namespace Taller_TIC1_Backend.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Servicio mappings
            CreateMap<Servicio, ServicioDTO>().ReverseMap();
            CreateMap<ServicioCreateDTO, Servicio>();

            // OrdenTrabajo mappings
            CreateMap<OrdenDeTrabajo, OrdenTrabajoDTO>().ReverseMap();
            CreateMap<OrdenTrabajoCreateDTO, OrdenDeTrabajo>();

            // Detalles mappings
            CreateMap<DetalleReparacionRepuesto, RepuestoCantidadDTO>()
                .ForMember(dest => dest.IdRepuesto, opt => opt.MapFrom(src => src.IdRepuesto))
                .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad));

            // Factura mappings
            CreateMap<Factura, FacturaDTO>().ReverseMap();
            CreateMap<FacturaCreateDTO, Factura>();
        }
    }
}

