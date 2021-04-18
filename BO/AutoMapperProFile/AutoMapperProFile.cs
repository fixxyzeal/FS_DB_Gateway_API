using AutoMapper;
using BO.Models.Mongo;
using BO.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BO.AutoMapperProFile
{
    public class AutoMapperProFile : Profile
    {
        public AutoMapperProFile()
        {
            CreateMap<LogViewModel, LogService>().ReverseMap();
        }
    }
}