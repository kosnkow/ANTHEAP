namespace NipApi.Model
{
    public class Result<T>
    {
        public T subject { get; set; }
    }

    public class RootObject<T>
    {
        public Result<T> result { get; set; }
    }

    public class NipResponse
    {
        public string nip { get; set; }
        public string name { get; set; }

        public string statusVat { get; set; }

        public string regon { get; set; }

        public string pesel { get; set; }

        public string registrationLegalDate { get; set; }

        public string[] accountNumbers { get; set; }
    }
}
