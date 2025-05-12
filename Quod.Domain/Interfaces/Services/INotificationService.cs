using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quod.Domain
{
    public interface INotificationService
    {
        Task AddAsync(NotificationViewModel notification);
    }
}
