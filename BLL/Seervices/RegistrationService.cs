using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
    public class RegistrationService
    {
       RegistrationRepo repo;
        Mapper mapper;

        public RegistrationService(RegistrationRepo repo)
        {
            this.repo = repo;
            this.mapper = MapperConfig.GetMapper();
        }

        public bool RegisterCustomer(CustomerDTO c)
        {
            // Map DTOs to Entities
            var user = mapper.Map<User>(c);
            user.Role = "customer";
            user.Status = "active";
            user.CreatedAt = DateTime.Now;
            user.Password = GetMd5(c.Password);
            var customer = mapper.Map<Customer>(c);
            return repo.RegisterCustomer(user, customer);
        }

        public bool RegisterEmployee(EmployeeDTO e)
        {
            var user = mapper.Map<User>(e);
            user.Role = "employee";
            user.Status = "pending";
            user.CreatedAt = DateTime.Now;
            user.Password = GetMd5(e.Password);
            var employee = mapper.Map<Employee>(e);

            return repo.RegisterEmployee(user, employee);
        }
        string GetMd5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // lowercase hex
                }

                return sb.ToString();
            }
        }
    }
}
