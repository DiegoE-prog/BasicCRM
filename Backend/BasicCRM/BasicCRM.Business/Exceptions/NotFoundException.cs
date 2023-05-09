using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Business.Exceptions
{
    public class NotFoundException : Exception
    {
        private const string DefaultMessage = "Cannot find that item";

        public NotFoundException() : base(DefaultMessage) { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(Exception innerException) : base(DefaultMessage, innerException) { }

        public NotFoundException(string message, Exception innerException) : base(DefaultMessage, innerException) { }
    }
}
