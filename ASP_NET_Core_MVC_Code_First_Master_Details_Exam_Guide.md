# ASP.NET Core MVC Code First Master-Details Exam Guide

## 1. Create a new project in Visual Studio

Use your own project namespace. In this guide I use `SMS`. Replace every `SMS` with your project name.

| Requirement      | Where It Is Used                                 |
| ---------------- | ------------------------------------------------ |
| ASP.NET Core MVC | ASP.NET Core Web App (Model-View-Controller)     |
| Code First       | Models + DbContext + Migration                   |
| Model            | Department, Student, Subject                     |
| Data Annotations | Required, StringLength, DataType, Range, Display |
| View Model       | StudentVM                                        |
| Partial View     | `_StudentForm.cshtml`                            |
| View Component   | `SubjectRowViewComponent`                        |
| Custom Routing   | `/Student`, `/Student/Edit/1`                    |
| Text             | Name, Course                                     |
| Number           | Credit                                           |
| Boolean          | Active                                           |
| Date             | AdmissionDate                                    |
| Image            | Photo upload                                     |
| Relational Data  | DepartmentId, StudentId                          |

Memory line:

```
DSS + VM + PVC + ROUTE
Department Student Subject + ViewModel + Partial View Component + Route
```

## 2. Install Packages

Tools > NuGet Package Manager >

```
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
```

## 3. Models

Create `Models/DbModels.cs`.

```csharp
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
```

Memory:

```
Department has Students.
Student has Subjects, DepartmentId.
Subject has StudentId.
```

## 4. View Model

Create `Models/StudentVM.cs`.

```csharp
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
```

Memory:

```
StudentVM = Student form + Photo + OldImage + Dropdown + Subjects
RowVM = index + one subject row
```

## 5. DbContext

Create `Data/AppDbContext.cs`.

```csharp
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
```

## 6. Add Connection String

Edit `appsettings.json`.

Add:

```json
"ConnectionStrings": {
  "con": "Server=.;Database=SMS_CodeFirst;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

## 7. Edit Program.cs

After this existing line:

```csharp
builder.Services.AddControllersWithViews();
```

add:

```csharp
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("con")));
```

Before the default route, add this custom route:

```csharp
app.MapControllerRoute(
    name: "student",
    pattern: "Student/{action=Index}/{id?}",
    defaults: new { controller = "Students" });
```

Custom route memory:

```
Default: {controller}/{action}/{id}
Custom:  Student/{action}/{id}
```

Test custom route:

```
/Student
/Student/Edit/1
```

## 8. Migration

Run:

```powershell
Add-Migration Initial
Update-Database
```

Do this before views. If this works, Code First is already proven.

## 9. Controller

Create `Controllers/StudentsController.cs`.

```csharp
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
```

Controller memory:

```
Index = list
Create = insert
Edit = update
Delete = remove
Fill = dropdown + one subject row
Clean = keep filled subject rows only
SaveImage = save photo
```

## 10. View Component

Create `ViewComponents/SubjectRowViewComponent.cs`.

```csharp
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
```

Create `Views/Shared/Components/SubjectRow/Default.cshtml`.

```cshtml
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
```

View component memory:

```
Class: SubjectRowViewComponent
Call: SubjectRow
View: Views/Shared/Components/SubjectRow/Default.cshtml
Hidden Subjects.Index = no reindex needed
```

## 11. Index View

Create `Views/Students/Index.cshtml`.

```cshtml
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
```

## 12. Partial View

Create `Views/Students/_StudentForm.cshtml`.

```cshtml
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
```

Memory:

```
Student fields first.
Then Subject table.
Subject row comes from View Component.
Add = template replace __i__, append row, i++
Delete = click X, remove row
```

Important trick:

```
Subjects.Index hidden input lets ASP.NET Core bind rows after delete.
So no reindexing code is needed.
```

## 13. Create View

Create `Views/Students/Create.cshtml`.

```cshtml
@model SMS.Models.StudentVM

<h2>Create Student</h2>

<form asp-action="Create" method="post" enctype="multipart/form-data">
    @Html.AntiForgeryToken()
    <partial name="_StudentForm" model="Model" />
    <button>Save</button>
    <a asp-action="Index">Back</a>
</form>
```

## 14. Edit View

Create `Views/Students/Edit.cshtml`.

```
Copy Create.cshtml.
Paste into Edit.cshtml.
Change only 3 things.
```

Change 1:

```cshtml
<h2>Create Student</h2> to <h2>Edit Student</h2>
```

Change 2:

```cshtml
<form asp-action="Create" method="post" enctype="multipart/form-data">
to
<form asp-action="Edit" method="post" enctype="multipart/form-data">
```

Change 3:

```cshtml
<button>Save</button> to <button>Update</button>
```

## 15. Run And Test

Run the project.

Test:

```
/Students
/Student
/Student/Edit/1
Create student with photo
Add subject row
Delete subject row
Edit student
Delete student
```

# What To Say To Teacher

Say:

```
This is an ASP.NET Core MVC Code First master-details CRUD project.
Student is the master and Subject is the detail.
Department is relational data.
I used data annotations in the models.
I used StudentVM for the create/edit form.
I used _StudentForm.cshtml as the partial view.
I used SubjectRowViewComponent to render subject detail rows.
I used Add Subject and X buttons to add/delete subject rows.
I used a custom route: Student/{action}/{id}.
Index is the Read part, so a separate Details page is not needed.
Delete is done by normal POST from the Index page.
```

# Common Mistakes

| Problem                               | Fix                                                                |
| ------------------------------------- | ------------------------------------------------------------------ |
| View component not found              | Folder must be `Views/Shared/Components/SubjectRow/Default.cshtml` |
| Subject rows do not save              | Keep hidden `Subjects.Index` and names like `Subjects[0].Course`   |
| Deleted subject rows cause wrong save | Do not remove `Subjects.Index`; it avoids reindexing               |
| Image does not show                   | Keep `app.UseStaticFiles()` from the template                      |
| Dropdown empty                        | Run `Update-Database` so seed departments are inserted             |
| Custom route not working              | Put custom route before default route                              |
