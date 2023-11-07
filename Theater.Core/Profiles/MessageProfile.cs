using AutoMapper;
using Theater.Contracts.Messages;
using Theater.Contracts.Rabbit;
using Theater.Entities.Rooms;

namespace Theater.Core.Profiles;

public sealed class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<MessageEntity, MessageModel>();
        CreateMap<MessageSentModel, MessageModel>();
    }
}