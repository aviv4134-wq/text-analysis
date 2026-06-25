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
                if (report.Length != 5) continue;
                
                string unitname = report[0].Trim();
                if (string.IsNullOrWhiteSpace(unitname)) continue;
                
                string reporttype = report[1].Trim();
                if (!Enum.TryParse(reporttype, true, out ReportType parsedType)) continue;
                
                string priority = report[2].Trim();
                if (!int.TryParse(priority, out int priorityParsed)) continue;
                if (1 > priorityParsed || priorityParsed > 5) continue;
                
                string score = report[3].Trim();
                if (!double.TryParse(score, out double scoreParsed)) continue;
                if (0.0 > scoreParsed || scoreParsed > 100.0) continue;
                
                string status = report[4].Trim();
                if(!Enum.TryParse(status, true, out Status parsedStatus)) continue;

                Unitnames[validRecordsCount] = unitname;
                ReportType[validRecordsCount] = parsedType;
                Priority[validRecordsCount] = priorityParsed;
                Score[validRecordsCount] = scoreParsed;
                Status[validRecordsCount] = parsedStatus;
                validRecordsCount += 1;
            }
            return validRecordsCount;
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
            Console.WriteLine($"""
                
                === Report Statistics ===
                Total Reports: {validRecordsCount}
                Average Score: {avrage_score}
                Highest Score: {maxScore}
                Lowest Score: {minScore}
                """);
        }

        static void DisplayStatusCounts(Status[] statuses, int validRecordsCount)
        {
            int ApprovedCount = CountByStatus(statuses, Status.Approved, validRecordsCount);
            int PendingCount = CountByStatus(statuses, Status.Pending, validRecordsCount);
            int RejectedCount = CountByStatus(statuses, Status.Rejected, validRecordsCount);

            Console.WriteLine($"""
                
                === Reports by Status ===
                Pending: {PendingCount}
                Approved: {ApprovedCount}
                Rejected: {RejectedCount}
                """);
        }

        static void DisplayTypeCounts(ReportType[] ReportTypes, int validRecordsCount)
        {
            int ReconCount = CountByType(ReportTypes, ReportType.Recon, validRecordsCount);
            int AnalyzedCount = CountByType(ReportTypes, ReportType.Analyze, validRecordsCount);
            int CollectCount = CountByType(ReportTypes, ReportType.Collect, validRecordsCount);
            int IntelCount = CountByType(ReportTypes, ReportType.Intel, validRecordsCount);

            Console.WriteLine($"""
                
                === Reports by Type ===
                Collect: {CollectCount}
                Analyze: {AnalyzedCount}
                Recon: {ReconCount}
                Intel: {IntelCount}
                """);
        }

        static void DisplayHighestPriorityApproved(string[] Unitnames, ReportType[] ReportTypes, int[] Priority, double[] Score,Status[] Statuses, int validRecordsCount)
        {
            int HighestPriorityApprovedIndex = 0;
            for (int index = 0;index < validRecordsCount; index++)
            {
                if (Statuses[index] == Status.Approved & Priority[index] > HighestPriorityApprovedIndex)
                    HighestPriorityApprovedIndex = index;
            }
            Console.WriteLine($"""
                === Highest Priority Approved Report ===
                Unit: {Unitnames[HighestPriorityApprovedIndex]}
                Type: {ReportTypes[HighestPriorityApprovedIndex]}
                Priority: {Priority[HighestPriorityApprovedIndex]}
                Score: {Score[HighestPriorityApprovedIndex]}
                """);
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
        


        static void Main()
        {
            string[] Unitnames = new string[100];
            ReportType[] ReportTypes = new ReportType[100];
            int[] Priority = new int[100];
            double[] Score = new double[100];
            Status[] Statuses = new Status[100];

            string? path = "reports.txt";       
            string[] linesText = LoadFile(path);
            if (linesText == null) return;
            int linesCount = linesText.Length;
            Console.WriteLine($"File loaded: {linesCount} lines found.");
            
            int validRecordsCount = ProcessReports(linesText, Unitnames, ReportTypes, Priority, Score, Statuses);
            int invalidRecordsCount = linesCount - validRecordsCount;
            Console.WriteLine($"""
                
                Processing complete.
                Valid records:{validRecordsCount}
                Invalid records:{invalidRecordsCount}
                Stored {validRecordsCount} valid records for analysis
                """);
            DisplayBasicStatistics(Score, validRecordsCount);
            DisplayStatusCounts(Statuses, validRecordsCount);
            DisplayTypeCounts(ReportTypes, validRecordsCount);
            DisplayHighestPriorityApproved(Unitnames, ReportTypes, Priority, Score, Statuses, validRecordsCount);
            DisplayAverageByPriority(Priority, Score, validRecordsCount);

        }
    }
}
