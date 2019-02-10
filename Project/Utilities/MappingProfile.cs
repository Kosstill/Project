using System.Linq;
using AutoMapper;
using Project.Models;

namespace Project.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // For organization
            CreateMap<Organization, OrganizationViewModel>();
            CreateMap<Organization, OrganizationDetailsViewModel>();
            CreateMap<OrganizationDetailsViewModel, Organization>();

            // For country
            CreateMap<Country, CountryViewModel>();
            CreateMap<Country, CountryDetailsViewModel>();
            CreateMap<CountryDetailsViewModel, Country>();

            // For business
            CreateMap<Business, BusinessViewModel>();
            CreateMap<BusinessViewModel, Business>();

            // For family
            CreateMap<Family, FamilyViewModel>();
            CreateMap<FamilyViewModel, Family>();

            // For offering
            CreateMap<Offering, OfferingViewModel>();
            CreateMap<OfferingViewModel, Offering>();

            // For department
            CreateMap<Department, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, Department>();
        }
    }
}