using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digident_Group3.Interfaces
{
    public interface IDatabaseService
    {
        int RegisterUser(string email, string password, string firstName, string lastName, DateTime dateOfBirth, string address, string phoneNumber);
        int ValidateCredentials(string email, string password); // Add this method to the interface
    }



}
