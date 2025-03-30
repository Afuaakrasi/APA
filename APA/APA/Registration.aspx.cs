using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APA.APA
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (VerifyForm())
            {
                // Process form data if validation passes
                string prefix = DRPPrefix.SelectedValue;
                string firstName = TXTFirstName.Text.Trim();
                string mi = TXTMI.Text.Trim();
                string lastName = TXTLastName.Text.Trim();
                string mailingAddress = TXTMailingAddress.Text.Trim();
                string city = TXTCity.Text.Trim();
                string stateProvince = TXTStateProvince.Text.Trim();
                string zipPostalCode = TXTZipPostalCode.Text.Trim();
                string country = TXTCountry.Text.Trim();
                string daytimePhone = TXTDaytimePhone.Text.Trim();
                string cellPhone = TXTCellPhone.Text.Trim();
                string email = TXTEmailAddress.Text.Trim();
                string institution = TXTInstitution.Text.Trim();
                string cityIfDifferent = TXTCityIfDifferent.Text.Trim();
                string stateProvinceIfDifferent = TXTStateProvince1.Text.Trim();
                string countryIfDifferent = TXTCountry1.Text.Trim();
                string institutionalCode = TXTInstitutionalCode.Text.Trim();
                bool firstAPAYes = OptYes.Checked;
                bool firstAPANo = OptNo.Checked;
                bool programsRequested = ChkRequest.Checked;
                string disability = TXTPersonWithDisability.Text.Trim();
                bool earlyCareerPsychologistYes = ChkYes.Checked;
                bool earlyCareerPsychologistNo = ChkNo.Checked;
                bool confirmCorrect = Chktrue.Checked;
                bool visa = ChkVisa.Checked;
                bool masterCard = ChkMasterCard.Checked;
                bool americanExpress = ChkAmericanExpress.Checked;
                string creditCardNumber = txtCredit.Text.Trim();
                string expirationDate = txtExpiration.Text.Trim();

                // Redirect upon successful submission
                Response.Redirect("ThankYou.aspx");
            }
            else
            {
                ValidationSummary1.Visible = true; // Show validation errors
            }
        }


        private bool VerifyForm()
        {
            bool isValid = true;

            // Clear any existing custom validators
            var customValidators = Page.Validators.OfType<CustomValidator>().ToList();
            foreach (var validator in customValidators)
            {
                Page.Validators.Remove(validator);
            }

            // APA Member Number validation
            if (string.IsNullOrWhiteSpace(TXTAPAMemberNumber.Text))
            {
                AddCustomValidator("APA Member Number is required.", "TXTAPAMemberNumber");
                isValid = false;
            }

            // First Name validation
            if (string.IsNullOrWhiteSpace(TXTFirstName.Text))
            {
                AddCustomValidator("First Name is required.", "TXTFirstName");
                isValid = false;
            }

            // Last Name validation
            if (string.IsNullOrWhiteSpace(TXTLastName.Text))
            {
                AddCustomValidator("Last Name is required.", "TXTLastName");
                isValid = false;
            }

            // Email Address validation
            if (string.IsNullOrWhiteSpace(TXTEmailAddress.Text))
            {
                AddCustomValidator("Email Address is required.", "TXTEmailAddress");
                isValid = false;
            }
            else if (!IsValidEmail(TXTEmailAddress.Text))
            {
                AddCustomValidator("Please enter a valid email address.", "TXTEmailAddress");
                isValid = false;
            }

            // Daytime Phone validation
            if (string.IsNullOrWhiteSpace(TXTDaytimePhone.Text))
            {
                AddCustomValidator("Daytime Phone is required.", "TXTDaytimePhone");
                isValid = false;
            }

            // Payment method validation
            if (ChkVisa.Checked == false && ChkMasterCard.Checked == false && ChkAmericanExpress.Checked == false)
            {
                AddCustomValidator("At least one payment method must be selected.", "ChkVisa");
                isValid = false;
            }

            // Credit Card Number validation
            if (string.IsNullOrWhiteSpace(txtCredit.Text))
            {
                AddCustomValidator("Credit Card Number is required.", "txtCredit");
                isValid = false;
            }
            else if (!IsValidCreditCardNumber(txtCredit.Text))
            {
                AddCustomValidator("Please enter a valid credit card number.", "txtCredit");
                isValid = false;
            }

            // Expiration Date validation
            if (string.IsNullOrWhiteSpace(txtExpiration.Text))
            {
                AddCustomValidator("Expiration Date is required.", "txtExpiration");
                isValid = false;
            }
            else if (!IsValidExpirationDate(txtExpiration.Text))
            {
                AddCustomValidator("Please enter a valid expiration date (MM/YY format).", "txtExpiration");
                isValid = false;
            }

            // Confirmation checkbox validation
            if (!Chktrue.Checked)
            {
                AddCustomValidator("You must confirm that all information provided is correct.", "Chktrue");
                isValid = false;
            }

            return isValid;
        }

        // Helper method to add custom validators
        private void AddCustomValidator(string errorMessage, string controlToValidate)
        {
            CustomValidator validator = new CustomValidator();
            validator.ErrorMessage = errorMessage;
            validator.IsValid = false;
            validator.ValidationGroup = "FormValidation";
            validator.Display = ValidatorDisplay.None; // Don't show error next to control, use the ValidationSummary

            if (!string.IsNullOrEmpty(controlToValidate))
            {
                validator.ControlToValidate = controlToValidate;
            }

            Page.Validators.Add(validator);
        }

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Helper method to validate credit card number
        private bool IsValidCreditCardNumber(string cardNumber)
        {
            // Remove any non-digit characters
            cardNumber = new string(cardNumber.Where(char.IsDigit).ToArray());

            // Check if empty
            if (string.IsNullOrEmpty(cardNumber))
                return false;

            // Check length (most card numbers are 13-19 digits)
            if (cardNumber.Length < 13 || cardNumber.Length > 19)
                return false;

            // Implement Luhn algorithm for basic validation
            int sum = 0;
            bool alternate = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(cardNumber[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }

            return (sum % 10 == 0);
        }

        // Helper method to validate expiration date
        private bool IsValidExpirationDate(string expirationDate)
        {
            // Check format (MM/YY)
            if (!Regex.IsMatch(expirationDate, @"^\d{2}/\d{2}$"))
                return false;

            string[] parts = expirationDate.Split('/');
            int month = int.Parse(parts[0]);
            int year = int.Parse(parts[1]) + 2000; // Assuming 20xx

            // Check month range
            if (month < 1 || month > 12)
                return false;

            // Check if not expired
            DateTime expiry = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            return expiry > DateTime.Now;
        }
    }
}



