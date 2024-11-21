using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VicemAPI.Data;
using VicemAPI.DTOs.Student;
using VicemAPI.DTOs;
using VicemAPI.DTOs.Advisor;
using VicemAPI.Models.Entities;
using VicemAPI.DTOs.Topic;
using VicemAPI.DTOs.Council;


namespace VicemAPI.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudent(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10,
        [FromQuery] string keyword = "",
        [FromQuery] int? advisorId = null,
        [FromQuery] int? departmentId = null,
        [FromQuery] string? status = null,
        [FromQuery] string? type = "")
        {
            var query = _context.Student
                .Include(s => s.Department)
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .Include(s => s.Topic)
                .Include(s => s.Council)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => s.Name.Contains(keyword) || s.Code.Contains(keyword) || s.Email.Contains(keyword));
            }

            if (advisorId.HasValue)
            {
                query = query.Where(s => s.AdvisorID == advisorId.Value);
            }

            if (departmentId.HasValue)
            {
                query = query.Where(s => s.DepartmentID == departmentId.Value);
            }

            if (!string.IsNullOrEmpty(status) && status != "Tất cả")
            {
                query = query.Where(s => s.Status == status);
            }

            if (type == "createCouncil")
            {
                query = query.Where(s =>
                    s.Topic != null &&
                    s.Status == "Đang thực tập");
            }

            var filteredQuery = query; // Lưu truy vấn sau khi lọc cho `statusCounts`

            // Tính toán tổng quan trạng thái sau khi áp dụng các bộ lọc
            var statusCounts = await filteredQuery
                .GroupBy(s => s.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                })
                .ToListAsync();

            // Tạo danh sách tổng quan trạng thái
            var statusSummary = new List<object>
    {
        new { label = "Tất cả", count = await filteredQuery.CountAsync() },
        new { label = "Mới", count = statusCounts.FirstOrDefault(x => x.Status == "Mới")?.Count ?? 0 },
        new { label = "Đang thực tập", count = statusCounts.FirstOrDefault(x => x.Status == "Đang thực tập")?.Count ?? 0 },
        new { label = "Chờ bảo vệ", count = statusCounts.FirstOrDefault(x => x.Status == "Chờ bảo vệ")?.Count ?? 0 },
        new { label = "Hoàn thành", count = statusCounts.FirstOrDefault(x => x.Status == "Hoàn thành")?.Count ?? 0 },
        new { label = "Không đạt", count = statusCounts.FirstOrDefault(x => x.Status == "Không đạt")?.Count ?? 0 },
    };

            var totalRecords = await query.CountAsync();

            var students = await query
                .Select(s => new StudentDTO
                {
                    ID = s.ID,
                    Name = s.Name,
                    Code = s.Code,
                    Address = s.Address,
                    DateOfBirth = s.DateOfBirth,
                    Email = s.Email,
                    Phone = s.Phone,
                    Class = s.Class,
                    Course = s.Course,
                    Description = s.Description,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    DepartmentID = s.Department!.ID,
                    Department = new DepartmentDTO
                    {
                        ID = s.Department.ID,
                        Name = s.Department.Name,
                        Code = s.Department.Code,
                    },
                    AdvisorID = s.Advisor != null ? s.Advisor.ID : (int?)null,
                    Advisor = s.Advisor == null ? null : new AdvisorSimpleDTO
                    {
                        ID = s.Advisor.ID,
                        Name = s.Advisor.Name,
                        Code = s.Advisor.Code,
                    },
                    CompanyID = s.CompanyID,
                    Company = s.Company,
                    TopicID = s.TopicID,
                    Topic = s.Topic == null ? null : new TopicDto
                    {
                        ID = s.Topic.ID,
                        Name = s.Topic.Name,
                        Description = s.Topic.Description,
                        Status = s.Topic.Status,
                    },
                    CouncilID = s.CouncilID,
                    Council = s.Council == null ? null : new CouncilSimpleDto
                    {
                        ID = s.Council.ID,
                        Name = s.Council.Name,
                        Description = s.Council.Description,
                        ReviewTime = s.Council.ReviewTime,
                        Location = s.Council.Location,
                        DepartmentID = s.Council.DepartmentID,
                    },
                    Status = s.Status,
                    FirstScore = s.FirstScore,
                    SecondScore = s.SecondScore,
                    ThirdScore = s.ThirdScore,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                })
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)limit);

            var response = new
            {
                Items = students,
                PageNumber = page,
                PageSize = limit,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Counts = statusSummary,
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("dashboards")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetCountStudent()
        {
            var query = _context.Student
                .Include(s => s.Department)
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .Include(s => s.Topic)
                .Include(s => s.Council)
                .AsQueryable();

            var filteredQuery = query; // Lưu truy vấn sau khi lọc cho `statusCounts`

            // Tính toán tổng quan trạng thái sau khi áp dụng các bộ lọc
            var statusCounts = await filteredQuery
                .GroupBy(s => s.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                })
                .ToListAsync();

            // Tạo danh sách tổng quan trạng thái
            var statusSummary = new
            {
                all = await filteredQuery.CountAsync(),
                newCount = statusCounts.FirstOrDefault(x => x.Status == "Mới")?.Count ?? 0,
                during = statusCounts.FirstOrDefault(x => x.Status == "Đang thực tập")?.Count ?? 0,
                waiting = statusCounts.FirstOrDefault(x => x.Status == "Chờ bảo vệ")?.Count ?? 0,
                done = statusCounts.FirstOrDefault(x => x.Status == "Hoàn thành")?.Count ?? 0,
                failed = statusCounts.FirstOrDefault(x => x.Status == "Không đạt")?.Count ?? 0,
            };

            return Ok(statusSummary);
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            var student = await _context.Student
                .Include(s => s.Department)
                .Include(s => s.Advisor)
                .Include(s => s.Company)
                .Include(s => s.Council)
                .Where(s => s.ID == id)
                .Select(s => new StudentDTO
                {
                    ID = s.ID,
                    Name = s.Name,
                    Code = s.Code,
                    Address = s.Address,
                    DateOfBirth = s.DateOfBirth,
                    Email = s.Email,
                    Phone = s.Phone,
                    Class = s.Class,
                    Course = s.Course,
                    Description = s.Description,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    DepartmentID = s.Department!.ID,
                    Department = new DepartmentDTO
                    {
                        ID = s.Department.ID,
                        Name = s.Department.Name,
                        Code = s.Department.Code,
                    },
                    AdvisorID = s.Advisor != null ? s.Advisor.ID : (int?)null,
                    Advisor = s.Advisor == null ? null : new AdvisorSimpleDTO
                    {
                        ID = s.Advisor.ID,
                        Name = s.Advisor.Name,
                        Code = s.Advisor.Code,
                    },
                    CompanyID = s.CompanyID,
                    Company = s.Company,
                    TopicID = s.TopicID,
                    Topic = s.Topic == null ? null : new TopicDto
                    {
                        ID = s.Topic.ID,
                        Name = s.Topic.Name,
                        Description = s.Topic.Description,
                        Status = s.Topic.Status,
                    },
                    CouncilID = s.CouncilID,
                    Council = s.Council == null ? null : new CouncilSimpleDto
                    {
                        ID = s.Council.ID,
                        Name = s.Council.Name,
                        Description = s.Council.Description,
                        ReviewTime = s.Council.ReviewTime,
                        Location = s.Council.Location,
                        DepartmentID = s.Council.DepartmentID,
                    },
                    Status = s.Status,
                    FirstScore = s.FirstScore,
                    SecondScore = s.SecondScore,
                    ThirdScore = s.ThirdScore,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound($"Không tìm thấy sinh viên với ID: {id}");
            }

            return Ok(student);
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.ID)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { success = true, message = "Cập nhật thành công" });
        }

        // PUT: api/Student/5/Advisor
        [HttpPut("{id}/advisor")]
        public async Task<IActionResult> UpdateStudentAdvisor(int id, int advisorId)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { success = false, message = "Sinh viên không tồn tại" });
            }

            var advisor = await _context.Advisor
                                        .Include(a => a.Students)
                                        .FirstOrDefaultAsync(a => a.ID == advisorId);
            if (advisor == null)
            {
                return NotFound(new { success = false, message = "Cố vấn không tồn tại" });
            }

            if (advisor.CurrentSlot >= advisor.MaxSlot)
            {
                return BadRequest(new { success = false, message = "Cố vấn đã đạt đến giới hạn sinh viên" });
            }

            student.AdvisorID = advisorId;
            student.UpdatedAt = DateTime.Now;

            if (advisor.Students == null)
            {
                advisor.Students = new List<Student>();
            }

            if (!advisor.Students.Any(s => s.ID == student.ID))
            {
                advisor.Students.Add(student);
                advisor.CurrentSlot++;
            }

            _context.Entry(student).State = EntityState.Modified;
            _context.Entry(advisor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound(new { success = false, message = "Sinh viên không tồn tại" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { success = true, message = "Đăng ký GVHD thành công" });
        }

        [HttpPut("{id}/company")]
        public async Task<IActionResult> UpdateStudentCompany(int id, Company company)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { success = false, message = "Sinh viên không tồn tại" });
            }

            student.CompanyID = company.ID;
            student.Company = company;
            student.Company.Status = "Chờ duyệt địa điểm";
            student.UpdatedAt = DateTime.Now;
            student.StartDate = company.StartDate;
            student.EndDate = company.EndDate;

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound(new { success = false, message = "Sinh viên không tồn tại" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(student);
        }

        [HttpPut("{id}/company/approve")]
        public async Task<IActionResult> UpdateStudentCompany(int id, string type)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { success = false, message = "Sinh viên không tồn tại" });
            }

            if (type == "reject")
            {
                var company = await _context.Company.FindAsync(student.CompanyID);
                if (company != null)
                {
                    _context.Company.Remove(company);
                }

                student.CompanyID = null;
                student.Company = null;
                student.UpdatedAt = DateTime.Now;
            }
            else if (type == "approve")
            {
                var company = await _context.Company.FindAsync(student.CompanyID);
                if (company != null)
                {
                    company.Status = "Đã duyệt";
                }

                student.Status = "Đang thực tập";
                student.UpdatedAt = DateTime.Now;
            }
            else
            {
                return BadRequest(new { success = false, message = "Loại phê duyệt không hợp lệ" });
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound(new { success = false, message = "Sinh viên không tồn tại" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { success = true, message = "Phê duyệt thực tập thành công" });
        }

        [HttpPut("{id}/topic")]
        public async Task<IActionResult> UpdateStudentTopic(int id, TopicDto topicDto)
        {
            // Validate model
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList(),
                });
            }

            // Tìm sinh viên theo ID và bao gồm cả Topic
            var student = await _context.Student
                .Include(s => s.Topic)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (student == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Sinh viên không tồn tại",
                });
            }

            // Kiểm tra nếu Student đã có Topic
            if (student.Topic != null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Sinh viên đã có topic và không thể cập nhật topic mới.",
                });
            }

            // Tạo topic mới
            var topic = new Topic
            {
                Name = topicDto.Name,
                Description = topicDto.Description,
                CreatedAt = DateTime.Now,
            };

            // Cập nhật thông tin
            student.Topic = topic;
            student.UpdatedAt = DateTime.Now;

            // Thêm và cập nhật vào database
            _context.Topic.Add(topic);
            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Gửi duyệt đề tài thành công" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Sinh viên không tồn tại",
                    });
                }

                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi cập nhật dữ liệu",
                    error = ex.Message,
                });
            }
        }

        [HttpPut("{id}/topic/approve")]
        public async Task<IActionResult> UpdateStudentTopic(int id, string type)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { success = false, message = "Sinh viên không tồn tại" });
            }

            if (type == "reject")
            {
                var topic = await _context.Topic.FindAsync(student.TopicID);
                if (topic != null)
                {
                    _context.Topic.Remove(topic);
                }

                student.TopicID = null;
                student.Topic = null;
                student.UpdatedAt = DateTime.Now;
            }
            else if (type == "approve")
            {
                var topic = await _context.Topic.FindAsync(student.TopicID);
                if (topic != null)
                {
                    topic.Status = "Đã duyệt";
                }

                student.UpdatedAt = DateTime.Now;
            }
            else
            {
                return BadRequest(new { success = false, message = "Loại phê duyệt không hợp lệ" });
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound(new { success = false, message = "Sinh viên không tồn tại" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { success = true, message = "Phê duyệt đề tài thành công" });
        }

        [HttpPut("{id}/score")]
        public async Task<IActionResult> UpdateStudentScore(int id, ScoreDtos score)
        {
            string isValidScore = ValidateScores(score.FirstScore, score.SecondScore, score.ThirdScore);

            if (isValidScore != "Hợp lệ")
            {
                return BadRequest(new { success = false, message = isValidScore });
            }

            // Tìm sinh viên
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { success = false, message = "Sinh viên không tồn tại" });
            }

            // Cập nhật thông tin
            student.FirstScore = score.FirstScore;
            student.SecondScore = score.SecondScore;
            student.ThirdScore = score.ThirdScore;
            student.Status = IsAverageLessThanFour(score.FirstScore, score.SecondScore, score.ThirdScore) ? "Không đạt" : "Hoàn thành";
            student.UpdatedAt = DateTime.Now;

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound(new { success = false, message = "Sinh viên không tồn tại" });
                }
                else
                {
                    throw;
                }
            }

            // Trả về phản hồi sau khi cập nhật
            return Ok(new
            {
                success = true,
                message = "Cập nhật điểm thành công",
                data = new
                {
                    firstScore = student.FirstScore,
                    secondScore = student.SecondScore,
                    thirdScore = student.ThirdScore,
                },
            });
        }

        // POST: api/Student
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(CreateStudentDto studentDto)
        {
            if (_context.Student.Any(s => s.Code == studentDto.Code))
            {
                return BadRequest(new { success = false, message = "Mã sinh viên đã tồn tại" });
            }

            if (_context.Student.Any(s => s.Phone == studentDto.Phone))
            {
                return BadRequest(new { success = false, message = "Số điện thoại đã tồn tại" });
            }

            if (_context.Student.Any(s => s.Email == studentDto.Email))
            {
                return BadRequest(new { success = false, message = "Email đã tồn tại" });
            }

            var student = new Student
            {
                Name = studentDto.Name,
                Code = studentDto.Code,
                Address = studentDto.Address,
                DateOfBirth = studentDto.DateOfBirth,
                Email = studentDto.Email,
                Phone = studentDto.Phone,
                Class = studentDto.Class,
                Course = studentDto.Course,
                DepartmentID = studentDto.DepartmentID,
                AdvisorID = studentDto.AdvisorID,
                Description = studentDto.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _context.Student.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.ID }, student);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Xóa sinh viên thành công" });
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }

        private string ValidateScores(float firstScore, float secondScore, float? thirdScore)
        {
            // Kiểm tra giá trị điểm từ 0 đến 10
            if (firstScore < 0 || firstScore > 10)
            {
                return "Điểm thứ nhất không hợp lệ.";
            }

            if (secondScore < 0 || secondScore > 10)
            {
                return "Điểm thứ hai không hợp lệ.";
            }

            if (thirdScore.HasValue && (thirdScore < 0 || thirdScore > 10))
            {
                return "Điểm thứ ba không hợp lệ.";
            }

            // Kiểm tra chênh lệch giữa các điểm, nếu chênh lệch lớn hơn 2 thì trả về thông báo chung
            if (Math.Abs(firstScore - secondScore) > 2 ||
                (thirdScore.HasValue && (Math.Abs(firstScore - thirdScore.Value) > 2 || Math.Abs(secondScore - thirdScore.Value) > 2)))
            {
                return "Điểm số không được chênh lệch quá 2 điểm.";
            }

            // Nếu tất cả điều kiện đều hợp lệ
            return "Hợp lệ";
        }

        private bool IsAverageLessThanFour(double firstScore, double secondScore, double? thirdScore)
        {
            // Kiểm tra giá trị của thirdScore
            double average;
            if (thirdScore.HasValue)
            {
                // Tính trung bình cộng 3 số nếu thirdScore có giá trị
                average = (firstScore + secondScore + thirdScore.Value) / 3;
            }
            else
            {
                // Tính trung bình cộng 2 số nếu thirdScore là null
                average = (firstScore + secondScore) / 2;
            }

            // Kiểm tra trung bình cộng
            return average < 4.0;
        }
    }
}
