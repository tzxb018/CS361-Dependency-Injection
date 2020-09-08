using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjection.Core.Models;

namespace DependencyInjection.Core.Accessors
{
    public interface IContactsAccessor
    {

         IQueryable<Contact> GetAll();


         Contact Find(int id);


         Contact Insert(Contact contact);


         void Update(Contact contact);


         Contact Delete(Contact contact);


         bool Exists(int id);


         int SaveChanges();

    }
}
