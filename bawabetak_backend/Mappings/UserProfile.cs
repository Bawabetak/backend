namespace bawabetak_backend.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CompleteRegisterDto, ApplicationUser>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.IdentityDocument, opt => opt.Ignore());
        }
    }
}
