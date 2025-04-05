using AutoMapper;
using KhaoThi_2024_net_client.Models.Auth;
using KhaoThi_2024_net_client.Models.Users;

namespace KhaoThi_2024_net_client
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KhaoThiUserModel, UserInfo>();
        }
    }
}
