using ClosedXML.Excel;
using hamayes;
using System.Collections.Generic;
using System.IO;


namespace hamayes;
public class ExcelHelper
{
    public List<Persons> ReadExcel(Stream fileStream)
    {
        var employees = new List<Persons>();
      string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SmsLog.txt"); // Full path for the log file


        using (var workbook = new XLWorkbook(fileStream))
        {
            var worksheet = workbook.Worksheet(1); // Get the first worksheet
            var rows = worksheet.RowsUsed();

            foreach (var row in rows.Skip(2)) // Skip header
            {
                var person = new Persons
                {
                    Title = row.Cell(1).GetString(), // Column 1
                    FullName = row.Cell(2).GetString(),// Column 2
                    PhoneNumber = row.Cell(3).GetString(), // Column 2
                    TehToKerDateTime = row.Cell(4).GetString(), // Column 2
                    TerminalNumber = row.Cell(5).GetString(), // Column 2
                    Airline = row.Cell(6).GetString(), // Column 2
                    FlightNumber = row.Cell(7).GetString(), // Column 2
                    FightTime = row.Cell(8).GetString(), // Column 2
                    ExiteDate = row.Cell(9).GetString(), // Column 2
                    ExitAirline = row.Cell(10).GetString(), // Column 2
                    ExitFilghtNumber = row.Cell(11).GetString(), // Column 2
                    ExiteTime = row.Cell(12).GetString(), // Column 2
                };
                employees.Add(person);


                // Send SMS for the current person
                var smsMessage = new PARSGREEN.CORE.RESTful.SMS.Message(
                    "GUID FROM SMS SERVICE CENTER",
                    "http://sms.parsgreen.ir/Apiv2");

                var smsBody1 = $"{person.Title} {person.FullName}, پرواز شما از تهران به کرمان در تاریخ {person.TehToKerDateTime}  از ترمینال شماره{person.TerminalNumber} فرودگاه مهر اباد با خط هوایی {person.Airline} شماره پرواز  {person.FlightNumber} در ساعت {person.FightTime} صادر گردیده است.\n"+
                "هماهنگی 09139464102 \n+ www.khayerin.mubam.ac.ir";
                var mobile = new[] { person.PhoneNumber };
                try
                {
                    // Send SMS
                    var response1 = smsMessage.SendSms(smsBody1, mobile);
                     LogSmsResponse(response1, smsBody1, person.PhoneNumber);


                      // Second SMS: Additional Information
                        var smsBody2 = $"اطلاعات تکمیلی: پرواز کرمان تهران شما در تاریخ {person.ExiteDate} از خط هوایی {person.ExitAirline} با شماره {person.ExitFilghtNumber} در ساعت {person.ExiteTime}میباشد .";
                        var response2 = smsMessage.SendSms(smsBody2, mobile);
                        LogSmsResponse(response2, smsBody2, person.PhoneNumber);

                }
                    // File name logic
                    // string baseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SmsLog.txt");
                    // string filePath = GetUniqueFilePath(baseFilePath);


                    // Prepare log message
                   catch (Exception ex)
                    {
                        string errorMessage = $"ERROR: Could not send SMS to {person.PhoneNumber}. Exception: {ex.Message}";
                        Console.WriteLine(errorMessage);

                        // Log error to file
                        AppendToFile(logFilePath, errorMessage);
                    }
                    // Append to file
                  

              
            }
        }
          return employees;
    }
     private static void LogSmsResponse(PARSGREEN.CORE.RESTful.SMS.Model.Message.Message_SendSms_Res  response, string smsBody, string phoneNumber)
        {
            // Log success or failure after sending SMS
            string logMessage;
            if (response.R_Success)
            {
                logMessage = $"SUCCESS: {smsBody} -> {phoneNumber}";
                Console.WriteLine($"SMS successfully sent to {phoneNumber}");
            }
            else
            {
                logMessage = $"FAILED: {smsBody} -> {phoneNumber}. Error: {response.R_Error}";
                Console.WriteLine($"Failed to send SMS to {phoneNumber}: {response.R_Error}");
            }

            // Log the message to a file
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SmsLog.txt");
            AppendToFile(logFilePath, logMessage);
        }

     private static string GetUniqueFilePath(string baseFilePath)
        {
            string directory = Path.GetDirectoryName(baseFilePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(baseFilePath);
            string extension = Path.GetExtension(baseFilePath);

            int counter = 1;
            string uniqueFilePath = baseFilePath;

            while (File.Exists(uniqueFilePath))
            {
                uniqueFilePath = Path.Combine(directory, $"{fileNameWithoutExtension}_{counter}{extension}");
                counter++;
            }

            return uniqueFilePath;
        }

        private static void AppendToFile(string filePath, string message)
        {
            try
            {
                using (var writer = new StreamWriter(filePath, append: true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

}