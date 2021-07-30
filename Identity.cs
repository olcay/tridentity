namespace OtomatikMuhendis.TRIdentity.Api
{
    public class Identity
    {
        public string Number { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Birthyear { get; set; }

        public Identity(string number)
        {
            Number = number;
        }

        public Identity(string number, string firstName, string lastName, int birthyear)
        {
            Number = number;
            FirstName = firstName.Trim().ToUpperInvariant();
            LastName = lastName.Trim().ToUpperInvariant();
            Birthyear = birthyear;
        }

        public bool IsNumberValid()
        {
            int sumEven = 0, sumOdd = 0, sumFirst10 = 0, i = 0;

            if (Number.Length != 11)
            {
                return false;
            }
            if (Number[0] == '0')
            {
                return false;
            }

            while (i <= 8)
            {
                int temp = int.Parse(Number[i].ToString());
                sumFirst10 += temp;
                if (i % 2 == 1) sumEven += temp;
                else sumOdd += temp;
                i++;
            }
            sumFirst10 += int.Parse(Number[9].ToString());

            if (!(((sumEven * 9) + (sumOdd * 7)) % 10 == int.Parse(Number[9].ToString()) && (sumFirst10 % 10 == int.Parse(Number[10].ToString()))))
            {
                return false;
            }

            return true;
        }
    }
}
