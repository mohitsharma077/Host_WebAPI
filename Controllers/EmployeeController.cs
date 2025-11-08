using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.ID)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.ID }, employee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }



        ////excel file as an input API
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> UpsertEmployeeViaExcel([FromForm] FileUploadDto upload)
        //{
        //    var obj = new ApiResponse
        //    {
        //        status_code = 500,
        //        success = false,
        //        message = "Something went wrong",
        //        total_records = 0,
        //        data = null,
        //        inserted_employees = 0,
        //        updated_employees = 0,
        //    };

        //    try
        //    {
        //        var file = upload.File;
        //        if (file == null || file.Length == 0)
        //            return BadRequest("File not selected.");

        //        var employees = new List<Employee>();
        //        int inserted_employees = 0, updated_employees = 0;

        //        //OpenXML
        //        using (var stream = new MemoryStream())
        //        {
        //            await file.CopyToAsync(stream);
        //            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, false))
        //            {
        //                WorkbookPart workbookPart = doc.WorkbookPart!;
        //                Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();
        //                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id!);
        //                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

        //                var rows = sheetData.Descendants<Row>().Skip(1); // skip header

        //                foreach (Row row in rows)
        //                {
        //                    var id = GetCellValue(workbookPart, row, 1);
        //                    var age = GetCellValue(workbookPart, row, 3);
        //                    var salary = GetCellValue(workbookPart, row, 5);
        //                    Employee emp = new Employee
        //                    {
        //                        ID = !String.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0,
        //                        Name = GetCellValue(workbookPart, row, 2),
        //                        Age = !String.IsNullOrEmpty(age) ? Convert.ToInt32(age) : 0,
        //                        Address = GetCellValue(workbookPart, row, 4),
        //                        Salary = !String.IsNullOrEmpty(salary) ? Convert.ToInt32(salary) : 0,
        //                        Designation = GetCellValue(workbookPart, row, 6),
        //                        Is_Active = GetCellValue(workbookPart, row, 7) == "1",
        //                        Created_By = GetCellValue(workbookPart, row, 8),
        //                        Created_Date = DateTime.Now,
        //                        Modified_By = GetCellValue(workbookPart, row, 9),
        //                        Modified_Date = DateTime.Now
        //                    };

        //                    employees.Add(emp);
        //                }
        //            }
        //        }

        //        foreach (var emp in employees)
        //        {
        //            var existing = await _context.Employees.FindAsync(emp.ID);
        //            if (existing != null)
        //            {
        //                // Update existing
        //                existing.Name = emp.Name;
        //                existing.Age = emp.Age;
        //                existing.Address = emp.Address;
        //                existing.Salary = emp.Salary;
        //                existing.Designation = emp.Designation;
        //                existing.Is_Active = emp.Is_Active;
        //                existing.Modified_By = emp.Modified_By;
        //                existing.Modified_Date = DateTime.Now;

        //                _context.Employees.Update(existing);
        //                updated_employees++;
        //            }
        //            else
        //            {
        //                // Insert new
        //                _context.Employees.Add(emp);
        //                inserted_employees++;
        //            }
        //        }

        //        await _context.SaveChangesAsync();

        //        return Ok(new ApiResponse
        //        {
        //            status_code = 200,
        //            success = true,
        //            message = "Excel processed successfully",
        //            total_records = employees.Count,
        //            data = null,
        //            inserted_employees = inserted_employees,
        //            updated_employees = updated_employees
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new ApiResponse
        //        {
        //            status_code = 500,
        //            success = false,
        //            message = ex.Message,
        //            total_records = 0,
        //            inserted_employees = 0,
        //            updated_employees = 0,
        //            data = null
        //        });
        //    }
        //}

        //private string GetCellValue(WorkbookPart workbookPart, Row row, int cellIndex)
        //{
        //    Cell cell = row.Elements<Cell>().ElementAtOrDefault(cellIndex - 1);
        //    if (cell == null)
        //        return string.Empty;

        //    string value = cell.InnerText;

        //    if (cell.DataType != null && cell.DataType == CellValues.SharedString)
        //    {
        //        var stringTable = workbookPart.SharedStringTablePart.SharedStringTable;
        //        value = stringTable.ElementAt(int.Parse(value)).InnerText;
        //    }

        //    return value;
        //}


    }
}
