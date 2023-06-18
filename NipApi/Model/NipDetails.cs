namespace NipApi.Model
{
    public class NipDetails
    {
        public NipDetails()
        {
            AccountList = new List<string>();
        } 
        public long Nip { get; set; }
        public string Name { get; set; }
        public string StatusVat { get; set; }

        public long Regon { get; set; }
        public long Pesel { get; set; }

        public DateTime RegistrationLegalDate { get; set; }

        public List<string> AccountList { get; set; }
    }
}
