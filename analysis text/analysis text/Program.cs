using System.IO;
using System.Runtime.CompilerServices;


namespace project
{
    enum ReportType
    {
        Collect,
        Analyze,
        Recon,
        Intel
    }

    enum  Status
    {
        Pending,
        Approved,
        Rejected
    }

    class Text_Analysis
    {
        static string[] LoadFile(string path)
        {
            return File.ReadAllLines(path);
        }

        static bool IsFileExists(string path) //work
        {
            if (File.Exists(path)) return true;
            return false;
        }

        static bool IsFileEmpty(string[] data) //work
        {
            if (data.Length == 0) return false;
            foreach (string line in data)
            {
                if (! line.IsWhiteSpace()) return true;
            }
            return false;   

        }

        static int CountLines(string[] data)
        {
            return data.Length;
        }
        //start main load faile

        static void ProcessReports(string[]linesText, string[]Unitnames, ReportType[] ReportType, int[] Priority, double[] Score, Status[] Status)
        {
            int valid_records = 0;
            int invalid_records = 0;
            foreach (string line in linesText)
            {
                string[] report = line.Split(",");
                if (!IsLength4(report))
                    continue;
                string unitname = report[0].Trim();
                string reporttype = report[1].Trim();
                string priority = report[2].Trim();
                string score = report[3].Trim();
                string status = report[4].Trim();
                
                if (unitname.IsWhiteSpace())
                {
                    invalid_records += 1;
                    continue;
                }
                if (!IsReportTypeValid(reporttype))
                {
                    invalid_records += 1;
                    continue;
                }
                if (!IsStatusValid(status))
                {
                    invalid_records += 1;
                    continue;
                }
                if (!int.TryParse(priority, out int priority_int))
                {
                    invalid_records += 1;
                    continue;
                }
                if (1 > priority_int || priority_int > 5)
                {
                    invalid_records += 1;
                    continue;
                }
                if (!double.TryParse(score, out double score_double))
                {
                    invalid_records += 1;
                    continue;
                }
                if (0.0 > score_double || score_double > 100.0)
                {
                    invalid_records += 1;
                    continue;
                }
                
                valid_records += 1;
            }
            Console.WriteLine(invalid_records);
        }

        static bool IsLength4(string[] report)
        {
            if (report.Length != 5) return false;
            return true;
        }

        static bool IsReportTypeValid(string report_type)
        {
            if (Enum.TryParse(report_type, true, out ReportType _)) return true;
            return false;
            
        }

        static bool IsStatusValid(string priority)
        {
            if (Enum.TryParse(priority,true,out Status _)) return true;
            return true;
            
        }


        static void Main()
        {
            string[] Unitnames = new string[100];
            ReportType[] ReportType = new ReportType[100];
            int[] Priority = new int[100];
            double[] Score = new double[100];
            Status[] Status = new Status[100];

            string path = "reports.txt";       
            string[] linesText = LoadFile(path);
            
            ProcessReports(linesText,Unitnames, ReportType, Priority, Score, Status);
           

        }

        
    }
}
