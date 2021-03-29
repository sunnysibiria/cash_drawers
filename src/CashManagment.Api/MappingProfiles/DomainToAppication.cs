using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CashManagment.Application.V10.Models;
using CashManagment.Domain.Models;

namespace CashManagment.Api.MappingProfiles
{
    public class DomainToAppication : Profile
    {
        public DomainToAppication()
        {
            CreateMap<RealContainer, Cassette>()
                .ForMember(dest => dest.Num, opts => opts.MapFrom(source => source.RealContainerId))
                .ForMember(dest => dest.AtCheck, opts => opts.MapFrom(source => source.NeedCheck))
                .ForMember(dest => dest.AtRemove, opts => opts.MapFrom(source => source.WroteOff))
                .ForMember(dest => dest.Curr, opts => opts.MapFrom(source => source.Currency))
                .ForMember(dest => dest.IdCity, opts => opts.MapFrom(source => source.CityGuid.ToString()))
                .ForMember(dest => dest.Model, opts => opts.MapFrom(source => source.Model))
                .ForMember(dest => dest.Value, opts => opts.MapFrom(source => source.CassetteNominal));
            CreateMap<RealContainer, CassetteProperties>()
                .ForMember(dest => dest.Num, opts => opts.MapFrom(source => source.RealContainerId))
                .ForMember(dest => dest.AtCheck, opts => opts.MapFrom(source => source.NeedCheck))
                .ForMember(dest => dest.AtRemove, opts => opts.MapFrom(source => source.WroteOff));
        }
    }
}
