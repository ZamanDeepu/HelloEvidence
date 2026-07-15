# ASP.NET Core MVC Code First Master-Details

`Models/DbModels.cs`.

using System.ComponentModel.DataAnnotations;

namespace SMS.Models;

public class Department
{
public int Id { get; set; }

    [Required, StringLength(30)]
    public string Name { get; set; } = "";

    public List<Student> Students { get; set; } = new();

}

public class Student
{
public int Id { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = "";

    [DataType(DataType.Date)]
    public DateTime AdmissionDate { get; set; } = DateTime.Today;

    public bool Active { get; set; } = true;

    public string? Image { get; set; }

    [Display(Name = "Department")]
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public List<Subject> Subjects { get; set; } = new();

}

public class Subject
{
public int Id { get; set; }

    [StringLength(40)]
    public string? Course { get; set; }

    [Range(0, 5)]
    public decimal Credit { get; set; }

    public int StudentId { get; set; }
    public Student? Student { get; set; }

}

`Models/StudentVM.cs`.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models;

public class StudentVM
{
public int Id { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = "";

    [DataType(DataType.Date)]
    public DateTime AdmissionDate { get; set; } = DateTime.Today;

    public bool Active { get; set; } = true;

    [Display(Name = "Photo")]
    public IFormFile? Photo { get; set; }

    public string? OldImage { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Select department")]
    [Display(Name = "Department")]
    public int DepartmentId { get; set; }

    public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
    public List<Subject> Subjects { get; set; } = new();

}

public class RowVM
{
public int Index { get; set; }
public Subject Subject { get; set; } = new();
}

`Data/AppDbContext.cs`.

using Microsoft.EntityFrameworkCore;
using SMS.Models;

namespace SMS.Data;

public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Subject> Subjects => Set<Subject>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Department>().HasData(
            new Department { Id = 1, Name = "CSE" },
            new Department { Id = 2, Name = "EEE" },
            new Department { Id = 3, Name = "BBA" }
        );
    }

}

`appsettings.json`.

"ConnectionStrings": {
"con": "Server=.;Database=SMS_CodeFirst;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}

`Program.cs`.

builder.Services.AddDbContext<AppDbContext>(o =>
o.UseSqlServer(builder.Configuration.GetConnectionString("con")));

app.MapControllerRoute(
name: "student",
pattern: "Student/{action=Index}/{id?}",
defaults: new { controller = "Students" });

`Controllers/StudentsController.cs`.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMS.Data;
using SMS.Models;

namespace SMS.Controllers;

public class StudentsController : Controller
{
private readonly AppDbContext db;
private readonly IWebHostEnvironment env;

    public StudentsController(AppDbContext db, IWebHostEnvironment env)
    {
        this.db = db;
        this.env = env;
    }

    public IActionResult Index()
    {
        var students = db.Students
            .Include(s => s.Department)
            .Include(s => s.Subjects)
            .OrderBy(s => s.Name)
            .ToList();

        return View(students);
    }

    public IActionResult Create()
    {
        return View(Fill(new StudentVM()));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(StudentVM vm)
    {
        if (!ModelState.IsValid) return View(Fill(vm));

        var s = new Student
        {
            Name = vm.Name,
            AdmissionDate = vm.AdmissionDate,
            Active = vm.Active,
            DepartmentId = vm.DepartmentId,
            Image = SaveImage(vm.Photo),
            Subjects = Clean(vm.Subjects)
        };

        db.Students.Add(s);
        db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var s = db.Students.Include(x => x.Subjects).FirstOrDefault(x => x.Id == id);
        if (s == null) return NotFound();

        var vm = new StudentVM
        {
            Id = s.Id,
            Name = s.Name,
            AdmissionDate = s.AdmissionDate,
            Active = s.Active,
            DepartmentId = s.DepartmentId,
            OldImage = s.Image,
            Subjects = s.Subjects.ToList()
        };

        return View(Fill(vm));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(StudentVM vm)
    {
        if (!ModelState.IsValid) return View(Fill(vm));

        var s = db.Students.Include(x => x.Subjects).FirstOrDefault(x => x.Id == vm.Id);
        if (s == null) return NotFound();

        s.Name = vm.Name;
        s.AdmissionDate = vm.AdmissionDate;
        s.Active = vm.Active;
        s.DepartmentId = vm.DepartmentId;
        s.Image = SaveImage(vm.Photo) ?? vm.OldImage;

        db.Subjects.RemoveRange(s.Subjects);
        s.Subjects = Clean(vm.Subjects);

        db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var s = db.Students.Find(id);
        if (s != null)
        {
            db.Students.Remove(s);
            db.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }

    private StudentVM Fill(StudentVM vm)
    {
        if (vm.Subjects.Count == 0)
            vm.Subjects.Add(new Subject());

        vm.Departments = new SelectList(db.Departments.OrderBy(d => d.Name), "Id", "Name", vm.DepartmentId);
        return vm;
    }

    private static List<Subject> Clean(List<Subject> rows)
    {
        return rows
            .Where(x => !string.IsNullOrWhiteSpace(x.Course))
            .Select(x => new Subject { Course = x.Course!, Credit = x.Credit })
            .ToList();
    }

    private string? SaveImage(IFormFile? photo)
    {
        if (photo == null || photo.Length == 0) return null;

        var folder = Path.Combine(env.WebRootPath, "images");
        Directory.CreateDirectory(folder);

        var file = Guid.NewGuid() + Path.GetExtension(photo.FileName);
        using var stream = new FileStream(Path.Combine(folder, file), FileMode.Create);
        photo.CopyTo(stream);

        return "/images/" + file;
    }

}

`ViewComponents/SubjectRowViewComponent.cs`.

using Microsoft.AspNetCore.Mvc;
using SMS.Models;

namespace SMS.ViewComponents;

public class SubjectRowViewComponent : ViewComponent
{
public IViewComponentResult Invoke(int index, Subject subject)
{
return View(new RowVM { Index = index, Subject = subject });
}
}

`Views/Shared/Components/SubjectRow/Default.cshtml`.

@model SMS.Models.RowVM

<tr>
    <td>
        <input type="hidden" name="Subjects.Index" value="@Model.Index" />
        <input name="Subjects[@Model.Index].Course"
               value="@Model.Subject.Course"
               class="form-control" />
    </td>
    <td>
        <input name="Subjects[@Model.Index].Credit"
               value="@Model.Subject.Credit"
               type="number" step="0.25" min="0" max="5"
               class="form-control" />
    </td>
    <td>
        <button type="button" class="delSub">X</button>
    </td>
</tr>

`Views/Students/Index.cshtml`.

@model IEnumerable<SMS.Models.Student>

<h2>Students</h2>

<p>
    <a asp-action="Create" class="btn btn-primary">Add Student</a>
</p>

<table class="table table-bordered table-sm">
    <tr>
        <th>Photo</th>
        <th>Name</th>
        <th>Department</th>
        <th>Date</th>
        <th>Active</th>
        <th>Subjects</th>
        <th>Action</th>
    </tr>

    @foreach (var s in Model)
    {
        <tr>
            <td>
                @if (s.Image != null)
                {
                    <img src="@s.Image" height="45" />
                }
            </td>
            <td>@s.Name</td>
            <td>@s.Department?.Name</td>
            <td>@s.AdmissionDate.ToString("dd-MMM-yyyy")</td>
            <td>@(s.Active ? "Yes" : "No")</td>
            <td>@string.Join(", ", s.Subjects.Select(x => x.Course + " " + x.Credit))</td>
            <td>
                <a asp-action="Edit" asp-route-id="@s.Id">Edit</a>

                <form asp-action="Delete" asp-route-id="@s.Id" method="post" style="display:inline">
                    @Html.AntiForgeryToken()
                    <button>Delete</button>
                </form>
            </td>
        </tr>
    }

</table>

`Views/Students/_StudentForm.cshtml`.

@model SMS.Models.StudentVM

<div asp-validation-summary="ModelOnly" class="text-danger"></div>

<input type="hidden" asp-for="Id" />
<input type="hidden" asp-for="OldImage" />

<p>
    <label asp-for="Name"></label><br />
    <input asp-for="Name" />
    <span asp-validation-for="Name" class="text-danger"></span>
</p>

<p>
    <label asp-for="AdmissionDate"></label><br />
    <input asp-for="AdmissionDate" type="date" value="@Model.AdmissionDate.ToString("yyyy-MM-dd")" />
</p>

<p>
    <label asp-for="DepartmentId"></label><br />
    <select asp-for="DepartmentId" asp-items="Model.Departments">
        <option value="">Select Department</option>
    </select>
    <span asp-validation-for="DepartmentId" class="text-danger"></span>
</p>

<p>
    <label asp-for="Photo"></label><br />
    <input asp-for="Photo" type="file" />
    @if (Model.OldImage != null)
    {
        <br />
        <img src="@Model.OldImage" height="60" />
    }
</p>

<p>
    <input asp-for="Active" />
    <label asp-for="Active"></label>
</p>

<h4>Subjects</h4>

<table class="table table-sm">
    <tr>
        <th>Course</th>
        <th>Credit</th>
        <th></th>
    </tr>

    <tbody id="subBody">
        @for (int i = 0; i < Model.Subjects.Count; i++)
        {
            @await Component.InvokeAsync("SubjectRow", new { index = i, subject = Model.Subjects[i] })
        }
    </tbody>

</table>

<button type="button" id="addSub">Add Subject</button>

<template id="subTemplate">
    <tr>
        <td>
            <input type="hidden" name="Subjects.Index" value="__i__" />
            <input name="Subjects[__i__].Course" class="form-control" />
        </td>
        <td>
            <input name="Subjects[__i__].Credit" type="number" step="0.25" min="0" max="5" class="form-control" />
        </td>
        <td>
            <button type="button" class="delSub">X</button>
        </td>
    </tr>
</template>

<script>
    let i = @Model.Subjects.Count;

    document.getElementById('addSub').onclick = function () {
        let row = document.getElementById('subTemplate').innerHTML.replaceAll('__i__', i++);
        document.getElementById('subBody').insertAdjacentHTML('beforeend', row);
    };

    document.addEventListener('click', function (e) {
        if (e.target.classList.contains('delSub'))
            e.target.closest('tr').remove();
    });
</script>

`Views/Students/Create.cshtml`.

@model SMS.Models.StudentVM

<h2>Create Student</h2>

<form asp-action="Create" method="post" enctype="multipart/form-data">
    @Html.AntiForgeryToken()
    <partial name="_StudentForm" model="Model" />
    <button>Save</button>
    <a asp-action="Index">Back</a>
</form>
