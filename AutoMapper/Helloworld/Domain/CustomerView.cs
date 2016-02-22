using AutoMapper;

namespace Domain
{
    public class CustomerView : ValueResolver<Customer, string>
    {
        public string Name { get; set; }
        public string Address { get; set; }

        protected override string ResolveCore(Customer source)
        {
            return source.FirstName + "-" + source.LastName;
        }
    }


}