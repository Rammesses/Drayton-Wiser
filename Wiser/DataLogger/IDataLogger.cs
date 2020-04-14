using Wiser.DataObjects;

namespace Wiser.DataLogger
{
    public interface IDataLogger
    {
        void WriteValveData(HeatHub hub);
        void WriteRoomData(HeatHub hub);
    }
}
