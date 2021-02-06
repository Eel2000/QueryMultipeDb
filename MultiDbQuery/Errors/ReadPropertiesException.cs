using System;
using System.Collections.Generic;
using System.Text;

namespace MultiDbQuery.Errors
{
    public class ReadPropertiesException : Exception
    {
        public ReadPropertiesException()
        {
            throw new ReadPropertiesException("Error occur while readind class properties.\r" +
                "It's seems like this class does not contains any property.");

        }

        public ReadPropertiesException(string message):base(message)
        {

        }
    }
}
