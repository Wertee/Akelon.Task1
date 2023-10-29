namespace PracticTask1
{
    class Program
    {
        private const int CountOfVacationDays = 28;
        private static readonly int[] VacationSteps = { 7, 14 };
        static void Main(string[] args)
        {
            var vacationDictionary = new Dictionary<string, List<DateTime>>()
            {
                ["Иванов Иван Иванович"] = new List<DateTime>(), ["Петров Петр Петрович"] = new List<DateTime>(),
                ["Юлина Юлия Юлиановна"] = new List<DateTime>(), ["Сидоров Сидор Сидорович"] = new List<DateTime>(),
                ["Павлов Павел Павлович"] = new List<DateTime>(), ["Георгиев Георг Георгиевич"] = new List<DateTime>()
            };
            var availableWorkingDaysOfWeekWithoutWeekends = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            
            foreach (var vacationList in vacationDictionary)
            {
                List<DateTime> allVacations = vacationDictionary.SelectMany(x => x.Value).ToList();
                Random gen = new Random();
                DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime endOfYear = new DateTime(DateTime.Today.Year, 12, 31);
                int vacationCount = CountOfVacationDays;
                while (vacationCount > 0)
                {
                    int range = (endOfYear - startOfYear).Days;
                    var startDate = startOfYear.AddDays(gen.Next(range));
                    DateTime endDate;
                    if (availableWorkingDaysOfWeekWithoutWeekends.Contains(startDate.DayOfWeek.ToString()))
                    {
                        int vacIndex = gen.Next(VacationSteps.Length);
                        int countOfVacationDays = VacationSteps[vacIndex];
                        if (countOfVacationDays > vacationCount)
                            countOfVacationDays = vacationCount;
                        
                        endDate = startDate.AddDays(countOfVacationDays);
                        
                        // Проверка условий по отпуску
                        bool canCreateVacation = false;
                        
                        if (!vacationList.Value.Any(element => element >= startDate && element <= endDate))
                        {
                            if (!vacationDictionary.SelectMany(x => x.Value).ToList().Any(element => element.AddDays(3) >= startDate && element.AddDays(3) <= endDate)) {
                                var existStart = vacationList.Value.Any(element => element.AddMonths(1) >= startDate && element.AddMonths(-1) <= endDate);
                                var existEnd = vacationList.Value.Any(element => element.AddMonths(-1) <= endDate && element.AddMonths(1) >= startDate);
                                if (!existStart || !existEnd)
                                    canCreateVacation = true; }
                        }

                        if (canCreateVacation) {
                            for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1))
                            {   
                                vacationList.Value.Add(dt);
                            }
                            vacationCount -= countOfVacationDays;
                        }
                    }
                }
            }
            foreach (var vacationList in vacationDictionary)
            {
                Console.WriteLine("Дни отпуска " + vacationList.Key + ": ");
                foreach (var vacationDay in vacationList.Value)
                {
                    Console.WriteLine(vacationDay);
                }
            }
            Console.ReadKey();
        }
    }
}
