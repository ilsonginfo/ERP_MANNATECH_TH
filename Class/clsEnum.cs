using System.ComponentModel;


namespace MLM_Program
{
    class clsEnum
    {
        /// <summary>
        /// 태국 EDC결제 	Payment method : 
        /// </summary>
        public enum EDC_Payment_Method : int
        {
            [Description("")]
            EDC_Default = 0,
            [Description("CreditCard")]
            EDC_Card = 3,
            [Description("PromptPay")]
            EDC_Promt = 8,
        }

    }
}
