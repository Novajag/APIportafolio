using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portafolio.Data;
using Portafolio.Models;
using Portafolio.Services;

namespace Portafolio.Controllers
{
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactMessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TelegramService _telegramService;

        public ContactMessagesController(ApplicationDbContext context, TelegramService telegramService)
        {
            _context = context;
            _telegramService = telegramService;
        }

        // POST: api/contactmessages
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageContact newMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set current timestamp
            newMessage.CreatedAt = DateTime.UtcNow;

            // Save to MySQL database
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Enviar notificación por Telegram (no interrumpe la respuesta si falla)
            _ = _telegramService.SendNotificationAsync(
                newMessage.Name,
                newMessage.Email,
                newMessage.Message
            );

            return Ok(new
            {
                message = "Message sent successfully",
                id = newMessage.Id
            });
        }

        // GET: api/contactmessages
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.Messages
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return Ok(messages);
        }
    }
}