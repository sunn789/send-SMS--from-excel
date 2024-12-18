
markdown
Copy code
# SMS Notification System

A simple .NET Core application to read employee details from an Excel file and send SMS notifications about flight details using the ParsGreen SMS API. The system also logs the status of each SMS sent to a log file (`SmsLog.txt`). 

## Features

- **Read Excel File**: Reads employee details like name, phone number, flight details from an Excel file.
- **Send SMS**: Sends a two-part SMS to each employee: one for flight details and one for additional information (like exit date and flight).
- **Logging**: Logs success and failure details of each SMS sent into a log file (`SmsLog.txt`).
- **Unique Log File Generation**: If the `SmsLog.txt` file exists, it creates a new log file with an incremented name like `SmsLog_1.txt`, `SmsLog_2.txt`, etc.
- **ParsGreen SMS Service**: The SMS notifications are sent using the ParsGreen SMS service API.

## Requirements

- .NET Core 3.1 or later.
- Access to ParsGreen SMS API with a valid API key and a unique GUID.
- An Excel file with employee details.

## Installation

### 1. Clone the repository

Clone this repository to your local machine:

```bash
git clone https://github.com/yourusername/sms-notification-system.git
2. Install dependencies
The project uses ClosedXML for reading Excel files. To install the required dependencies, run the following command in your project directory:

bash
Copy code
dotnet restore
3. Add API Key and GUID
In the code, replace the API_KEY and GUID in the Message class with your own ParsGreen API key and GUID:

csharp
Copy code
new PARSGREEN.CORE.RESTful.SMS.Message("YOUR_API_KEY", "http://sms.parsgreen.ir/Apiv2");
API Key: You will need to request a valid API key from ParsGreen for the SMS service.
GUID: You also need a unique GUID for sending SMS messages, which is provided by ParsGreen.
4. Prepare the Excel File
Make sure your Excel file is structured with the following columns:

Title	Full Name	Phone Number	Teh To Ker DateTime	Terminal Number	Airline	Flight Number	Flight Time	Exit Date	Exit Airline	Exit Flight Number	Exit Time
Mr.	John Doe	09123456789	2024-12-20 10:00	1	Iran Air	IR101	10:00	2024-12-22	Iran Air	IR102	18:00
5. Run the Application
To run the application, execute the following command:

bash
Copy code
dotnet run
You will be prompted to upload an Excel file. The system will then send SMS messages to each person listed in the file.

Storing Logs in a Database
By default, this project stores log messages in a text file (SmsLog.txt). However, if you prefer to store logs in a database (such as SQL Server, MySQL, or SQLite), you can implement this functionality by following these steps:

Set up a Database: Create a database to store the log data. A simple table with the following structure is recommended:

sql
Copy code
CREATE TABLE SmsLogs (
    Id INT PRIMARY KEY IDENTITY,
    Timestamp DATETIME,
    Message VARCHAR(255),
    Status VARCHAR(50)
);
Modify the Code: Replace the file logging method with a method that writes to the database. Use an ORM like Entity Framework Core or ADO.NET to interact with the database.

Configure the Connection String: Set up the connection string to your database in the appsettings.json or in your environment variables.

Example code to log SMS status to a database:

csharp
Copy code
public void LogToDatabase(string message, string status)
{
    using (var context = new SmsDbContext())
    {
        var log = new SmsLog
        {
            Timestamp = DateTime.Now,
            Message = message,
            Status = status
        };

        context.SmsLogs.Add(log);
        context.SaveChanges();
    }
}
Database Context Class: Define a DbContext class to interact with the database:
csharp
Copy code
public class SmsDbContext : DbContext
{
    public DbSet<SmsLog> SmsLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionString");
    }
}
This feature is not implemented by default, but it can easily be integrated by following the steps above.

File Structure
bash
Copy code
/sms-notification-system
│
├── ExcelHelper.cs         # Logic for reading Excel and sending SMS
├── SmsLog.txt             # Log file where SMS sending results are stored
├── Program.cs             # Main entry point for the application
├── Persons.cs             # Model representing a person (employee)
├── SmsDbContext.cs        # Optional: Database context for logging to a database
└── README.md              # Project README file (this one)
Log Files
The system generates a log file (SmsLog.txt) in the root directory.
If the file already exists, it creates new files with names like SmsLog_1.txt, SmsLog_2.txt, etc.
Each log entry includes the status of the SMS sent, with details of success or failure.
Example of Log Entries
yaml
Copy code
2024-12-18 14:00:00: SUCCESS: آقای محمد رضایی, پرواز شما از تهران به کرمان در تاریخ 1402/12/01 از ترمینال شماره 3 فرودگاه مهرآباد با خط هوایی ایران ایر شماره پرواز 789 در ساعت 14:30 صادر گردیده است. -> 09123456789
2024-12-18 14:01:00: SUCCESS: اطلاعات تکمیلی: تاریخ خروج شما 1402/12/03 از خط هوایی ماهان پرواز شماره 123 در ساعت 15:00. -> 09123456789
Contributing
If you would like to contribute to this project, please feel free to fork the repository, create a new branch, and submit a pull request. We welcome improvements, bug fixes, or suggestions!

License
This project is licensed under the MIT License - see the LICENSE file for details.

Contact
For any questions, feel free to reach out to [sunn789] at [sunn789@gmail.com].