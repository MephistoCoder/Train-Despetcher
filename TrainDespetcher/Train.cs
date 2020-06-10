using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainDespetcher
{
    class Train
    {
        private string number;
        private string startTime;
        private string wayTime;
        private string city;
        private int tickets;
        public string Number => number;
        public string StartTime => startTime;
        public string WayTime => wayTime;
        public string City => city;
        public int Tickets => tickets;



        public Train(string number, string city, string startTime, string wayTime,  int tickets)
        {
            this.number = number;
            this.city = city;
            this.startTime = startTime;
            this.wayTime = wayTime;
            this.tickets = tickets;
        }
        public static Train searchTrainByNumber(Train[] trains, string number)
        {
            for(int i = 0; i < trains.Length; i++)
            {
                if(trains[i].Number == number) 
                {
                    return trains[i];
                }
            }
            return null;
        }
        public static List<Train> searchFromAtoB(Train[] trains, string start, string end, string city)
        {
            
            List<Train> arrayList = new List<Train>();
            DateTime startTime, endTime;
            startTime = Convert.ToDateTime(start);
            endTime = Convert.ToDateTime(end);
            DateTime[] trainTimes = new DateTime[trains.Length];
            
            for (int i = 0; i < trains.Length; i++)
            {
                DateTime.TryParse(trains[i].StartTime,
                    CultureInfo.CreateSpecificCulture("en-US"),
                    DateTimeStyles.None,
                    out trainTimes[i]);
            }
            for (int i = 0; i < trains.Length; i++)
            {
                if (trains[i].City == city)
                {
                    if (trainTimes[i].TimeOfDay > startTime.TimeOfDay && trainTimes[i].TimeOfDay <= endTime.TimeOfDay)
                    {

                        arrayList.Add(trains[i]);
                    }
                }
            }
            
            return arrayList;
        }

        public override string ToString()
        {
            return number + " " + city + ", з: " + startTime + " , у дорозі: " + wayTime +
                " год, кв.: " + tickets;
        }
    }
}
