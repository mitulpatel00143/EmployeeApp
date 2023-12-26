using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class EmployeeOperation
    {
        public List<EmployeeDetailsViewModel> EmployeeDetails { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
    }
}
