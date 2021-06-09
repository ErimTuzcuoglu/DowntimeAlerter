using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace DowntimeAlerter.Infrastructure.Helper
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(JsonConvert.SerializeObject(new List<string> {message}))
        {
        }

        public CustomException(string message, Exception exception) : base(
            JsonConvert.SerializeObject(new List<string> {message}),
            exception)
        {
        }

        public CustomException(IEnumerable<string> message) : base(JsonConvert.SerializeObject(message))
        {
        }

        public CustomException(IEnumerable<string> message, Exception exception) : base(
            JsonConvert.SerializeObject(message), exception)
        {
        }

        public override string ToString()
        {
            if (InnerException == null)
            {
                return base.ToString();
            }

            return string.Format(CultureInfo.InvariantCulture, "{0} [See nested exception: {1}]", base.ToString(),
                InnerException);
        }
    }
}