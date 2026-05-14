using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class MapperConfig
    {
        public static MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.CreateMap<User, UserDTO>().ReverseMap();
            cfg.CreateMap<CustomerDTO, User>().ReverseMap();
            cfg.CreateMap<EmployeeDTO, User>().ReverseMap();
            cfg.CreateMap<Employee, EmployeeDTO>().ReverseMap();
            cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();

        });
        public static Mapper GetMapper()
        {
            return new Mapper(config);
        }
    }
}
