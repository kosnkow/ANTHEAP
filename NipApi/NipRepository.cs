using NipApi.Model;
using System.Data;
using System.Data.SqlClient;

namespace NipApi
{
    public class NipRepository: INipRepository
    {
        private readonly SqlConnection _con;

        public NipRepository(IConfiguration config)
        {
            _con = new SqlConnection(config.GetConnectionString("NipDb"));
        }

        public bool NipExists(long nip)
        {
            SqlCommand command = new SqlCommand("SELECT Nip FROM NipDetails;", _con);
            _con.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if(reader.GetInt64(0) == nip)
                        return true;
                }
            }

            reader.Close();
            return false;
        }

        public void Add(NipDetails nipDetails)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT into NipDetails (Nip, Name, StatusVat, Regon, Pesel, RegistrationLegalDate) VALUES (@Nip, @Name, @StatusVat, @Regon, @Pesel, @RegistrationLegalDate)";
            cmd.Connection = _con;

            cmd.Parameters.AddWithValue("@Nip", nipDetails.Nip);
            cmd.Parameters.AddWithValue("@Name", nipDetails.Name);
            cmd.Parameters.AddWithValue("@StatusVat", nipDetails.StatusVat);
            cmd.Parameters.AddWithValue("@Regon", nipDetails.Regon);
            cmd.Parameters.AddWithValue("@Pesel", nipDetails.Pesel);
            cmd.Parameters.AddWithValue("@RegistrationLegalDate", nipDetails.RegistrationLegalDate);

            _con.Open();
            cmd.ExecuteNonQuery();

            List<string> accounts = new List<string>() ;

            foreach (var account in nipDetails.AccountList)
            {
                accounts.Add($"(@Nip, '{account}')");
            }

            cmd.CommandText = $"INSERT into Account (Nip, AccountNumber) VALUES {string.Join(",", accounts)}";

            cmd.ExecuteNonQuery();

            _con.Close();
        }
    }

    public interface INipRepository
    {
        public void Add(NipDetails nipDetails);

        public bool NipExists(long nip);
    }
}
