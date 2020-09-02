using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace TestingWorkspace
{

    class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
    {
        public async Task<int> GetBikeCountInStation(string stationName)
        {
            try
            {
                var uri = new Uri("https://api.digitransit.fi/routing/v1/routers/hsl/bike_rental");
                HttpClient httpClient = new HttpClient();
                string stations = await httpClient.GetStringAsync(uri);
                BikeRentalStationList bikeRentalStationList = JsonConvert.DeserializeObject<BikeRentalStationList>(stations);

                for (int i = 0; i < bikeRentalStationList.stations.Length; i++)
                {
                    if (stationName.Equals(bikeRentalStationList.stations[i].name))
                    {
                        return bikeRentalStationList.stations[0].bikesAvailable;
                        //Console.WriteLine(bikeRentalStationList.stations[0].bikesAvailable + " TEST");
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            try
            {
                throw new ArgumentException("Not Found");
            }
            catch (ArgumentException a)
            {
                Console.WriteLine(a.Message);
            }
            return 0;
        }
    }

    class OfflineCityBikeDataFetcher : ICityBikeDataFetcher
    {
        public async Task<int> GetBikeCountInStation(string stationName)
        {
            readFile(stationName);
            return 0;
        }

        public string readFile(string stationName)
        {
            string[] lines = System.IO.File.ReadAllLines(@"D:\VSCodeFolders\TestingWorkspace\bikedata.txt");

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Pyöriä = ");
            foreach (string line in lines)
            {
                string[] words = line.Split(' ');
                // Use a tab to indent each line of the file.
                if (words[0].Equals(stationName))
                {
                    Console.WriteLine(words[2]);
                    break;

                }
            }
            return lines[0];
        }
    }

    class Program
    {
        //static HttpClient client = new HttpClient();

        public static async Task Main(string[] args)
        {
            string asema = args[0];
            if (asema.Any(char.IsDigit)) throw new ArgumentException("Invalid argument: String contains numbers");
            OfflineCityBikeDataFetcher test2 = new OfflineCityBikeDataFetcher();
            await test2.GetBikeCountInStation(asema);
            //RealTimeCityBikeDataFetcher test = new RealTimeCityBikeDataFetcher();
            //Console.WriteLine(await test.GetBikeCountInStation(asema));

        }

    }

    public class BikeRentalStationList
    {
        public Station[] stations { get; set; }

    }

    public class Station
    {
        public int id { get; set; }
        public string name { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public int bikesAvailable { get; set; }
        public int spacesAvailable { get; set; }

    }
    public interface ICityBikeDataFetcher
    {
        Task<int> GetBikeCountInStation(string stationName);
    }

}

