// =============================================================
// DISCLAIMER: This test file is created for LEARNING PURPOSES
// ONLY as part of interview preparation for Iron Software.
//
// IronPDF is used under free trial terms.
// Only free trial features are used — no paid license required.
// No PDFs are saved to disk or distributed.
//
// All credit for IronPDF goes to Iron Software:
// https://ironsoftware.com
// =============================================================

using IronPdf;
using System.Diagnostics;
using Xunit;

namespace IronPdfQA.Tests;

/// <summary>
/// Comprehensive xUnit test suite for IronPDF HTML-to-PDF conversion.
/// Uses only free trial features: ChromePdfRenderer, RenderHtmlAsPdf, PageCount.
/// No SaveAs, BinaryData, or file-system operations are used.
/// Covers happy path, edge cases, output validation, performance, and negative scenarios.
/// Author: Reevan D'Souza
/// </summary>
public class IronPdfTests : IDisposable
{
    private readonly ChromePdfRenderer _renderer;

    /// <summary>Initialises the ChromePdfRenderer for all tests.</summary>
    public IronPdfTests()
    {
        _renderer = new ChromePdfRenderer();
    }

    // ─────────────────────────────────────────────
    // HAPPY PATH (5 tests)
    // ─────────────────────────────────────────────

    /// <summary>
    /// TC001: Basic HTML to PDF — verify result is not null and PageCount is greater than zero.
    /// </summary>
    [Fact]
    [Trait("Category", "HappyPath")]
    public void TC001_BasicHtmlToPdf_ReturnsValidDocument()
    {
        // Arrange
        const string html = "<h1>Hello IronPDF</h1><p>Basic conversion test.</p>";

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC002: HTML with CSS styling — verify result is not null and PageCount is greater than zero.
    /// </summary>
    [Fact]
    [Trait("Category", "HappyPath")]
    public void TC002_HtmlWithCss_GeneratesPdf()
    {
        // Arrange
        const string html = """
            <html>
            <head>
              <style>
                body { font-family: Arial, sans-serif; background: #f4f4f4; }
                h1   { color: #2c3e50; border-bottom: 2px solid #3498db; }
                p    { line-height: 1.6; color: #555; }
              </style>
            </head>
            <body><h1>Styled Document</h1><p>CSS styling test.</p></body>
            </html>
            """;

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC003: HTML with table — verify result is not null and PageCount is greater than zero.
    /// </summary>
    [Fact]
    [Trait("Category", "HappyPath")]
    public void TC003_HtmlWithTable_GeneratesPdf()
    {
        // Arrange
        const string html = """
            <html><body>
            <h2>Test Results Table</h2>
            <table border="1" cellpadding="5" cellspacing="0">
              <thead><tr><th>ID</th><th>Name</th><th>Status</th></tr></thead>
              <tbody>
                <tr><td>TC001</td><td>Basic HTML</td><td>PASS</td></tr>
                <tr><td>TC002</td><td>CSS Styling</td><td>PASS</td></tr>
                <tr><td>TC003</td><td>Table HTML</td><td>PASS</td></tr>
              </tbody>
            </table>
            </body></html>
            """;

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC004: Multi-page HTML with 500 paragraphs — verify PageCount is greater than one.
    /// </summary>
    [Fact]
    [Trait("Category", "HappyPath")]
    public void TC004_MultiPageHtml_ReturnsMoreThanOnePage()
    {
        // Arrange
        var paragraphs = string.Concat(
            Enumerable.Range(1, 500).Select(i =>
                $"<p>Paragraph {i}: This is content line {i} of the long document used to verify multi-page PDF generation in IronPDF.</p>")
        );
        var html = $"<html><body><h1>Multi-Page Document</h1>{paragraphs}</body></html>";

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 1, $"Expected PageCount > 1 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC005: HTML with heading and paragraph — verify result is not null and PageCount is greater than zero.
    /// </summary>
    [Fact]
    [Trait("Category", "HappyPath")]
    public void TC005_HtmlWithHeadingAndParagraph_GeneratesPdf()
    {
        // Arrange
        const string html = """
            <html><body>
            <h1>Report Title</h1>
            <h2>Section One</h2>
            <p>This is a paragraph under section one describing the first topic in detail.</p>
            <h2>Section Two</h2>
            <p>This is a paragraph under section two with additional details and context.</p>
            </body></html>
            """;

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    // ─────────────────────────────────────────────
    // EDGE CASES (5 tests)
    // ─────────────────────────────────────────────

    /// <summary>
    /// TC006: Empty HTML string — IronPDF rejects null or empty input with ArgumentException.
    /// </summary>
    [Fact]
    [Trait("Category", "EdgeCase")]
    public void TC006_EmptyHtmlString_ThrowsArgumentException()
    {
        // Arrange
        const string html = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _renderer.RenderHtmlAsPdf(html));
    }

    /// <summary>
    /// TC007: Special characters (café, naïve, résumé, CJK, Arabic) — verify result is not null.
    /// </summary>
    [Fact]
    [Trait("Category", "EdgeCase")]
    public void TC007_SpecialCharacters_HandledCorrectly()
    {
        // Arrange
        const string html = """
            <html><meta charset="UTF-8"><body>
            <h1>Special Characters</h1>
            <p>café naïve résumé über Ñoño João</p>
            <p>Chinese: 你好世界 Japanese: こんにちは Arabic: مرحبا</p>
            <p>Symbols: © ® ™ € £ ¥ § ° ± × ÷</p>
            </body></html>
            """;

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC008: Very long single paragraph (~44 000 chars) — verify result is not null and PageCount is greater than zero.
    /// </summary>
    [Fact]
    [Trait("Category", "EdgeCase")]
    public void TC008_VeryLongSingleParagraph_GeneratesPdf()
    {
        // Arrange
        var longText = string.Concat(Enumerable.Repeat("IronPDF handles long text content reliably. ", 1000));
        var html = $"<html><body><p>{longText}</p></body></html>";

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC009: HTML with inline styles — verify result is not null and PageCount is greater than zero.
    /// </summary>
    [Fact]
    [Trait("Category", "EdgeCase")]
    public void TC009_HtmlWithInlineStyles_GeneratesPdf()
    {
        // Arrange
        const string html = """
            <html><body>
            <div style="background:#0d1117; color:#e6edf3; padding:20px; font-family:monospace;">
              <h1 style="color:#00ff88; font-size:2em; margin-bottom:10px;">Inline Styled</h1>
              <p style="color:#8b949e; font-size:1.1em; line-height:1.8;">
                This paragraph uses inline styles including background color, text color,
                padding, font family, font size, and line height.
              </p>
              <span style="color:#ffaa00; font-weight:bold;">Warning Text</span>
            </div>
            </body></html>
            """;

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC010: Whitespace-only HTML — IronPDF rejects whitespace-only input with ArgumentException.
    /// </summary>
    [Fact]
    [Trait("Category", "EdgeCase")]
    public void TC010_WhitespaceOnlyHtml_ThrowsArgumentException()
    {
        // Arrange
        const string html = "   \n\t\r\n   ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _renderer.RenderHtmlAsPdf(html));
    }

    // ─────────────────────────────────────────────
    // OUTPUT VALIDATION (4 tests)
    // ─────────────────────────────────────────────

    /// <summary>
    /// TC011: Single render — verify result is not null and PageCount is greater than zero.
    /// </summary>
    [Fact]
    [Trait("Category", "OutputValidation")]
    public void TC011_SingleRender_ReturnsValidDocument()
    {
        // Arrange
        const string html = "<h1>Output Validation</h1><p>Single render returning a valid document.</p>";

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC012: Large document — verify PageCount is greater than one.
    /// </summary>
    [Fact]
    [Trait("Category", "OutputValidation")]
    public void TC012_LargeDocument_PageCountGreaterThanOne()
    {
        // Arrange
        var paragraphs = string.Concat(
            Enumerable.Range(1, 500).Select(i =>
                $"<p>Output line {i}: Verifying that a 500-paragraph document produces more than one PDF page.</p>")
        );
        var html = $"<html><body><h1>Large Document</h1>{paragraphs}</body></html>";

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 1, $"Expected PageCount > 1 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC013: Page count is always positive — verify PageCount is greater than zero.
    /// </summary>
    [Fact]
    [Trait("Category", "OutputValidation")]
    public void TC013_PageCount_AlwaysPositive()
    {
        // Arrange
        const string html = "<h1>Page Count Test</h1><p>Ensuring PageCount is always at least 1.</p>";

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC014: Multiple renders — verify all three results are not null and each has PageCount greater than zero.
    /// </summary>
    [Fact]
    [Trait("Category", "OutputValidation")]
    public void TC014_MultipleRenders_AllReturnValidDocuments()
    {
        // Arrange
        var inputs = Enumerable.Range(1, 3)
            .Select(i => $"<h1>Document {i}</h1><p>Multiple render validation — document {i}.</p>")
            .ToList();

        // Act
        var results = inputs.Select(html => _renderer.RenderHtmlAsPdf(html)).ToList();

        // Assert
        foreach (var (pdf, index) in results.Select((p, i) => (p, i + 1)))
        {
            Assert.NotNull(pdf);
            Assert.True(pdf.PageCount > 0, $"Document {index}: Expected PageCount > 0 but got {pdf.PageCount}.");
        }
    }

    // ─────────────────────────────────────────────
    // PERFORMANCE (2 tests)
    // ─────────────────────────────────────────────

    /// <summary>
    /// TC015: Simple conversion completes under 5 000 ms — measured with Stopwatch.
    /// </summary>
    [Fact]
    [Trait("Category", "Performance")]
    public void TC015_SimpleConversion_CompletesUnder5000ms()
    {
        // Arrange
        const string html = "<h1>Performance Test</h1><p>Timing simple HTML-to-PDF conversion.</p>";
        var sw = Stopwatch.StartNew();

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);
        sw.Stop();

        // Assert
        Assert.NotNull(pdf);
        Assert.True(sw.ElapsedMilliseconds < 5000,
            $"Expected conversion under 5 000ms but took {sw.ElapsedMilliseconds}ms.");
    }

    /// <summary>
    /// TC016: Large 500-paragraph document completes under 30 000 ms — measured with Stopwatch.
    /// </summary>
    [Fact]
    [Trait("Category", "Performance")]
    public void TC016_LargeDocument_CompletesUnder30000ms()
    {
        // Arrange
        var paragraphs = string.Concat(
            Enumerable.Range(1, 500).Select(i =>
                $"<p>Performance line {i}: large document timing test for IronPDF rendering pipeline.</p>")
        );
        var html = $"<html><body><h1>Large Document Performance Test</h1>{paragraphs}</body></html>";
        var sw = Stopwatch.StartNew();

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);
        sw.Stop();

        // Assert
        Assert.NotNull(pdf);
        Assert.True(sw.ElapsedMilliseconds < 30000,
            $"Expected large document under 30 000ms but took {sw.ElapsedMilliseconds}ms.");
    }

    // ─────────────────────────────────────────────
    // NEGATIVE (2 tests)
    // ─────────────────────────────────────────────

    /// <summary>
    /// TC017: Broken image URL — PDF still generates; IronPDF continues rendering when an image cannot be loaded.
    /// </summary>
    [Fact]
    [Trait("Category", "Negative")]
    public void TC017_BrokenImageUrl_PdfStillGenerates()
    {
        // Arrange
        const string html = """
            <html><body>
            <h1>Broken Image Test</h1>
            <img src="https://this-domain-does-not-exist-xyz.invalid/image.png" alt="broken" />
            <p>The PDF should still render even when an image cannot be loaded.</p>
            </body></html>
            """;

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    /// <summary>
    /// TC018: Invalid HTML tags — PDF still generates; IronPDF handles malformed markup gracefully.
    /// </summary>
    [Fact]
    [Trait("Category", "Negative")]
    public void TC018_InvalidHtmlTags_HandlesGracefully()
    {
        // Arrange
        const string html = """
            <html><body>
            <fakeelement attr="bad">This tag does not exist</fakeelement>
            <h1>Invalid Tag Test</h1>
            <unclosedtag>Unclosed
            <p>Normal paragraph after invalid markup.</p>
            </body></html>
            """;

        // Act
        var pdf = _renderer.RenderHtmlAsPdf(html);

        // Assert
        Assert.NotNull(pdf);
        Assert.True(pdf.PageCount > 0, $"Expected PageCount > 0 but got {pdf.PageCount}.");
    }

    // ─────────────────────────────────────────────
    // CLEANUP
    // ─────────────────────────────────────────────

    /// <summary>Disposes resources. No files created — nothing to clean up.</summary>
    public void Dispose()
    {
        // No file-system operations are performed in this suite.
        // IDisposable is implemented to follow xUnit best practices.
    }
}
