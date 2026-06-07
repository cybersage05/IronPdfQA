# IronPDF QA Dashboard

> ⚠️ **DISCLAIMER:** This project is created purely for
> learning and interview preparation purposes.
> IronPDF is used under free trial terms for educational
> use only. No paid features or license keys are used.
> All credit for IronPDF goes to
> [Iron Software](https://ironsoftware.com).

![CI](https://github.com/cybersage05/IronPdfQA/actions/workflows/daily-tests.yml/badge.svg)
![Tests](https://img.shields.io/badge/tests-18%20passing-00ff88)
![Pass Rate](https://img.shields.io/badge/pass%20rate-100%25-00ff88)
![IronPDF](https://img.shields.io/badge/IronPDF-2026.6.1-0099ff)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)

A professional automated QA test suite for [IronPDF](https://ironpdf.com) — running 18 xUnit tests across 5 categories on GitHub Actions, with a live dark-themed HTML dashboard showing real CI/CD metrics.

Built by **Reevan D'Souza** as part of a QA Engineer interview project for [Iron Software](https://ironsoftware.com).

---

## Why This Project Exists

IronPDF is a production-grade HTML-to-PDF library used in enterprise .NET applications. This project validates that the core conversion pipeline is stable, performant, and resilient to edge inputs — across scheduled and triggered CI runs — with a visual dashboard that non-technical stakeholders can read at a glance.

---

## IronPDF Overview

[IronPDF](https://ironpdf.com) is a .NET library by Iron Software that converts HTML, CSS, JavaScript, and URLs into pixel-perfect PDF documents using a Chromium rendering engine. Key capabilities:

- HTML → PDF with full CSS/JS support
- URL → PDF (web scraping to PDF)
- PDF merging, splitting, stamping
- Digital signatures and password protection
- Headers, footers, and watermarks
- Available on .NET 6, 7, 8 — Windows, Linux, macOS, Docker

---

## Project Structure

```
IronPdfQA/
├── tests/
│   └── IronPdfQA.Tests/
│       ├── IronPdfTests.cs          # 18 xUnit tests
│       └── IronPdfQA.Tests.csproj
├── dashboard/
│   ├── index.html                   # Live QA dashboard
│   └── run-info.json                # Runtime data injected by CI
├── docs/
│   └── TestSummaryReport.md         # Full test report
├── .github/
│   └── workflows/
│       └── daily-tests.yml          # GitHub Actions pipeline
├── README.md
├── .gitignore
└── IronPdfQA.sln
```

---

## Build and Run Tests

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Internet access (NuGet restore)

### Clone and restore

```bash
git clone https://github.com/cybersage05/IronPdfQA.git
cd IronPdfQA
dotnet restore
```

### Build

```bash
dotnet build
```

### Run all tests

```bash
dotnet test --verbosity normal
```

### Run a specific category

```bash
# Happy Path only
dotnet test --filter "Category=HappyPath"

# Edge Cases only
dotnet test --filter "Category=EdgeCase"

# Output Validation only
dotnet test --filter "Category=OutputValidation"

# Performance only
dotnet test --filter "Category=Performance"

# Negative only
dotnet test --filter "Category=Negative"

# Regression (Happy Path + Output Validation)
dotnet test --filter "Category=HappyPath|Category=OutputValidation"
```

### Run with TRX output

```bash
dotnet test --logger "trx;LogFileName=TestResults.trx" --results-directory docs/TestResults
```

---

## Trigger a Manual Run

1. Go to [GitHub Actions](https://github.com/cybersage05/IronPdfQA/actions/workflows/daily-tests.yml)
2. Click **Run workflow**
3. Select a **Test Type** from the dropdown
4. Enter an optional **Reason**
5. Click **Run workflow**

Or use the **Manual Trigger panel** on the [Live Dashboard](https://cybersage05.github.io/IronPdfQA/).

---

## Live Dashboard

**[https://cybersage05.github.io/IronPdfQA/](https://cybersage05.github.io/IronPdfQA/)**

The dashboard reads `dashboard/run-info.json` — populated automatically by GitHub Actions on every run with real system values (OS, .NET version, CPU cores, RAM, run number, commit hash, etc.).

---

## Test Cases

| ID    | Test Name                        | Category          | Description                                           |
|-------|----------------------------------|-------------------|-------------------------------------------------------|
| TC001 | Basic HTML to PDF                | Happy Path        | Verify not null and page count > 0                    |
| TC002 | HTML with CSS Styling            | Happy Path        | Verify PDF generated with embedded styles             |
| TC003 | HTML with Table                  | Happy Path        | Verify table-based HTML produces valid PDF            |
| TC004 | Multi-Page (500 paragraphs)      | Happy Path        | Verify page count > 1                                 |
| TC005 | Heading and Paragraph            | Happy Path        | Verify heading/paragraph structure renders            |
| TC006 | Empty HTML String                | Edge Case         | Handle gracefully, no exception                       |
| TC007 | Special Characters               | Edge Case         | UTF-8 chars: café, naïve, résumé, CJK, Arabic        |
| TC008 | Very Long Single Paragraph       | Edge Case         | 44 000-character paragraph — verify PDF generated     |
| TC009 | HTML with Inline Styles          | Edge Case         | Verify inline styles applied correctly                |
| TC010 | Whitespace-Only HTML             | Edge Case         | Handle gracefully, no exception                       |
| TC011 | Save PDF to File                 | Output Validation | Verify file exists on disk after SaveAs               |
| TC012 | PDF Binary Data                  | Output Validation | Verify BinaryData.Length > 0                          |
| TC013 | Page Count Validation            | Output Validation | Verify PageCount > 0                                  |
| TC014 | Multiple File Saves              | Output Validation | Verify all 3 files saved correctly                    |
| TC015 | Simple Conversion Performance    | Performance       | Completes under 5 000ms                               |
| TC016 | Large Document Performance       | Performance       | 200-row table completes under 30 000ms                |
| TC017 | Broken Image URL                 | Negative          | PDF still generates despite broken img src            |
| TC018 | Invalid HTML Tags                | Negative          | Malformed/unknown tags handled gracefully             |

---

## Scheduled Runs

The pipeline runs automatically at:

| Time (UTC) | Bangkok (ICT) |
|------------|---------------|
| 01:00 UTC  | 08:00 AM ICT  |
| 13:00 UTC  | 08:00 PM ICT  |

A countdown to the next scheduled run is shown on the dashboard.

---

## Author

**Reevan D'Souza**
GitHub: [@cybersage05](https://github.com/cybersage05)
