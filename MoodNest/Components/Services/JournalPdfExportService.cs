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

public class JournalPdfExportService
{
    public async Task<string> ExportAsync(
        List<JournalEntry> entries,
        DateTime from,
        DateTime to)
    {
        var filePath = Path.Combine(
            FileSystem.AppDataDirectory,
            $"MoodNest_{from:yyyyMMdd}_{to:yyyyMMdd}.pdf"
        );

        await Task.Run(() =>
        {
            using var writer = new PdfWriter(filePath);
            using var pdf = new PdfDocument(writer);
            using var doc = new Document(pdf);

            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            // ===== TITLE =====
            doc.Add(
                new Paragraph("MoodNest Journal Export")
                    .SetFont(boldFont)
                    .SetFontSize(20)
            );

            doc.Add(
                new Paragraph(
                    $"From {from:dd MMM yyyy} to {to:dd MMM yyyy}"
                )
                .SetFont(normalFont)
                .SetFontSize(11)
            );

            doc.Add(new Paragraph(" "));

            foreach (var entry in entries)
            {
                // Entry title
                doc.Add(
                    new Paragraph(entry.Title ?? "Untitled Entry")
                        .SetFont(boldFont)
                        .SetFontSize(16)
                );

                // Date
                doc.Add(
                    new Paragraph(
                        entry.CreatedAt.ToString("dd MMM yyyy")
                    )
                    .SetFont(normalFont)
                    .SetFontSize(10)
                );

                // Mood
                doc.Add(
                    new Paragraph(
                        $"Mood: {entry.PrimaryMood}"
                    )
                    .SetFont(normalFont)
                    .SetFontSize(11)
                );

                // Tags
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

                // Content
                doc.Add(
                    new Paragraph(
                        StripHtml(entry.ContentHtml)
                    )
                    .SetFont(normalFont)
                    .SetFontSize(11)
                );

                doc.Add(new Paragraph(" "));
            }

            doc.Close();
        });

        return filePath;
    }

    private static string StripHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        return Regex.Replace(html, "<.*?>", string.Empty);
    }
}
