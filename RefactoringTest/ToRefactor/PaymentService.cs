using System;
using System.Collections.Generic;
using RefactoringTest.Entities;

namespace RefactoringTest.ToRefactor
{
    public class PaymentService
    {
        public void ProcessPendingPayments(IDataService dataService, OnlineInstructionHandler instructionHandler, 
            FaxInstructionHandler faxInstructionHandler)
        {
            try
            {
                List<PaymentInfo> payments = dataService.GetPendingPayments();
                
                var instructionHandlerFactory = new InstructionHandlerFactory(dataService, instructionHandler, faxInstructionHandler);
                
                foreach (var pay in payments)
                {
                    instructionHandlerFactory.ProcessInstruction(pay);
                }
            }
            catch (Exception exPendingPayments)
            {
                Logger.Log(exPendingPayments.Message);
            }
        }
    }
}
