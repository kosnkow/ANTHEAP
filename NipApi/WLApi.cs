using System.Text.Json;
using NipApi.Model;

namespace NipApi
{
    public class WLApi: IWLApi
    {
        private readonly HttpClient client;
        private readonly string apiUrl;

        public WLApi(IConfiguration config)
        {
            client = new HttpClient();
            apiUrl = config.GetValue<string>("AppSettings:WLApiUrl");
        }
        
        public async Task<NipDetails> GetNipDetails(long nip)
        {
            NipDetails nipDetails = new NipDetails();

            string apiRequestUrl = $"{apiUrl}{nip}?date={DateTime.UtcNow:yyyy-MM-dd}";

            HttpResponseMessage response = await client.GetAsync(apiRequestUrl);
            if (!response.IsSuccessStatusCode) 
                return nipDetails;

            var json = await response.Content.ReadAsStringAsync();
            RootObject<NipResponse>? apiResponse = JsonSerializer.Deserialize<RootObject<NipResponse>>(json);

            if (apiResponse?.result == null)
                return nipDetails;

            nipDetails.Name = apiResponse.result.subject.name;
            nipDetails.StatusVat = apiResponse.result.subject.statusVat;

            if (long.TryParse(apiResponse.result.subject.nip, out var parsedNip))
            {
                nipDetails.Nip = parsedNip;
            }

            if (long.TryParse(apiResponse.result.subject.regon, out var parsedRegon))
            {
                nipDetails.Regon = parsedRegon;
            }

            if (long.TryParse(apiResponse.result.subject.pesel, out var parsedPesel))
            {
                nipDetails.Pesel = parsedPesel;
            }

            if (DateTime.TryParse(apiResponse.result.subject.registrationLegalDate, out var parsedRegistrationLegalDate))
            {
                nipDetails.RegistrationLegalDate = parsedRegistrationLegalDate;
            }

            foreach (var account in apiResponse.result.subject.accountNumbers)
            {
                if (!string.IsNullOrEmpty(account))
                {
                    nipDetails.AccountList.Add(account);
                }
            }
            return nipDetails;
        }
    }

    public interface IWLApi
    {
        Task<NipDetails> GetNipDetails(long nip);
    }
}
