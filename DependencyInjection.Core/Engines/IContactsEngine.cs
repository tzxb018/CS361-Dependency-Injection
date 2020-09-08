using DependencyInjection.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection.Core.Engines
{
    public interface IContactsEngine
    {
        IEnumerable<Contact> GetContacts();

        Contact GetContact(int id);

        Contact InsertContact(Contact newContact);

        Contact UpdateContact(int id, Contact updated);

        Contact DeleteContact(int id);
    }
}
