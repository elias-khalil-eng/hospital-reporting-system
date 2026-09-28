# Hospital Reporting System

[![Build](https://github.com/elias-khalil-eng/hospital-reporting-system/actions/workflows/build.yml/badge.svg)](https://github.com/elias-khalil-eng/hospital-reporting-system/actions/workflows/build.yml)

Windows desktop application that turns hospital stay records into insurance
claim batches. Staff enter a patient file number, the app pulls the stay from
the hospital information system, validates the claim fields and saves it to a
batch that can be exported to Excel.

## Features

- Patient file lookup against the hospital's Oracle database, with patient name shown before saving
- Claim entry with contract number, approval number and up to four ICD-10 diagnosis codes, validated on input
- Claim batches: create, fill, close and track the status of each batch
- Pricing rules from reference tables (flat-rate codes, annex codes, insurer codes) applied to each line
- Edit or delete saved files inside an open batch
- Excel export of batch details
- Admin panel to view, edit and export reference tables
- Bilingual interface (Arabic and French labels)

## Tech Stack

- VB.NET, Windows Forms, .NET Framework 4.7.2
- Oracle (read, via Oracle.ManagedDataAccess) and SQL Server (write)
- ClosedXML and Office Interop for Excel export
- GitHub Actions build with MSBuild

## Getting Started

### Prerequisites

- Windows with Visual Studio 2019 or later (".NET desktop development" workload)
- Access to an Oracle database and a SQL Server database with the expected schema

### Build and run

```bash
git clone https://github.com/elias-khalil-eng/hospital-reporting-system.git
cd hospital-reporting-system
nuget restore HospitalReporting.sln
msbuild HospitalReporting.sln /p:Configuration=Release
```

Or open `HospitalReporting.sln` in Visual Studio and press F5.

### Configuration

Edit `HospitalReporting/App.config` and replace the placeholder values of the
`OracleConnection` and `MSSQLConnection` connection strings. Do not commit real
values.

## Project Structure

```
HospitalReporting.sln
HospitalReporting/
  Main.vb              Batch list and main workflow
  Form1.vb             Claim entry, Oracle lookup, validation, Excel export
  FormEditNDA.vb       Edit or delete files inside a batch
  FormMoulhak.vb       Annex code management
  AdminForm.vb         Admin panel and reference tables
  TableEditorForm.vb   Generic table editor
  App.config           Connection string template
```

## Confidentiality

This is a sanitized portfolio version of a system built for a hospital. Real
infrastructure details, connection strings and patient data are not included.

## License

[MIT](LICENSE)
