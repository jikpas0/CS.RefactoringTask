using System;
using RefactoringTest;
using RefactoringTest.Entities;
using RefactoringTest.ToRefactor;

namespace RefactoringTestConsole
{
    public class MockInstructionHandler : IInstructionHandler
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

        }
    }
}
