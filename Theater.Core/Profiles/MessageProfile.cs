using AutoMapper;
using Theater.Abstractions.Filters;
using Theater.Contracts.Filters;
using Theater.Contracts.Messages;
using Theater.Contracts.Rabbit;
using Theater.Contracts.Rooms;
using Theater.Entities.Rooms;

namespace Theater.Core.Profiles;

public sealed class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<MessageEntity, MessageModel>()
            .ForMember(destination => destination.Author, options => options.MapFrom(exp => exp.User))
            ;
        CreateMap<MessageSentModel, MessageModel>();
        CreateMap<MessageFilterParameters, MessageFilterSettings>();
        CreateMap<MessageEntity, LastMessageModel>()
            .ForMember(destination => destination.AuthorId, options => options.MapFrom(exp => exp.User.Id))
            ;

        CreateMap<MessageReadModel, ReadMessageModel>();
    }
}