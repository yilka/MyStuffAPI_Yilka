﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStuffAPI_Yilka.Models;
using MyStuffAPI_Yilka.Attributes;
using MyStuffAPI_Yilka.Tools;

namespace MyStuffAPI_Yilka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class UsersController : ControllerBase
    {
        private readonly MyStuffDBContext _context;

        public UsersController(MyStuffDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //este get, recibe por param el email (encriptado para que no vaya a ser capturado cuando
        //se llama a la ruta del API, ya que puede ser usado para spam) y el pass también encriptado 
        //por seguridad. 

        // GET: api/Users/email/pass
        [HttpGet("{email}/{pass}")]
        public async Task<ActionResult<User>> ValidateUser(string email, string pass)
        {
            Crypto MiEncriptador = new Crypto();

            string Email = MiEncriptador.DesEncriptarData(email);

            var user = await _context.Users.SingleOrDefaultAsync(e => e.Username == Email && e.UserPassword == pass && e.UserStatusId == 1);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Users/ValidateUser2
        [HttpGet("ValidateUser2")]
        public async Task<ActionResult<User>> ValidateUser2(string email, string pass)
        {
            var user = await _context.Users.SingleOrDefaultAsync(e => e.Username == email && e.UserPassword == pass && e.UserStatusId == 1);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
