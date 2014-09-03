using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interext.Models
{
    public class EventVsWaitingApprovalUser
    {
        
            [Key, Column(Order = 0)]
            public int EventId { get; set; }
            [Key, Column(Order = 1)]
            public string UserId { get; set; }

            public virtual Event Event { get; set; }
            public virtual ApplicationUser WaitingApprovalUser { get; set; }
        
    }
}