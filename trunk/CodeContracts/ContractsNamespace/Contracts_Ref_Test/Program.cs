using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Diagnostics.Contracts.Internal;

namespace Contracts_Ref_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Contract.Requires(false, "Message");

            //Contract.ContractFailed += (sender, e) =>
            //{
            //    e.SetHandled();
            //};

            //ContractHelper.TriggerFailure(ContractFailureKind.Assert,"Display",
            //    "usermessage","Cond", new InvalidOperationException());
        }
    }
}
