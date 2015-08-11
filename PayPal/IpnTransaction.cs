/* ASPnetDating 
 * Copyright (C) 2003-2014 eStream 
 * http://www.aspnetdating.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at http://www.aspnetdating.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using PayPal.Web.Controls;

namespace ASPNET.StarterKit.Commerce
{

    #region Enums used with IPN

    /// <summary>
    /// The possible IPN transaction types (txn_type).
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// The payment was setn by your customer via Buy Now Buttons, Donations,
        /// or Smart Logos.
        /// </summary>
        Web_Accept = 1,

        /// <summary>
        /// The payment was sent by your customer via the PayPal Shopping
        /// Cart.
        /// </summary>
        Cart,

        /// <summary>
        /// This payment was sent by your customer from the PayPal website,
        /// using the "Send Money" tab.
        /// </summary>
        Send_Money,

        /// <summary>
        /// This payment indicates that a customer has signed up for a subscription.
        /// </summary>
        Subscr_Signup,

        /// <summary>
        /// This payment indicates that the subscription payment was completed.
        /// </summary>
        Subscr_Payment,

        /// <summary>
        /// This indicates that the subscription has been cancelled.
        /// </summary>
        Subscr_Cancel,

        /// <summary>
        /// This indicates that the subscription payment has failed.
        /// </summary>
        Subscr_Failed,

        /// <summary>
        /// This indicates that the subscription has reached the End of Term (EOT).
        /// </summary>
        Subscr_Eot,

        /// <summary>
        /// This indicates that the subscription has been modified.
        /// </summary>
        Subscr_Modify,

        /// <summary>
        /// This means the transaction was reversed. If <see cref="IpnTransaction.PaymentStatus"/>
        /// is <see cref="PaymentStatus.Completed"/>, then a reversal to a transaction
        /// has occured, and money has been transferred from your account back to the 
        /// customer. If it is <see cref="PaymentStatus.Canceled"/>, then a previous
        /// reversal has been reversed, and the money has been returned to your account.
        /// </summary>
        Reversal // Additional entries from specs, but not in excel doc
    }

    /// <summary>
    /// The possible payment statuses (payment_status).
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// If referring to an initial purchase, this means the payment has been completed 
        /// and the funds are successfully in your account balance. If referring to a reversal
        /// (ie. <see cref="TransactionType"/> = Reversal), then it means the reversal has
        /// been completed and funds have been removed from your account and returned to the 
        /// customer.
        /// </summary>
        Completed = 1,

        /// <summary>
        /// The payment is pending; see <see cref="PendingReason"/>. Note: You will receive another
        /// IPN when the payment becomes <see cref="PaymentStatus.Completed"/>, <see cref="PaymentStatus.Failed"/>,
        /// or <see cref="PaymentStatus.Denied"/>.
        /// </summary>
        Pending,

        /// <summary>
        /// The payment has failed. This will only happen if the payment was made from your customer's
        /// bank account.
        /// </summary>
        Failed,

        /// <summary>
        /// You, the merchant, denied the payment. This will only happen if the payment was previously 
        /// pending due to one of the <see cref="PendingReason"/>.
        /// </summary>
        Denied,

        /// <summary>
        /// You, the merchant, rfunded the payment.
        /// </summary>
        Refunded, // Additional entries from specs, but not in excel doc

        /// <summary>
        /// This means a reversal has been canceled (eg. you, the merchant, won a dispute with the
        /// customer and the funds for the transaction that was reversed has been returned to you).
        /// </summary>
        Canceled
    }

    /// <summary>
    /// The possible pending reasons (pending_reason).
    /// </summary>
    public enum PendingReason
    {
        /// <summary>
        /// The payment is pending because it was made by an eCheck, which has not yet cleared.
        /// </summary>
        Echeck = 1,

        /// <summary>
        /// You do not have a balance in the currency sent, and you do not have your Payment Receiving 
        /// Preferences set to automatically convert and accept this payment. You must manually accept
        /// or deny this payment.
        /// </summary>
        Multi_Currency,

        /// <summary>
        /// The payment is pending because you, the merchant, hold an international account and do not 
        /// have a withdrawal method. You must manually accept or deny this payment from your Account 
        /// Overview.
        /// </summary>
        Intl,

        /// <summary>
        /// The payment is pending because you, the merchant, are not yet Verified. You must verify your
        /// account before you can accept this payment.
        /// </summary>
        Verify,

        /// <summary>
        /// The payment is pending because your customer did not include a Confirmed shipping address and 
        /// you, the merchant, have your Payment ReceivingPreferences set such that you want to manually 
        /// accept or deny each of these payments. To change your preference, go to the Preferences section 
        /// of your Profile.
        /// </summary>
        Address,

        /// <summary>
        /// The payment is pending because it was made via credit card and you, the merchant, must upgrade 
        /// your account to Business or Premier status in order to receive the funds
        /// </summary>
        Upgrade,

        /// <summary>
        /// The payment is pending because it was made to an email address that is not yet Registered or 
        /// Confirmed.
        /// </summary>
        Unilateral,

        /// <summary>
        /// The payment is pending for a reason other than those listed above. For more information, contact 
        /// customer service.
        /// </summary>
        Other
    }

    /// <summary>
    /// The possible reasons for the specific <see cref="PaymentStatus"/> (reason_code). Depending on the
    /// value of <see cref="PaymentStatus"/>, this could either refer to the reversal of a transaction, or
    /// to the reversal of a previous reversal.
    /// </summary>
    public enum ReasonCode
    {
        /// <summary>
        /// A reversal has occurred on this transaction due to a chargeback by your customer.
        /// </summary>
        Chargeback = 1,

        /// <summary>
        /// A reversal has occurred on this transaction due to your customer triggering a money-back guarantee.
        /// </summary>
        Guarantee,

        /// <summary>
        /// A reversal has occurred on this transaction due to a complaint about the transaction from your 
        /// customer
        /// </summary>
        Buyer_Complaint,

        /// <summary>
        /// A reversal has occurred on this transaction for a reason other than those listed.
        /// </summary>
        Other
    }

    /// <summary>
    /// The possible payment types (payment_type).
    /// </summary>
    public enum PaymentType
    {
        /// <summary>
        /// This payment was funded with an eCheck.
        /// </summary>
        Echeck = 1,

        /// <summary>
        /// This payment was funded with PayPal balance, credit card, or 
        /// instant Transfer.
        /// </summary>
        Instant
    }

    /// <summary>
    /// The possible address statuses (address_status).
    /// </summary>
    public enum AddressStatus
    {
        /// <summary>
        /// Customer provided a confirmed address.
        /// </summary>
        Confirmed = 1,

        /// <summary>
        /// Customer provided an unconfirmed address.
        /// </summary>
        Unconfirmed
    }

    /// <summary>
    /// The possible payer statuses (payer_status).
    /// </summary>
    public enum PayerStatus
    {
        /// <summary>
        /// Customer has a Verified US PayPal account.
        /// </summary>
        Verified = 1,

        /// <summary>
        /// Customer has an Unverified US Paypal account.
        /// </summary>
        Unverified,

        /// <summary>
        /// Customer has a Verified International PayPal account.
        /// </summary>
        Intl_Verified
    }

    #endregion

    /// <summary>
    /// Encapsulates a single IPN transaction.
    /// </summary>
    public class IpnTransaction
    {
        #region IPN Data variables

        // Basic Information
        private ShoppingItemCollection _shoppingItems;

        // Buyer Information
        private IpnCustomerInfo _ipnCustomerInfo;

        // Subscription Items
        private SubscriptionItem[] _subscriptionItems;

        #endregion

        #region IPN Variable Properties

        #region Basic Information

        /// <summary>
        /// Gets or sets the email address or account ID of the payment recipient (ie. the merchant). 
        /// Equivalent to the <see cref="ReceiverEmail"/> if payment is sent to the primary account, 
        /// and essentially an echo of the <see cref="PayPal.Web.Controls.PayPalButton.BusinessEmail"/> 
        /// variable passed in the website payment button. Corresponds to the PayPal 'business' 
        /// variable.
        /// </summary>
        public string Business
        {
            get { return _ipnProperties["business"]; }
            set { _ipnProperties["business"] = value; }
        }

        /// <summary>
        /// Gets or sets the primary email address of the payment recipient (ie. the merchant). If the 
        /// payment is set to a non-primary email address on your PayPal account, the <see cref="ReceiverEmail"/>
        /// will still be your primary email. Corresponds to the PayPal 'receiver_email' variable.
        /// </summary>
        public string ReceiverEmail
        {
            get { return _ipnProperties["receiver_email"]; }
            set { _ipnProperties["receiver_email"] = value; }
        }

        /// <summary>
        /// Gets or sets the unique account ID of the payment recipient (ie. the merchant). This 
        /// is the same as the recipient's referral ID. Corresponds to the PayPal 'receiver_id' 
        /// variable.
        /// </summary>
        public string ReceiverID
        {
            get { return _ipnProperties["receiver_id"]; }
            set { _ipnProperties["receiver_id"] = value; }
        }

        /// <summary>
        /// Gets a <see cref="PayPal.Web.Controls.ShoppingItemCollection"/> of <see cref="CartItem"/> 
        /// that were purchased. The instance of the items in the collection could be of either 
        /// <see cref="PayPal.Web.Controls.CartItem"/> or SubscriptionItem depending on what type 
        /// of items were purchased by the customer.
        /// </summary>
        /// <remarks>
        /// <p>
        /// The <see cref="PayPal.Web.Controls.ShoppingItem.ItemName"/> is what was passed 
        /// by you, the merchant. Or, if not passed by you, as entered by your customer. 
        /// Corresponds to the PayPal 'item_nameX' variable.
        /// </p>
        /// <p>
        /// The <see cref="PayPal.Web.Controls.ShoppingItem.ItemNumber"/> is what was passed
        /// by you, the merchant. Corresponds to the PayPal 'item_numberX' variable.
        /// </p>
        /// <p>
        /// The <see cref="PayPal.Web.Controls.CartItem.Quantity"/> as entered by your customer
        /// or as passed by you, the merchant. Corresponds to the PayPal 'quantityX' variable.
        /// </p>
        /// <p>
        /// The <see cref="System.Collections.CollectionBase.Count"/> of the total number of items in 
        /// the shopping cart if this was a shopping cart transaction. Corresponds to the PayPal
        /// 'num_cart_items' variable.
        /// </p>
        /// </remarks>
        public ShoppingItemCollection Items
        {
            get
            {
                // TODO: Determine what todo with ShoppingItems collection
                Debug.Assert(_shoppingItems != null,
                             "The Items collection is NULL! Should have been created in the constructor!");
                return _shoppingItems;
            }
        }

        #endregion

        #region Advanced and Custom Information

        /// <summary>
        /// Gets or sets the invoice number as passed by you, the merchant. See 
        /// <see cref="PayPal.Web.Controls.PayPalButton.Invoice"/>. Your customer 
        /// is not able to view or edit this. It must be unique per transaction. 
        /// Corresponds to the PayPal 'invoice' variable.
        /// </summary>
        public string Invoice
        {
            get { return _ipnProperties["invoice"]; }
            set { _ipnProperties["invoice"] = value; }
        }

        /// <summary>
        /// Gets or sets the custom value as passed by you, the merchant. See 
        /// <see cref="PayPal.Web.Controls.PayPalButton.Custom"/>. These values are
        /// pass-through variables that are never presented to your customer. Corresponds
        /// to the PayPal 'custom' variable.
        /// </summary>
        public string Custom
        {
            get { return _ipnProperties["custom"]; }
            set { _ipnProperties["custom"] = value; }
        }

        /// <summary>
        /// Gets or sets the memo as entered by your customer in PayPal Website Payments 
        /// note field. Corresponds to the PayPal 'memo' variable.
        /// </summary>
        public string Memo
        {
            get { return _ipnProperties["memo"]; }
            set { _ipnProperties["memo"] = value; }
        }

        /// <summary>
        /// Gets or sets the amount of tax charged on the payment. See 
        /// <see cref="PayPal.Web.Controls.PayPalButton.Tax"/>. Corresponds to the PayPal
        /// 'tax' variable.
        /// </summary>
        public float Tax
        {
            get { return Convert.ToSingle(_ipnProperties["tax"], CultureInfo.InvariantCulture); }
            set { _ipnProperties["tax"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the option 1 name as requested by you. See <see cref="PayPal.Web.Controls.ShoppingItem.Option1FieldName"/>.
        /// Corresponds to the PayPal 'option_name1' variable.
        /// </summary>
        public string Option1FieldName
        {
            get { return _ipnProperties["option_name1"]; }
            set { _ipnProperties["option_name1"] = value; }
        }

        /// <summary>
        /// Gets or sets the option 1 choice as entered by your customer. See <see cref="PayPal.Web.Controls.ShoppingItem.Option1Values"/>.
        /// Corresponds to the PayPal 'option_selection1' variable.
        /// </summary>
        public string Option1Selection
        {
            get { return _ipnProperties["option_selection1"]; }
            set { _ipnProperties["option_selection1"] = value; }
        }

        /// <summary>
        /// Gets or sets the option 2 name as requested by you. See <see cref="PayPal.Web.Controls.ShoppingItem.Option2FieldName"/>.
        /// Corresponds to the PayPal 'option_name2' variable.
        /// </summary>
        public string Option2FieldName
        {
            get { return _ipnProperties["option_name2"]; }
            set { _ipnProperties["option_name2"] = value; }
        }

        /// <summary>
        /// Gets or sets the option 2 choice as entered by your customer. See <see cref="PayPal.Web.Controls.ShoppingItem.Option2Values"/>.
        /// Corresponds to the PayPal 'option_selection2' variable.
        /// </summary>
        public string Option2Selection
        {
            get { return _ipnProperties["option_selection2"]; }
            set { _ipnProperties["option_selection2"] = value; }
        }

        #endregion

        #region Transaction Information

        /// <summary>
        /// Gets or sets the <see cref="PaymentStatus"/> of this IPN.
        /// Corresponds to the PayPal 'payment_status' variable.
        /// </summary>
        public PaymentStatus PaymentStatus
        {
            get { return (PaymentStatus) Enum.Parse(typeof (PaymentStatus), _ipnProperties["payment_status"], true); }
            set { _ipnProperties["payment_status"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the <see cref="PendingReason"/> of this IPN.
        /// Corresponds to the PayPal 'pending_reason' variable.
        /// </summary>
        public PendingReason PendingReason
        {
            get { return (PendingReason) Enum.Parse(typeof (PendingReason), _ipnProperties["pending_reason"], true); }
            set { _ipnProperties["pending_reason"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the <see cref="ReasonCode"/> of this IPN.
        /// Corresponds to the PayPal 'reason_code' variable.
        /// </summary>
        public ReasonCode ReasonCode
        {
            get { return (ReasonCode) Enum.Parse(typeof (ReasonCode), _ipnProperties["reason_code"], true); }
            set { _ipnProperties["reason_code"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the date/time stamp generator be PayPal system. 
        /// Corresponds to the PayPal 'payment_date' variable.
        /// </summary>
        public DateTime PaymentDate
        {
            get { return DateTime.Parse(_ipnProperties["payment_date"].Substring(0, 21)); }
            set { _ipnProperties["payment_date"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the unique transaction ID generated by the PayPal system.
        /// Corresponds to the PayPal 'txn_id' variable.
        /// </summary>
        public string TransactionID
        {
            get { return _ipnProperties["txn_id"]; }
            set { _ipnProperties["txn_id"] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="TransactionType"/> of this IPN.
        /// Corresponds to the PayPal 'txn_type' variable.
        /// </summary>
        public TransactionType TransactionType
        {
            get { return (TransactionType) Enum.Parse(typeof (TransactionType), _ipnProperties["txn_type"], true); }
            set { _ipnProperties["txn_type"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the <see cref="PaymentType"/> of this IPN. Corresponds 
        /// to the PayPal 'payment_type' variable.
        /// </summary>
        public PaymentType PaymentType
        {
            get { return (PaymentType) Enum.Parse(typeof (PaymentType), _ipnProperties["payment_type"], true); }
            set { _ipnProperties["payment_type"] = value.ToString(); }
        }

        #endregion

        #region Currency and Exchange Information

        /// <summary>
        /// Gets or sets the full amount of the customer's payment, before transaction fee
        /// is subtracted. Corresponds to the PayPal 'mc_gross' variable.
        /// </summary>
        public float McGross
        {
            get { return Convert.ToSingle(_ipnProperties["mc_gross"], CultureInfo.InvariantCulture); }
            set { _ipnProperties["mc_gross"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the transaction fee associated with the payment. <see cref="McGross"/>
        /// minus <see cref="McFee"/> will equal the amount deposited into the <see cref="ReceiverEmail"/>
        /// account. Corresponds to the PayPal 'mc_fee' variable.
        /// </summary>
        public float McFee
        {
            get { return Convert.ToSingle(_ipnProperties["mc_fee"], CultureInfo.InvariantCulture); }
            set { _ipnProperties["mc_fee"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the currency of the payment. Corresponds to the PayPal 'mc_currency'
        /// variable.
        /// <seealso cref="PayPal.Web.Controls.CurrencyCode"/>
        /// </summary>
        public CurrencyCode McCurrency
        {
            get { return (CurrencyCode) Enum.Parse(typeof (CurrencyCode), _ipnProperties["mc_currency"], true); }
            set { _ipnProperties["mc_currency"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the amount that is deposited into the account's primary balance after a currency
        /// conversion from automatic conversion (through your Payment Receiving Preferences)
        /// or manual conversion (through manually accepting a payment). Corresponds to the PayPal
        /// 'settle_amount' variable.
        /// </summary>
        public float SettleAmount
        {
            get { return Convert.ToSingle(_ipnProperties["settle_amount"], CultureInfo.InvariantCulture); }
            set { _ipnProperties["settle_amount"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the currency of the <see cref="SettleAmount"/>. Corresponds to the 
        /// PayPal 'settle_currency' variable.
        /// </summary>
        public CurrencyCode SettleCurrency
        {
            get { return (CurrencyCode) Enum.Parse(typeof (CurrencyCode), _ipnProperties["settle_currency"], true); }
            set { _ipnProperties["settle_currency"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the exchange rate used if a currency conversion occurred. Corresponds
        /// to the PayPal 'exchange_rate' variable.
        /// </summary>
        public float ExchangeRate
        {
            get { return Convert.ToSingle(_ipnProperties["exchange_rate"], CultureInfo.InvariantCulture); }
            set { _ipnProperties["exchange_rate"] = value.ToString(); }
        }

        #endregion

        #region Auction Information

        /// <summary>
        /// This is an auction payment (payments made using Pay for eBay items or Smart Logos, as well as
        /// Send Money/Money Request payments with the type 'eBay items' or 'Auction Goods (non-eBay)'.
        /// Corresponds to the PayPal 'for_auction' variable.
        /// </summary>
        public bool ForAuction
        {
            get { return Convert.ToBoolean(_ipnProperties["for_auction"]); }
            set { _ipnProperties["for_auction"] = value.ToString(); }
        }

        /// <summary>
        /// This is for an auction payment - it is the customer's auction ID. Corresponds to the
        /// PayPal 'auction_buyer_id' variable.
        /// </summary>
        public string AuctionBuyerID
        {
            get { return _ipnProperties["auction_buyer_id"]; }
            set { _ipnProperties["auction_buyer_id"] = value; }
        }

        /// <summary>
        /// This is for an auction payment - it is the auction's closing date. Corresponds to the
        /// PayPal 'auction_closing_date' variable.
        /// </summary>
        public DateTime AuctionClosingTime
        {
            get { return DateTime.Parse(_ipnProperties["auction_closing_date"]); }
            set { _ipnProperties["auction_closing_date"] = value.ToString(); }
        }

        /// <summary>
        /// This is a counter used for multi-item auction payments. It allows you to customize your script to
        /// only count the <see cref="McGross"/> for the first IPN you receive from a multi-item auction (
        /// <see cref="AuctionMultiItem"/> - 1), since each item from the auction will generate an IPN showing 
        /// the amount for the entire auction. Corresponds to the PayPal 'auction_multi_item' variable.
        /// </summary>
        public int AuctionMultiItem
        {
            get { return Convert.ToInt32(_ipnProperties["auction_multi_item"]); }
            set { _ipnProperties["auction_multi_item"] = value.ToString(); }
        }

        #endregion

        #region Buyer Information 

        /// <summary>
        /// Gets or sets the buyer's information.
        /// <seealso cref="IpnCustomerInfo"/>
        /// <seealso cref="PayPal.Web.Controls.CustomerInfo"/>
        /// </summary>
        public IpnCustomerInfo CustomerInfo
        {
            // TODO: Determine what todo with CustomerInfo
            get { return _ipnCustomerInfo; }
            set { _ipnCustomerInfo = value; }
        }

        #endregion

        #region IPN Information

        /// <summary>
        /// Gets or sets the version of IPN you are using. Corresponds to the PayPal 'notify_version'
        /// variable.
        /// </summary>
        public string IPNVersion
        {
            get { return _ipnProperties["notify_version"]; }
            set { _ipnProperties["notify_version"] = value; }
        }

        #endregion

        #region Security Information

        /// <summary>
        /// Gets or sets the encrypted string used to validate the authenticity of the 
        /// transaction. See the PayPal IPN documentation for more details. Corresponds
        /// to the PayPal 'verify_sign' variable.
        /// </summary>
        public string VerifySign
        {
            get { return _ipnProperties["verify_sign"]; }
            set { _ipnProperties["verify_sign"] = value; }
        }

        #endregion

        #region Subscription Information

        /// <summary>
        /// Gets the start date or the cancellation date depending on whether <see cref="IpnTransaction.TransactionType"/> 
        /// is <see cref="TransactionType.Subscr_Signup"/> or <see cref="TransactionType.Subscr_Cancel"/>.
        /// Corresponds to the PayPal 'subscr_date' variable.
        /// </summary>
        public DateTime SubscrDate
        {
            get { return DateTime.Parse(_ipnProperties["subscr_date"].Substring(0, 21)); }
            set { _ipnProperties["subscr_date"] = value.ToString(); }
        }

        /// <summary>
        /// Gets the date when the subscription modifiecation will be effective (this is only for
        /// <see cref="IpnTransaction.TransactionType"/> is <see cref="TransactionType.Subscr_Modify"/>. 
        /// Corresponds to the PayPal 'subscr_effective' variable.
        /// </summary>
        public DateTime SubscrEffectiveDate
        {
            get { return DateTime.Parse(_ipnProperties["subscr_effective"].Substring(0, 21)); }
            set { _ipnProperties["subscr_effective"] = value.ToString(); }
        }

        /// <summary>
        /// Gets a collection of the subscription items. See <see cref="PayPal.Web.Controls.SubscriptionItem"/>
        /// for more information.
        /// </summary>
        public SubscriptionItem[] SubscriptionItems
        {
            get
            {
                Debug.Assert(_subscriptionItems != null, "SubscriptionItems was null.");
                Debug.Assert(_subscriptionItems.Length == 3, "SubscriptionItems was not length 3.");

                return _subscriptionItems;
            }
        }

        public float MC_AMOUNT3
        {
            get
            {
                return Single.Parse(_ipnProperties["mc_amount3"], CultureInfo.InvariantCulture);
            }
        }
        
        /// <summary>
        /// Indicates whether the regular rate recurs. TRUE will recur, FALSE will not recur.
        /// Corresponds to the PayPal 'recurring' variable.
        /// </summary>
        public bool IsRecurring
        {
            get { return Convert.ToBoolean(_ipnProperties["recurring"]); }
            set { _ipnProperties["recurring"] = value.ToString(); }
        }

        /// <summary>
        /// Indicates whether reattempts should occur upon payment failures. TRUE will reattempt,
        /// FALSE will not reattempt. Corresponds to the PayPal 'reattempt' variable.
        /// </summary>
        public bool ReattemptFailedPayments
        {
            get { return Convert.ToBoolean(_ipnProperties["reattempt"]); }
            set { _ipnProperties["reattempt"] = value.ToString(); }
        }

        /// <summary>
        /// Gets the date that failed subscription payments will retry. Corresponds to the PayPal
        /// 'retry_at' variable.
        /// </summary>
        public DateTime RetryFailedPaymentsDate
        {
            get { return DateTime.Parse(_ipnProperties["retry_at"]); }
            set { _ipnProperties["retry_at"] = value.ToString(); }
        }

        /// <summary>
        /// Gets the number of payment installments that will occur at the regular rate. Corresponds
        /// to the PayPal 'recur_times' variable.
        /// </summary>
        public int RecurTimes
        {
            get { return Convert.ToInt32(_ipnProperties["recur_times"]); }
            set { _ipnProperties["recur_times"] = value.ToString(); }
        }

        /// <summary>
        /// Gets the username generated by PayPal given to the subscriber to access the subscription.
        /// Corresponds to the PayPal 'username' variable.
        /// </summary>
        public string Username
        {
            get { return _ipnProperties["username"]; }
            set { _ipnProperties["username"] = value; }
        }

        /// <summary>
        /// Gets the password generated by PayPal give to the subscriber to access the subscription.
        /// The password will be hashed. Corresponds to the PayPal 'password' variable.
        /// </summary>
        public string Password
        {
            get { return _ipnProperties["password"]; }
            set { _ipnProperties["password"] = value; }
        }

        /// <summary>
        /// Gets the ID generated by PayPal for the subscriber. Corresponds to the PayPal 'subscr_id'
        /// variable.
        /// </summary>
        public string SubscriberID
        {
            get { return _ipnProperties["subscr_id"]; }
            set { _ipnProperties["subscr_id"] = value; }
        }

        #endregion

        /// <summary>
        /// Gets or sets the raw IPN data for this <see cref="IpnTransaction"/>. This corresponds to 
        /// the actual IPN message that was received from PayPal.
        /// </summary>
        public string RawIPN
        {
            get { return _ipnProperties["RawIPN"]; }
            set { _ipnProperties["RawIPN"] = value; }
        }

        #endregion

        /// <summary>
        /// Used to generically hold all IPN properties. This can be set directly by using 
        /// <see cref="SetProperty"/> and <see cref="GetProperty"/>, but these methods should
        /// only be used if no corresponding property exists.
        /// </summary>
        private NameValueCollection _ipnProperties;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public IpnTransaction()
        {
            _shoppingItems = new ShoppingItemCollection();
            _ipnCustomerInfo = new IpnCustomerInfo();
            _subscriptionItems =
                new SubscriptionItem[] {new SubscriptionItem(), new SubscriptionItem(), new SubscriptionItem()};

            // initial the container for the IPN properties
            _ipnProperties = new NameValueCollection();
        }

        #region Get/Set Property Methods

        /// <summary>
        /// Sets a custom IPN property.
        /// </summary>
        /// <remarks>
        /// WARNING!! Using this method to access the IPN properties is not type safe. 
        /// Instead use the corresponding property of <see cref="IpnTransaction"/> to 
        /// set the desired IPN property. Overwriting the values of an existing
        /// property may result in data corruption and exceptions being thrown. Only
        /// use this method when no corresponding property exists.
        /// </remarks>
        /// <param name="name">The name of the property.</param>
        /// <param name="textValue">The value of the property.</param>
        public void SetProperty(string name, string textValue)
        {
            _ipnProperties.Add(name, textValue);
        }

        /// <summary>
        /// Retrieves a custom IPN property.
        /// </summary>
        /// <remarks>
        /// WARNING!! Using this method to set the IPN properties is not type safe. 
        /// Instead use the corresponding property of <see cref="IpnTransaction"/> to 
        /// get the desired IPN property. Overwritten values of an existing property
        /// may result in data corruption and exceptions being thrown. Only use this
        /// method when no corresponding property exists.
        /// </remarks>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value corresponding to the property.</returns>
        public string GetProperty(string name)
        {
            return _ipnProperties[name];
        }

        #endregion
    }

    /// <summary>
    /// Represents a customer in an IPN message. This should be accessed through 
    /// <see cref="IpnTransaction.CustomerInfo"/>.
    /// </summary>
    /// <remarks>
    /// <seealso cref="PayPal.Web.Controls.CustomerInfo"/>
    /// </remarks>
    public class IpnCustomerInfo : CustomerInfo
    {
        #region IPN Customer Data Properties

        /// <summary>
        /// Gets or sets the Customer's company name. Corresponds to the PayPal
        /// 'payer_business_name' variable.
        /// </summary>
        public string BusinessName
        {
            get { return _ipnCustomerProperties["payer_business_name"]; }
            set { _ipnCustomerProperties["payer_business_name"] = value; }
        }

        /// <summary>
        /// Gets or sets the name used with the address (included when the customer
        /// provides a Gift Address). Corresponds to the PayPal 'address_name' variable.
        /// </summary>
        public string AddressName
        {
            get { return _ipnCustomerProperties["address_name"]; }
            set { _ipnCustomerProperties["address_name"] = value; }
        }

        /// <summary>
        /// Gets or sets the country of the customer's address. Corresponds to the
        /// PayPal 'address_country' variable.
        /// </summary>
        public string Country
        {
            get { return _ipnCustomerProperties["address_country"]; }
            set { _ipnCustomerProperties["address_country"] = value; }
        }

        /// <summary>
        /// Gets or sets the address status of the customer. Corresponds to
        /// the PayPal 'address_status' variable.
        /// </summary>
        public AddressStatus AddressStatus
        {
            get { return (AddressStatus) Enum.Parse(typeof (AddressStatus), _ipnCustomerProperties["address_status"], true); }
            set { _ipnCustomerProperties["address_status"] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the customer's primary email address. Use this email 
        /// to provide any credits. Corresponds to the PayPal 'payer_email' 
        /// variable.
        /// </summary>
        public string Email
        {
            get { return _ipnCustomerProperties["payer_email"]; }
            set { _ipnCustomerProperties["payer_email"] = value; }
        }

        /// <summary>
        /// Gets or sets the status of the customer. Corresponds to the PayPal
        /// 'payer_status' variable.
        /// </summary>
        public PayerStatus PayerStatus
        {
            get { return (PayerStatus) Enum.Parse(typeof (PayerStatus), _ipnCustomerProperties["payer_status"], true); }
            set { _ipnCustomerProperties["payer_status"] = value.ToString(); }
        }

        /// <summary>
        /// Gets the unique customer ID. Corresponds to the PayPal 'payer_id' 
        /// variable.
        /// </summary>
        public string PayerID
        {
            get { return _ipnCustomerProperties["payer_id"]; }
            set { _ipnCustomerProperties["payer_id"] = value; }
        }

        #endregion

        /// <summary>
        /// Used to hold the IPN customer properties.
        /// </summary>
        private NameValueCollection _ipnCustomerProperties;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public IpnCustomerInfo()
        {
            _ipnCustomerProperties = new NameValueCollection();
        }

        #region Get/Set Property Methods

        /// <summary>
        /// Sets a custom property of the customer.
        /// </summary>
        /// <remarks>
        /// WARNING!! Using this method to access the IPN properties is not type safe. 
        /// Instead use the corresponding property of <see cref="IpnCustomerInfo"/> to 
        /// set the desired IPN property. Overwriting the values of an existing
        /// property may result in data corruption and exceptions being thrown. Only
        /// use this method when no corresponding property exists.
        /// </remarks>
        /// <param name="name">The name of the property.</param>
        /// <param name="textValue">The value of the property.</param>
        public void SetProperty(string name, string textValue)
        {
            _ipnCustomerProperties.Add(name, textValue);
        }

        /// <summary>
        /// Retrieves a custom IPN property.
        /// </summary>
        /// <remarks>
        /// WARNING!! Using this method to set the IPN properties is not type safe. 
        /// Instead use the corresponding property of <see cref="IpnCustomerInfo"/> to 
        /// get the desired IPN property. Overwritten values of an existing property
        /// may result in data corruption and exceptions being thrown. Only use this
        /// method when no corresponding property exists.
        /// </remarks>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value corresponding to the property.</returns>
        public string GetProperty(string name)
        {
            return _ipnCustomerProperties[name];
        }

        #endregion
    }
}