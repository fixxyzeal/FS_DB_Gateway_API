using System;
using System.Collections.Generic;
using System.Text;

namespace BO.StaticModels
{
    public static class Formatter
    {
        public static readonly DateTime DB_CreatedDate = DateTime.UtcNow;
        public static readonly string InformationLogType = "Information";
        public static readonly string ErrorLogType = "Error";
    }
}