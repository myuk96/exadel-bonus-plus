using System;
using System.Collections.Generic;
using System.Text;

namespace ExadelBonusPlus.Services.Models
{
    public class UserHistoryDto
    {
        public Guid HistoryId { get; set; }
        public BonusDto BonusDto { get; set; }
        public DateTime UsageDate { get; set; }
        public int Rating { get; set; }
    }
}
