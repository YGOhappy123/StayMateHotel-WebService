using server.Models;
using System.Threading.Tasks;

namespace server.Interfaces.Repositories
{
    public interface IRoomClassFeatureRepository
    {
        // Phương thức để xóa RoomClassFeature
        Task DeleteRoomClassFeature(RoomClassFeature roomClassFeature);

        // Bạn có thể thêm các phương thức khác nếu cần thiết như Get, Add, Update, etc.
    }
}
