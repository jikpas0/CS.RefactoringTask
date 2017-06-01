using System;
using RefactoringTest.Entities;
using RefactoringTest.Enumerations;

namespace RefactoringTest.ToRefactor

{
    public class InstructionHandlerFactory
    {
        private readonly IDataService _dataService;
        private readonly IInstructionHandler _onlineInstructionHandler;
        private readonly IInstructionHandler _faxInstructionHandler;

        public InstructionHandlerFactory(IDataService dataService, OnlineInstructionHandler onlineInstructionHandler, 
            FaxInstructionHandler faxInstructionHandler)
        {
            _dataService = dataService;
            _onlineInstructionHandler = onlineInstructionHandler;
            _faxInstructionHandler = faxInstructionHandler;
        }

        public void ProcessInstruction(PaymentInfo paymentInfo)
        {
            Console.WriteLine("ProcessInstruction on Payment {0}", paymentInfo.ID);

            switch (paymentInfo.Method)
            {
                case PaymentMethod.Fax:
                    ProcessFaxInstruction(paymentInfo);
                    break;

                case PaymentMethod.Online:
                    ProcessOnlineInstruction(paymentInfo, true);
                    break;

                case PaymentMethod.WebEntry:
                    ProcessWebEntryInstruction(paymentInfo);
                    break;

                default:
                    break;
            }

            ProcessReverseInstruction(paymentInfo);

            if (paymentInfo.Method == PaymentMethod.Fax)
            {
                ProcessPaymentReceipt(paymentInfo);
            }
            Console.WriteLine();
        }

        private void ProcessReverseInstruction(PaymentInfo paymentInfo)
        {
            if (paymentInfo.ReverseLeg == null)
            {
                return;
            }

            ReverseLeg reverseLegDetails = paymentInfo.ReverseLeg;

            switch (reverseLegDetails.PaymentMethod)
            {
                case PaymentMethod.Fax:
                    Console.WriteLine("ProcessReverseInstruction on Payment {0}", paymentInfo.ID);
                    ProcessReverseFaxInstruction(paymentInfo);
                    break;

                case PaymentMethod.Online:
                    Console.WriteLine("ProcessReverseInstruction on Payment {0}", paymentInfo.ID);
                    ProcessOnlineInstruction(paymentInfo, false);
                    break;

                case PaymentMethod.WebEntry:
                    // Nothing to do - assuming user realises there is work to do e.g. notify JPMorgan that there is a receipt
                    break;
            }
        }

        private void ProcessFaxInstruction(PaymentInfo paymentInfo)
        {
            _faxInstructionHandler.ProcessInstruction(paymentInfo);
            UpdateCompleted(paymentInfo);
        }

        private void ProcessReverseFaxInstruction(PaymentInfo paymentInfo)
        {
            _faxInstructionHandler.ProcessReverseInstruction(paymentInfo);
        }

        private void ProcessOnlineInstruction(PaymentInfo paymentInfo, bool originalLeg)
        {
            _onlineInstructionHandler.ProcessInstruction(paymentInfo, originalLeg);

            if (originalLeg)
            {
                UpdateCompleted(paymentInfo);
            }
        }
        
        private void ProcessWebEntryInstruction(PaymentInfo paymentInfo)
        {
            UpdateCompleted(paymentInfo);
        }

        private void ProcessPaymentReceipt(PaymentInfo paymentInfo)
        {
            Console.WriteLine("ProcessPaymentReceipt on Payment {0}", paymentInfo.ID);
            // Call the conversion method and get back the path to the converted PDF document
            string fileName = PDFBuilder.GenerateReceiptFax(paymentInfo);
            Emailer.SendEmailWithAttachment(Config.EmailContact, fileName);
        }

        private void UpdateCompleted(PaymentInfo paymentInfo)
        {
            _dataService.ReleasePayment(paymentInfo.ID);
        }
    }
}