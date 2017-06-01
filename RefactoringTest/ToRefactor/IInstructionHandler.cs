

using RefactoringTest.Entities;

namespace RefactoringTest.ToRefactor
{
    public interface IInstructionHandler
    {
        void ProcessInstruction(PaymentInfo paymentInfo);
        void ProcessInstruction(PaymentInfo paymentInfo, bool originalLeg);
        void ProcessReverseInstruction(PaymentInfo pay);
    }
}