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
    public class PatientLoginPageIntegrationTests
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
        public void Loginbutton_ShouldShowMessage_WhenCredentialsAreValid()
        {
            // Arrange
            _mockDatabaseService.Setup(service => service.ValidateCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            _page.UsernameTextBox.Text = "validUser";
            _page.PasswordBox.Password = "validPassword";


            // Act
            _page.Loginbutton(null, null);

            // Assert
            _mockMessageBoxService.Verify(mb => mb.Show("Login successful.", "Success", MessageBoxButton.OK), Times.Once);
        }




        [Test]
        public void Loginbutton_ShouldShowErrorMessage_WhenCredentialsAreInvalid()
        {
            // Arrange
            string username = "wrong@example.com";
            string password = "wrongpassword";
            _mockDatabaseService.Setup(service => service.ValidateCredentials(username, password)).Returns(false);

            // Act
            _page.UsernameTextBox.Text = username;
            _page.PasswordBox.Password = password;
            _page.Loginbutton(null, null);

            // Assert
            // Check if the error message is shown
            _mockMessageBoxService.Verify(mb => mb.Show("Invalid credentials.", "Error", MessageBoxButton.OK), Times.Once);
        }
    }


    [TestFixture, Apartment(ApartmentState.STA)]
    public class RegisterIntegrationTests
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
        public void SubmitButton_ShouldRegisterUser_WhenValidInputIsProvided()
        {
            // Arrange
            _mockDatabaseService.Setup(ds => ds.RegisterUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _mockMessageBoxService.Setup(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>())).Returns(MessageBoxResult.OK);

            // Simulate valid input
            _registerPage.EmailTextBox.Text = "test@example.com";
            _registerPage.PasswordBox.Password = "ValidP@ssw0rd";
            _registerPage.ConfirmPasswordBox.Password = "ValidP@ssw0rd";
            _registerPage.FirstNameTextBox.Text = "John";
            _registerPage.LastNameTextBox.Text = "Doe";
            _registerPage.DateOfBirthPicker.SelectedDate = DateTime.Now.AddYears(-30);
            _registerPage.AddressTextBox.Text = "123 Main St";
            _registerPage.PhoneNumberTextBox.Text = "1234567890";

            // Act
            _registerPage.SubmitButton_Click(null, null);

            // Assert
            _mockDatabaseService.Verify(ds => ds.RegisterUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockMessageBoxService.Verify(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>()), Times.AtLeastOnce);
        }

        [Test]
        public void SubmitButton_ShouldShowErrorMessage_WhenEmailIsInvalid()
        {
            // Arrange
            _mockDatabaseService.Setup(ds => ds.RegisterUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockMessageBoxService.Setup(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>())).Returns(MessageBoxResult.OK);

            // Simulate invalid email input
            _registerPage.EmailTextBox.Text = "invalid-email";
            _registerPage.PasswordBox.Password = "ValidP@ssw0rd";
            _registerPage.ConfirmPasswordBox.Password = "ValidP@ssw0rd";
            _registerPage.FirstNameTextBox.Text = "John";
            _registerPage.LastNameTextBox.Text = "Doe";
            _registerPage.DateOfBirthPicker.SelectedDate = DateTime.Now.AddYears(-30);
            _registerPage.AddressTextBox.Text = "123 Main St";
            _registerPage.PhoneNumberTextBox.Text = "1234567890";

            // Act
            _registerPage.SubmitButton_Click(null, null);

            // Assert
            _mockMessageBoxService.Verify(mb => mb.Show("Please check the fields!", "Error", MessageBoxButton.OK), Times.Once);
        }




        [Test]
        public void SubmitButton_ShouldShowErrorMessage_WhenDateOfBirthIsInvalid()
        {
            // Arrange
            _mockDatabaseService.Setup(ds => ds.RegisterUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockMessageBoxService.Setup(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>())).Returns(MessageBoxResult.OK);

            // Simulate invalid date of birth
            _registerPage.EmailTextBox.Text = "test@example.com";
            _registerPage.PasswordBox.Password = "ValidP@ssw0rd";
            _registerPage.ConfirmPasswordBox.Password = "ValidP@ssw0rd";
            _registerPage.FirstNameTextBox.Text = "John";
            _registerPage.LastNameTextBox.Text = "Doe";
            _registerPage.DateOfBirthPicker.SelectedDate = DateTime.Now.AddYears(1); // Future date
            _registerPage.AddressTextBox.Text = "123 Main St";
            _registerPage.PhoneNumberTextBox.Text = "1234567890";

            // Act
            _registerPage.SubmitButton_Click(null, null);

            // Assert
            _mockMessageBoxService.Verify(mb => mb.Show("Please check the fields!", "Error", MessageBoxButton.OK), Times.Once);
        }

        [Test]
        public void SubmitButton_ShouldShowErrorMessage_WhenPhoneNumberIsInvalid()
        {
            // Arrange
            _mockDatabaseService.Setup(ds => ds.RegisterUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _mockMessageBoxService.Setup(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>())).Returns(MessageBoxResult.OK);

            // Simulate invalid phone number
            _registerPage.EmailTextBox.Text = "test@example.com";
            _registerPage.PasswordBox.Password = "ValidP@ssw0rd";
            _registerPage.ConfirmPasswordBox.Password = "ValidP@ssw0rd";
            _registerPage.FirstNameTextBox.Text = "John";
            _registerPage.LastNameTextBox.Text = "Doe";
            _registerPage.DateOfBirthPicker.SelectedDate = DateTime.Now.AddYears(-30);
            _registerPage.AddressTextBox.Text = "123 Main St";
            _registerPage.PhoneNumberTextBox.Text = "invalidphone"; // Invalid phone number

            // Act
            _registerPage.SubmitButton_Click(null, null);

            // Assert
            _mockMessageBoxService.Verify(mb => mb.Show("Please check the fields!", "Error", MessageBoxButton.OK), Times.Once);
        }
    }
        
}
