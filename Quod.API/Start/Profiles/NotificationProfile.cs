using AutoMapper;
using Quod.Domain;

namespace Quod.API
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationViewModel, Notification>();
            CreateMap<DeviceViewModel, Device>();
            CreateMap<MetadataViewModel, Metadata>();

            CreateMap<Notification, NotificationViewModel>();
            CreateMap<Device, DeviceViewModel>();
            CreateMap<Metadata, MetadataViewModel>();
        }
    }
}
