using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainClass;

namespace DataModels
{
    public class SignedContractContext: DbContext
    {
        public DbSet<SignedContract> SignedContracts { get; set; }
    }
}
