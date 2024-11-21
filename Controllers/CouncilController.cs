using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using VicemAPI.Data;
using VicemAPI.DTOs;
using VicemAPI.DTOs.Advisor;
using VicemAPI.DTOs.Council;
using VicemAPI.DTOs.Student;
using VicemAPI.Models.Entities;

namespace VicemAPI.Controllers
{
    [Route("api/council")]
    [ApiController]
    public class CouncilController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CouncilController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Council
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouncilDto>>> GetCouncil([FromQuery] int? departmentId = null)
        {
            var query = _context.Council
                .Include(c => c.Department)
                .Include(c => c.Students)
                .Include(c => c.Advisors)
                .AsQueryable();

            if (departmentId.HasValue)
            {
                query = query.Where(c => c.DepartmentID == departmentId.Value);
            }

            var result = await query
                .Select(c => new CouncilDto
                {
                    ID = c.ID,
                    Name = c.Name,
                    Description = c.Description,
                    ReviewTime = c.ReviewTime,
                    Location = c.Location,
                    Department = new DepartmentDTO
                    {
                        ID = c.Department.ID,
                        Name = c.Department.Name,
                        Code = c.Department.Code,
                    },
                    Students = c.Students.Select(s => new StudentSimpleDto
                    {
                        ID = s.ID,
                        Name = s.Name,
                        Code = s.Code,
                        Class = s.Class,
                        AdvisorName = s.Advisor!.Name,
                    }).ToList(),
                    Advisors = c.Advisors.Select(a => new AdvisorSimpleDTO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Code = a.Code,
                    }).ToList(),
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/Council/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Council>> GetCouncil(int id)
        {
            var council = await _context.Council.FindAsync(id);

            if (council == null)
            {
                return NotFound();
            }

            return council;
        }

        // PUT: api/Council/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCouncil(int id, Council council)
        {
            if (id != council.ID)
            {
                return BadRequest();
            }

            _context.Entry(council).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouncilExists(id))
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

        // POST: api/Council
        [HttpPost]
        public async Task<ActionResult<Council>> PostCouncil(CreateCouncilDto council)
        {
            var students = await _context.Student
                .Where(s => council.StudentIds.Contains(s.ID))
                .ToListAsync();

            var advisors = await _context.Advisor
                .Where(a => council.AdvisorIds.Contains(a.ID))
                .ToListAsync();

            // Check if any student is already in another council
            var existingStudentIds = await _context.Council
                .SelectMany(c => c.Students)
                .Where(s => council.StudentIds.Contains(s.ID))
                .Select(s => s.ID)
                .ToListAsync();

            if (existingStudentIds.Any())
            {
                return BadRequest(new { success = false, message = "Sinh viên đã thuộc hội đồng khác" });
            }

            // Check if any student's advisor is part of the council advisors
            var studentAdvisorIds = students
                .Select(s => s.AdvisorID)
                .Distinct()
                .ToList();

            var conflictAdvisorIds = advisors
                .Where(a => studentAdvisorIds.Contains(a.ID))
                .Select(a => a.ID)
                .ToList();

            if (conflictAdvisorIds.Any())
            {
                return BadRequest(new { success = false, message = "Thành viên hội đồng không được trùng với giảng viên hướng dẫn của sinh viên nghiệm thu" });
            }

            var newCouncil = new Council
            {
                Name = council.Name,
                DepartmentID = council.DepartmentID,
                Location = council.Location,
                ReviewTime = council.ReviewTime,
                Students = students,
                Advisors = advisors,
                CreatedAt = DateTime.Now,
            };

            foreach (var student in students)
            {
                student.Status = "Chờ bảo vệ";
            }

            _context.Council.Add(newCouncil);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Tạo hội đồng thành công" });
        }

        // DELETE: api/Council/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCouncil(int id)
        {
            var council = await _context.Council
                .Include(c => c.Students)
                .Include(c => c.Advisors)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (council == null)
            {
                return NotFound(new { success = false, message = "Hội đồng không tồn tại" });
            }

            foreach (var student in council.Students)
            {
                student.Status = "Đang thực tập";
            }

            // Xóa mối quan hệ với Students và Advisors
            council.Students.Clear();
            council.Advisors.Clear();

            _context.Council.Remove(council);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Xóa hội đồng thành công" });
        }

        private bool CouncilExists(int id)
        {
            return _context.Council.Any(e => e.ID == id);
        }
    }
}
