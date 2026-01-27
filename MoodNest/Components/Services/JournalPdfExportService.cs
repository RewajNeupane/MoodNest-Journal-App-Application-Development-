using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using Microsoft.Maui.Storage;
using MoodNest.Entities;

namespace MoodNest.Components.Services;

/// <summary>
/// Service responsible for exporting journal entries
/// into a formatted PDF document.
/// </summary>
public class JournalPdfExportService
{
    /// <summary>
    /// Exports the provided journal entries within a date range
    /// to a PDF file stored in the application data directory.
    /// </summary>
    /// <param name="entries">List of journal entries to export</param>
    /// <param name="from">Start date of export range</param>
    /// <param name="to">End date of export range</param>
    /// <returns>Absolute file path of the generated PDF</returns>
    public async Task<string> ExportAsync(
        List<JournalEntry> entries,
        DateTime from,
        DateTime to)
    {
        // Generate a unique file path based on date range
        var filePath = Path.Combine(
            FileSystem.AppDataDirectory,
            $"MoodNest_{from:yyyyMMdd}_{to:yyyyMMdd}.pdf"
        );

        // Run PDF generation on a background thread
        await Task.Run(() =>
        {
            using var writer = new PdfWriter(filePath);
            using var pdf = new PdfDocument(writer);
            using var doc = new Document(pdf);

            // Fonts used throughout the document
            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            /* =======================
               DOCUMENT HEADER
               ======================= */

            // Main title
            doc.Add(
                new Paragraph("MoodNest Journal Export")
                    .SetFont(boldFont)
                    .SetFontSize(20)
            );

            // Export date range
            doc.Add(
                new Paragraph(
                    $"From {from:dd MMM yyyy} to {to:dd MMM yyyy}"
                )
                .SetFont(normalFont)
                .SetFontSize(11)
            );

            // Spacer
            doc.Add(new Paragraph(" "));

            /* =======================
               JOURNAL ENTRIES
               ======================= */

            foreach (var entry in entries)
            {
                // Entry title
                doc.Add(
                    new Paragraph(entry.Title ?? "Untitled Entry")
                        .SetFont(boldFont)
                        .SetFontSize(16)
                );

                // Creation date
                doc.Add(
                    new Paragraph(
                        entry.CreatedAt.ToString("dd MMM yyyy")
                    )
                    .SetFont(normalFont)
                    .SetFontSize(10)
                );

                // Primary mood
                doc.Add(
                    new Paragraph(
                        $"Mood: {entry.PrimaryMood}"
                    )
                    .SetFont(normalFont)
                    .SetFontSize(11)
                );

                // Tags (optional)
                if (!string.IsNullOrWhiteSpace(entry.Tags))
                {
                    doc.Add(
                        new Paragraph(
                            $"Tags: {entry.Tags}"
                        )
                        .SetFont(normalFont)
                        .SetFontSize(10)
                    );
                }

                // Journal content (HTML stripped for clean PDF output)
                doc.Add(
                    new Paragraph(
                        StripHtml(entry.ContentHtml)
                    )
                    .SetFont(normalFont)
                    .SetFontSize(11)
                );

                // Spacer between entries
                doc.Add(new Paragraph(" "));
            }

            // Finalize and close the document
            doc.Close();
        });

        return filePath;
    }

    /// <summary>
    /// Removes HTML tags from stored journal content
    /// to produce clean, readable PDF text.
    /// </summary>
    private static string StripHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        return Regex.Replace(html, "<.*?>", string.Empty);
    }
}
