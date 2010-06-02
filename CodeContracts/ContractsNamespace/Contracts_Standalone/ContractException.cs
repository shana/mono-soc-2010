using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Diagnostics.Contracts {

    class ContractException : Exception {

        internal ContractException (string failure, ContractFailureKind kind, string condition, string userMessage, Exception innerException)
            : base (failure, innerException)
        {
            this.Failure = failure;
            this.Kind = kind;
            this.Condition = condition;
            this.UserMessage = userMessage;
        }

        public string Failure { get; private set; }

        public ContractFailureKind Kind { get; private set; }

        public string Condition { get; private set; }

        public string UserMessage { get; private set; }

    }

}
