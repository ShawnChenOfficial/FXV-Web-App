using System;
using System.Security.Cryptography;
using System.Text;

namespace FXV.PasswordHasher
{
    public class PasswordHandler
    {
        public string GetEncrptedPWD(string inputPwd, string salt1, string salt2)
        {
            return EncryptPWD(inputPwd, salt1, salt2);
        }

        public bool IsMatch(string inputPwd, string _PwdInDb, string _salt1, string _salt2)
        {
            return _PwdInDb == EncryptPWD(inputPwd, _salt1, _salt2);
        }
        
        private string EncryptPWD(string inputPwd, string _s1, string _s2)
        {
            int position1 = inputPwd.Length / 3;
            int position2 = inputPwd.Length - position1 - 1;

            string CombinedPwd = (inputPwd.Insert(position2, _s2)).Insert(position1, _s1);

            byte[] hashedPwdBytes = new SHA256Managed().ComputeHash(new UTF8Encoding().GetBytes(CombinedPwd));

            string result = Convert.ToBase64String(hashedPwdBytes);

            return result;
        }
    }
}
