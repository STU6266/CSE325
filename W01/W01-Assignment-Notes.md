# CSE 325 – Week 01 Assignment Notes

## 1. Pizza Service – Added Pizza

I created a Pizza API using ASP.NET Core and .NET 8.

The original pizzas are:
- ID 1: Classic Italian
- ID 2: Veggie

I added one more pizza to the PizzaService list:
- ID 3: Chicken BBQ

The GET request returned all three pizzas.

## 2. API Tests

### GET – Get All Pizzas

**Request:**

`curl -i http://localhost:5055/pizza`

**Response:**

Status: `200 OK`

```json
[
  {"id":1,"name":"Classic Italian","isGlutenFree":false},
  {"id":2,"name":"Veggie","isGlutenFree":true},
  {"id":3,"name":"Chicken BBQ","isGlutenFree":false}
]
```

### POST – Create a Pizza

**Request:**

```bash
curl -i -X POST http://localhost:5055/pizza \
  -H "Content-Type: application/json" \
  -d '{"name":"Hawaiian","isGlutenFree":false}'
```

**Response:**

Status: `201 Created`

```json
{"id":4,"name":"Hawaiian","isGlutenFree":false}
```

### PUT – Update a Pizza

**Request:**

```bash
curl -i -X PUT http://localhost:5055/pizza/4 \
  -H "Content-Type: application/json" \
  -d '{"id":4,"name":"Hawaiian Special","isGlutenFree":true}'
```

**Response:**

Status: `204 No Content`

The response body was empty.

I checked the updated pizza with GET and received:

Status: `200 OK`

```json
{"id":4,"name":"Hawaiian Special","isGlutenFree":true}
```

### DELETE – Delete a Pizza

**Request:**

```bash
curl -i -X DELETE http://localhost:5055/pizza/4
```

**Response:**

Status: `204 No Content`

The response body was empty.

I checked the deleted pizza with GET and received:

Status: `404 Not Found`

This confirmed that the pizza was deleted.

## 3. Sales Summary Function

I created a function that calculates the sales total and generates a detailed report for all five sales files. The program then saves the report to a text file.

**My working function:**

```csharp
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
```

**Generated Report:**

```text
Sales Summary
----------------------------
Total Sales: $2,012.20

Details:
  stores/201/sales.json: $501.22
  stores/202/sales.json: $1,234.22
  stores/203/sales.json: $99.00
  stores/204/sales.json: $88.88
  stores/sales.json: $88.88
```

The report was saved in `salesTotalDir/totals.txt`.

The project built successfully with .NET 8.