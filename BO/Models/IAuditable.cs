using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }

        Guid CreatedBy { get; set; }

        DateTime UpdateDate { get; set; }

        Guid UpdateBy { get; set; }
    }
}