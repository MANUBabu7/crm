
using AutoMapper;
using CrmTracker.Models.EntityModels;
using CrmTracker.Models.ResourceModels;

namespace CrmTracker.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerResource>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.customer_id))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.customer_name))
            .ForMember(dest => dest.CustomerMobile, opt => opt.MapFrom(src => src.customer_mobile))
            .ForMember(dest => dest.CustomerAddress, opt => opt.MapFrom(src => src.customer_address))
            .ForMember(dest => dest.CustomerStatus, opt => opt.MapFrom(src => src.customer_status))
            .ForMember(dest => dest.CustomerWebsite, opt => opt.MapFrom(src => src.customer_website))
            .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.customer_type))
            .ForMember(dest => dest.CustomerCompany, opt => opt.MapFrom(src => src.customer_company))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.customer_email))
            .ForMember(dest => dest.CustomerLocation, opt => opt.MapFrom(src => src.customer_location));

        }
    }
}