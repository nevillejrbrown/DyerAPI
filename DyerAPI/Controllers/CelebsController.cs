using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DyerAPI.Models;
using DyerAPI.Models.Data;
using DyerAPI.Models.Service;

namespace DyerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CelebsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGameService _gameService;
        public CelebsController(ApplicationDbContext context, IGameService gameService)
        {
            _context = context;
            _gameService = gameService;
        }

        // GET: api/Celebs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Celeb>>> GetCeleb()
        {
            return await _context.Celeb.ToListAsync();
        }

        // GET: api/Celebs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Celeb>> GetCeleb(int id)
        {
            var celeb = await _context.Celeb.FindAsync(id);

            if (celeb == null)
            {
                return NotFound();
            }

            return celeb;
        }

        // PUT: api/Celebs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCeleb(int id, Celeb celeb)
        {
            if (id != celeb.Id)
            {
                return BadRequest();
            }

            _context.Entry(celeb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CelebExists(id))
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


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCeleb(int id, Celeb celeb)
        {
            if (id != celeb.Id)
            {
                return BadRequest();
            }

            var celebRetrieved = _gameService.GetCeleb(id);
            if (celebRetrieved.State != celeb.State)
            {
                switch (celeb.State)
                {
                    case CelebState.GUESSED: 
                        _gameService.CelebGuessed(id);
                        break;
                    case CelebState.BURNED:
                        _gameService.CelebBurned(id);
                        break;
                }
            }

            return NoContent();
        }

        // POST: api/Celebs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Celeb>> PostCeleb(Celeb celeb)
        {
            _context.Celeb.Add(celeb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCeleb", new { id = celeb.Id }, celeb);
        }

        // DELETE: api/Celebs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Celeb>> DeleteCeleb(int id)
        {
            var celeb = await _context.Celeb.FindAsync(id);
            if (celeb == null)
            {
                return NotFound();
            }

            _context.Celeb.Remove(celeb);
            await _context.SaveChangesAsync();

            return celeb;
        }

        private bool CelebExists(int id)
        {
            return _context.Celeb.Any(e => e.Id == id);
        }
    }
}
