using CrudDatatable.Models;
using CrudDatatable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrudDatatable.App_Start
{

    public class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Employee, EmployeeVM>();
                config.CreateMap<EmployeeVM, Employee>();
            });
        }
    }
}