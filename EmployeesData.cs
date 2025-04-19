

namespace MyDbApp.Models
{
    public class EmployeesData
    {
        public int employees_id { get; set; }
        
        public int manager_id { get; set; }

        public string first_name { get; set; }
        public string last_name { get; set; }

        public int branch_id { get; set; }

        public int salary { get; set; }

    }
}
