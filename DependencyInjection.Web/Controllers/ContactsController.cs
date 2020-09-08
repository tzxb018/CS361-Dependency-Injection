using DependencyInjection.Web.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace DependencyInjection.Web.Controllers
{
    [RoutePrefix("api/contact")]
    public class ContactsController : ApiController
    {
        private readonly ApplicationDbContext db;

        public ContactsController()
        {
            db = new ApplicationDbContext();
        }

        // GET: api/contact
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetContacts()
        {
            return Ok(db.Contacts);
        }

        // GET: api/contact/5
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetContact(string id)
        {
            var parsedId = int.Parse(id);
            Contact contact = db.Contacts.Find(parsedId);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }


        // POST: api/Contacts
        [Route("")]
        [HttpPost]
        public IHttpActionResult PostContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // using email validation function to determine if email address is valid
            if (emailValidation(contact.EmailAddress))
            {
                // using regex to determine if phone number is valid
                if (phoneValidation(contact.PhoneNumber))
                {
                    db.Contacts.Add(contact);
                }
                else
                {
                    Console.WriteLine("Phone number invalid");
                    BadRequest("Phone number is invalid.");
                }
            }
            else
            {
                Console.WriteLine("Email address invlaid");
                BadRequest("Email Address is invalid");
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ContactExists(contact.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(contact);
        }

        // PUT: api/Contacts/5
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult PutContact(string id, Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var parsedId = int.Parse(id);

            if (parsedId != contact.Id)
            {
                return BadRequest();
            }

            // using email validation function to determine if email address is valid
            if (emailValidation(contact.EmailAddress))
            {
                // using regex to determine if phone number is valid
                if (phoneValidation(contact.PhoneNumber))
                {
                    db.Entry(contact).State = EntityState.Modified;
                }
                else
                {
                    Console.WriteLine("Phone number invalid");
                    BadRequest("Phone number is invalid.");
                }
            }
            else
            {
                Console.WriteLine("Email address invlaid");
                BadRequest("Email Address is invalid");
            }




            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(parsedId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }



        // DELETE: api/Contacts/5
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteContact(string id)
        {
            var parsedId = int.Parse(id);

            Contact contact = db.Contacts.Find(parsedId);
            if (contact == null)
            {
                return NotFound();
            }

            db.Contacts.Remove(contact);
            db.SaveChanges();

            return Ok(contact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactExists(int id)
        {
            return db.Contacts.Count(e => e.Id == id) > 0;
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
