using System;

namespace Performance.Management.BLL.Helper
{
    public class Common
    {
        public DateTime ExcludeNotWorkDay(DateTime date, int days)
        {
            DateTime NewDate = date;
            for (int i = 0; i < days; i++)
            {
                NewDate = NewDate.AddDays(1);

                if (NewDate.DayOfWeek == DayOfWeek.Friday)
                {
                    NewDate = NewDate.AddDays(1);
                }
                if (NewDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    NewDate = NewDate.AddDays(1);
                }
            }
            return NewDate;
        }
    }
}
