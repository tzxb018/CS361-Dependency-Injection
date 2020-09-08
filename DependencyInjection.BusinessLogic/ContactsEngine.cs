using DependencyInjection.Accessors;
using DependencyInjection.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace DependencyInjection.BusinessLogic
{
    public class ContactsEngine
    {
        private readonly ContactsAccessor _contactsAccessor;

        public ContactsEngine()
        {
            _contactsAccessor = new ContactsAccessor();
        }

        public IEnumerable<Contact> GetContacts()
        {
            return _contactsAccessor.GetAll().ToList();
        }

        public Contact GetContact(int id)
        {
            if (_contactsAccessor.Exists(id))
            {
                return _contactsAccessor.Find(id);
            }
            return null;

        }

        public Contact InsertContact(Contact newContact)
        {
            // using email validation function to determine if email address is valid
            if (emailValidation(newContact.EmailAddress))
            {
                // using regex to determine if phone number is valid
                if (phoneValidation(newContact.PhoneNumber))
                {
                    _contactsAccessor.Insert(newContact);
                    _contactsAccessor.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Phone number is invalid.");
                }
            }
            else
            {
                Console.WriteLine("Email Address is invalid");
            }


            return newContact;
            //throw new NotImplementedException();
        }

        public Contact UpdateContact(int id, Contact updated)
        {
            var contact = _contactsAccessor.Find(id);
            if (contact != null)
            {
                // using email validation function to determine if email address is valid
                if (emailValidation(updated.EmailAddress))
                {
                    // using regex to determine if phone number is valid
                    if (phoneValidation(updated.PhoneNumber))
                    {

                        _contactsAccessor.Update(updated);
                        _contactsAccessor.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Phone number is invalid.");
                    }
                }
                else
                {
                    Console.WriteLine("Email Address is invalid");
                }

            }

            return updated; 
            //throw new NotImplementedException();
        }

        public Contact DeleteContact(int id)
        {
            var contact = _contactsAccessor.Find(id);
            if (contact != null)
            {
                _contactsAccessor.Delete(contact);
                _contactsAccessor.SaveChanges();
            }
            return contact;
        }

        // derived from https://stackoverflow.com/questions/5342375/regex-email-validation
        public bool emailValidation(string email_address)
        {
            try
            {

                MailAddress mailAddress = new MailAddress(email_address);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // derived from https://forums.asp.net/t/2119206.aspx?C+code+to+validate+email+and+10+digit+US+phone+number
        public bool phoneValidation(string phone_num)
        {
            return Regex.IsMatch(phone_num, @"^(\([0-9]{3}\)|[0-9]{3}-)[0-9]{3}-[0-9]{4}|(\([0-9]{3}\)|[0-9]{3})[0-9]{3}[0-9]{4}$");

        }
    }
}
