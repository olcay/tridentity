using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OtomatikMuhendis.TRIdentity.Api.Services
{
    public class NVIService
    {
        private readonly HttpClient _httpClient;

        public NVIService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://tckimlik.nvi.gov.tr");

            _httpClient = client;
        }

        private static string GetVerifyRequest(Identity identity)
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                <soap12:Body>
                <TCKimlikNoDogrula xmlns = ""http://tckimlik.nvi.gov.tr/WS"" >
                    <TCKimlikNo>{identity.Number}</TCKimlikNo>
                    <Ad>{identity.FirstName}</Ad>
                    <Soyad>{identity.LastName}</Soyad>
                    <DogumYili>{identity.Birthyear}</DogumYili>
                </TCKimlikNoDogrula >
                </soap12:Body>
            </soap12:Envelope>";
        }

        public async Task<bool> Verify(Identity identity, CancellationToken cancellationToken)
        {
            var identityStringContent = new StringContent(
                GetVerifyRequest(identity),
                Encoding.UTF8,
                MediaTypeNames.Application.Soap);

            using var httpResponse =
                await _httpClient.PostAsync("/Service/KPSPublic.asmx", identityStringContent, cancellationToken);

            httpResponse.EnsureSuccessStatusCode();

            var responseString = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            var xDocument = XDocument.Parse(responseString);
            var result = xDocument.Descendants().SingleOrDefault((XElement x) => x.Name.LocalName == "TCKimlikNoDogrulaResult").Value;
            return bool.Parse(result);
        }
    }
}
