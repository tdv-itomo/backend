using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VicemAPI.Data;
using VicemAPI.DTOs;
using VicemAPI.DTOs.Advisor;
using VicemAPI.DTOs.Student;
using VicemAPI.Models.Entities;

namespace VicemAPI.Controllers
{
    [Route("api/advisor")]
    [ApiController]
    public class AdvisorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdvisorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Advisor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvisorDTO>>> GetAllAdvisor([FromQuery] int? departmentId = null)
        {
            var query = _context.Advisor
                                .Include(a => a.Department)
                                .AsQueryable();

            // Optional Department filtering
            if (departmentId.HasValue)
            {
                query = query.Where(a => a.DepartmentID == departmentId.Value);
            }

            var advisors = await query
                                .Select(a => new AdvisorDTO
                                {
                                    ID = a.ID,
                                    Name = a.Name,
                                    Code = a.Code,
                                    Address = a.Address,
                                    DateOfBirth = a.DateOfBirth,
                                    Email = a.Email,
                                    Phone = a.Phone,
                                    Description = a.Description,
                                    CreatedAt = a.CreatedAt,
                                    UpdatedAt = a.UpdatedAt,
                                    DepartmentID = a.Department!.ID,
                                    Department = new DepartmentDTO
                                    {
                                        ID = a.Department.ID,
                                        Name = a.Department.Name,
                                        Code = a.Department.Code,
                                    },
                                    Students = new List<StudentDTO>(),
                                    CurrentSlot = a.CurrentSlot,
                                    MaxSlot = a.MaxSlot,
                                })
                                .ToListAsync();

            return Ok(advisors);
        }

        // GET: api/Advisor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdvisorDTO>> GetAdvisor(int id)
        {
            var advisor = await _context.Advisor
                                        .Include(a => a.Department) // Lấy thông tin của Department
                                        .Where(a => a.ID == id)
                                        .Select(a => new AdvisorDTO
                                        {
                                            ID = a.ID,
                                            Name = a.Name,
                                            Code = a.Code,
                                            Address = a.Address,
                                            DateOfBirth = a.DateOfBirth,
                                            Email = a.Email,
                                            Phone = a.Phone,
                                            Description = a.Description,
                                            CreatedAt = a.CreatedAt,
                                            UpdatedAt = a.UpdatedAt,
                                            DepartmentID = a.Department!.ID, // Thêm DepartmentID
                                            Department = new DepartmentDTO
                                            {
                                                ID = a.Department.ID,
                                                Name = a.Department.Name,
                                                Code = a.Department.Code,
                                            },
                                        })
                                        .FirstOrDefaultAsync();

            if (advisor == null)
            {
                return NotFound();
            }

            return Ok(advisor);
        }

        // PUT: api/Advisor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdvisor(int id, AdvisorDTO advisorDto)
        {
            if (id != advisorDto.ID)
            {
                return BadRequest();
            }

            var advisor = await _context.Advisor.FindAsync(id);
            if (advisor == null)
            {
                return NotFound();
            }

            advisor.Name = advisorDto.Name;
            advisor.Code = advisorDto.Code;
            advisor.Address = advisorDto.Address;
            advisor.DateOfBirth = advisorDto.DateOfBirth;
            advisor.Email = advisorDto.Email;
            advisor.Phone = advisorDto.Phone;
            advisor.Description = advisorDto.Description;
            advisor.MaxSlot = advisorDto.MaxSlot;
            advisor.UpdatedAt = DateTime.Now;

            if (advisorDto.DepartmentID != advisor.DepartmentID)
            {
                var department = await _context.Department.FindAsync(advisorDto.DepartmentID);
                if (department != null)
                {
                    advisor.DepartmentID = advisorDto.DepartmentID;
                    advisor.Department = department;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvisorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { success = true, message = "Cập nhật giảng viên thành công" });
        }

        // POST: api/Advisor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/Advisor
        [HttpPost]
        public async Task<ActionResult<AdvisorDTO>> PostAdvisor(AdvisorDTO advisorDto)
        {
            // Kiểm tra xem DepartmentID có hợp lệ không
            var department = await _context.Department.FindAsync(advisorDto.DepartmentID);

            if (department == null)
            {
                return BadRequest("Department not found.");
            }

            if (advisorDto.MaxSlot <= 0)
            {
                return BadRequest("Số lượng sinh viên hướng dẫn không hợp lệ");
            }

            var advisor = new Advisor
            {
                Name = advisorDto.Name,
                Code = advisorDto.Code,
                Address = advisorDto.Address,
                DateOfBirth = advisorDto.DateOfBirth,
                Email = advisorDto.Email,
                Phone = advisorDto.Phone,
                Description = advisorDto.Description,
                CurrentSlot = 0,
                MaxSlot = advisorDto.MaxSlot,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                DepartmentID = advisorDto.DepartmentID,
            };

            _context.Advisor.Add(advisor);
            await _context.SaveChangesAsync();

            var createdAdvisorDto = new AdvisorDTO
            {
                Name = advisor.Name,
                Code = advisor.Code,
                Address = advisor.Address,
                DateOfBirth = advisor.DateOfBirth,
                Email = advisor.Email,
                Phone = advisor.Phone,
                Description = advisor.Description,
                CreatedAt = advisor.CreatedAt,
                UpdatedAt = advisor.UpdatedAt,
                DepartmentID = advisor.DepartmentID,
                CurrentSlot = advisor.CurrentSlot,
                MaxSlot = advisor.MaxSlot,
                Department = new DepartmentDTO
                {
                    ID = department.ID,
                    Name = department.Name,
                    Code = department.Code,
                },
            };

            return CreatedAtAction("GetAdvisor", new { id = advisor.ID }, createdAdvisorDto);
        }

        // DELETE: api/Advisor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvisor(int id)
        {
            var advisor = await _context.Advisor.FindAsync(id);
            if (advisor == null)
            {
                return NotFound();
            }

            _context.Advisor.Remove(advisor);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Xóa giảng viên thành công" });
        }

        private bool AdvisorExists(int id)
        {
            return _context.Advisor.Any(e => e.ID == id);
        }
    }
}
