using AutoMapper;
using Quod.Domain;

namespace Quod.API
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationViewModel>().ReverseMap();
            CreateMap<DeviceViewModel, DeviceViewModel>().ReverseMap();
            CreateMap<Metadata, MetadataViewModel>().ReverseMap();  
        }
    }
}
