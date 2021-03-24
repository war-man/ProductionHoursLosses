using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionHoursLosses.Models
{
    public class ResultSelect2<T>
    {
        public List<T> results { get; set; }
        public Pagination pagination { get; set; }

        public void GetResultsPaged(int page, int pageSize)
        {
            if (page == 1)
                results.Take(pageSize);
            else
                results.AddRange(results.Skip(page * pageSize).Take(pageSize));

            if (page * pageSize < results.Count())
                pagination.more = true;
            else
                pagination.more = false;
        }
    }
}