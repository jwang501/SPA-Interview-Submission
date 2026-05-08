using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using Sponsored_Programs_Administration_Interview_submission;

namespace SpreadsheetReaderTest
{

    public class SpreadsheetReaderTest
    {
        [Fact]

        public void Spreadsheet1Test() {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Spreadsheet1");
            Dictionary <string,int> subrecipients = SpreadsheetReader.PrintSubrecipients(folderPath,true);

            List<string> answers = new List<string> { "Indiana","Mayo","Purdue","Florida"};

            Assert.Equal(answers.Count, subrecipients.Count);
            foreach (string answer in answers)
            {
                Assert.Contains(answer, subrecipients.Keys);
            }
        }

        [Fact]
        public void Spreadsheet2Test() {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Spreadsheet2");
            Dictionary<string, int> subrecipients = SpreadsheetReader.PrintSubrecipients(folderPath, true);

            Dictionary<string, int> answers = new Dictionary<string, int> { {"Mayo", 24727},{"Ecotek", 100000},{"Purdue", 25000} };

            Assert.Equal(answers.Count, subrecipients.Count);
            foreach (var pair in answers)
            {
                Assert.Contains(pair.Key, subrecipients.Keys);
                Assert.Equal(pair.Value, subrecipients[pair.Key]);
            }
        }
    }
}
