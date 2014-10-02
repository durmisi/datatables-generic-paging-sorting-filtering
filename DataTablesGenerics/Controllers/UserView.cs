using System;

namespace DataTablesGenerics.Controllers
{
    public class UserView
    {

        public string Name { get; set; }

        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public bool AHiddenColumn { get; set; }


        public decimal Salary { get; set; }

        public string Position { get; set; }

        public DateTime? Hired { get; set; }

        public Numbers Number { get; set; }

        public string ThisColumnIsExcluded { get { return "asdf"; } }

    }
}