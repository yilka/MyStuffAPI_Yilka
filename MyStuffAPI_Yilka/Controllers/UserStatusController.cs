﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStuffAPI_Yilka.Models;

namespace MyStuffAPI_Yilka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatusController : ControllerBase
    {
        private readonly MyStuffDBContext _context;

        public UserStatusController(MyStuffDBContext context)
        {
            _context = context;
        }

        // GET: api/UserStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserStatus>>> GetUserStatuses()
        {
            return await _context.UserStatuses.ToListAsync();
        }

        // GET: api/UserStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserStatus>> GetUserStatus(int id)
        {
            var userStatus = await _context.UserStatuses.FindAsync(id);

            if (userStatus == null)
            {
                return NotFound();
            }

            return userStatus;
        }

        // PUT: api/UserStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserStatus(int id, UserStatus userStatus)
        {
            if (id != userStatus.UserStatusId)
            {
                return BadRequest();
            }

            _context.Entry(userStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserStatusExists(id))
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

        // POST: api/UserStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserStatus>> PostUserStatus(UserStatus userStatus)
        {
            _context.UserStatuses.Add(userStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserStatus", new { id = userStatus.UserStatusId }, userStatus);
        }

        // DELETE: api/UserStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserStatus(int id)
        {
            var userStatus = await _context.UserStatuses.FindAsync(id);
            if (userStatus == null)
            {
                return NotFound();
            }

            _context.UserStatuses.Remove(userStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserStatusExists(int id)
        {
            return _context.UserStatuses.Any(e => e.UserStatusId == id);
        }
    }
}
