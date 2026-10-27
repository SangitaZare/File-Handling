using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FileHandlingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private record Employee(int Id, string Name, string Department, decimal Salary);

    private static readonly List<Employee> Data = new()
    {
        new(1, "Asha Patil", "IT", 50000),
        new(2, "Rahul Sharma", "HR", 42000),
        new(3, "Neha Joshi", "Finance", 55000)
    };

    [HttpGet("pdf")]
    public IActionResult GetPdf()
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var bytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                page.Header().Text("Employee Report").FontSize(20).Bold();

                page.Content().PaddingVertical(10).Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.ConstantColumn(40);
                        c.RelativeColumn();
                        c.RelativeColumn();
                        c.RelativeColumn();
                    });

                    table.Header(h =>
                    {
                        h.Cell().Text("Id").Bold();
                        h.Cell().Text("Name").Bold();
                        h.Cell().Text("Department").Bold();
                        h.Cell().Text("Salary").Bold();
                    });

                    foreach (var e in Data)
                    {
                        table.Cell().Text(e.Id.ToString());
                        table.Cell().Text(e.Name);
                        table.Cell().Text(e.Department);
                        table.Cell().Text(e.Salary.ToString("N0"));
                    }
                });
            });
        }).GeneratePdf();

        return File(bytes, "application/pdf", "EmployeeReport.pdf");
    }

    [HttpGet("excel")]
    public IActionResult GetExcel()
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Employees");

        ws.Cell(1, 1).Value = "Id";
        ws.Cell(1, 2).Value = "Name";
        ws.Cell(1, 3).Value = "Department";
        ws.Cell(1, 4).Value = "Salary";
        ws.Row(1).Style.Font.Bold = true;

        int row = 2;
        foreach (var e in Data)
        {
            ws.Cell(row, 1).Value = e.Id;
            ws.Cell(row, 2).Value = e.Name;
            ws.Cell(row, 3).Value = e.Department;
            ws.Cell(row, 4).Value = e.Salary;
            row++;
        }

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);

        return File(ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "EmployeeReport.xlsx");
    }
}