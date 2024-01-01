using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Microservice.Domain.SeedWork
{
    public static class CPFValidator
    {
        public static bool IsCpf(string cpf)
        {
            int[] multiply1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiply2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCPF;
            string digit;

            int sum;
            int rest;

            try
            {
                cpf = cpf.Trim();
                cpf = cpf.Replace(".", "").Replace("-", "");

                if (cpf.Length != 11)
                    return false;

                tempCPF = cpf.Substring(0, 9);
                sum = 0;

                for (int i = 0; i < 9; i++)
                    sum += int.Parse(tempCPF[i].ToString()) * multiply1[i];

                rest = sum % 11;

                if (rest < 2)
                    rest = 0;
                else
                    rest = 11 - rest;

                digit = rest.ToString();
                tempCPF = tempCPF + digit;
                sum = 0;

                for (int i = 0; i < 10; i++)
                    sum += int.Parse(tempCPF[i].ToString()) * multiply2[i];

                rest = sum % 11;

                if (rest < 2)
                    rest = 0;
                else
                    rest = 11 - rest;

                digit = digit + rest.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return cpf.EndsWith(digit);
        }
    }
}
