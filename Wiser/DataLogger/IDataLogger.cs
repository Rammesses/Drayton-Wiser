using System;
using System.Collections.Generic;
using Wiser.DataObjects;

namespace Wiser.DataLogger
{
    public interface IDataLogger
    {
        void WriteValveData(HeatHub hub);
        void WriteRoomData(HeatHub hub);

        List<RoomData> GetRoomData(string roomName, DateTime startDate, DateTime endDate);
    }
}
