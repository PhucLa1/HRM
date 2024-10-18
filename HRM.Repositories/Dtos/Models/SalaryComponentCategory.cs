using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models;

public class SalaryComponentCategory
{
    public string Name { get; set; } = "";
    public List<SalaryComponent> ListSalaryComponents { get; set; } = new List<SalaryComponent>();
}

public class SalaryComponent
{
    public string Name { get; set; } = "";
    public string ParameterName { get; set; } = "";
}
