using Digident_Group3;
using Digident_Group3.Interfaces;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Windows.Controls;
using System.Windows;
using Digident_Group3.Services;
using System.Configuration;
using System.Windows.Controls.Primitives;

namespace DigidentTest_Group3
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class PatientLoginPageTests
    {
        private PatientLoginPage _page;
        private Mock<IDatabaseService> _mockDatabaseService;
        private Mock<IMessageBoxService> _mockMessageBoxService;

        [SetUp]
        public void SetUp()
        {
            // Initialize mocks
            _mockDatabaseService = new Mock<IDatabaseService>();
            _mockMessageBoxService = new Mock<IMessageBoxService>();

            // Create the page instance with the mocks
            _page = new PatientLoginPage(_mockDatabaseService.Object, _mockMessageBoxService.Object);

            // Initialize UI elements
            _page.UsernameTextBox = new TextBox();
            _page.PasswordBox = new PasswordBox();
        }


        [Test]
        public void Loginbutton_ShouldShowErrorMessage_WhenUsernameIsEmpty()
        {
            // Arrange
            _page.UsernameTextBox.Text = ""; // Empty username
            _page.PasswordBox.Password = "password";

            // Act
            _page.Loginbutton(null, null);

            // Assert
            _mockMessageBoxService.Verify(mb => mb.Show("Please enter both username and password.", "Error", MessageBoxButton.OK), Times.Once);
        }

        [Test]
        public void Loginbutton_ShouldShowErrorMessage_WhenPasswordIsEmpty()
        {
            // Arrange
            _page.UsernameTextBox.Text = "validUser";
            _page.PasswordBox.Password = ""; // Empty password

            // Act
            _page.Loginbutton(null, null);

            // Assert
            _mockMessageBoxService.Verify(mb => mb.Show("Please enter both username and password.", "Error", MessageBoxButton.OK), Times.Once);
        }


    }



    [TestFixture, Apartment(ApartmentState.STA)]
    public class RegisterTests
    {
        private Mock<IDatabaseService> _mockDatabaseService;
        private Mock<IMessageBoxService> _mockMessageBoxService;
        private Register _registerPage;

        [SetUp]
        public void SetUp()
        {
            // Ensure configuration is loaded
            // Log the connection string to verify it's being read correctly
          


            _mockDatabaseService = new Mock<IDatabaseService>();
            _mockMessageBoxService = new Mock<IMessageBoxService>();

            _registerPage = new Register(_mockDatabaseService.Object, _mockMessageBoxService.Object);

            // Initialize UI components
            _registerPage.EmailTextBox = new TextBox();
            _registerPage.PasswordBox = new PasswordBox();
            _registerPage.ConfirmPasswordBox = new PasswordBox();
            _registerPage.FirstNameTextBox = new TextBox();
            _registerPage.LastNameTextBox = new TextBox();
            _registerPage.DateOfBirthPicker = new DatePicker();
            _registerPage.AddressTextBox = new TextBox();
            _registerPage.PhoneNumberTextBox = new TextBox();
        }

       

        [Test]
        public void ValidateEmail_ShouldShowError_WhenEmailIsInvalid()
        {
            // Arrange
            _registerPage.EmailTextBox.Text = "invalid-email";

            // Act
            _registerPage.ValidateEmail();

            // Assert
            Assert.AreEqual("Please enter a valid email address.", _registerPage.EmailErrorText.Text);
        }
        [Test]
        public void ValidatePassword_ShouldShowError_WhenPasswordIsInvalid()
        {
            // Arrange
            _registerPage.PasswordBox.Password = "short";

            // Act
            _registerPage.ValidatePassword();

            // Assert
            Assert.AreEqual("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.", _registerPage.PasswordErrorText.Text);
        }
        [Test]
        public void ValidateConfirmPassword_ShouldShowError_WhenPasswordsDoNotMatch()
        {
            // Arrange
            _registerPage.PasswordBox.Password = "ValidP@ssw0rd";
            _registerPage.ConfirmPasswordBox.Password = "DifferentP@ssw0rd";

            // Act
            _registerPage.ValidateConfirmPassword();

            // Assert
            Assert.AreEqual("Passwords do not match.", _registerPage.ConfirmPasswordErrorText.Text);
        }

        [Test]
        public void ValidateFirstName_ShouldShowError_WhenFirstNameIsEmpty()
        {
            // Arrange
            _registerPage.FirstNameTextBox.Text = "";

            // Act
            _registerPage.ValidateFirstName();

            // Assert
            Assert.AreEqual("First Name is required.", _registerPage.FirstNameErrorText.Text);
        }

        [Test]
        public void ValidateLastName_ShouldShowError_WhenLastNameIsInvalid()
        {
            // Arrange
            _registerPage.LastNameTextBox.Text = "123Invalid";

            // Act
            _registerPage.ValidateLastName();

            // Assert
            Assert.AreEqual("Last Name can only contain letters, apostrophes, and hyphens.", _registerPage.LastNameErrorText.Text);
        }

        [Test]
        public void ValidateDateOfBirth_ShouldShowError_WhenDateOfBirthIsInTheFuture()
        {
            // Arrange
            _registerPage.DateOfBirthPicker.SelectedDate = DateTime.Now.AddYears(1);

            // Act
            _registerPage.ValidateDateOfBirth();

            // Assert
            Assert.AreEqual("Please enter a valid date of birth.", _registerPage.DateOfBirthErrorText.Text);
        }

        [Test]
        public void ValidateAddress_ShouldShowError_WhenAddressIsEmpty()
        {
            // Arrange
            _registerPage.AddressTextBox.Text = "";

            // Act
            _registerPage.ValidateAddress();

            // Assert
            Assert.AreEqual("Address is required.", _registerPage.AddressErrorText.Text);
        }

        [Test]
        public void ValidatePhoneNumber_ShouldShowError_WhenPhoneNumberIsInvalid()
        {
            // Arrange
            _registerPage.PhoneNumberTextBox.Text = "invalidphone";

            // Act
            _registerPage.ValidatePhoneNumber();

            // Assert
            Assert.AreEqual("Phone Number can only contain numbers.", _registerPage.PhoneNumberErrorText.Text);
        }

        [Test]
        public void ValidatePhoneNumber_ShouldShowError_WhenPhoneNumberIsNotTenDigits()
        {
            // Arrange
            _registerPage.PhoneNumberTextBox.Text = "123";

            // Act
            _registerPage.ValidatePhoneNumber();

            // Assert
            Assert.AreEqual("Phone Number must be 10 digits long.", _registerPage.PhoneNumberErrorText.Text);
        }

        [Test]
        public void SubmitButton_ShouldShowErrorMessage_WhenAnyFieldIsInvalid()
        {
            // Arrange
            _registerPage.EmailTextBox.Text = "invalid-email";
            _registerPage.PasswordBox.Password = "short";
            _registerPage.ConfirmPasswordBox.Password = "short";
            _registerPage.FirstNameTextBox.Text = "";
            _registerPage.LastNameTextBox.Text = "123Invalid";
            _registerPage.DateOfBirthPicker.SelectedDate = DateTime.Now.AddYears(1);
            _registerPage.AddressTextBox.Text = "";
            _registerPage.PhoneNumberTextBox.Text = "123";

            // Act
            _registerPage.SubmitButton_Click(null, null);

            // Assert
            _mockMessageBoxService.Verify(mb => mb.Show("Please check the fields!", "Error", MessageBoxButton.OK), Times.Once);
        }
    }
}
