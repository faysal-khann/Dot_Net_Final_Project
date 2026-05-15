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
            cfg.CreateMap<PendingEmployeeDTO, Employee>().ReverseMap();
            cfg.CreateMap<PendingEmployeeDTO, User>().ReverseMap();


            cfg.CreateMap<RoomDTO, Room>().ReverseMap();
            cfg.CreateMap<RoomDTO, RoomType>().ReverseMap();
            cfg.CreateMap<RoomTypeDTO, RoomType>().ReverseMap();
            cfg.CreateMap<RoomTypeDTO, Room>().ReverseMap();

           
            

        });
        public static Mapper GetMapper()
        {
            return new Mapper(config);
        }
    }
}
