using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digident_Group3.Interfaces
{
    public interface IDatabaseService
    {
        bool ValidateCredentials(string email, string password);
        bool RegisterUser(string email, string password, string firstName, string lastName, DateTime dateOfBirth, string address, string phoneNumber);
    }
}
