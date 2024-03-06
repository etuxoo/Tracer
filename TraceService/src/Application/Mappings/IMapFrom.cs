using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace TraceService.Application.Mappings
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), this.GetType()).ReverseMap();
        }
    }
}
