//TODO: cost share column handling

using ClosedXML.Excel;
using Sponsored_Programs_Administration_Interview_submission;

Console.WriteLine("Please enter folder path: ");
string? folderPath = Console.ReadLine();

if (folderPath == null) {
    return;
}

if (Directory.Exists(folderPath)) {
    SpreadsheetReader.PrintSubrecipients(folderPath);
} else {
    Console.WriteLine("Error: Directory does not exist.");
}

