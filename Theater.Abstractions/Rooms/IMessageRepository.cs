using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Entities.Rooms;

namespace Theater.Abstractions.Rooms;

public interface IMessageRepository : ICrudRepository<MessageEntity>
{
}