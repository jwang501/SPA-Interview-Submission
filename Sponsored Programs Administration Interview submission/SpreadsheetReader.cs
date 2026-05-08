using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sponsored_Programs_Administration_Interview_submission
{
    public class SpreadsheetReader
    {
        const int SUBAWARD_COL = 2;
        const int SUBRECIPIENT_COL = 3;
        const int MIN_SUBRECIPIENT_COL_LENGTH = 15;
        const int MIN_AMOUNT_COL_LENGTH = 10;

        /**
         * Reads all Excel files in the specified folder, extracts subrecipient names and total subaward amounts, and prints a summary of total subaward amount by subrecipient.
         * Assumes that each Excel file has a one of two formats:
         * 1. Subrecipient names appear in a cell in the format "Subaward: [Subrecipient Name]", and the total subaward amount is in the "Total" column in the same row.
         * 2. Subrecipient names appear in the cell next to the "Subaward:" cell (e.g. "Subaward:" in column B and subrecipient name in column C), and the total subaward amount
         *    is divided between a "Sponsor" column and "Cost Share" column.
         * If the format is invalid, it prints an error message and skips that file.
         */
        public static Dictionary<string,int> PrintSubrecipients(string folderPath, bool testMode = false) {
            string[] files = Directory.GetFiles(folderPath, "*.xlsx");
            // Key = subrecipient, Value = total subaward amount
            Dictionary<string, int> subrecipients = new Dictionary<string, int>();

            if (!testMode) { Console.WriteLine(new string('=', Console.WindowWidth)); }
            foreach (string filepath in files) {
                using var workbook = new XLWorkbook(filepath);
                var worksheet = workbook.Worksheets.First();

                // Find total column (first find the title row by searching for "Period 1", then find the "Total" column in that row)
                var titleRow = worksheet.RowsUsed().FirstOrDefault(row => row.CellsUsed().Any(cell => cell.GetString().Trim() == "Period 1"));
                var totalColumn = titleRow?.CellsUsed().FirstOrDefault(cell => cell.GetString().Trim() == "Total");
                if (totalColumn == null) {
                    if (!testMode) { Console.WriteLine("Invalid format: missing \"Period\" or \"Total\" columns"); }
                    continue;
                }
                // If total column is split into "Sponsor" and "Cost Share" columns, we sum them to get total subaward amount
                bool hasCostShare = totalColumn.IsMerged();

                // Find subawards and subrecipients
                if (!testMode) { Console.WriteLine($"Subrecipients from file: {Path.GetFileName(filepath)}"); }
                foreach (var cell in worksheet.Column(SUBAWARD_COL).CellsUsed()) {
                    string subawardText = cell.GetString().Trim();
                    if (subawardText.StartsWith("Subaward:")) {
                        int row = cell.Address.RowNumber;
                        string subrecipient = subawardText.Replace("Subaward:", "").Trim();
                        if (subrecipient == "") {
                            subrecipient = worksheet.Cell(row, SUBRECIPIENT_COL).GetString().Trim();
                        }
                        // If subrecipient is still empty, print an error and skip this subaward
                        if (subrecipient == "") {
                            if (!testMode) { Console.WriteLine($"Error: missing subrecipient for subaward at row {row}"); }
                            continue;
                        }
                        if (!testMode) { Console.WriteLine(subrecipient); }

                        int subawardAmount = (int)worksheet.Cell(row, totalColumn.Address.ColumnNumber).GetDouble();
                        int exemptAmount = (int)worksheet.Cell(row + 1, totalColumn.Address.ColumnNumber).GetDouble();
                        if (hasCostShare) {
                            subawardAmount += (int)worksheet.Cell(row, totalColumn.Address.ColumnNumber + 1).GetDouble();
                            exemptAmount += (int)worksheet.Cell(row + 1, totalColumn.Address.ColumnNumber + 1).GetDouble();
                        }
                        if (!subrecipients.ContainsKey(subrecipient)) {
                            subrecipients[subrecipient] = 0;

                        }
                        subrecipients[subrecipient] += subawardAmount + exemptAmount;
                    }
                }
                if (!testMode) { Console.WriteLine(new string('=', Console.WindowWidth)); }
            }

            if (!testMode) {
                // Calculate column widths for printing
                int maxSubrecipientLength = 0;
                int maxAmountLength = 0;
                foreach (var pair in subrecipients) {
                    maxSubrecipientLength = Math.Max(maxSubrecipientLength, pair.Key.Length);
                    maxAmountLength = Math.Max(maxAmountLength, pair.Value.ToString().Length);
                }

                int col1Length = Math.Max(maxSubrecipientLength + 3, MIN_SUBRECIPIENT_COL_LENGTH);
                int col2Length = Math.Max(maxAmountLength + 3, MIN_AMOUNT_COL_LENGTH);

                // Print total subaward amount by subrecipient
                Console.WriteLine("Total subaward amount by subrecipient:");
                Console.WriteLine("Subrecipient".PadRight(col1Length) + "|" + "Name".PadRight(col2Length));
                Console.WriteLine(new string('-', col1Length + col2Length + 1));
                foreach (var pair in subrecipients) {
                    Console.WriteLine(pair.Key.PadRight(col1Length) + "|" + pair.Value.ToString().PadRight(col2Length));
                }
            }
            return subrecipients;
        }
    }
}
