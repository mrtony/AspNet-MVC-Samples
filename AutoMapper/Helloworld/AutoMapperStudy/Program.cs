using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using AutoMapper;
using ServiceStack.Text;

namespace AutoMapperStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            Helloworld1Version50();
            ValueResolverTest();
        }

        private static void HelloworldBeforeVersion40()
        {
            var customer = new Customer() { FirstName = "Tony", LastName = "Chen", Address = "Taipei", Age = 30 };
            var config = Mapper.CreateMap<Customer, CustomerView>();
            var destination = Mapper.Map<CustomerView>(customer);
            Console.WriteLine("CustomerView內容:{0}", destination.Dump());
        }

        /// <summary>
        /// 最基本的應用: 將Customer映射到CustomerView
        /// </summary>
        private static void Helloworld1Version50()
        {
            var customer = new Customer() {FirstName = "Tony", LastName = "Chen", Address = "Taipei", Age = 30};
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerClone>());
            var mapper = config.CreateMapper();

            var destination = mapper.Map<CustomerClone>(customer);
            Console.WriteLine("------------------\nAutomapper helloworld範例:\n---------------\n");
            Console.WriteLine("來源:CustomerView內容:{0}", customer.Dump());
            Console.WriteLine("目的:CustomerView內容:{0}", destination.Dump());
        }

        /// <summary>
        /// Custom value resolvers : https://github.com/AutoMapper/AutoMapper/wiki/Custom-value-resolvers
        /// 建立Resolver處理映射到目的物件時, 將來源物件做處理後再映射到目的物件.
        /// 這個範例是將來源物件的FirstName+LastName後, 映射到目的物件的Name
        /// </summary>
        private static void ValueResolverTest()
        {
            var customer = new Customer() { FirstName = "Tony", LastName = "Chen", Address = "Taipei", Age = 30 };
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerView>()
                .ForMember(dest => dest.Name, opt => opt.ResolveUsing<CustomerView>()));
            var mapper = config.CreateMapper();

            //驗證: 如果0個field被mapping到目的地, 就會發生assert
            config.AssertConfigurationIsValid();

            var destination = mapper.Map<CustomerView>(customer);
            Console.WriteLine("------------------\nResolver處理映射到目的物件範例:\n---------------\n");
            Console.WriteLine("來源:CustomerView內容:{0}", customer.Dump());
            Console.WriteLine("目的:CustomerView內容(經過resolver):{0}", destination.Dump());
        }
    }
}
