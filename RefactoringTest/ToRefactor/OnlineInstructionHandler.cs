using System;
using RefactoringTest.Entities;

namespace RefactoringTest.ToRefactor
{
    public class OnlineInstructionHandler : IInstructionHandler
    {
        public void ProcessInstruction(PaymentInfo paymentInfo)
        {
            throw new NotImplementedException("Online instructions do not support this overload of ProcessInstruction");
        }

        public void ProcessInstruction(PaymentInfo paymentInfo, bool originalLeg)
        {
            OnlineInstructionBuilder.BuildOnlineInstructionFile(paymentInfo, originalLeg);
        }

        public void ProcessReverseInstruction(PaymentInfo pay)
        {
            throw new NotImplementedException("Online instructions do not support this overload of ProcessReverseInstruction");
        }
    }
}