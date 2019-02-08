using AutoMapper;
using Project.Models;

namespace Project.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Organization, OrganizationViewModel>();
            CreateMap<Organization, OrganizationDetailsViewModel>();
            CreateMap<OrganizationDetailsViewModel, Organization>();
        }
    }
}