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
            CreateMap<Servicio, ServicioDTO>()
                .ForMember(dest => dest.EstadoDescripcion, opt => opt.MapFrom(src => src.Estado != null ? src.Estado.Descripcion : null))
                .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre : null))
                .ForMember(dest => dest.EmpleadoNombre, opt => opt.MapFrom(src => src.Empleado != null ? $"{src.Empleado.Nombre} {src.Empleado.Apellido}".Trim() : null))
                .ForMember(dest => dest.VehiculoPlaca, opt => opt.MapFrom(src => src.Cliente != null && src.Cliente.Vehiculo != null ? src.Cliente.Vehiculo.Placa : null))
                .ForMember(dest => dest.VehiculoMarca, opt => opt.MapFrom(src => src.Cliente != null && src.Cliente.Vehiculo != null ? src.Cliente.Vehiculo.Marca : null))
                .ForMember(dest => dest.VehiculoModelo, opt => opt.MapFrom(src => src.Cliente != null && src.Cliente.Vehiculo != null ? src.Cliente.Vehiculo.Modelo : null))
                .ReverseMap();
            CreateMap<ServicioCreateDTO, Servicio>();

            // OrdenTrabajo mappings
            CreateMap<Models.OrdenDeTrabajo, OrdenTrabajoDTO>().ReverseMap();
            CreateMap<OrdenTrabajoCreateDTO, Models.OrdenDeTrabajo>();

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

