using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace APA.APA
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (VerifyForm() == true)
            {

                try
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["APAConnection"].ConnectionString;
                string sql = "INSERT INTO APARegistration (APAMemberNumber, Prefix, FirstName, MiddleInitial, LastName, MailingAddress, City, StateProvince, ZipPostalCode, Country,DaytimePhone, CellPhone, EmailAddress, Institution, InstitutionCity, InstitutionStateProvince, InstitutionCountry, InstitutionalCode, IsFirstAPAConvention, RequestedProgram, DisabilityInformation, IsEarlyCareerPsychologist, PaymentMethod,CardholderName, ConventionFee, CardholderAddress, CardholderPhone, CreditCardNumber, ExpirationDate, CardholderSignature, RegistrantNameIfDifferent, ConfirmedInformationCorrect,IPAddress, UserAgent) VALUES (@APAMemberNumber, @Prefix, @FirstName, @MiddleInitial, @LastName, @MailingAddress, @City, @StateProvince, @ZipPostalCode, @Country,@DaytimePhone, @CellPhone, @EmailAddress, @Institution,@InstitutionCity, @InstitutionStateProvince, @InstitutionCountry,@InstitutionalCode, @IsFirstAPAConvention, @RequestedProgram,@DisabilityInformation, @IsEarlyCareerPsychologist, @PaymentMethod,@CardholderName, @ConventionFee, @CardholderAddress, @CardholderPhone, @CreditCardNumber, @ExpirationDate, @CardholderSignature,@RegistrantNameIfDifferent, @ConfirmedInformationCorrect, @IPAddress, @UserAgent)";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@APAMemberNumber", TXTAPAMemberNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@Prefix", DRPPrefix.SelectedValue);
                    cmd.Parameters.AddWithValue("@FirstName", TXTFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@MiddleInitial", TXTMI.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastName", TXTLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@MailingAddress", TXTMailingAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@City", TXTCity.Text.Trim());
                    cmd.Parameters.AddWithValue("@StateProvince", TXTStateProvince.Text.Trim());
                    cmd.Parameters.AddWithValue("@ZipPostalCode", TXTZipPostalCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", TXTCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@DaytimePhone", TXTDaytimePhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@CellPhone", TXTCellPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@EmailAddress", TXTEmailAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Institution", TXTInstitution.Text.Trim());
                    cmd.Parameters.AddWithValue("@InstitutionCity", TXTCityIfDifferent.Text.Trim());
                    cmd.Parameters.AddWithValue("@InstitutionStateProvince", TXTStateProvince1.Text.Trim());
                    cmd.Parameters.AddWithValue("@InstitutionCountry", TXTCountry1.Text.Trim());
                    cmd.Parameters.AddWithValue("@InstitutionalCode", TXTInstitutionalCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@IsFirstAPAConvention", OptYes.Checked);
                    cmd.Parameters.AddWithValue("@RequestedProgram", ChkRequest.Checked);
                    cmd.Parameters.AddWithValue("@DisabilityInformation", TXTPersonWithDisability.Text.Trim());
                    cmd.Parameters.AddWithValue("@IsEarlyCareerPsychologist", ChkYes.Checked);
                    cmd.Parameters.AddWithValue("@CardholderAddress", txtaddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@CardholderPhone", txtDaytime.Text.Trim());
                    cmd.Parameters.AddWithValue("@CreditCardNumber", txtCredit.Text.Trim());
                    cmd.Parameters.AddWithValue("@ExpirationDate", txtExpiration.Text.Trim());
                    cmd.Parameters.AddWithValue("@CardholderSignature", txtCardholder.Text.Trim());
                    cmd.Parameters.AddWithValue("@RegistrantNameIfDifferent", txtRegistrant.Text.Trim());
                    cmd.Parameters.AddWithValue("@ConfirmedInformationCorrect", Chktrue.Checked);
                    cmd.Parameters.AddWithValue("@IPAddress", Request.UserHostAddress);
                    cmd.Parameters.AddWithValue("@UserAgent", Request.UserAgent);

                    string paymentMethod = "";
                    if (ChkVisa.Checked) paymentMethod = "Visa";
                    else if (ChkMasterCard.Checked) paymentMethod = "MasterCard";
                    else if (ChkAmericanExpress.Checked) paymentMethod = "AmericanExpress";
                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                    cmd.Parameters.AddWithValue("@CardholderName", txtname.Text.Trim());

                    decimal fee = 0;
                    if (Decimal.TryParse(txtfee.Text.Trim(), out fee))
                    {
                        cmd.Parameters.AddWithValue("@ConventionFee", fee);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ConventionFee", DBNull.Value);
                    }



                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                Response.Redirect("ThankYou.aspx");
                }
                catch (Exception ex)
                {
                    Response.Redirect("Error.aspx");
                }

            }
            
        }

        private bool VerifyForm()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(TXTAPAMemberNumber.Text))
                errors.Add("APA Member Number is required.");

            if (string.IsNullOrWhiteSpace(TXTFirstName.Text))
                errors.Add("First Name is required.");

            if (string.IsNullOrWhiteSpace(TXTLastName.Text))
                errors.Add("Last Name is required.");

            if (string.IsNullOrWhiteSpace(TXTEmailAddress.Text))
                errors.Add("Email Address is required.");
            else if (!IsValidEmail(TXTEmailAddress.Text))
                errors.Add("Please enter a valid email address.");

            if (string.IsNullOrWhiteSpace(TXTDaytimePhone.Text))
                errors.Add("Daytime Phone is required.");

            if (ChkVisa.Checked == false && ChkMasterCard.Checked == false && ChkAmericanExpress.Checked == false)
                errors.Add("At least one payment method must be selected.");

            if (string.IsNullOrWhiteSpace(txtCredit.Text))
                errors.Add("Credit Card Number is required.");
            else if (!IsValidCreditCardNumber(txtCredit.Text))
                errors.Add("Please enter a valid credit card number.");

            if (string.IsNullOrWhiteSpace(txtExpiration.Text))
                errors.Add("Expiration Date is required.");
            else if (!IsValidExpirationDate(txtExpiration.Text))
                errors.Add("Please enter a valid expiration date (MM/YY format).");

            if (!Chktrue.Checked)
                errors.Add("You must confirm that all information provided is correct.");

            if (errors.Count > 0)
            {
                ValidationSummary1.HeaderText = "Please correct the following errors:";
                ValidationSummary1.Controls.Clear();
                foreach (string error in errors)
                {
                    ValidationSummary1.Controls.Add(new LiteralControl("<li>" + error + "</li>"));
                }
                return false;
            }

            return true;
        }

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

        private bool IsValidCreditCardNumber(string cardNumber)
        {
            cardNumber = new string(cardNumber.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(cardNumber))
                return false;

            if (cardNumber.Length < 13 || cardNumber.Length > 19)
                return false;

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

        private bool IsValidExpirationDate(string expirationDate)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(expirationDate, @"^\d{2}/\d{2}$"))
                return false;

            string[] parts = expirationDate.Split('/');
            int month = int.Parse(parts[0]);
            int year = int.Parse(parts[1]) + 2000;

            if (month < 1 || month > 12)
                return false;

            DateTime expiry = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            return expiry > DateTime.Now;
        }
        // Add these methods to your Registration.aspx.cs file

protected void cvConfirmation_ServerValidate(object source, ServerValidateEventArgs args)
{
    args.IsValid = Chktrue.Checked;
}

protected void cvPaymentMethod_ServerValidate(object source, ServerValidateEventArgs args)
{
    args.IsValid = ChkVisa.Checked || ChkMasterCard.Checked || ChkAmericanExpress.Checked;
}

protected void cvCreditCard_ServerValidate(object source, ServerValidateEventArgs args)
{
    args.IsValid = IsValidCreditCardNumber(txtCredit.Text);
}
    }
}
