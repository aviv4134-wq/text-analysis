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

    enum Status 
    {
        Pending,
        Approved,
        Rejected
    }

    class Text_Analysis
    {
        static string[] ReadFile(string path)
        {
            return File.ReadAllLines(path);
        }

        static bool IsFileExists(string path) 
        {
            if (File.Exists(path)) return true;
            return false;
        }

        static bool IsFileEmpty(string[] data) 
        {
            if (data.Length == 0) return false;
            foreach (string line in data)
            {
                if (! line.IsWhiteSpace()) return true;
            }
            return false;   
        }

        static string[]? LoadFile(string path)
        {
            if (!IsFileExists(path))
            {
                Console.WriteLine("error the file not exists");
                return null;
            }
            string[] reports = ReadFile(path);
            if (! IsFileEmpty(reports))
            {
                Console.WriteLine("error the file is empty");
                return null;
            }
            return reports;
        }

      
        static int ProcessReports(string[]linesText, string[]Unitnames, ReportType[] ReportType, int[] Priority, double[] Score, Status[] Status)
        {
            int validRecordsCount = 0;
            for (int i = 0; i < linesText.Length; i++)
            {
                string[] report = linesText[i].Split(",");
              
                if (! IsLength5(report)) 
                    continue;
                string unitName = report[0].Trim();
                string reportType = report[1].Trim();
                string priority = report[2].Trim();
                string score = report[3].Trim();
                string status = report[4].Trim();
                if (!IsReportValid(unitName, reportType, priority, score, status)) 
                    continue;
                
                ReportType parsedReportType = Enum.Parse<ReportType>(reportType,true);
                int priorityParsed = int.Parse(priority);
                double scoreParsed = double.Parse(score);
                Status parsedStatus = Enum.Parse<Status>(status, true);
                
                Unitnames[validRecordsCount] = unitName;
                ReportType[validRecordsCount] = parsedReportType;
                Priority[validRecordsCount] = priorityParsed;
                Score[validRecordsCount] = scoreParsed;
                Status[validRecordsCount] = parsedStatus;
                validRecordsCount += 1;
            }
            return validRecordsCount;
        }

        static bool IsLength5(string[] report)
        {
            if (report.Length != 5)
            {
                Console.WriteLine("{Invalid record:the report must be with 5 fields}");
                return false;
            }
            return true;
        }

        static bool IsReportValid(string unitName, string reportType, string priority, string score, string status)
        {
            if (string.IsNullOrWhiteSpace(unitName))
            {
                Console.WriteLine("{Invalid record:the unitname is empty}");
                return false;
            }
            if (!Enum.TryParse(reportType, true, out ReportType _))
            {
                Console.WriteLine("{Invalid record:the report type is not valid}");
                return false;
            }
            if (!int.TryParse(priority, out int priorityParsed)) 
            {
                Console.WriteLine("{Invalid record:priority is not hole number}");
                return false;
            }
            if (1 > priorityParsed || priorityParsed > 5) 
            {
                Console.WriteLine("{Invalid record: Priority out of range}");
                return false;
            }
            if (!double.TryParse(score, out double scoreParsed))
            {
                Console.WriteLine("{Invalid record: Score is not a valid number}");
                return false;
            }
            if (0.0 > scoreParsed || scoreParsed > 100.0)
            {
                Console.WriteLine("{Invalid record: Score is not a valid number}");
                return false;
            }
            if (!Enum.TryParse(status, true, out Status parsedStatus))
            {
                Console.WriteLine("{Invalid record: the status not valid}");
                return false;
            }
            Console.WriteLine("Valid record processed.");
            return true;
    }

        static double CalculateAverage(double[] Score,int validRecordsCount)
        {
            double sumScore = 0;
            foreach (int score in Score)
            {
                sumScore += score;
            }
            double avrage_score = sumScore / validRecordsCount;
            return avrage_score; 
        }

        static double FindMaxScore(double[] Scores)
        {
            double maxScore = 0;
            foreach (double score in Scores)
            {
                if (score > maxScore) maxScore = score;
            }
            return maxScore;
        }

        static double FindMinScore(double[] Scores)
        {
            double minScore = Scores[0];
            foreach (double score in Scores)
            {
                if (score < minScore) minScore = score;
            }
            return minScore;
        }


        static int CountByStatus(Status[] statuses,Status statusChoise,int validRecordsCount)
        {
            int countByStatus = 0;
            for (int index = 0;index < validRecordsCount; index++)
            {
                if (statuses[index] == statusChoise) countByStatus++; 
            }
            return countByStatus;
        }

        static int CountByType(ReportType[] reportTypes,ReportType reportChoise, int validRecordsCount)
        {
            int countByType = 0;
            for (int index = 0; index < validRecordsCount; index++)
            {
                if (reportTypes[index] == reportChoise) countByType++;
            }
            return countByType;
        }

        static void DisplayBasicStatistics(double[] Score,int validRecordsCount)
        {
            double avrage_score = CalculateAverage(Score, validRecordsCount);
            double maxScore = FindMaxScore(Score);
            double minScore = FindMinScore(Score);
            string output =($"""
                
                === Report Statistics ===
                Total Reports: {validRecordsCount}
                Average Score: {avrage_score}
                Highest Score: {maxScore}
                Lowest Score: {minScore}
                """);
            SaveToFile(output);
            Console.WriteLine(output);
        }

        static void DisplayStatusCounts(Status[] statuses, int validRecordsCount)
        {
            int ApprovedCount = CountByStatus(statuses, Status.Approved, validRecordsCount);
            int PendingCount = CountByStatus(statuses, Status.Pending, validRecordsCount);
            int RejectedCount = CountByStatus(statuses, Status.Rejected, validRecordsCount);

            string output = ($"""
                
                === Reports by Status ===
                Pending: {PendingCount}
                Approved: {ApprovedCount}
                Rejected: {RejectedCount}
                """);
            SaveToFile(output);

            Console.WriteLine(output);
        }

        static void DisplayTypeCounts(ReportType[] ReportTypes, int validRecordsCount)
        {
            int ReconCount = CountByType(ReportTypes, ReportType.Recon, validRecordsCount);
            int AnalyzedCount = CountByType(ReportTypes, ReportType.Analyze, validRecordsCount);
            int CollectCount = CountByType(ReportTypes, ReportType.Collect, validRecordsCount);
            int IntelCount = CountByType(ReportTypes, ReportType.Intel, validRecordsCount);

            string output = ($"""
                
                === Reports by Type ===
                Collect: {CollectCount}
                Analyze: {AnalyzedCount}
                Recon: {ReconCount}
                Intel: {IntelCount}
                """);
            SaveToFile(output);
            Console.WriteLine(output);


        }

        static void DisplayHighestPriorityApproved(string[] Unitnames, ReportType[] ReportTypes, int[] Priority, double[] Score,Status[] Statuses, int validRecordsCount)
        {
            int HighestPriorityApprovedIndex = 0;
            for (int index = 0;index < validRecordsCount; index++)
            {
                if (Statuses[index] == Status.Approved & Priority[index] > HighestPriorityApprovedIndex)
                    HighestPriorityApprovedIndex = index;
            }
            string output = ($"""
                === Highest Priority Approved Report ===
                Unit: {Unitnames[HighestPriorityApprovedIndex]}
                Type: {ReportTypes[HighestPriorityApprovedIndex]}
                Priority: {Priority[HighestPriorityApprovedIndex]}
                Score: {Score[HighestPriorityApprovedIndex]}
                """);
            SaveToFile(output);
            Console.WriteLine(output);
        }

        static void DisplayAverageByPriority(int[] Priority, double[] Score, int validRecordsCount)
        {
            Console.WriteLine("=== Average Score by Priority ===");

            for (int currentPriority = 1; currentPriority <= 5; currentPriority++)
            {
                int count = 0;
                double totalScore = 0;

                for (int index = 0; index < validRecordsCount; index++)
                {
                    if (Priority[index] == currentPriority)
                    {
                        count++;
                        totalScore += Score[index];
                    }
                }

                if (count > 0)
                {
                    double average = totalScore / count;
                    Console.WriteLine($"Priority {currentPriority}: {average:F2}");
                }
                else
                {
                    Console.WriteLine($"Priority {currentPriority}: NO REPORTS");
                }
            }
        }
        
        static void DisplayCountReportsValdation(int linesCount, int validRecordsCount)
        {
            int invalidRecordsCount = linesCount - validRecordsCount;

            string output = ($"""
                
                Processing complete.
                Valid records:{validRecordsCount}
                Invalid records:{invalidRecordsCount}
                Stored {validRecordsCount} valid records for analysis
                """);

            SaveToFile(output);
            Console.WriteLine(output);
        }

        static void SaveToFile(string output)
        {
            
            File.AppendAllText("output.txt", output);
            
        }


        static void Main()
        {
            string[] Unitnames = new string[100];
            ReportType[] ReportTypes = new ReportType[100];
            int[] Priority = new int[100];
            double[] Score = new double[100];
            Status[] Statuses = new Status[100];

            File.Create("output.txt").Close();
            string path = "reports.txt";
            string[] linesText = LoadFile(path);

            int linesCount = linesText.Length;
            Console.WriteLine($"File loaded: {linesCount} lines found.");
            
            int validRecordsCount = ProcessReports(linesText, Unitnames, ReportTypes, Priority, Score, Statuses);
            
            DisplayCountReportsValdation(linesCount, validRecordsCount);
            DisplayBasicStatistics(Score, validRecordsCount);
            DisplayStatusCounts(Statuses, validRecordsCount);
            DisplayTypeCounts(ReportTypes, validRecordsCount);
            DisplayHighestPriorityApproved(Unitnames, ReportTypes, Priority, Score, Statuses, validRecordsCount);
            DisplayAverageByPriority(Priority, Score, validRecordsCount);
            Console.ReadLine();

        }
    }
}
