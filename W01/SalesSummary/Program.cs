using System.Globalization;
using System.Text;
using Newtonsoft.Json;

var storesDir = Path.Combine(
    Directory.GetCurrentDirectory(), "stores");

var outputDir = Path.Combine(
    Directory.GetCurrentDirectory(), "salesTotalDir");

Directory.CreateDirectory(outputDir);

var salesFiles = FindFiles(storesDir)
    .OrderBy(file => file)
    .ToList();

string report = GenerateSalesSummaryReport(salesFiles);

File.WriteAllText(
    Path.Combine(outputDir, "totals.txt"), report);

Console.WriteLine(report);

// Find all sales files
IEnumerable<string> FindFiles(string folderName)
{
    return Directory.EnumerateFiles(
        folderName, "sales.json",
        SearchOption.AllDirectories);
}

// Calculate total sales
double CalculateSalesTotal(IEnumerable<string> files)
{
    double total = 0;

    foreach (var file in files)
    {
        string json = File.ReadAllText(file);
        SalesData? data =
            JsonConvert.DeserializeObject<SalesData>(json);

        total += data?.Total ?? 0;
    }

    return total;
}

// Generate the detailed sales report
string GenerateSalesSummaryReport(List<string> files)
{
    var culture = CultureInfo.GetCultureInfo("en-US");
    var report = new StringBuilder();

    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");

    double total = CalculateSalesTotal(files);
    report.AppendLine(
        $"Total Sales: {total.ToString("C", culture)}");

    report.AppendLine();
    report.AppendLine("Details:");

    foreach (var file in files)
    {
        double amount = CalculateSalesTotal(new[] { file });
        string name = Path.GetRelativePath(
            Directory.GetCurrentDirectory(), file);

        report.AppendLine(
            $"  {name}: {amount.ToString("C", culture)}");
    }

    return report.ToString();
}

record SalesData(double Total);